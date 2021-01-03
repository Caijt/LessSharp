using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessSharp.WebApi.AuthorizationPolicies
{
    public class AuthorizationPolicyNames
    {
        /// <summary>
        /// 企业微信认证
        /// </summary>
        public const string QyWeixin = "QYWX";
        /// <summary>
        /// 管理员认证
        /// </summary>
        public const string Admin = "ADMIN";
        /// <summary>
        /// 员工认证
        /// </summary>
        public const string Employee = "EMPLOYEE";
        /// <summary>
        /// 接口访问权限
        /// </summary>
        public const string ApiPermission = "API_PERMISSION";
    }
}
