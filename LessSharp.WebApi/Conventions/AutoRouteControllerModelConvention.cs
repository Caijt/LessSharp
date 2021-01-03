using LessSharp.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace LessSharp.WebApi.Conventions
{
    public class AutoRouteControllerModelConvention : IControllerModelConvention
    {
        /// <summary>
        /// 路径前缀
        /// </summary>
        private readonly string _prefix;
        public AutoRouteControllerModelConvention(string prefix)
        {
            _prefix = prefix;
        }
        public void Apply(ControllerModel controller)
        {
            //判断是否是ApiConventionController的派生控制器
            if (controller.ControllerType.BaseType != typeof(AutoRouteControllerBase))
            {
                return;
            }
            //判断是否有自定义Route特性
            if (controller.ControllerType.GetCustomAttributes(typeof(RouteAttribute), false).Length > 0)
            {
                return;
            }
            string controllerNamespace = controller.ControllerType.Namespace;
            string temp = "Controllers.";
            int index = controllerNamespace.IndexOf(temp);
            string prefix = _prefix.Trim('/');
            if (index > -1)
            {
                prefix += "/" + controllerNamespace.Substring(index + temp.Length);
            }
            if (string.IsNullOrEmpty(prefix))
            {
                return;
            }
            if (!string.IsNullOrEmpty(prefix))
            {
                prefix = prefix.Replace(".", "/");
            }
            
            foreach (var selector in controller.Selectors.Where(s => s.AttributeRouteModel != null))
            {
                selector.AttributeRouteModel.Template = prefix + "/" + selector.AttributeRouteModel.Template;
            }
        }
    }
}
