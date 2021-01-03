using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LessSharp.Common
{
    /// <summary>
    /// 接口失败编码
    /// 1000 至 1999 是登录认证相关的失败编码
    /// 1000 至 1099 是认证失败编码
    /// 1100 至 1199 是授权失败编码
    /// </summary>
    public enum ApiFailCode
    {
        [Description("未检测到登录信息")]
        NO_LOGIN = 1001,

        [Description("登录信息已过期")]
        TOKEN_EXPIRE = 1002,

        [Description("登录信息已失效")]
        TOKEN_INVALID = 1003,

        [Description("拒绝访问")]
        NO_PERMISSION = 2001,

        [Description("当前用户没有此接口访问权限")]
        NO_API_PERMISSION = 2002,
        [Description("当前登录信息已禁用，请重新登录")]
        NO_API_PERMISSION2 = 2003,

        [Description("参数错误")]
        PARAMETER_ERROR = 3001,


        /// <summary>
        /// 这是系统内业务逻辑出错，是由开发人员手动抛出异常，例如像编号已重复、无法删除含有子记录的主表
        /// </summary>
        [Description("操作错误")]
        OPERATION_FAIL = 4001,
        [Description("Api接口错误")]
        API_FAIL = 5001
    }
}
