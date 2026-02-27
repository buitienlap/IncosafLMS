using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncosafCMS.Core.DomainModels
{
    /// <summary>
    /// Content type of a course lesson.
    /// </summary>
    public enum CourseContentType
    {
        Html = 0,
        Pdf = 1,
        PowerPoint = 2,
        Video = 3
    }

    [Table("Course")]
    public class Course : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        [Required]
        public int CourseCategoryId { get; set; }

        [ForeignKey("CourseCategoryId")]
        public virtual CourseCategory CourseCategory { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // ---- Content fields ----

        /// <summary>
        /// Type of content: Html, Pdf, PowerPoint, Video.
        /// </summary>
        public CourseContentType ContentType { get; set; } = CourseContentType.Html;

        /// <summary>
        /// HTML body content (used when ContentType = Html).
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string ContentBody { get; set; }

        /// <summary>
        /// URL to external resource: PDF file path, PowerPoint file path, or video streaming URL.
        /// For uploaded files this is a relative path like ~/App_Data/CourseFiles/xxx.pdf
        /// For video this is a full URL like https://stream.example.com/video/123
        /// </summary>
        [StringLength(1000)]
        public string ContentUrl { get; set; }

        /// <summary>
        /// Estimated duration in minutes.
        /// </summary>
        public int? DurationMinutes { get; set; }

        // ---- Statistics (denormalized for display performance) ----

        /// <summary>
        /// Number of distinct users who joined this course.
        /// </summary>
        public int ParticipantCount { get; set; }

        /// <summary>
        /// Total number of learning sessions (views).
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Cached average rating (1-5). Null if no ratings yet.
        /// </summary>
        [Column(TypeName = "float")]
        public double? AverageRating { get; set; }

        /// <summary>
        /// Number of ratings submitted.
        /// </summary>
        public int RatingCount { get; set; }

        // Navigation
        public virtual System.Collections.Generic.ICollection<CourseRating> Ratings { get; set; }
    }
}
