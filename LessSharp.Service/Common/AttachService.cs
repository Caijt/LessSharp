using AutoMapper;
using LessSharp.Common;
using LessSharp.Data;
using LessSharp.Dto;
using LessSharp.Entity;
using LessSharp.Option;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LessSharp.Service
{
    /// <summary>
    /// 上传附件服务类
    /// </summary>
    public class AttachService : EntityService<AttachSaveDto, AttachSaveDto, AttachDto, AttachQueryDto, Attach, int>
    {
        private readonly UploadOption _uploadOption;
        private readonly IHostingEnvironment _env;
        public AttachService(AppDbContext appDbContext, IMapper mapper,
            IEnumerable<IEntityHandler<Attach>> entityHandlers,
            IEnumerable<IQueryFilter<Attach>> queryFilters,
            IOptionsSnapshot<UploadOption> options,
            IHostingEnvironment env)
            : base(appDbContext, mapper, entityHandlers, queryFilters)
        {
            _uploadOption = options.Value;
            _env = env;
            this.IsFindOldEntity = true;
            this.OrderDescDefaultValue = false;
        }

        protected override IQueryable<Attach> OnFilter(IQueryable<Attach> query, AttachQueryDto queryDto)
        {
            query = base.OnFilter(query, queryDto);
            if (queryDto.EntityGuid.HasValue)
            {
                query = query.Where(e => e.EntityGuid == queryDto.EntityGuid.Value);
            }
            if (queryDto.Guid.HasValue)
            {
                query = query.Where(e => e.EntityGuid == queryDto.Guid.Value);
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Type))
            {
                query = query.Where(e => e.Type == queryDto.Type);
            }
            return query;
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="uploadDto"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public async Task<AttachDto> UploadAsync(AttachUploadDto uploadDto, Type entityType = null)
        {
            string uploadPath = GetUploadPath(uploadDto.IsPublic);
            string entityName = "";
            if (entityType != null)
            {
                entityName = GetEntityTypeName(entityType);
            }
            string prefixPath = Path.Combine(entityName, DateTime.Now.ToString("yyyyMMdd"));
            string finalPath = Path.Combine(uploadPath, prefixPath);
            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }
            string fileName = Path.GetRandomFileName();
            if (uploadDto.IsPublic)
            {
                fileName += Path.GetExtension(uploadDto.File.FileName);
            }
            using (FileStream stream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
            {
                await uploadDto.File.CopyToAsync(stream);
            }
            var saveDto = new AttachSaveDto()
            {
                Path = Path.Combine(prefixPath, fileName).Replace("\\", "/"),
                Size = Convert.ToInt32(uploadDto.File.Length),
                Ext = Path.GetExtension(uploadDto.File.FileName).ToLower(),
                Name = uploadDto.File.FileName,
                EntityGuid = uploadDto.EntityGuid.Value,
                EntityName = entityName,
                IsPublic = uploadDto.IsPublic,
                Type = uploadDto.Type
            };
            int id = await this.SaveAsync(saveDto);
            return await GetByIdAsync(id);
        }

        public async Task<AttachDto> UploadAsync<T>(AttachUploadDto uploadDto)
        {
            return await UploadAsync(uploadDto, typeof(T));
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public AttachDownloadResultDto Download(int id, Type entityType = null)
        {
            var query = DbQuery;
            if (entityType != null)
            {
                var entityName = GetEntityTypeName(entityType);
                query.Where(e => e.EntityName == entityName);
            }
            var attach = query.FirstOrDefault(e => e.Id == id);
            if (attach == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "找不到该附件");
            }
            string uploadPath = GetUploadPath(attach.IsPublic);
            string filePath = Path.Combine(uploadPath, attach.Path);
            var result = new AttachDownloadResultDto();
            if (new FileExtensionContentTypeProvider().TryGetContentType(attach.Name, out string contentType))
            {
                result.ContentType = contentType;
            }
            result.FileName = attach.Name;
            result.Stream = File.OpenRead(filePath);
            return result;
        }


        public AttachDownloadResultDto Download<T>(int id)
        {
            return Download(id, typeof(T));
        }
        public async override Task<List<AttachDto>> GetListAsync(AttachQueryDto queryDto)
        {
            var list = await base.GetListAsync(queryDto);
            list.ForEach(item =>
            {
                AddPublicPrefix(item);
            });
            return list;
        }

        public async override Task<AttachDto> GetByIdAsync(int id)
        {
            var dto = await base.GetByIdAsync(id);
            AddPublicPrefix(dto);
            return dto;
        }

        protected override void OnDeleting(Attach entity)
        {
            base.OnDeleting(entity);
            if (entity == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "找不到该附件");
            }
            string uploadPath = GetUploadPath(entity.IsPublic);
            var filePath = Path.Combine(uploadPath, entity.Path);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<int> DeleteByIdAsync<T>(int id)
        {
            string entityName = GetEntityTypeName(typeof(T));
            if (!DbSet.Where(e => e.EntityName == entityName && e.Id == id).Any())
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "找不到该附件");
            }
            return await DeleteByIdAsync(id);
        }

        /// <summary>
        /// 给dto增加前缀路径
        /// </summary>
        /// <param name="dto"></param>
        private void AddPublicPrefix(AttachDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.PublicPath))
            {
                dto.PublicPath = Path.Combine(_uploadOption.PublicPathPrefix, dto.PublicPath).Replace("\\", "/");
            }
        }

        /// <summary>
        /// 获取实体类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetEntityTypeName(Type type)
        {
            string entityName = type.Name;
            string assemblyName = type.Assembly.GetName().Name + ".";
            int index = type.FullName.IndexOf(assemblyName);
            if (index != -1)
            {
                entityName = type.FullName.Substring(index + assemblyName.Length);
            }
            return entityName;
        }

        private string GetUploadPath(bool isPublic)
        {
            string uploadPath;
            if (isPublic)
            {
                uploadPath = _uploadOption.PublicUploadPath;
            }
            else
            {
                uploadPath = _uploadOption.PrivateUploadPath;
            }
            if (uploadPath.StartsWith("~"))
            {
                uploadPath = uploadPath.Replace("~", _env.WebRootPath);
            }
            if (!Path.IsPathRooted(uploadPath))
            {
                uploadPath = Path.GetFullPath(uploadPath);
            }
            return uploadPath;
        }

        public async Task DeleteByGuidAsync(Guid guid)
        {
            this.AutoSaveChanges = false;
            var attachList = DbQuery.Where(e => e.EntityGuid == guid);
            foreach (var item in attachList)
            {
                await this.DeleteAsync(item);
            }
            this.AutoSaveChanges = true;
            await DbContext.SaveChangesAsync();
        }
        public async Task SoftDeleteByGuidAsync(Guid guid)
        {
            this.AutoSaveChanges = false;
            var attachList = DbQuery.Where(e => e.EntityGuid == guid);
            foreach (var item in attachList)
            {
                await this.SoftDeleteAsync(item);
            }
            this.AutoSaveChanges = true;
            await DbContext.SaveChangesAsync();
        }
    }
}
