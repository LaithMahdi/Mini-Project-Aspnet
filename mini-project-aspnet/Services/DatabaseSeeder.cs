using mini_project_aspnet.Models;
using System.Security.Cryptography;
using System.Text;

namespace mini_project_aspnet.Services
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.users.Any(u => u.email == "admin@itbs.com"))
            {
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
            }

            if (!context.sections.Any())
            {
                var sections = new List<Section>
                {
                    new Section { name = "First Year - A", code = "FY-A", isActive = true },
                    new Section { name = "First Year - B", code = "FY-B", isActive = true },
                    new Section { name = "Second Year - A", code = "SY-A", isActive = true }
                };

                context.sections.AddRange(sections);
            }

            if (!context.students.Any())
            {
                var students = new List<Student>
                {
                    new Student
                    {
                        id = Guid.NewGuid(),
                        fullName = "Yassine El Idrissi",
                        email = "yassine.student@itbs.com",
                        role = UserRole.Student,
                        password = HashPassword("2004-05-17"),
                        createdBy = "System",
                        gender = Gender.Man,
                        dateOfBirth = new DateTime(2004, 5, 17),
                        cinNumber = "AB123456",
                        phoneNumber = "+212600000001",
                        secondPhoneNumber = "+212600000011",
                        address = "Casablanca",
                        isActive = true,
                        enrollmentDate = DateTime.UtcNow
                    },
                    new Student
                    {
                        id = Guid.NewGuid(),
                        fullName = "Salma Bennani",
                        email = "salma.student@itbs.com",
                        role = UserRole.Student,
                        password = HashPassword("2005-03-08"),
                        createdBy = "System",
                        gender = Gender.Woman,
                        dateOfBirth = new DateTime(2005, 3, 8),
                        cinNumber = "CD789012",
                        phoneNumber = "+212600000002",
                        secondPhoneNumber = "+212600000022",
                        address = "Rabat",
                        isActive = true,
                        enrollmentDate = DateTime.UtcNow
                    }
                };

                context.students.AddRange(students);
            }

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
