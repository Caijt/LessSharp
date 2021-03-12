using AutoMapper;
using LessSharp.Common;
using LessSharp.Common.CacheHelper;
using LessSharp.Data;
using LessSharp.Dto.Sys;
using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LessSharp.Service.Sys
{
    public class TokenService : EntityService<TokenDto, TokenDto, TokenDto, TokenQueryDto, Token, int>
    {
        private readonly AuthContext _authContext;
        private readonly ICacheHelper _cacheHelper;
        public static string DisabledTokenCacheKey = "DisabledToken";
        public TokenService(AppDbContext appDbContext, AuthContext authContext, ICacheHelper cacheHelper, IMapper mapper, IEnumerable<IEntityHandler<Token>> entityHandlers, IEnumerable<IQueryFilter<Token>> queryFilters) : base(appDbContext, mapper, entityHandlers, queryFilters)
        {
            _authContext = authContext;
            _cacheHelper = cacheHelper;
            IsFindOldEntity = true;
            OrderDefaultField = e => e.CreateTime;
        }
        protected override void OnCreating(Token entity, TokenDto createDto)
        {
            entity.Ip = _authContext.UserIP.ToString();
        }
        protected override IQueryable<Token> OnFilter(IQueryable<Token> query, TokenQueryDto queryDto)
        {
            query = base.OnFilter(query, queryDto);
            if (!string.IsNullOrEmpty(queryDto.UserLoginName))
            {
                query = query.Where(e => e.User.LoginName.Contains(queryDto.UserLoginName));
            }
            if (!string.IsNullOrEmpty(queryDto.AccessToken))
            {
                query = query.Where(e => e.AccessToken.Contains(queryDto.AccessToken));
            }
            if (!string.IsNullOrEmpty(queryDto.RefreshToken))
            {
                query = query.Where(e => e.RefreshToken.Contains(queryDto.RefreshToken));
            }
            if (queryDto.InCacheDisabled.HasValue)
            {
                var disabledTokens = _cacheHelper.SetGet(DisabledTokenCacheKey);
                if (disabledTokens == null)
                {
                    disabledTokens = new HashSet<string>();
                }
                if (queryDto.InCacheDisabled.Value)
                {
                    query = query.Where(e => disabledTokens.Contains(e.AccessToken));
                }
                else
                {
                    query = query.Where(e => !disabledTokens.Contains(e.AccessToken));
                }

            }
            return query;
        }

        /// <summary>
        /// 禁用Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task DisableTokenAsync(string accessToken)
        {
            var token = await this.DbQuery.Where(e => e.AccessToken == accessToken).FirstOrDefaultAsync();
            if (token == null)
            {
                return;
            }
            token.IsDisabled = true;
            await this.DbContext.SaveChangesAsync();
            //await this.UpdateEntityAsync(new Token
            //{
            //    Id = token.Id,
            //    IsDisabled = true
            //}, new List<Expression<Func<Token, object>>> { e => e.IsDisabled }, false);
            _cacheHelper.SetAdd(DisabledTokenCacheKey, accessToken);
        }

        /// <summary>
        /// 根据用户Id禁用Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task DisableTokenByUserIdAsync(int userId)
        {
            var tokens = await DbQuery.Where(e => e.UserId == userId && e.RefreshExpire >= DateTime.Now).Select(e => e.AccessToken).ToListAsync();
            tokens.ForEach(token =>
            {
                this.DisableTokenAsync(token).Wait();
            });
        }

        /// <summary>
        /// 重载已禁用的AccessToken到缓存中
        /// </summary>
        /// <returns></returns>
        public async Task ReloadDisabledTokenAsync()
        {
            var tokens = await DbQuery.Where(e => e.IsDisabled == true && e.AccessExpire > DateTime.Now).Select(e => e.AccessToken).ToListAsync();
            _cacheHelper.Delete(DisabledTokenCacheKey);
            if (tokens.Count > 0)
            {
                _cacheHelper.SetAdd(DisabledTokenCacheKey, tokens.ToArray());
            }

        }

        /// <summary>
        /// 根据AccessToken获取Token记录
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<TokenDto> GetByAccessTokenAsync(string accessToken)
        {
            return await QueryToSingleAsync<TokenDto>(this.DbQuery, e => e.AccessToken == accessToken);
        }

    }
}
