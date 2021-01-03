using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessSharp.WebApi.ApiGroup
{
    /// <summary>
    /// 系统分组特性
    /// </summary>
    public class ApiGroupAttribute : Attribute, IApiDescriptionGroupNameProvider, IApiDescriptionVisibilityProvider
    {
        public ApiGroupAttribute(ApiGroupNames name)
        {
            GroupName = name.ToString();
        }
        public string GroupName { get; private set; }

        public bool IgnoreApi { get; set; }
    }
}
