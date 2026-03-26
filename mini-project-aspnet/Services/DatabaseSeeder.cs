using mini_project_aspnet.Models;
using System.Security.Cryptography;
using System.Text;

namespace mini_project_aspnet.Services
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Only seed if no users exist
            if (context.users.Any())
            {
                return;
            }

            // Create default admin user
            var adminUser = new User
            {
                id = Guid.NewGuid(),
                fullName = "Administrator",
                email = "admin@itbs.com",
                password = HashPassword("123456789"),
                role = UserRole.Admin,
                createdBy = "System",
                createdAt = DateTime.UtcNow,
                updatedAt = DateTime.UtcNow
            };

            context.users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
