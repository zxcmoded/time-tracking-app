using System.ComponentModel.DataAnnotations;

namespace tracking_app.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); // Auto-generate UUID

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    }
}