using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LessSharp.EntityConfiguration.Sys
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasIndex(e => e.Name).IsUnique();
            builder.HasData(new Role()
            {
                Id = -1,
                Name = "超级角色",
                CreateTime = DateTime.MinValue,
                UpdateTime = DateTime.MinValue
            });
        }
    }
}
