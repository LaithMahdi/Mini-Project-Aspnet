using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_project_aspnet.Models
{
    public class Teacher : User
    {
        [MaxLength(500)]
        public string avatarUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string specialization { get; set; }

        [Required]
        public Gender gender { get; set; }

        [Required]
        public DateTime hireDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal salary { get; set; }

        public bool isActive { get; set; } = true;
    }
}
