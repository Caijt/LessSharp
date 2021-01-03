using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessSharp.EntityConfiguration.Sys
{
    public class ApiConfiguration : IEntityTypeConfiguration<Api>
    {
        public void Configure(EntityTypeBuilder<Api> builder)
        {
            builder.HasIndex(e => e.Name).IsUnique();
            builder.HasIndex(e => e.Path).IsUnique();
            //初始数据
            builder.HasData(
               new Api { Id = 1, Name = "获取接口分页列表", Path = "/Sys/Api/GetPageList", PermissionType = ApiPermissionType.READ },
                new Api { Id = 2, Name = "删除接口", Path = "/Sys/Api/DeleteById", PermissionType = ApiPermissionType.WRITE },
                new Api { Id = 3, Name = "保存接口", Path = "/Sys/Api/Save", PermissionType = ApiPermissionType.WRITE },
                new Api { Id = 4, Name = "获取接口公共分页列表", Path = "/Sys/Api/GetCommonPageList", IsCommon = true },
                new Api { Id = 5, Name = "获取角色公共选项列表", Path = "/Sys/Role/GetCommonOptionList", IsCommon = true },
                new Api { Id = 6, Name = "获取角色分页列表", Path = "/Sys/Role/GetPageList", PermissionType = ApiPermissionType.READ }
                );
        }
    }
}
