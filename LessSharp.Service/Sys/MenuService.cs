using AutoMapper;
using LessSharp.Common;
using LessSharp.Data;
using LessSharp.Dto;
using LessSharp.Dto.Sys;
using LessSharp.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LessSharp.Service.Sys
{
    public class MenuService : EntityService<MenuEditDto, MenuEditDto, MenuDto, MenuQueryDto, Menu, int>
    {
        public MenuService(AppDbContext appDbContext, IMapper mapper, IEnumerable<IEntityHandler<Menu>> entityHandlers, IEnumerable<IQueryFilter<Menu>> queryFilters) : base(appDbContext, mapper, entityHandlers, queryFilters)
        {
            this.OrderDefaultField = e => e.Order;
            this.OrderDescDefaultValue = false;
        }
        protected override IQueryable<Menu> OnFilter(IQueryable<Menu> query, MenuQueryDto queryDto)
        {
            query = base.OnFilter(query, queryDto);
            if (!string.IsNullOrEmpty(queryDto.Name))
            {
                query = query.Where(e => e.Name.Contains(queryDto.Name));
            }
            if (!string.IsNullOrEmpty(queryDto.Path))
            {
                query = query.Where(e => e.Name.Contains(queryDto.Path));
            }
            if (queryDto.NotId.HasValue && queryDto.NotId.Value != 0)
            {
                query = query.Where(e => e.Id != queryDto.NotId.Value);
            }
            if (queryDto.IsPage.HasValue)
            {
                query = query.Where(e => e.IsPage == queryDto.IsPage.Value);
            }
            return query;
        }

        protected override void OnCreating(Menu entity, MenuEditDto createDto)
        {
            base.OnCreating(entity, createDto);
            if (entity.ParentId.HasValue)
            {
                var parentIds = DbSet.Where(e => e.Id == entity.ParentId.Value).Select(e => e.ParentIds).FirstOrDefault();
                entity.ParentIds = string.IsNullOrEmpty(parentIds) ? entity.ParentId.ToString() : (parentIds + "," + entity.ParentId);
            }
        }
        protected override void OnUpdating(Menu newEntity, Menu oldEntity, MenuEditDto updateDto, List<Expression<Func<Menu, object>>> excludeProps, bool isExcludeField)
        {
            if (newEntity.ParentId.HasValue)
            {
                var parentIds = DbSet.Where(e => e.Id == newEntity.ParentId.Value).Select(e => e.ParentIds).FirstOrDefault();
                newEntity.ParentIds = string.IsNullOrEmpty(parentIds) ? newEntity.ParentId.ToString() : (parentIds + "," + newEntity.ParentId);
            }
            else
            {
                newEntity.ParentIds = null;
            }
            DbSet.Where(e => ("," + e.ParentIds + ",").Contains("," + newEntity.Id.ToString() + ",")).ToList().ForEach(e =>
            {
                string a = "," + e.ParentIds + ",";
                string b = "," + newEntity.Id.ToString() + ",";
                string c = a.Substring(a.IndexOf(b)).Trim(',');
                e.ParentIds = string.IsNullOrEmpty(newEntity.ParentIds) ? c : ((newEntity.ParentIds ?? "") + "," + c);
            });
            newEntity.MenuApis.ForEach(m =>
            {
                if (!DbContext.Set<Api>().Where(e => e.Id == m.ApiId).Select(e => e.IsCommon).FirstOrDefault())
                {
                    DbContext.RemoveRange(DbContext.Set<MenuApi>().Where(e => e.ApiId == m.ApiId && e.MenuId != m.MenuId));
                }
            });
            SaveEntities(newEntity.MenuApis, DbQuery.Where(e => e.Id == newEntity.Id).SelectMany(e => e.MenuApis).ToList(), e => e.ApiId);
        }

        protected override void OnSaving(Menu newEntity, Menu oldEntity, MenuEditDto saveDto)
        {
            if (newEntity.IsPage)
            {
                if (DbQuery.Any(e => e.Id != newEntity.Id && e.ParentId == newEntity.ParentId))
                {
                    throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "作为子页面的菜单必须在底层菜单下级创建！");
                }
            }
        }

        protected override void OnDeleting(Menu entity)
        {
            if (DbQuery.Where(e => e.ParentId == entity.Id).Any())
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "此菜单下还有子菜单，无法删除！");
            }
            DbContext.RemoveRange(DbContext.Set<MenuApi>().Where(e => e.MenuId == entity.Id));
            DbContext.RemoveRange(DbContext.Set<RoleMenu>().Where(e => e.MenuId == entity.Id));
        }

        public async Task<MenuEditDto> GetEditById(int id)
        {
            var dto = await this.QueryToSingleByIdAsync<MenuEditDto>(DbQuery, id);
            if (dto.ParentId.HasValue)
            {
                dto.ParentName = DbQuery.Where(e => e.Id == dto.ParentId.Value).Select(e => e.Name).FirstOrDefault();
            }
            return dto;
        }


    }
}
