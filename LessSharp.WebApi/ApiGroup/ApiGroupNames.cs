using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessSharp.WebApi.ApiGroup
{
    /// <summary>
    /// 系统分组枚举值
    /// </summary>
    public enum ApiGroupNames
    {
        [ApiGroupInfo(Title = "公共模块", Description = "系统公共接口", Version = "1.0")]
        Common,
        //[ApiGroupInfo(Title = "登录认证", Description = "登录认证相关接口", Version = "1.0")]
        //Auth,
        [ApiGroupInfo(Title = "系统管理", Description = "系统后台管理接口", Version = "1.0")]
        Sys
    }
}
