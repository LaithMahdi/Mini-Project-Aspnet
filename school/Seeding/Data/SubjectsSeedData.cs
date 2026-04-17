using school.Models;

namespace school.Seeding.Data
{
    public static class SubjectsSeedData
    {
        public static IEnumerable<Subject> Build(DateTime now)
        {
            return new List<Subject>
            {
                new Subject { Name = "Programmation Java", Code = "JAVA", Coefficient = 2, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Structures de Donnees", Code = "SD", Coefficient = 3, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Systemes d'Exploitation", Code = "OS", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Deep Learning", Code = "DL", Coefficient = 3, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Big Data", Code = "BD", Coefficient = 3, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Administration Reseaux", Code = "ADMIN-NET", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Securite Informatique", Code = "SEC", Coefficient = 3, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Comptabilite Generale", Code = "COMPTA-G", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Comptabilite Analytique", Code = "COMPTA-A", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Finance d'Entreprise", Code = "FIN", Coefficient = 3, HoursPerWeek = 4, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Marketing Fondamental", Code = "MKT-F", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Marketing Digital", Code = "MKT-D", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Management des Entreprises", Code = "MGMT", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Droit des Affaires", Code = "LAW", Coefficient = 2, HoursPerWeek = 2, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Techniques de Communication", Code = "COMM", Coefficient = 1, HoursPerWeek = 2, Type = SubjectType.TD, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Statistiques", Code = "STAT", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Subject { Name = "Probabilites", Code = "PROBA", Coefficient = 2, HoursPerWeek = 3, Type = SubjectType.COURSE, IsActive = true, CreatedAt = now, UpdatedAt = now }
            };
        }
    }
}
