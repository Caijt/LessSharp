using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessSharp.Dto;
using LessSharp.Dto.Sys;
using LessSharp.Service.Sys;
using LessSharp.WebApi.ApiGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using LessSharp.WebApi.AuthorizationPolicies;

namespace LessSharp.WebApi.Controllers.Sys
{
    [OpenApiTag("菜单管理")]
    [ApiGroup(ApiGroupNames.Sys)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class MenuController : AutoRouteAuthorizeControllerBase
    {
        private readonly MenuService _menuService;
        public MenuController(MenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<List<MenuDto>>> GetList([FromQuery] MenuQueryDto queryDto)
        {
            return await _menuService.GetListAsync(queryDto);
        }

        /// <summary>
        /// 获取菜单公共列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<List<MenuDto>>> GetCommonList([FromQuery] MenuQueryDto queryDto)
        {
            return await _menuService.GetListAsync(queryDto);
        }
        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<int>> Save(MenuEditDto dto)
        {
            return await _menuService.SaveAsync(dto);
        }

        /// <summary>
        /// 根据Id值进行删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> DeleteById([FromBody] int id)
        {
            await _menuService.DeleteByIdAsync(id);
            return ApiResultDto.Success();
        }

        public async Task<ApiResultDto<MenuEditDto>> GetEditById(int id)
        {
            return await _menuService.GetEditById(id);
        }
        
    }
}