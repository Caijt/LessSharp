using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessSharp.EntityConfiguration.Sys
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            //初始数据
            builder.HasData(
                new Menu { Id = 1, Name = "系统管理", Path = "sys", Order = 99 },
                new Menu { Id = 2, Name = "用户管理", ParentId = 1, ParentIds = "1", Path = "user", Order = 1, HasRead = true, HasWrite = true },
                new Menu { Id = 3, Name = "角色管理", ParentId = 1, ParentIds = "1", Path = "role", Order = 2, HasRead = true, HasWrite = true },
                new Menu { Id = 4, Name = "菜单管理", ParentId = 1, ParentIds = "1", Path = "menu", Order = 3, HasRead = true, HasWrite = true },
                new Menu { Id = 5, Name = "接口管理", ParentId = 1, ParentIds = "1", Path = "api", Order = 4, HasRead = true, HasWrite = true },
                new Menu { Id = 6, Name = "配置管理", ParentId = 1, ParentIds = "1", Path = "config", Order = 5, HasRead = true, HasWrite = true },
                new Menu { Id = 7, Name = "Token管理", ParentId = 1, ParentIds = "1", Path = "token", Order = 6, HasRead = true, HasWrite = true }
                );
        }
    }
}
