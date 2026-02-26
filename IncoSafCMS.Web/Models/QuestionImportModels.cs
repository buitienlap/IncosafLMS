using System.Collections.Generic;

namespace IncosafCMS.Web.Models
{
    /// <summary>
    /// DTO for importing a single question with its answers from JSON or Excel.
    /// </summary>
    public class QuestionImportDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        /// <summary>
        /// Match against CourseCategory.Code to resolve FK.
        /// </summary>
        public string CourseCategoryCode { get; set; }
        /// <summary>
        /// VeryEasy | Easy | Medium | Hard | VeryHard
        /// </summary>
        public string Difficulty { get; set; }
        public decimal Points { get; set; } = 1m;
        public string Explanation { get; set; }
        public string Tags { get; set; }
        public List<AnswerImportDto> Answers { get; set; } = new List<AnswerImportDto>();
    }

    public class AnswerImportDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; }
    }

    public class ImportResultDto
    {
        public int Imported { get; set; }
        public int Skipped { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
