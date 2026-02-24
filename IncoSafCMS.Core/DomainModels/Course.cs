using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncosafCMS.Core.DomainModels
{
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
    }
}
