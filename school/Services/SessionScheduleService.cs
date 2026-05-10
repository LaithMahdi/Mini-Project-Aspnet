namespace school.Services
{
    /// <summary>
    /// Defines the daily session schedule with fixed time slots and breaks.
    /// </summary>
    public class SessionSlot
    {
        public int SlotNumber { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public interface ISessionScheduleService
    {
        List<SessionSlot> GetAvailableSlots();
        SessionSlot? GetSlotByNumber(int slotNumber);
        string GetSlotDisplayName(TimeOnly startTime, TimeOnly endTime);
    }

    public class SessionScheduleService : ISessionScheduleService
    {
        /// <summary>
        /// Fixed daily session schedule:
        /// Session 1: 08:30 - 10:00 (90 minutes)
        /// Break: 10:00 - 10:10
        /// Session 2: 10:10 - 11:40 (90 minutes)
        /// Break: 11:40 - 12:10 (lunch break)
        /// Session 3: 12:10 - 13:40 (90 minutes)
        /// Break: 13:40 - 13:50
        /// Session 4: 13:50 - 15:20 (90 minutes)
        /// </summary>
        private static readonly List<SessionSlot> FixedSlots = new()
        {
            new SessionSlot
            {
                SlotNumber = 1,
                StartTime = TimeOnly.Parse("08:30"),
                EndTime = TimeOnly.Parse("10:00"),
                DisplayName = "Session 1 (08:30 - 10:00)"
            },
            new SessionSlot
            {
                SlotNumber = 2,
                StartTime = TimeOnly.Parse("10:10"),
                EndTime = TimeOnly.Parse("11:40"),
                DisplayName = "Session 2 (10:10 - 11:40)"
            },
            new SessionSlot
            {
                SlotNumber = 3,
                StartTime = TimeOnly.Parse("12:10"),
                EndTime = TimeOnly.Parse("13:40"),
                DisplayName = "Session 3 (12:10 - 13:40)"
            },
            new SessionSlot
            {
                SlotNumber = 4,
                StartTime = TimeOnly.Parse("13:50"),
                EndTime = TimeOnly.Parse("15:20"),
                DisplayName = "Session 4 (13:50 - 15:20)"
            }
        };

        public List<SessionSlot> GetAvailableSlots()
        {
            return FixedSlots;
        }

        public SessionSlot? GetSlotByNumber(int slotNumber)
        {
            return FixedSlots.FirstOrDefault(s => s.SlotNumber == slotNumber);
        }

        public string GetSlotDisplayName(TimeOnly startTime, TimeOnly endTime)
        {
            var slot = FixedSlots.FirstOrDefault(s => s.StartTime == startTime && s.EndTime == endTime);
            return slot?.DisplayName ?? $"{startTime:HH\\:mm} - {endTime:HH\\:mm}";
        }
    }
}
