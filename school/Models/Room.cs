using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace school.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Room Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Room Name must be between 1 and 100 characters.")]
        [Display(Name = "Room Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Room Type is required.")]
        [Display(Name = "Room Type")]
        public RoomType Type { get; set; }

        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, 500, ErrorMessage = "Capacity must be between 1 and 500.")]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [StringLength(100, ErrorMessage = "Building cannot exceed 100 characters.")]
        [Display(Name = "Building")]
        public string? Building { get; set; }

        [Range(0, 20, ErrorMessage = "Floor must be between 0 and 20.")]
        [Display(Name = "Floor")]
        public int? Floor { get; set; }

        [Display(Name = "Has Projector")]
        public bool HasProjector { get; set; } = false;

        [Display(Name = "Has Computers")]
        public bool HasComputers { get; set; } = false;

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;

        [StringLength(500, ErrorMessage = "Equipment cannot exceed 500 characters.")]
        [Display(Name = "Equipment")]
        public string? Equipment { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
