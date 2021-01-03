using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using LessSharp.Data;
using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace LessSharp.Service
{
    /// <summary>
    /// 授权中心
    /// </summary>
    public class AuthContext
    {
        private readonly AppDbContext _appDbContext;
        private User _user;

        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public User User
        {
            get
            {
                if (_user == null)
                {
                    _user = _appDbContext.Set<User>().AsNoTracking().Where(e => e.Id == UserId).FirstOrDefault();
                }
                return _user;
            }
        }
        public int? EmployeeId { get; set; }
        public IPAddress UserIP { get; set; }
        public bool IsLogin
        {
            get
            {
                return UserId != 0;
            }
        }
        public AuthContext(IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            var uid = httpContextAccessor.HttpContext.User.FindFirst("uid")?.Value;
            if (uid != null)
            {
                UserId = int.Parse(uid);
            }
            var employeeId = httpContextAccessor.HttpContext.User.FindFirst("employeeid")?.Value;
            if (employeeId != null)
            {
                EmployeeId = int.Parse(employeeId);
            }
            UserIP = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            AccessToken = httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
        }
    }
}
