using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IncosafCMS.Core.DomainModels.Identity;

namespace IncosafCMS.Core.DomainModels
{
    public enum ActivityType
    {
        Learning = 10,
        Exam = 20,
        Practice = 30,
        Other = 99
    }

    public class ActivityLog : BaseEntity
    {
        public ActivityLog()
        {
            Timestamp = DateTime.UtcNow;
        }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual IncosafCMS.Core.DomainModels.Identity.AppUser User { get; set; }

        [Required]
        public ActivityType Type { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        // Duration in seconds
        public int? Duration { get; set; }

        // Optional foreign keys to domain objects (Course, Exam, etc.) stored as json or simple id
        public string RelatedId { get; set; }

        // Additional metadata as JSON
        public string Metadata { get; set; }
    }
}
