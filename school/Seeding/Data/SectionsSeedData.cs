using school.Models;

namespace school.Seeding.Data
{
    public static class SectionsSeedData
    {
        public static IEnumerable<Section> Build(DateTime now)
        {
            return new List<Section>
            {
                new Section { Name = "Licence Informatique", Code = "L-INFO", Description = "Licence en Informatique (Programmation, bases, systemes)", CreatedAt = now, UpdatedAt = now },
                new Section { Name = "Licence Reseaux et Telecommunications", Code = "L-RT", Description = "Licence en reseaux et communication", CreatedAt = now, UpdatedAt = now },
                new Section { Name = "Licence Mathematiques Appliquees", Code = "L-MA", Description = "Licence en mathematiques pour ingenieurs", CreatedAt = now, UpdatedAt = now },
                new Section { Name = "Master Genie Logiciel", Code = "M-GL", Description = "Master en architecture logicielle et systemes complexes", CreatedAt = now, UpdatedAt = now },
                new Section { Name = "Master Data Science & IA", Code = "M-DSAI", Description = "Master en Big Data, IA et Machine Learning", CreatedAt = now, UpdatedAt = now },
                new Section { Name = "Master Cybersecurite", Code = "M-SEC", Description = "Master en securite informatique et reseaux", CreatedAt = now, UpdatedAt = now },
                new Section { Name = "Cycle Ingenieur Informatique", Code = "ING-INFO", Description = "Cycle ingenieur en informatique", CreatedAt = now, UpdatedAt = now },
                new Section { Name = "Cycle Ingenieur Reseaux", Code = "ING-RT", Description = "Cycle ingenieur en reseaux et systemes", CreatedAt = now, UpdatedAt = now },
                new Section { Name = "Cycle Ingenieur Data Science", Code = "ING-DS", Description = "Cycle ingenieur specialise en data science", CreatedAt = now, UpdatedAt = now }
            };
        }
    }
}
