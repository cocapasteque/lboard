using Microsoft.EntityFrameworkCore;

namespace LBoard.Context
{
    public class LboardDbContext : DbContext
    {
        public LboardDbContext(DbContextOptions<LboardDbContext> options)
            : base(options)
        {
            
        }
    }
}