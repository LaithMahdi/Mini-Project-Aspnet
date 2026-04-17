using school.Models;

namespace school.Seeding.Data
{
    public sealed record UserSeedItem(
        string FullName,
        Role Role,
        Gender? Gender,
        string UserName,
        string Email,
        string PhoneNumber,
        string Password,
        bool IsActive = true);

    public static class UsersSeedData
    {
        public const string DefaultAdminPassword = "admin";
        public const string DefaultTeacherPassword = "Teacher@123";
        public const string DefaultStudentPassword = "Student@123";

        public static UserSeedItem Admin { get; } = new(
            FullName: "System Administrator",
            Role: Role.Admin,
            Gender: Gender.Man,
            UserName: "admin",
            Email: "admin@school.local",
            PhoneNumber: "0000000000",
            Password: DefaultAdminPassword);
    }
}
