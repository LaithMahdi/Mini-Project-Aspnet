using school.Models;

namespace school.Seeding.Data
{
    public static class RoomsSeedData
    {
        public static IEnumerable<Room> Build(DateTime now)
        {
            return new List<Room>
            {
                new Room { Name = "Room A1", Type = RoomType.TD, Capacity = 40, Building = "Bloc A", Floor = 1, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Whiteboard", CreatedAt = now, UpdatedAt = now },
                new Room { Name = "Room A2", Type = RoomType.TD, Capacity = 40, Building = "Bloc A", Floor = 1, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Whiteboard", CreatedAt = now, UpdatedAt = now },
                new Room { Name = "Lab A1", Type = RoomType.Labo, Capacity = 25, Building = "Bloc A", Floor = 2, HasProjector = false, HasComputers = true, IsAvailable = true, Equipment = "25 Computers, Server", CreatedAt = now, UpdatedAt = now },
                new Room { Name = "Lab A2", Type = RoomType.Labo, Capacity = 25, Building = "Bloc A", Floor = 2, HasProjector = false, HasComputers = true, IsAvailable = true, Equipment = "25 Computers, Server", CreatedAt = now, UpdatedAt = now },
                new Room { Name = "Room B1", Type = RoomType.TP, Capacity = 30, Building = "Bloc B", Floor = 1, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Whiteboard", CreatedAt = now, UpdatedAt = now },
                new Room { Name = "Room B2", Type = RoomType.TP, Capacity = 30, Building = "Bloc B", Floor = 1, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Whiteboard", CreatedAt = now, UpdatedAt = now },
                new Room { Name = "Amphitheater 1", Type = RoomType.AMPHI, Capacity = 100, Building = "Main Hall", Floor = 0, HasProjector = true, HasComputers = false, IsAvailable = true, Equipment = "Projector, Sound System", CreatedAt = now, UpdatedAt = now }
            };
        }
    }
}
