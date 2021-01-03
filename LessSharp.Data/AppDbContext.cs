using Microsoft.EntityFrameworkCore;

namespace LessSharp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new EntityConfiguration.Configuration().AutoRegisterModel(modelBuilder);
        }
    }


}
