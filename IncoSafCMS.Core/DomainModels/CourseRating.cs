using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncosafCMS.Core.DomainModels
{
    [Table("CourseRating")]
    public class CourseRating : BaseEntity
    {
        [Required]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Rating from 1 to 5 stars.
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int Stars { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
