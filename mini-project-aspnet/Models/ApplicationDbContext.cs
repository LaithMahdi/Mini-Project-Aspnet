using Microsoft.EntityFrameworkCore;

namespace mini_project_aspnet.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

    }
}
