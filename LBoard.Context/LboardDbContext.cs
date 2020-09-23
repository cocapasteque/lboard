using LBoard.Models.Leaderboard;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LBoard.Context
{
    public class LboardDbContext : IdentityDbContext
    {
        public LboardDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Remove AspNet from table names
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var table = entityType.GetTableName();
                if (table.StartsWith("AspNet"))
                {
                    entityType.SetTableName(table.Substring(6));
                }
            }
        }
    }
}