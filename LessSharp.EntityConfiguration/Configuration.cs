using LessSharp.Common;
using LessSharp.Entity;
using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LessSharp.EntityConfiguration
{
    public class Configuration
    {

        /// <summary>
        /// 是否开启表名、列名小写加下划线命名方式
        /// </summary>
        private readonly bool EnableLowerUnderscoreName = true;

        private readonly bool EnableNamespaceNamePrefix = true;
        /// <summary>
        /// 禁用外键级联删除
        /// </summary>
        private readonly bool DisabledForeignKeyDeleteCascade = true;

        /// <summary>
        /// 用来配合小写加小划线命名，数组里的字段类型才会被重命名
        /// </summary>
        private readonly Type[] FieldTypes = new Type[] {
            typeof(Guid),
            typeof(string),
            typeof(int),
            typeof(int?),
            typeof(bool),
            typeof(bool?),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?),
            typeof(char),
            typeof(char?),
            typeof(decimal),
            typeof(decimal?),
            typeof(float),
            typeof(float?)
        };

        /// <summary>
        /// 自动注册模型实体，并根据EnableLowerUnderScoreName值判断是否将表名及列名转换为小写加下划线
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void AutoRegisterModel(ModelBuilder modelBuilder)
        {
            //var b = modelBuilder.Model.GetEntityTypes().ToList();
            //var a = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).Where(e => e.DeleteBehavior == DeleteBehavior.Cascade).ToList();
            var assembly = Assembly.GetAssembly(typeof(IEntity));
            string assemblyName = assembly.GetName().Name + ".";
            var types = assembly.GetExportedTypes().Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass);
            foreach (var type in types)
            {
                bool isEntity = typeof(IEntity).IsAssignableFrom(type);
                bool isQuery = typeof(IQuery).IsAssignableFrom(type);
                if (!isEntity && !isQuery)
                {
                    continue;
                }
                var entityBuilder = modelBuilder.Entity(type);
                if (isQuery)
                {
                    entityBuilder.HasNoKey();
                }
                if (!EnableLowerUnderscoreName && !EnableNamespaceNamePrefix)
                {
                    continue;
                }
                var tableName = type.Name;
                if (EnableNamespaceNamePrefix)
                {
                    int index = type.Namespace.IndexOf(assemblyName);
                    if (index != -1 && type.Namespace.Length > assemblyName.Length)
                    {
                        string prefix = type.Namespace.Substring(index + assemblyName.Length);
                        tableName = prefix + tableName;
                    }
                }
                if (isQuery)
                {
                    tableName = "V_" + tableName;
                }
                if (EnableLowerUnderscoreName)
                {
                    tableName = CommonHelper.CamelCaseToLowerUnderScore(tableName);
                }
                if (isEntity)
                {
                    entityBuilder.ToTable(tableName);
                }
                else
                {
                    entityBuilder.ToView(tableName);
                }
                foreach (var prop in type.GetProperties())
                {
                    if (FieldTypes.Any(e => e == prop.PropertyType) || prop.PropertyType.IsEnum)
                    {
                        string columnName = CommonHelper.CamelCaseToLowerUnderScore(prop.Name);
                        entityBuilder.Property(prop.Name).HasColumnName(columnName);
                    }
                }
            }

            #region 关闭外键级联删除
            if (DisabledForeignKeyDeleteCascade)
            {
                var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
                foreach (var fk in foreignKeys)
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
