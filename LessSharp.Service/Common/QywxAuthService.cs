using LessSharp.ApiService.Qywx;
using LessSharp.Data;
using LessSharp.Dto;
using LessSharp.Option;
using Microsoft.Extensions.Options;

namespace LessSharp.Service
{
    public class QywxAuthService
    {
        private readonly JwtOption _jwtOption;
        private readonly QywxApiService _qywxApiService;
        private readonly AppDbContext _appDbContext;
        public QywxAuthService(QywxApiService qywxApiService, IOptions<JwtOption> option, AppDbContext appDbContext)
        {
            _jwtOption = option.Value;
            _qywxApiService = qywxApiService;
            _appDbContext = appDbContext;
        }

        public string GetAuthUrl(string redirectUrl, string state = null)
        {
            return _qywxApiService.BuildAuthUrl(redirectUrl, state);
        }

        //public async Task<LoginResultDto> GetTokenByCode(string code)
        //{
        //    LoginResultDto dto = new LoginResultDto();
        //    var userInfo = await _qywxApiService.GetUserInfoByCodeAsync(code);
        //    if (!string.IsNullOrWhiteSpace(userInfo.UserId))
        //    {
        //        var user = await _qywxApiService.GetUserAsync(userInfo.UserId);
        //        var employee = _appDbContext.Set<Employee>().Include(e => e.User).Where(e => e.Phone == user.Mobile && e.DeleteTime == null && e.Status != EmployeeStatus.DIMISSION).FirstOrDefault();
        //        if (employee == null)
        //        {
        //            dto.Status = LoginResultDto.EMPLOYEE_FAIL;
        //        }
        //        else if (employee.User == null)
        //        {
        //            dto.Status = LoginResultDto.USER_FAIL;
        //        }
        //        else
        //        {
        //            List<Claim> claims = new List<Claim>();
        //            claims.Add(new Claim("qywxid", user.Userid));
        //            claims.Add(new Claim("employeeid", employee.Id.ToString()));
        //            claims.Add(new Claim("uid", employee.User.Id.ToString()));
        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));
        //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //            var jwt = new JwtSecurityToken(
        //                claims: claims,
        //                issuer: _jwtOption.Issuer,
        //                audience: _jwtOption.Audience,
        //                notBefore: DateTime.Now,
        //                expires: DateTime.Now.AddSeconds(_jwtOption.ExpiresIn),
        //                signingCredentials: creds
        //                );
        //            dto.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
        //            dto.ExpiresIn = _jwtOption.ExpiresIn;
        //            dto.Status = LoginResultDto.SUCCESS;
        //        }
        //    }
        //    else
        //    {
        //        dto.Status = LoginResultDto.QYWX_USER_FAIL;
        //    }
        //    return dto;

        //}

        public WeixinJssdkConfigDto GetJssdkConfig(string url)
        {
            return _qywxApiService.GetJssdkConfig(url);
        }
    }
}
