using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncosafCMS.Core.DomainModels
{
    /// <summary>
    /// Represents a hierarchical category for course/question bank.
    /// Adjacency-list model with materialized path and depth to help hierarchy queries.
    /// </summary>
    [Table("CourseCategory")]
    public class CourseCategory : BaseEntity
    {
        // Id is inherited from BaseEntity

        /// <summary>
        /// Optional parent category id. Null for root nodes.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Short code for the category (unique).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        /// <summary>
        /// Display name of the category.
        /// </summary>
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Optional description.
        /// </summary>
        [StringLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// Materialized path using Ids separated by '/' (e.g. "/1/5/12/").
        /// Useful for fast subtree queries.
        /// </summary>
        [StringLength(2000)]
        public string Path { get; set; }

        /// <summary>
        /// Depth/level in the tree. Root = 0.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Ordering value among siblings.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Soft-delete / enable flag.
        /// </summary>
        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        #region Navigation properties (EF-friendly)
        [ForeignKey("ParentId")]
        public virtual CourseCategory Parent { get; set; }

        public virtual ICollection<CourseCategory> Children { get; set; } = new HashSet<CourseCategory>();
        #endregion
    }
}