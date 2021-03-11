using LessSharp.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessSharp.EntityConfiguration.Sys
{
    public class TokenConfiguration : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            //builder.HasKey(e => e.AccessToken);
        }
    }
}
