using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LessSharp.Dto;
using LessSharp.Entity;

namespace LessSharp.MapperConfiguration
{
    /// <summary>
    /// 自动进行Dto与Entity的映射配置，以Entity的类名与Dto的类名进行匹配
    /// 例如EasySharp.Data.Sys.User与EasySharp.Dto.Sys.UserDto、UserCreateDto、UserUpdateDto等都能自动映射配置
    /// </summary>
    public class Configuration : Profile
    {
        /// <summary>
        /// 排除的Dto类型数组，以下类型不会注册映射配置
        /// </summary>
        private readonly Type[] ExcludeDtoTypes = new Type[] {
            typeof(ApiResultDto),
            typeof(ApiResultDto<>),
            typeof(PageListDto<>),
            typeof(PageListSummaryDto<>),
            typeof(QueryDto)
        };

        public Configuration()
        {
            var entityAssembly = typeof(IEntity).Assembly;
            string entityAssemblyName = entityAssembly.GetName().Name + ".";
            var entityTypes = entityAssembly.GetExportedTypes().Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass);
            var dtoAssembly = typeof(QueryDto).Assembly;
            string dtoAssemblyName = dtoAssembly.GetName().Name + ".";
            var dtoTypes = dtoAssembly.GetExportedTypes().Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass);
            foreach (var entityType in entityTypes)
            {
                CreateMap(entityType, typeof(OptionDto));
                var entityName = entityType.Name;
                if (entityType.FullName.StartsWith(entityAssemblyName))
                {
                    entityName = entityType.FullName.Substring(entityAssemblyName.Length);
                }
                foreach (var dtoType in dtoTypes.Where(e => e.FullName.StartsWith(dtoAssemblyName + entityName)))
                {
                    if (!ExcludeDtoTypes.Any(t => t.IsAssignableFrom(dtoType)))
                    {
                        CreateMap(dtoType, entityType).ReverseMap();
                    }

                }
            };

        }
    }
}
