using school.Models;

namespace school.Seeding.Data
{
    public sealed record TeacherSeedItem(string Name, Gender Gender, string Specialization);

    public static class TeachersSeedData
    {
        public static IEnumerable<TeacherSeedItem> Get()
        {
            return new List<TeacherSeedItem>
            {
                new("Ahmed Ben Ali", Gender.Man, "Programming & Algorithms"),
                new("Sami Trabelsi", Gender.Man, "Database Systems"),
                new("Nour Gharbi", Gender.Man, "Web Development"),
                new("Hichem Mansouri", Gender.Man, "Network Architecture"),
                new("Amira Khlifi", Gender.Woman, "Data Science & AI"),
                new("Youssef Jaziri", Gender.Man, "Cybersecurity"),
                new("Fatima Ben Hadj", Gender.Woman, "Operating Systems"),
                new("Mohamed Ounissi", Gender.Man, "Software Engineering"),
                new("Leila Bouslama", Gender.Woman, "Mathematics"),
                new("Karim Hadji", Gender.Man, "Mobile Development"),
                new("Souad Hamza", Gender.Woman, "Cloud Computing"),
                new("Zaineb Mansour", Gender.Woman, "Machine Learning")
            };
        }
    }
}
