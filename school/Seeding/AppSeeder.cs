using Microsoft.EntityFrameworkCore;
using school.Models;

namespace school.Seeding
{
    public static class AppSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var now = DateTime.UtcNow;

            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "System Administrator",
                    Role = Role.Admin,
                    Gender = Gender.Man,
                    UserName = "admin",
                    Email = "admin@school.local",
                    PhoneNumber = "0000000000",
                    Password = "admin",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }

            if (!await context.Sections.AnyAsync())
            {
                context.Sections.AddRange(
                    new Section { Name = "Computer Science", Code = "CS", Description = "Computer Science Section", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Mathematics", Code = "MATH", Description = "Mathematics Section", CreatedAt = now, UpdatedAt = now }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Subjects.AnyAsync())
            {
                context.Subjects.AddRange(
                    new Subject { Name = "Algorithms", Code = "ALGO", Coefficient = 2.00m, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Databases", Code = "DB", Coefficient = 2.00m, HoursPerWeek = 3, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Rooms.AnyAsync())
            {
                context.Rooms.Add(new Room
                {
                    Name = "Room A1",
                    Type = RoomType.TD,
                    Capacity = 30,
                    Building = "Main",
                    Floor = 1,
                    HasProjector = true,
                    HasComputers = false,
                    IsAvailable = true,
                    Equipment = "Projector",
                    CreatedAt = now,
                    UpdatedAt = now
                });
                await context.SaveChangesAsync();
            }

            var teacherUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "teacher1");
            if (teacherUser == null)
            {
                teacherUser = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "Default Teacher",
                    Role = Role.Teacher,
                    Gender = Gender.Woman,
                    UserName = "teacher1",
                    Email = "teacher1@school.local",
                    PhoneNumber = "1111111111",
                    Password = "teacher123",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                context.Users.Add(teacherUser);
                await context.SaveChangesAsync();
            }

            if (!await context.Teachers.AnyAsync(t => t.UserId == teacherUser.Id))
            {
                context.Teachers.Add(new Teacher
                {
                    Id = Guid.NewGuid(),
                    UserId = teacherUser.Id,
                    Specialization = "Computer Science",
                    Gender = teacherUser.Gender,
                    HireDate = now,
                    Salary = 5000m,
                    IsActive = true
                });
                await context.SaveChangesAsync();
            }

            var studentUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "student1");
            if (studentUser == null)
            {
                studentUser = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "Default Student",
                    Role = Role.Student,
                    Gender = Gender.Man,
                    UserName = "student1",
                    Email = "student1@school.local",
                    PhoneNumber = "2222222222",
                    Password = "student123",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                context.Users.Add(studentUser);
                await context.SaveChangesAsync();
            }

            if (!await context.Students.AnyAsync(s => s.UserId == studentUser.Id))
            {
                context.Students.Add(new Student
                {
                    Id = Guid.NewGuid(),
                    UserId = studentUser.Id,
                    DateOfBirth = now.AddYears(-20),
                    Gender = studentUser.Gender,
                    CinNumber = "CIN123",
                    PhoneNumber = studentUser.PhoneNumber,
                    SecondPhoneNumber = "3333333333",
                    Address = "Default Address",
                    IsActive = true,
                    EnrollmentDate = now
                });
                await context.SaveChangesAsync();
            }

            if (!await context.Classes.AnyAsync())
            {
                var section = await context.Sections.FirstAsync();
                var teacher = await context.Teachers.FirstAsync();

                context.Classes.Add(new Classe
                {
                    Id = Guid.NewGuid(),
                    Name = "Class A",
                    Level = "1st Year",
                    Filiere = "Engineering",
                    MaxCapacity = 30,
                    IsArchived = false,
                    AcademicYear = "2026/2027",
                    CreatedAt = now,
                    UpdatedAt = now,
                    SectionId = section.Id,
                    ReferentTeacherId = teacher.Id
                });
                await context.SaveChangesAsync();
            }

            if (!await context.ClassSubjects.AnyAsync())
            {
                var classe = await context.Classes.FirstAsync();
                var subject = await context.Subjects.FirstAsync();
                var teacher = await context.Teachers.FirstAsync();

                context.ClassSubjects.Add(new ClassSubject
                {
                    ClassId = classe.Id,
                    SubjectId = subject.Id,
                    TeacherId = teacher.Id,
                    AssignedAt = now
                });
                await context.SaveChangesAsync();
            }

            if (!await context.Sessions.AnyAsync())
            {
                var room = await context.Rooms.FirstAsync();
                var subject = await context.Subjects.FirstAsync();
                var teacher = await context.Teachers.FirstAsync();

                context.Sessions.Add(new Session
                {
                    DayOfWeek = "Monday",
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(10, 0),
                    Status = SessionStatus.PLANNED,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now,
                    RoomId = room.Id,
                    SubjectId = subject.Id,
                    TeacherId = teacher.Id
                });
                await context.SaveChangesAsync();
            }

            if (!await context.SessionAuditLogs.AnyAsync())
            {
                var session = await context.Sessions.FirstAsync();

                context.SessionAuditLogs.Add(new SessionAuditLog
                {
                    SessionId = session.Id,
                    Action = "CREATED",
                    Details = "Seeded initial session",
                    ActorId = adminUser.Id,
                    CreatedAt = now
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
