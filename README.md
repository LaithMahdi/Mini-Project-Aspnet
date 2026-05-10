![Project Logo](assets/logo.svg)

# School Management System — Professional Edition

An ASP.NET Core MVC application for managing students, teachers, classes, sessions and notifications with a focus on stability, usability, and productivity.

## Key Features

- Notifications: real-time alerts and an admin notification center.
- Advanced Class Enrollment: capacity checks, transfers, and live search.
- Session Scheduling: conflict detection and fixed time slots.
- Role-based dashboards for Admins, Teachers and Students.
- Robust validation on client and server sides.

## Requirements

- .NET 10 SDK or later
- SQL Server (LocalDB, Express, or full instance)
- Node.js (for Tailwind CSS build tasks)
- Git (recommended)

## Quickstart

1. Clone the repository:

```bash
git clone https://github.com/LaithMahdi/mini-project-aspnet.git
cd mini-project-aspnet
```

2. Restore dependencies and run database migrations:

```bash
dotnet restore
dotnet ef database update
```

3. Build and run the app:

```bash
dotnet build
dotnet run
```

4. (Optional) Build Tailwind CSS assets:

```bash
npm install
npm run tailwind:build
```

The site will be available at https://localhost:5001 by default.

## Project Structure (high level)

```
school/
├── Controllers/        # MVC controllers
├── Models/             # Entity models and enums
├── Views/              # Razor views and partials
├── Services/           # Business logic services
├── Migrations/         # EF Core migrations
├── wwwroot/            # Static assets
└── README.md           # This file
```

## Technologies

- ASP.NET Core 10
- C# 14
- Entity Framework Core
- Razor Views & ViewComponents
- Tailwind CSS (frontend styling)

## Development Checklist

- [x] Notifications system
- [x] Advanced enrollment with capacity checks
- [x] Session scheduling and conflict detection
- [x] Data seeding for demo/testing
- [ ] PDF export for schedules (planned)
- [ ] Internal messaging between teachers (planned)

## Author

Laith Mahdi — https://github.com/LaithMahdi

---

**License & Contact**

This project is provided as-is for demonstration and educational purposes. For questions or commercial licensing, contact the author via the GitHub profile above.

Footer: Built with care • © 2026 Laith Mahdi

```bash
# Common commands
dotnet restore
dotnet build
dotnet run
dotnet ef database update
npm install
npm run tailwind:build
```
