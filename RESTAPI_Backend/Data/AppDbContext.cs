using Microsoft.EntityFrameworkCore;
using RESTAPI_Backend.Models;

namespace RESTAPI_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
    }
}
