namespace school.Seeding.Data
{
    public static class StudentsSeedData
    {
        public static IReadOnlyList<string> FirstNames { get; } =
        [
            "Ali", "Mohamed", "Yasmine", "Sara", "Omar", "Hiba", "Karim", "Fatima", "Noor", "Hassan",
            "Leila", "Amina", "Khaled", "Zaineb", "Rami", "Nadia", "Tarek", "Layla", "Samir", "Dina"
        ];

        public static IReadOnlyList<string> LastNames { get; } =
        [
            "Ben Ali", "Trabelsi", "Jebali", "Gharbi", "Khlifi", "Mansouri", "Hadj", "Ounissi", "Bouslama", "Hadji",
            "Hamza", "Mansour", "Salah", "Mejri", "Lahbib"
        ];
    }
}
