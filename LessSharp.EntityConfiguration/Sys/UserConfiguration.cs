using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LessSharp.EntityConfiguration.Sys
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(e => e.LoginName).IsUnique();
            builder.HasData(new User
            {
                Id = -1,
                LoginName = "superadmin",
                LoginPassword = "admin",
                RoleId = -1,
                CreateTime = DateTime.MinValue,
                UpdateTime = DateTime.MinValue
            });
        }
    }
}
