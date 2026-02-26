using System;
using System.Collections.Generic;
using IncosafCMS.Core.DomainModels;

namespace IncosafCMS.Web.Models
{
    public class PendingExamDto
    {
        public int Id { get; set; }
        public string ExamTitle { get; set; }
        public int QuestionCount { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public DateTime? Deadline { get; set; }
        public ExamAssignmentStatus Status { get; set; }
    }

    public class HomeDashboardViewModel
    {
        public List<Course> LatestCourses { get; set; } = new List<Course>();
        public List<ActivityLog> RecentActivities { get; set; } = new List<ActivityLog>();
        public List<PendingExamDto> PendingExams { get; set; } = new List<PendingExamDto>();
        public string UserDisplayName { get; set; }
        public int TotalCourses { get; set; }
        public int TotalExams { get; set; }
        public int TotalPractice { get; set; }
        public int TotalActivities { get; set; }
    }
}
