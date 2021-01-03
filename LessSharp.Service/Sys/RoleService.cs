using AutoMapper;
using LessSharp.Data;
using LessSharp.Dto.Sys;
using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LessSharp.Dto;
using LessSharp.Common;

namespace LessSharp.Service.Sys
{
    public class RoleService : EntityService<RoleSaveDto, RoleSaveDto, RoleDto, RoleQueryDto, Role, int>
    {
        public RoleService(AppDbContext appDbContext, IMapper mapper, IEnumerable<IEntityHandler<Role>> entityHandlers, IEnumerable<IQueryFilter<Role>> queryFilters) : base(appDbContext, mapper, entityHandlers, queryFilters)
        {
            this.SummaryExpression = e => new { total = e.Count() };
        }

        protected override IQueryable<Role> OnFilter(IQueryable<Role> query, RoleQueryDto queryDto)
        {
            query = base.OnFilter(query, queryDto);
            if (!string.IsNullOrEmpty(queryDto.Name))
            {
                query = query.Where(e => e.Name.Contains(queryDto.Name));
            }
            return query;
        }

        /// <summary>
        /// 获取角色选项列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetOptionList(RoleQueryDto dto)
        {
            return await QueryToListAsync<OptionDto>(DbQuery, dto);
        }

        public async Task<RoleSaveDto> GetEditById(int id)
        {
            var query = DbQuery.Include(e => e.RoleMenus);
            return await QueryToSingleByIdAsync<RoleSaveDto>(query, id);
        }

        protected override void OnUpdating(Role newEntity, Role oldEntity, RoleSaveDto updateDto, List<Expression<Func<Role, object>>> excludeFields, bool isExcludeField)
        {
            SaveEntities(newEntity.RoleMenus, DbContext.Set<RoleMenu>().Where(e => e.RoleId == newEntity.Id).ToList(), e => e.MenuId);
        }

        protected override void OnDeleting(Role entity)
        {
            if (entity.Id < 0)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "系统内置角色禁止删除");
            }
            if (DbContext.Set<Role>().Where(e => e.Id == entity.Id).Any(e => e.Users.Count > 0))
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "该角色下仍有用户，无法删除");
            }

        }

        /// <summary>
        /// 检查是否存在重复名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistByNameAsync(string path, int id = 0)
        {
            return await this.CheckExistAsync(DbQuery, e => e.Name == path && e.Id != id);
        }


        /// <summary>
        /// 根据角色Id获取角色全部菜单，包含上级菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<PermissionMenuDto>> GetRoleMenusAsync(int roleId)
        {
            List<PermissionMenuDto> permissionMenus;
            if (roleId > 0)
            {
                var menus = await DbContext.Set<RoleMenu>().Where(e => e.RoleId == roleId).ToListAsync();
                var menuIds = menus.Select(e => e.MenuId);
                var menuWithParentIds =await DbContext.Set<Menu>().Where(e => menuIds.Contains(e.Id)).Select(e => (e.ParentIds == null ? "" : e.ParentIds + ",") + e.Id.ToString()).ToListAsync();

                if (menuWithParentIds.Count > 0)
                {
                    var menuAllIds = string.Join(",", menuWithParentIds).Split(",").Distinct().Select(e => int.Parse(e));
                    permissionMenus =await DbContext.Set<Menu>().Where(e => menuAllIds.Contains(e.Id)).OrderBy(e => e.Order).Select(e => new PermissionMenuDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        ParentId = e.ParentId,
                        Path = e.Path,
                        Icon = e.Icon,
                        CanMultipleOpen = e.CanMultipleOpen,
                        IsPage = e.IsPage
                    }).ToListAsync();
                    permissionMenus.ForEach(item =>
                    {
                        var menu = menus.Where(e => e.MenuId == item.Id).FirstOrDefault();
                        if (menu != null)
                        {
                            item.CanRead = menu.CanRead;
                            item.CanWrite = menu.CanWrite;
                            item.CanReview = menu.CanReview;
                        }
                    });
                }
                else
                {
                    permissionMenus = new List<PermissionMenuDto>();
                }
            }
            else
            {
                permissionMenus =await DbContext.Set<Menu>().OrderBy(e => e.Order).Select(e => new PermissionMenuDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    ParentId = e.ParentId,
                    Path = e.Path,
                    Icon = e.Icon,
                    CanMultipleOpen = e.CanMultipleOpen,
                    IsPage = e.IsPage,
                    CanRead = true,
                    CanReview = true,
                    CanWrite = true
                }).ToListAsync();
            }
            return permissionMenus;
        }
    }
}
