using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IncosafCMS.Core.DomainModels.Identity;

namespace IncosafCMS.Core.DomainModels
{
    public enum ExamAssignmentStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }

    /// <summary>
    /// Represents an exam session assigned to a specific user by an admin.
    /// Each row = one user must take one exam.
    /// </summary>
    [Table("ExamAssignment")]
    public class ExamAssignment : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        /// <summary>
        /// Title of the exam (e.g. "Kiểm tra ATVSLĐ đợt 3/2025")
        /// </summary>
        [Required]
        [StringLength(500)]
        public string ExamTitle { get; set; }

        /// <summary>
        /// Optional description or instructions.
        /// </summary>
        [StringLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// Number of questions in this exam.
        /// </summary>
        public int QuestionCount { get; set; }

        /// <summary>
        /// Time limit in minutes. Null = no limit.
        /// </summary>
        public int? TimeLimitMinutes { get; set; }

        /// <summary>
        /// Deadline to complete the exam. Null = no deadline.
        /// </summary>
        public DateTime? Deadline { get; set; }

        public ExamAssignmentStatus Status { get; set; } = ExamAssignmentStatus.Pending;

        /// <summary>
        /// Score as percentage (0-100). Null if not yet completed.
        /// </summary>
        public decimal? Score { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Category codes (comma-separated) used to generate the exam.
        /// </summary>
        [StringLength(500)]
        public string CategoryCodes { get; set; }
    }
}
