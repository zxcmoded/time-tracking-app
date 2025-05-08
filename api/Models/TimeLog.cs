using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tracking_app.Models
{
    public class TimeLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
