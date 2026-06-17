using Microsoft.EntityFrameworkCore;
using TTBT_DTCD.Models;

namespace TTBT_DTCD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
