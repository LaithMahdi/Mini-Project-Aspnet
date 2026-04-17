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
                    new Section { Name = "Licence Informatique", Code = "L-INFO", Description = "Licence en Informatique (Programmation, bases, systèmes)", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Licence Réseaux et Télécommunications", Code = "L-RT", Description = "Licence en réseaux et communication", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Licence Mathématiques Appliquées", Code = "L-MA", Description = "Licence en mathématiques pour ingénieurs", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Master Génie Logiciel", Code = "M-GL", Description = "Master en architecture logicielle et systèmes complexes", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Master Data Science & IA", Code = "M-DSAI", Description = "Master en Big Data, IA et Machine Learning", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Master Cybersécurité", Code = "M-SEC", Description = "Master en sécurité informatique et réseaux", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Cycle Ingénieur Informatique", Code = "ING-INFO", Description = "Cycle ingénieur en informatique", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Cycle Ingénieur Réseaux", Code = "ING-RT", Description = "Cycle ingénieur en réseaux et systèmes", CreatedAt = now, UpdatedAt = now },
                    new Section { Name = "Cycle Ingénieur Data Science", Code = "ING-DS", Description = "Cycle ingénieur spécialisé en data science", CreatedAt = now, UpdatedAt = now }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Subjects.AnyAsync())
            {
                context.Subjects.AddRange(
                    new Subject { Name = "Programmation Java", Code = "JAVA", Coefficient = 2, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Structures de Données", Code = "SD", Coefficient = 3, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Systèmes d'Exploitation", Code = "OS", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Deep Learning", Code = "DL", Coefficient = 3, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Big Data", Code = "BD", Coefficient = 3, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Administration Réseaux", Code = "ADMIN-NET", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Sécurité Informatique", Code = "SEC", Coefficient = 3, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Comptabilité Générale", Code = "COMPTA-G", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Comptabilité Analytique", Code = "COMPTA-A", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Finance d'Entreprise", Code = "FIN", Coefficient = 3, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Marketing Fondamental", Code = "MKT-F", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Marketing Digital", Code = "MKT-D", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Management des Entreprises", Code = "MGMT", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Droit des Affaires", Code = "LAW", Coefficient = 2, HoursPerWeek = 2, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Techniques de Communication", Code = "COMM", Coefficient = 1, HoursPerWeek = 2, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Statistiques", Code = "STAT", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                    new Subject { Name = "Probabilités", Code = "PROBA", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now }
                );
                await context.SaveChangesAsync();
            }

            // Seed Rooms
            if (!await context.Rooms.AnyAsync())
            {
                var rooms = new List<Room>
                {
                    new Room { Name = "Room A1", Type = RoomType.TD, Capacity = 40, Building = "Bloc A", Floor = 1, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Whiteboard", CreatedAt = now, UpdatedAt = now },
                    new Room { Name = "Room A2", Type = RoomType.TD, Capacity = 40, Building = "Bloc A", Floor = 1, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Whiteboard", CreatedAt = now, UpdatedAt = now },
                    new Room { Name = "Lab A1", Type = RoomType.Labo, Capacity = 25, Building = "Bloc A", Floor = 2, HasProjector = false, HasComputers = true, IsAvailable = true, Equipment = "25 Computers, Server", CreatedAt = now, UpdatedAt = now },
                    new Room { Name = "Lab A2", Type = RoomType.Labo, Capacity = 25, Building = "Bloc A", Floor = 2, HasProjector = false, HasComputers = true, IsAvailable = true, Equipment = "25 Computers, Server", CreatedAt = now, UpdatedAt = now },
                    new Room { Name = "Room B1", Type = RoomType.TP, Capacity = 30, Building = "Bloc B", Floor = 1, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Whiteboard", CreatedAt = now, UpdatedAt = now },
                    new Room { Name = "Room B2", Type = RoomType.TP, Capacity = 30, Building = "Bloc B", Floor = 1, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Whiteboard", CreatedAt = now, UpdatedAt = now },
                    new Room { Name = "Amphitheater 1", Type = RoomType.AMPHI, Capacity = 100, Building = "Main Hall", Floor = 0, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Sound System", CreatedAt = now, UpdatedAt = now }
                };
                context.Rooms.AddRange(rooms);
                await context.SaveChangesAsync();
            }

            // Seed Teachers with correct genders and specializations (no duplicates)
            {
                var teachersData = new List<(string Name, Gender Gender, string Specialization)>
                {
                    ("Ahmed Ben Ali", Gender.Man, "Programming & Algorithms"),
                    ("Sami Trabelsi", Gender.Man, "Database Systems"),
                    ("Nour Gharbi", Gender.Man, "Web Development"),
                    ("Hichem Mansouri", Gender.Man, "Network Architecture"),
                    ("Amira Khlifi", Gender.Woman, "Data Science & AI"),
                    ("Youssef Jaziri", Gender.Man, "Cybersecurity"),
                    ("Fatima Ben Hadj", Gender.Woman, "Operating Systems"),
                    ("Mohamed Ounissi", Gender.Man, "Software Engineering"),
                    ("Leila Bouslama", Gender.Woman, "Mathematics"),
                    ("Karim Hadji", Gender.Man, "Mobile Development"),
                    ("Souad Hamza", Gender.Woman, "Cloud Computing"),
                    ("Zaineb Mansour", Gender.Woman, "Machine Learning")
                };

                var rnd = new Random();
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
                            Password = "Teacher@123",
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
                var firstNames = new[] { "Ali", "Mohamed", "Yasmine", "Sara", "Omar", "Hiba", "Karim", "Fatima", "Noor", "Hassan", "Leila", "Amina", "Khaled", "Zaineb", "Rami", "Nadia", "Tarek", "Layla", "Samir", "Dina" };
                var lastNames = new[] { "Ben Ali", "Trabelsi", "Jebali", "Gharbi", "Khlifi", "Mansouri", "Hadj", "Ounissi", "Bouslama", "Hadji", "Hamza", "Mansour", "Salah", "Mejri", "Lahbib" };

                var rnd = new Random();
                for (int i = 1; i <= 120; i++)
                {
                    var fname = firstNames[(i - 1) % firstNames.Length];
                    var lname = lastNames[((i - 1) / firstNames.Length) % lastNames.Length];
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
                        Password = "Student@123",
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

                var rnd = new Random();

                foreach (var section in sections)
                {
                    var teacher = teachers[rnd.Next(teachers.Count)];

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

            // Seed ClassSubjects
            if (!await context.ClassSubjects.AnyAsync())
            {
                var classes = await context.Classes.ToListAsync();
                var subjects = await context.Subjects.ToListAsync();
                var teachers = await context.Teachers.ToListAsync();

                var rnd = new Random();

                foreach (var classe in classes)
                {
                    var numSubjects = rnd.Next(4, 7);
                    var selectedSubjects = subjects.OrderBy(x => rnd.Next()).Take(numSubjects);

                    foreach (var subject in selectedSubjects)
                    {
                        var teacher = teachers[rnd.Next(teachers.Count)];

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

                var rnd = new Random();
                var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
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
                        var day = daysOfWeek[rnd.Next(daysOfWeek.Length)];
                        var timeSlot = timeSlots[rnd.Next(timeSlots.Length)];
                        var room = rooms[rnd.Next(rooms.Count)];

                        var session = new Session
                        {
                            DayOfWeek = day,
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
                var rnd = new Random();
                var actions = new[] { "CREATED", "MODIFIED", "CANCELLED" };

                foreach (var session in sessions.Take(20))
                {
                    context.SessionAuditLogs.Add(new SessionAuditLog
                    {
                        SessionId = session.Id,
                        Action = actions[rnd.Next(actions.Length)],
                        Details = $"Session {session.DayOfWeek} {session.StartTime}-{session.EndTime}",
                        ActorId = adminUser.Id,
                        CreatedAt = now
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
