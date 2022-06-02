using Microsoft.EntityFrameworkCore;
using TheBlogApp.Models.Entities;

namespace TheBlogApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
    }
}
