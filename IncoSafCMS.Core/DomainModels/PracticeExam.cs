using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IncosafCMS.Core.DomainModels.Identity;

namespace IncosafCMS.Core.DomainModels
{
    public enum PracticeExamStatus
    {
        InProgress = 0,
        Completed = 1
    }

    /// <summary>
    /// A practice exam session taken by a user.
    /// Contains metadata about the attempt and links to individual answers.
    /// </summary>
    [Table("PracticeExam")]
    public class PracticeExam : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        public int QuestionCount { get; set; }

        public int CorrectCount { get; set; }

        /// <summary>
        /// Score as percentage (0-100). Null while InProgress.
        /// </summary>
        public decimal? Score { get; set; }

        public PracticeExamStatus Status { get; set; } = PracticeExamStatus.InProgress;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Duration in seconds from start to completion.
        /// </summary>
        public int? DurationSeconds { get; set; }

        /// <summary>
        /// Comma-separated category IDs used to generate this practice exam.
        /// </summary>
        [StringLength(500)]
        public string CategoryFilter { get; set; }

        /// <summary>
        /// Difficulty filter used, if any.
        /// </summary>
        [StringLength(100)]
        public string DifficultyFilter { get; set; }

        /// <summary>
        /// Index of the current question being answered (0-based). Used for resume.
        /// </summary>
        public int CurrentQuestionIndex { get; set; }

        /// <summary>
        /// Ordered list of question IDs in this exam (comma-separated).
        /// Preserves the random order generated at start.
        /// </summary>
        public string QuestionIds { get; set; }

        public virtual ICollection<PracticeExamAnswer> Answers { get; set; } = new List<PracticeExamAnswer>();
    }

    /// <summary>
    /// Records the user's answer to a specific question in a practice exam.
    /// </summary>
    [Table("PracticeExamAnswer")]
    public class PracticeExamAnswer : BaseEntity
    {
        [Required]
        public int PracticeExamId { get; set; }

        [ForeignKey("PracticeExamId")]
        public virtual PracticeExam PracticeExam { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        /// <summary>
        /// The answer ID selected by the user. Null if not yet answered.
        /// </summary>
        public int? SelectedAnswerId { get; set; }

        [ForeignKey("SelectedAnswerId")]
        public virtual Answer SelectedAnswer { get; set; }

        public bool? IsCorrect { get; set; }

        /// <summary>
        /// Order of this question within the exam (0-based).
        /// </summary>
        public int QuestionOrder { get; set; }

        /// <summary>
        /// Comma-separated, pre-shuffled answer IDs shown to the user for this question.
        /// When a question has more answers than the configured max, only a random subset
        /// (always including the correct answer) is stored here in display order.
        /// </summary>
        public string ShuffledAnswerIds { get; set; }

        public DateTime? AnsweredAt { get; set; }
    }
}
