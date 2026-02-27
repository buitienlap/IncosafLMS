using System;
using System.Collections.Generic;
using IncosafCMS.Core.DomainModels;

namespace IncosafCMS.Web.Models
{
    // ---- List page ----

    public class PracticeExamListViewModel
    {
        public List<PracticeExamSummaryDto> Exams { get; set; } = new List<PracticeExamSummaryDto>();
        public PracticeExamStatsDto Stats { get; set; } = new PracticeExamStatsDto();
    }

    public class PracticeExamSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int QuestionCount { get; set; }
        public int CorrectCount { get; set; }
        public decimal? Score { get; set; }
        public PracticeExamStatus Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? DurationSeconds { get; set; }
        public int CurrentQuestionIndex { get; set; }
        public string CategoryFilter { get; set; }
        public string DifficultyFilter { get; set; }

        public string DurationDisplay
        {
            get
            {
                if (!DurationSeconds.HasValue || DurationSeconds.Value == 0) return "--";
                var ts = TimeSpan.FromSeconds(DurationSeconds.Value);
                if (ts.TotalHours >= 1)
                    return string.Format("{0}h {1:D2}m", (int)ts.TotalHours, ts.Minutes);
                return string.Format("{0}m {1:D2}s", ts.Minutes, ts.Seconds);
            }
        }

        public string ScoreDisplay => Score.HasValue ? Score.Value.ToString("0.#") + "%" : "--";

        public string StatusDisplay
        {
            get
            {
                switch (Status)
                {
                    case PracticeExamStatus.InProgress: return "Đang làm";
                    case PracticeExamStatus.Completed: return "Hoàn thành";
                    default: return Status.ToString();
                }
            }
        }

        public string StatusCssClass
        {
            get
            {
                switch (Status)
                {
                    case PracticeExamStatus.InProgress: return "warning";
                    case PracticeExamStatus.Completed: return "success";
                    default: return "default";
                }
            }
        }
    }

    public class PracticeExamStatsDto
    {
        public int TotalAttempts { get; set; }
        public int CompletedCount { get; set; }
        public int InProgressCount { get; set; }
        public decimal AverageScore { get; set; }
        public decimal BestScore { get; set; }
    }

    // ---- Setup (create new exam) ----

    public class PracticeExamSetupViewModel
    {
        public List<CourseCategoryOptionDto> Categories { get; set; } = new List<CourseCategoryOptionDto>();
        public int QuestionCount { get; set; } = 20;
        public int? CategoryId { get; set; }
        public string Difficulty { get; set; }
    }

    public class CourseCategoryOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuestionCount { get; set; }
    }

    /// <summary>
    /// Represents a category + percent pair for multi-category exam generation.
    /// Serialized as JSON: [{ categoryId: 1, percent: 30 }, ...]
    /// </summary>
    public class CategoryPercentConfig
    {
        public int CategoryId { get; set; }
        public int Percent { get; set; }
    }

    // ---- Take exam ----

    public class PracticeExamTakeViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public int TotalQuestions { get; set; }
        public int CurrentIndex { get; set; }
        public DateTime StartedAt { get; set; }

        public PracticeExamQuestionDto CurrentQuestion { get; set; }

        /// <summary>
        /// Track which questions have been answered (index -> true/false).
        /// </summary>
        public List<QuestionProgressDto> Progress { get; set; } = new List<QuestionProgressDto>();
    }

    public class PracticeExamQuestionDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public List<PracticeExamAnswerOptionDto> Options { get; set; } = new List<PracticeExamAnswerOptionDto>();
        public int? SelectedAnswerId { get; set; }
        public bool? IsCorrect { get; set; }
    }

    public class PracticeExamAnswerOptionDto
    {
        public int AnswerId { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; }
    }

    public class QuestionProgressDto
    {
        public int Index { get; set; }
        public bool IsAnswered { get; set; }
        public bool IsCurrent { get; set; }
        public bool? IsCorrect { get; set; }
    }

    // ---- Review ----

    public class PracticeExamReviewViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public int QuestionCount { get; set; }
        public int CorrectCount { get; set; }
        public decimal? Score { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string DurationDisplay { get; set; }

        public List<ReviewQuestionDto> Questions { get; set; } = new List<ReviewQuestionDto>();
    }

    public class ReviewQuestionDto
    {
        public int Index { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string Explanation { get; set; }
        public List<ReviewAnswerDto> Answers { get; set; } = new List<ReviewAnswerDto>();
        public int? SelectedAnswerId { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class ReviewAnswerDto
    {
        public int AnswerId { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
        public string Explanation { get; set; }
    }
}
