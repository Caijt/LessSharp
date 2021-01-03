using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LessSharp.Data;
using LessSharp.Dto.Sys;
using LessSharp.Entity.Sys;

namespace LessSharp.Service.Sys
{
    public class ApiService : EntityService<ApiDto, ApiDto, ApiDto, ApiQueryDto, Api, int>
    {
        public ApiService(AppDbContext appDbContext, IMapper mapper, IEnumerable<IEntityHandler<Api>> entityHandlers, IEnumerable<IQueryFilter<Api>> queryFilters) : base(appDbContext, mapper, entityHandlers, queryFilters)
        {
        }
        protected override IQueryable<Api> OnFilter(IQueryable<Api> query, ApiQueryDto queryDto)
        {
            query = base.OnFilter(query, queryDto);
            if (queryDto.NotIds != null && queryDto.NotIds.Length > 0)
            {
                query = query.Where(e => !queryDto.NotIds.Contains(e.Id));
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Name))
            {
                query = query.Where(e => e.Name.Contains(queryDto.Name));
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Path))
            {
                query = query.Where(e => e.Path.Contains(queryDto.Path));
            }
            return query;
        }

        protected override void OnDeleting(Api entity)
        {
            base.OnDeleting(entity);
            DbContext.RemoveRange(DbContext.Set<MenuApi>().Where(e => e.ApiId == entity.Id));
        }

        /// <summary>
        /// 检查是否存在重复名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistByNameAsync(string name, int id = 0)
        {
            return await this.CheckExistAsync(DbQuery, e => e.Name == name && e.Id != id);
        }
        /// <summary>
        /// 检查是否存在重复路径
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistByPathAsync(string path, int id = 0)
        {
            return await this.CheckExistAsync(DbQuery, e => e.Path == path && e.Id != id);
        }
    }
}
