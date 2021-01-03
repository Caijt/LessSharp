using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessSharp.EntityConfiguration.Sys
{
    public class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.HasKey(e => new { e.RoleId, e.MenuId });
            ////builder.HasOne(e => e.Menu).WithMany(e => e.RoleMenus).HasForeignKey(e => e.MenuId);
            ////builder.HasOne(e => e.Role).WithMany(e => e.RoleMenus).HasForeignKey(e => e.RoleId);
            //builder.HasData(
            //    new RoleMenu { RoleId = -1, MenuId = 2, CanRead = true, CanReview = true, CanWrite = true },
            //    new RoleMenu { RoleId = -1, MenuId = 3, CanRead = true, CanReview = true, CanWrite = true },
            //    new RoleMenu { RoleId = -1, MenuId = 4, CanRead = true, CanReview = true, CanWrite = true }
            //    );
        }
    }
}
