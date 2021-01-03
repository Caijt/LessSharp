using AutoMapper;
using LessSharp.Common;
using LessSharp.Data;
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
    public class UserService : EntityService<UserSaveDto, UserSaveDto, UserDto, UserQueryDto, User, int>
    {
        private readonly AuthContext _authContext;
        private readonly TokenService _tokenService;
        public UserService(AppDbContext appDbContext, IMapper mapper, AuthContext authContext, TokenService tokenService, IEnumerable<IEntityHandler<User>> entityHandlers, IEnumerable<IQueryFilter<User>> queryFilters) : base(appDbContext, mapper, entityHandlers, queryFilters)
        {
            _authContext = authContext;
            _tokenService = tokenService;
        }
        protected override IQueryable<User> OnFilter(IQueryable<User> query, UserQueryDto queryDto)
        {
            query = base.OnFilter(query, queryDto);
            if (!string.IsNullOrEmpty(queryDto.LoginName))
            {
                query = query.Where(e => e.LoginName.Contains(queryDto.LoginName));
            }
            if (!string.IsNullOrEmpty(queryDto.RoleName))
            {
                query = query.Where(e => e.Role.Name.Contains(queryDto.RoleName));
            }
            if (queryDto.IsDisabled.HasValue)
            {
                query = query.Where(e => e.IsDisabled == queryDto.IsDisabled.Value);
            }
            return query;
        }

        protected override void OnUpdating(User newEntity, User oldEntity, UserSaveDto updateDto, List<Expression<Func<User, object>>> excludeFields, bool isExcludeField)
        {
            if (newEntity.Id < 0)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "系统内置用户禁止修改");
            }
            if (!updateDto.ChangePassword)
            {
                excludeFields.Add(e => e.LoginPassword);
            }
            //禁用用户或修改密码的话，就得把之前登录的Token都禁用掉
            if (newEntity.IsDisabled || updateDto.ChangePassword)
            {
                _tokenService.DisableTokenByUserIdAsync(newEntity.Id).Wait();
            }
        }

        protected override void OnDeleting(User entity)
        {
            base.OnDeleting(entity);
            if (entity.Id < 0)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "系统内置用户禁止删除");
            }
        }

        /// <summary>
        /// 检查是否存在重复的登录名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistByLoginNameAsync(string loginName, int id = 0)
        {
            return await this.CheckExistAsync(DbQuery, e => e.LoginName == loginName && e.Id != id);
        }
    }
}
