using System.ComponentModel.DataAnnotations;

namespace mini_project_aspnet.Models
{
    public class Section : BaseEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string code { get; set; }

        public bool isActive { get; set; } = true;

    }
}
