using LessSharp.Common;
using LessSharp.Data;
using LessSharp.Dto;
using LessSharp.Entity.Sys;
using LessSharp.Option;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LessSharp.Dto.Common;
using LessSharp.Service.Interceptors;
using LessSharp.Service.Sys;
using LessSharp.Dto.Sys;
using System.Threading.Tasks;

namespace LessSharp.Service
{
    /// <summary>
    /// 认证服务
    /// </summary>
    public class AuthService
    {
        private readonly JwtOption _jwtOption;
        private readonly AppDbContext _appDbContext;
        private readonly AuthContext _authContext;
        private readonly TokenService _tokenService;
        private readonly RoleService _roleService;
        private readonly UserLoginLogService _userLoginLogService;
        public AuthService(AppDbContext dbContext, AuthContext authContext, RoleService roleService, TokenService tokenService, UserLoginLogService userLoginLogService, IOptionsSnapshot<JwtOption> option)
        {
            _jwtOption = option.Value;
            _appDbContext = dbContext;
            _authContext = authContext;
            _tokenService = tokenService;
            _roleService = roleService;
            _userLoginLogService = userLoginLogService;
        }
        /// <summary>
        /// 根据登录名及密码获取身份token
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [TransactionInterceptor]
        public async Task<LoginResultDto> GetTokenByLoginAsync(LoginDto loginDto)
        {
            var user = await _appDbContext.Set<User>().Where(e => e.IsDisabled == false && e.LoginName == loginDto.LoginName).FirstOrDefaultAsync();
            var result = new LoginResultDto();
            if (user == null)
            {
                result.Status = LoginResultDto.USER_FAIL;
                return result;
            }
            if (user.LoginPassword != loginDto.LoginPassword)
            {
                result.Status = LoginResultDto.PASSWORD_FAIL;
                return result;
            }
            await _userLoginLogService.CreateAsync(new UserLoginLogDto { UserId = user.Id });
            await _appDbContext.SaveChangesAsync();
            result.Status = LoginResultDto.SUCCESS;
            result.AccessToken = CreateAccessToken(user.Id);
            result.AccessExpiresIn = _jwtOption.AccessExpiresIn;
            result.RefreshToken = CreateRefreshToken(user.Id, loginDto.IsRemember);
            result.RefreshExpiresIn = loginDto.IsRemember ? _jwtOption.RememberRefreshExpiresIn : _jwtOption.RefreshExpiresIn;
            await SaveTokenRecordAsync(result, user.Id);
            return result;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            await _tokenService.DisableTokenAsync(_authContext.AccessToken);
        }

        /// <summary>
        /// 保存Token记录
        /// </summary>
        /// <param name="loginResult"></param>
        /// <param name="userId"></param>
        private async Task SaveTokenRecordAsync(LoginResultDto loginResult, int userId)
        {
            await _tokenService.SaveAsync(new Dto.Sys.TokenDto
            {
                AccessToken = loginResult.AccessToken,
                RefreshToken = loginResult.RefreshToken,
                UserId = userId,
                AccessExpire = DateTime.Now.AddSeconds(loginResult.AccessExpiresIn),
                RefreshExpire = DateTime.Now.AddSeconds(loginResult.RefreshExpiresIn)
            });
        }

        /// <summary>
        /// 刷新token，为什么使用缓存，因为考虑到可能同时并发多个接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [CacheInterceptor(ExpireSeconds = 20)]
        [TransactionInterceptor]
        public virtual async Task<LoginResultDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {

            var result = new LoginResultDto();

            try
            {
                if (!await _appDbContext.Set<Token>().AnyAsync(e => e.AccessToken == refreshTokenDto.AccessToken && e.RefreshToken == refreshTokenDto.RefreshToken && e.RefreshExpire >= DateTime.Now && e.IsDisabled == false))
                {
                    throw new SecurityTokenValidationException();
                }
                var tokenValidation = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtOption.Issuer,
                    ValidAudience = _jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.AccessSecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
                var handler = new JwtSecurityTokenHandler();
                var token2 = handler.ValidateToken(refreshTokenDto.AccessToken, tokenValidation, out SecurityToken _);
                tokenValidation.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.RefreshSecretKey));
                tokenValidation.ValidateLifetime = true;

                var token3 = new JwtSecurityTokenHandler().ValidateToken(refreshTokenDto.RefreshToken, tokenValidation, out SecurityToken _);

                if (token2.FindFirst("uid").Value != token3.FindFirst("uid").Value)
                {
                    throw new SecurityTokenValidationException();
                }
                var userId = int.Parse(token3.FindFirst("uid").Value);
                var user = await _appDbContext.Set<User>().Where(e => e.IsDisabled == false && e.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    result.Status = LoginResultDto.USER_FAIL;
                    return result;
                }
                await _userLoginLogService.CreateAsync(new UserLoginLogDto { UserId = user.Id });
                await _appDbContext.SaveChangesAsync();
                result.Status = LoginResultDto.SUCCESS;
                result.AccessToken = CreateAccessToken(user.Id);
                result.AccessExpiresIn = _jwtOption.AccessExpiresIn;
                result.RefreshToken = CreateRefreshToken(user.Id, refreshTokenDto.IsRemember);
                result.RefreshExpiresIn = refreshTokenDto.IsRemember ? _jwtOption.RememberRefreshExpiresIn : _jwtOption.RefreshExpiresIn;
                await SaveTokenRecordAsync(result, user.Id);
                await _tokenService.DisableTokenAsync(refreshTokenDto.AccessToken);
            }
            catch (SecurityTokenValidationException)
            {
                result.Status = LoginResultDto.TOKEN_FAIL;
            }
            return result;
        }

        /// <summary>
        /// 创建AccessToken
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string CreateAccessToken(int userId)
        {
            var claims = new Claim[] {
                new Claim("uid",userId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.AccessSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(_jwtOption.AccessExpiresIn),
                signingCredentials: creds
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        /// <summary>
        /// 创建RefreshToken
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string CreateRefreshToken(int userId, bool isRemember)
        {
            var claims = new Claim[] {
                new Claim("uid",userId.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.RefreshSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(isRemember ? _jwtOption.RememberRefreshExpiresIn : _jwtOption.RefreshExpiresIn),
                signingCredentials: creds
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        public async Task<AuthInfoDto> GetAuthInfoAsync()
        {
            var result = new AuthInfoDto();
            var user = await _appDbContext.Set<User>().Where(e => e.Id == _authContext.UserId).FirstOrDefaultAsync();
            result.PermissionMenus = await _roleService.GetRoleMenusAsync(user.RoleId);
            result.UserId = user.Id;
            result.UserName = user.LoginName;
            return result;
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<ApiPermissionCheckResultDto> CheckApiPermissionByPathAsync(string path)
        {
            var api = await _appDbContext.Set<Api>().AsNoTracking().Where(e => e.Path == path).FirstOrDefaultAsync();
            var user = await _appDbContext.Set<User>().AsNoTracking().Where(e => e.Id == _authContext.UserId).FirstOrDefaultAsync();
            var dto = new ApiPermissionCheckResultDto() { ApiName = api?.Name };
            if (user.RoleId < 0)
            {
                dto.IsSuccess = true;
                return dto;
            }
            if (api == null)
            {
                dto.IsSuccess = false;
                return dto;
            }
            if (api.IsCommon)
            {
                dto.IsSuccess = await _appDbContext.Set<User>().Where(e => e.Id == _authContext.UserId).SelectMany(e => e.Role.RoleMenus).Select(e => e.Menu).SelectMany(e => e.MenuApis).AnyAsync(e => e.ApiId == api.Id);
                return dto;
            }
            Expression<Func<RoleMenu, bool>> exp;
            if (api.PermissionType == ApiPermissionType.READ)
            {
                exp = e => e.CanRead;
            }
            else if (api.PermissionType == ApiPermissionType.WRITE)
            {
                exp = e => e.CanWrite;
            }
            else
            {
                exp = e => e.CanReview;
            }
            dto.IsSuccess = await _appDbContext.Set<Role>().Where(e => e.Id == user.RoleId).SelectMany(e => e.RoleMenus).Where(exp).Select(e => e.Menu).SelectMany(e => e.MenuApis).AnyAsync(e => e.ApiId == api.Id);
            return dto;
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (dto.OldPassword == dto.NewPassword)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "新密码与原密码一样，无法修改");
            }
            var user = await _appDbContext.Set<User>().Where(e => e.Id == _authContext.UserId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "用户不存在");
            }
            if (user.LoginPassword != dto.OldPassword)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "用户原密码不正确");
            }
            //user.LoginPassword = dto.NewPassword;
            //await _appDbContext.SaveChangesAsync();
            await _tokenService.DisableTokenByUserIdAsync(_authContext.UserId);
        }
    }
}
