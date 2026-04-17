using Microsoft.EntityFrameworkCore;
using school.Models;
using school.Seeding.Data;

namespace school.Seeding
{
    public static class AppSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var now = DateTime.UtcNow;
            var rnd = new Random();

            var adminSeed = UsersSeedData.Admin;
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == adminSeed.UserName);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = adminSeed.FullName,
                    Role = adminSeed.Role,
                    Gender = adminSeed.Gender,
                    UserName = adminSeed.UserName,
                    Email = adminSeed.Email,
                    PhoneNumber = adminSeed.PhoneNumber,
                    Password = adminSeed.Password,
                    IsActive = adminSeed.IsActive,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }

            if (!await context.Sections.AnyAsync())
            {
                context.Sections.AddRange(SectionsSeedData.Build(now));
                await context.SaveChangesAsync();
            }

            if (!await context.Subjects.AnyAsync())
            {
                context.Subjects.AddRange(SubjectsSeedData.Build(now));
                await context.SaveChangesAsync();
            }

            // Seed Rooms
            if (!await context.Rooms.AnyAsync())
            {
                context.Rooms.AddRange(RoomsSeedData.Build(now));
                await context.SaveChangesAsync();
            }

            // Seed Teachers with correct genders and specializations (no duplicates)
            {
                var teachersData = TeachersSeedData.Get();

                foreach (var (name, gender, specialization) in teachersData)
                {
                    var userName = name.Replace(" ", "").ToLower();
                    var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

                    if (user == null)
                    {
                        user = new User
                        {
                            Id = Guid.NewGuid(),
                            FullName = name,
                            Role = Role.Teacher,
                            Gender = gender,
                            UserName = userName,
                            Email = $"{userName}@university.tn",
                            PhoneNumber = "2" + rnd.Next(10000000, 99999999),
                            Password = UsersSeedData.DefaultTeacherPassword,
                            IsActive = true,
                            CreatedAt = now,
                            UpdatedAt = now
                        };

                        context.Users.Add(user);
                        await context.SaveChangesAsync();
                    }

                    var teacherExists = await context.Teachers.AnyAsync(t => t.UserId == user.Id);
                    if (!teacherExists)
                    {
                        context.Teachers.Add(new Teacher
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            Specialization = specialization,
                            Gender = gender,
                            HireDate = now.AddYears(-rnd.Next(2, 10)),
                            Salary = 2500 + rnd.Next(0, 3000),
                            IsActive = true
                        });
                    }
                }
                await context.SaveChangesAsync();
            }

            // Seed Students with proper birth dates for ID generation (no duplicates)
            {
                var firstNames = StudentsSeedData.FirstNames;
                var lastNames = StudentsSeedData.LastNames;

                for (int i = 1; i <= 120; i++)
                {
                    var fname = firstNames[(i - 1) % firstNames.Count];
                    var lname = lastNames[((i - 1) / firstNames.Count) % lastNames.Count];
                    var baseUserName = $"{fname}{lname}".Replace(" ", "").ToLowerInvariant();

                    var userName = baseUserName;
                    var suffix = 1;
                    while (await context.Users.AnyAsync(u => u.UserName == userName && u.Role != Role.Student))
                    {
                        userName = $"{baseUserName}{suffix}";
                        suffix++;
                    }

                    var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                    if (existingUser != null)
                    {
                        var studentExists = await context.Students.AnyAsync(s => s.UserId == existingUser.Id);
                        if (!studentExists)
                        {
                            var existingYear = now.Year - rnd.Next(18, 26);
                            var existingMonth = rnd.Next(1, 13);
                            var existingDay = rnd.Next(1, DateTime.DaysInMonth(existingYear, existingMonth) + 1);
                            var existingBirthDate = new DateTime(existingYear, existingMonth, existingDay);

                            context.Students.Add(new Student
                            {
                                Id = Guid.NewGuid(),
                                UserId = existingUser.Id,
                                Gender = existingUser.Gender,
                                DateOfBirth = existingBirthDate,
                                CinNumber = $"{rnd.Next(10000000, 99999999)}",
                                PhoneNumber = existingUser.PhoneNumber,
                                SecondPhoneNumber = $"5{rnd.Next(10000000, 99999999)}",
                                Address = $"Street {rnd.Next(1, 50)}, City",
                                IsActive = true,
                                EnrollmentDate = now.AddMonths(-rnd.Next(0, 8))
                            });
                        }
                        continue;
                    }

                    var gender = i % 2 == 0 ? Gender.Man : Gender.Woman;

                    // Generate realistic birth date (18-25 years old)
                    var year = now.Year - rnd.Next(18, 26);
                    var month = rnd.Next(1, 13);
                    var day = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
                    var birthDate = new DateTime(year, month, day);

                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        FullName = $"{fname} {lname}",
                        Role = Role.Student,
                        Gender = gender,
                        UserName = userName,
                        Email = $"{userName}@university.tn",
                        PhoneNumber = "5" + rnd.Next(10000000, 99999999),
                        Password = UsersSeedData.DefaultStudentPassword,
                        IsActive = true,
                        CreatedAt = now,
                        UpdatedAt = now
                    };

                    context.Users.Add(user);
                    await context.SaveChangesAsync();

                    var student = new Student
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Gender = gender,
                        DateOfBirth = birthDate,
                        CinNumber = $"{rnd.Next(10000000, 99999999)}",
                        PhoneNumber = user.PhoneNumber,
                        SecondPhoneNumber = $"5{rnd.Next(10000000, 99999999)}",
                        Address = $"Street {rnd.Next(1, 50)}, City",
                        IsActive = true,
                        EnrollmentDate = now.AddMonths(-rnd.Next(0, 8))
                    };

                    context.Students.Add(student);
                }
                await context.SaveChangesAsync();
            }

            // Seed Classes
            if (!await context.Classes.AnyAsync())
            {
                var sections = await context.Sections.ToListAsync();
                var teachers = await context.Teachers.ToListAsync();
                var teacherIndex = 0;

                foreach (var section in sections)
                {
                    var teacher = teachers[teacherIndex % teachers.Count];
                    teacherIndex++;

                    var classe = new Classe
                    {
                        Id = Guid.NewGuid(),
                        Name = $"{section.Code} - Cohort {now.Year}",
                        Level = section.Code.StartsWith("L") ? "Licence" : section.Code.StartsWith("M") ? "Master" : "Engineer",
                        Filiere = section.Name,
                        MaxCapacity = 30,
                        IsArchived = false,
                        AcademicYear = "2026/2027",
                        CreatedAt = now,
                        UpdatedAt = now,
                        SectionId = section.Id,
                        ReferentTeacherId = teacher.Id
                    };

                    context.Classes.Add(classe);
                }
                await context.SaveChangesAsync();
            }

            // Assign students to classes when not assigned yet.
            {
                var classes = await context.Classes
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                if (classes.Count > 0)
                {
                    var studentsWithoutClass = await context.Students
                        .Where(s => s.ClassId == null)
                        .OrderBy(s => s.EnrollmentDate)
                        .ToListAsync();

                    for (var i = 0; i < studentsWithoutClass.Count; i++)
                    {
                        studentsWithoutClass[i].ClassId = classes[i % classes.Count].Id;
                    }

                    if (studentsWithoutClass.Count > 0)
                    {
                        await context.SaveChangesAsync();
                    }
                }
            }

            // Seed ClassSubjects
            if (!await context.ClassSubjects.AnyAsync())
            {
                var classes = await context.Classes
                    .Include(c => c.ReferentTeacher)
                    .ToListAsync();
                var subjects = await context.Subjects.ToListAsync();
                var teachers = await context.Teachers.ToListAsync();

                foreach (var classe in classes)
                {
                    var numSubjects = rnd.Next(4, 7);
                    var selectedSubjects = subjects.OrderBy(x => rnd.Next()).Take(numSubjects);
                    var isFirstAssignment = true;

                    foreach (var subject in selectedSubjects)
                    {
                        var teacher = isFirstAssignment && classe.ReferentTeacherId.HasValue
                            ? teachers.FirstOrDefault(t => t.Id == classe.ReferentTeacherId.Value) ?? teachers[rnd.Next(teachers.Count)]
                            : teachers[rnd.Next(teachers.Count)];
                        isFirstAssignment = false;

                        context.ClassSubjects.Add(new ClassSubject
                        {
                            ClassId = classe.Id,
                            SubjectId = subject.Id,
                            TeacherId = teacher.Id,
                            AssignedAt = now
                        });
                    }
                }

                await context.SaveChangesAsync();
            }

            // Seed Sessions
            if (!await context.Sessions.AnyAsync())
            {
                var rooms = await context.Rooms.ToListAsync();
                var classSubjects = await context.ClassSubjects
                    .Include(cs => cs.Subject)
                    .Include(cs => cs.Teacher)
                    .ToListAsync();

                var timeSlots = new[] 
                { 
                    (8, 10), (10, 12), (12, 14), (14, 16), (16, 18) 
                };

                foreach (var classSubject in classSubjects.Take(30))
                {
                    if (classSubject.TeacherId == null) continue;

                    var numSessions = classSubject.Subject.HoursPerWeek / 2;

                    for (int i = 0; i < numSessions; i++)
                    {
                        var sessionDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(rnd.Next(0, 21)));
                        var timeSlot = timeSlots[rnd.Next(timeSlots.Length)];
                        var room = rooms[rnd.Next(rooms.Count)];

                        var session = new Session
                        {
                            SessionDate = sessionDate,
                            StartTime = new TimeOnly(timeSlot.Item1, 0),
                            EndTime = new TimeOnly(timeSlot.Item2, 0),
                            Status = SessionStatus.PLANNED,
                            IsActive = true,
                            CreatedAt = now,
                            UpdatedAt = now,
                            RoomId = room.Id,
                            SubjectId = classSubject.SubjectId,
                            TeacherId = classSubject.TeacherId.Value
                        };

                        context.Sessions.Add(session);
                    }
                }

                await context.SaveChangesAsync();
            }

            // Seed SessionAuditLogs
            if (!await context.SessionAuditLogs.AnyAsync())
            {
                var sessions = await context.Sessions.ToListAsync();
                var actions = new[] { "CREATED", "MODIFIED", "CANCELLED" };

                foreach (var session in sessions.Take(20))
                {
                    context.SessionAuditLogs.Add(new SessionAuditLog
                    {
                        SessionId = session.Id,
                        Action = actions[rnd.Next(actions.Length)],
                        Details = $"Session {session.SessionDate:yyyy-MM-dd} {session.StartTime}-{session.EndTime}",
                        ActorId = adminUser.Id,
                        CreatedAt = now
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
