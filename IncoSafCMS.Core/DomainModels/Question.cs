// PSEUDOCODE / PLAN (detailed):
// - Define an enum `DifficultyLevel` to represent question difficulty tiers.
// - Create `Question` entity with:
//   - primary key `Id`
//   - `Text` (the question content) and optional `Title`
//   - `CourseCategoryId` FK and navigation `CourseCategory` to link the question to a course category
//   - `Difficulty` using the `DifficultyLevel` enum
//   - `Points` that the question is worth
//   - `Explanation` describing the correct answer(s)
//   - `Tags` for quick filtering/search (comma separated)
//   - lifecycle fields: `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`, `IsActive`, `IsDeleted`
//   - navigation collection `Answers` (one-to-many)
//   - computed, not-mapped property `IsMultipleAnswer` to indicate if more than one correct answer exists
// - Create `Answer` entity with:
//   - primary key `Id`
//   - `QuestionId` FK and navigation `Question`
//   - `Text` for the answer content
//   - `IsCorrect` boolean (supports multiple correct answers if several `IsCorrect==true`)
//   - `Order` or `Label` (for display ordering or A/B/C labels)
//   - optional `Points` (to support partial grading per option) and `Explanation` (optional per-answer)
//   - lifecycle fields similar to `Question`
// - Add data annotations for required fields and relationships to work with EF (Code First).
// - Make navigation properties `virtual` to support lazy loading (if enabled).
// - Ensure sensible defaults and collections are initialized to avoid null refs.

// Implementation starts here:

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncosafCMS.Core.DomainModels
{
    public enum DifficultyLevel
    {
        VeryEasy = 0,
        Easy = 1,
        Medium = 2,
        Hard = 3,
        VeryHard = 4
    }

    /// <summary>
    /// Represents a question in the question bank.
    /// A question belongs to a CourseCategory and can have many Answers.
    /// Supports multiple correct answers and per-answer points for partial scoring.
    /// </summary>
    public class Question : BaseEntity
    {
        /// <summary>
        /// Short title or identifier for admin listings.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// Full text/content of the question.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Foreign key to CourseCategory (question classification).
        /// </summary>
        [Required]
        public int CourseCategoryId { get; set; }

        /// <summary>
        /// Navigation to the category this question belongs to.
        /// </summary>
        public virtual CourseCategory CourseCategory { get; set; }

        /// <summary>
        /// Difficulty level to enable filtering and assignment.
        /// </summary>
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Medium;

        /// <summary>
        /// Total points this question is worth (used if answers don't provide per-answer points).
        /// </summary>
        public decimal Points { get; set; } = 1m;

        /// <summary>
        /// Additional explanation or rationale for the correct answer(s).
        /// </summary>
        public string Explanation { get; set; }

        /// <summary>
        /// Comma-separated tags to help categorize or search questions.
        /// </summary>
        [MaxLength(500)]
        public string Tags { get; set; }

        /// <summary>
        /// Indicates whether the question is active and available for tests.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Soft-delete flag.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        [MaxLength(128)]
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(128)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Collection of answer options for the question.
        /// </summary>
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        /// <summary>
        /// Computed property indicating whether multiple answers are marked correct.
        /// Not mapped to database.
        /// </summary>
        [NotMapped]
        public bool IsMultipleAnswer
        {
            get
            {
                if (Answers == null) return false;
                int correctCount = 0;
                foreach (var a in Answers)
                {
                    if (a?.IsCorrect == true) correctCount++;
                    if (correctCount > 1) return true;
                }
                return false;
            }
        }
    }

    /// <summary>
    /// Represents a single answer/option for a question.
    /// </summary>
    public class Answer : BaseEntity
    {
        /// <summary>
        /// FK to the parent question.
        /// </summary>
        [Required]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        /// <summary>
        /// Text/content of the answer option.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Whether this option is considered correct. Multiple options may be correct.
        /// </summary>
        public bool IsCorrect { get; set; } = false;

        /// <summary>
        /// Display order (1,2,3...) or used to render labels like A/B/C.
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Optional points awarded for selecting this option (supports partial credit).
        /// If null, question.Points is used and scoring logic determines distribution.
        /// </summary>
        public decimal? Points { get; set; }

        /// <summary>
        /// Optional explanation for this particular answer.
        /// </summary>
        public string Explanation { get; set; }

        [MaxLength(128)]
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(128)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}