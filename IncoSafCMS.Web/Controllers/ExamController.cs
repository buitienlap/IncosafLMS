using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Identity;
using IncosafCMS.Web.Models;

namespace IncosafCMS.Web.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IApplicationUserManager userManager;

        public ExamController(IUnitOfWork _uow, IApplicationUserManager _userManager)
        {
            uow = _uow;
            userManager = _userManager;
        }

        // =============================================
        // GET: /Exam/PracticeExam — Main list page
        // =============================================
        public ActionResult PracticeExam()
        {
            var user = userManager.FindByName(User.Identity.Name);
            var exams = uow.Repository<PracticeExam>()
                .FindBy(e => e.UserId == user.Id)
                .OrderByDescending(e => e.StartedAt)
                .ToList();

            var completed = exams.Where(e => e.Status == PracticeExamStatus.Completed).ToList();

            var model = new PracticeExamListViewModel
            {
                Exams = exams.Select(e => new PracticeExamSummaryDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    QuestionCount = e.QuestionCount,
                    CorrectCount = e.CorrectCount,
                    Score = e.Score,
                    Status = e.Status,
                    StartedAt = e.StartedAt,
                    CompletedAt = e.CompletedAt,
                    DurationSeconds = e.DurationSeconds,
                    CurrentQuestionIndex = e.CurrentQuestionIndex,
                    CategoryFilter = e.CategoryFilter,
                    DifficultyFilter = e.DifficultyFilter
                }).ToList(),
                Stats = new PracticeExamStatsDto
                {
                    TotalAttempts = exams.Count,
                    CompletedCount = completed.Count,
                    InProgressCount = exams.Count(e => e.Status == PracticeExamStatus.InProgress),
                    AverageScore = completed.Any() ? completed.Average(e => e.Score ?? 0) : 0,
                    BestScore = completed.Any() ? completed.Max(e => e.Score ?? 0) : 0
                }
            };

            return View(model);
        }

        // =============================================
        // GET: /Exam/Setup — Setup new practice exam
        // =============================================
        public ActionResult Setup()
        {
            var categories = uow.Repository<CourseCategory>().GetAll()
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ToList();

            var questionCounts = uow.Repository<Question>()
                .FindBy(q => q.IsActive && !q.IsDeleted)
                .GroupBy(q => q.CourseCategoryId)
                .ToDictionary(g => g.Key, g => g.Count());

            var model = new PracticeExamSetupViewModel
            {
                Categories = categories.Select(c => new CourseCategoryOptionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    QuestionCount = questionCounts.ContainsKey(c.Id) ? questionCounts[c.Id] : 0
                }).ToList(),
                QuestionCount = 20
            };

            return PartialView("_PracticeSetup", model);
        }

        // =============================================
        // POST: /Exam/StartPractice — Generate and start
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StartPractice(int questionCount = 20, string difficulty = null, string categoryConfigJson = null, string examTitle = null)
        {
            var user = userManager.FindByName(User.Identity.Name);

            if (questionCount < 5) questionCount = 5;
            if (questionCount > 100) questionCount = 100;

            // Parse difficulty filter
            DifficultyLevel? diffLevel = null;
            if (!string.IsNullOrEmpty(difficulty))
            {
                DifficultyLevel parsed;
                if (Enum.TryParse(difficulty, out parsed))
                    diffLevel = parsed;
            }

            // Parse multi-category config: [{ categoryId: 1, percent: 30 }, ...]
            var categoryConfigs = new List<CategoryPercentConfig>();
            if (!string.IsNullOrEmpty(categoryConfigJson))
            {
                try
                {
                    categoryConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CategoryPercentConfig>>(categoryConfigJson)
                        ?? new List<CategoryPercentConfig>();
                }
                catch { }
            }

            var rng = new Random();
            var selectedIds = new List<int>();

            if (categoryConfigs.Any())
            {
                // --- Multi-category with percentage allocation ---
                int totalPct = categoryConfigs.Sum(c => c.Percent);

                foreach (var cfg in categoryConfigs)
                {
                    int countForCat = (int)Math.Round((double)cfg.Percent / 100 * questionCount);
                    if (countForCat < 1) countForCat = 1;

                    var catQuery = uow.Repository<Question>()
                        .FindBy(q => q.IsActive && !q.IsDeleted && q.CourseCategoryId == cfg.CategoryId)
                        .AsQueryable();

                    if (diffLevel.HasValue)
                        catQuery = catQuery.Where(q => q.Difficulty == diffLevel.Value);

                    var catIds = catQuery.Select(q => q.Id).ToList();
                    var picked = catIds.OrderBy(x => rng.Next()).Take(countForCat).ToList();
                    selectedIds.AddRange(picked);
                }

                // If total percent < 100, fill remainder from other categories
                if (totalPct < 100)
                {
                    int remainder = questionCount - selectedIds.Count;
                    if (remainder > 0)
                    {
                        var usedCatIds = categoryConfigs.Select(c => c.CategoryId).ToList();
                        var alreadyPicked = new HashSet<int>(selectedIds);

                        var restQuery = uow.Repository<Question>()
                            .FindBy(q => q.IsActive && !q.IsDeleted && !usedCatIds.Contains(q.CourseCategoryId))
                            .AsQueryable();

                        if (diffLevel.HasValue)
                            restQuery = restQuery.Where(q => q.Difficulty == diffLevel.Value);

                        var restIds = restQuery.Select(q => q.Id).Where(id => !alreadyPicked.Contains(id)).ToList();
                        selectedIds.AddRange(restIds.OrderBy(x => rng.Next()).Take(remainder));
                    }
                }

                // Shuffle final list
                selectedIds = selectedIds.OrderBy(x => rng.Next()).ToList();

                // Trim to exact count if over
                if (selectedIds.Count > questionCount)
                    selectedIds = selectedIds.Take(questionCount).ToList();
            }
            else
            {
                // --- All categories (random) ---
                var query = uow.Repository<Question>()
                    .FindBy(q => q.IsActive && !q.IsDeleted)
                    .AsQueryable();

                if (diffLevel.HasValue)
                    query = query.Where(q => q.Difficulty == diffLevel.Value);

                var allIds = query.Select(q => q.Id).ToList();
                selectedIds = allIds.OrderBy(x => rng.Next()).Take(questionCount).ToList();
            }

            if (!selectedIds.Any())
            {
                return Json(new { success = false, message = "Không tìm thấy câu hỏi phù hợp với bộ lọc." });
            }

            // Build title
            string titleParts = "";
            if (categoryConfigs.Any())
            {
                var catIds = categoryConfigs.Select(c => c.CategoryId).ToList();
                var catNames = uow.Repository<CourseCategory>().GetAll()
                    .Where(c => catIds.Contains(c.Id))
                    .ToDictionary(c => c.Id, c => c.Name);

                var parts = categoryConfigs
                    .Where(c => catNames.ContainsKey(c.CategoryId))
                    .Select(c => catNames[c.CategoryId] + " " + c.Percent + "%");
                titleParts = string.Join(", ", parts);
            }
            var autoTitle = "Thi thử"
                + (string.IsNullOrEmpty(titleParts) ? "" : " - " + titleParts)
                + " (" + selectedIds.Count + " câu)";
            var title = !string.IsNullOrWhiteSpace(examTitle) ? examTitle.Trim() : autoTitle;

            var exam = new PracticeExam
            {
                UserId = user.Id,
                Title = title,
                QuestionCount = selectedIds.Count,
                Status = PracticeExamStatus.InProgress,
                StartedAt = DateTime.UtcNow,
                CurrentQuestionIndex = 0,
                QuestionIds = string.Join(",", selectedIds),
                CategoryFilter = categoryConfigJson,
                DifficultyFilter = difficulty
            };

            uow.Repository<PracticeExam>().Insert(exam);
            uow.SaveChanges();

            // Read max answer options from config
            int maxOptions = 4;
            var maxOptSetting = ConfigurationManager.AppSettings["MaxAnswerOptionsPerQuestion"];
            if (!string.IsNullOrEmpty(maxOptSetting))
            {
                int parsed;
                if (int.TryParse(maxOptSetting, out parsed) && parsed >= 2)
                    maxOptions = parsed;
            }

            // Load all questions with their answers to pre-shuffle
            var questionsWithAnswers = uow.Repository<Question>()
                .GetAllIncluding(q => q.Answers)
                .Where(q => selectedIds.Contains(q.Id))
                .ToDictionary(q => q.Id);

            // Create answer slots with pre-shuffled answer selection
            for (int i = 0; i < selectedIds.Count; i++)
            {
                Question question;
                questionsWithAnswers.TryGetValue(selectedIds[i], out question);

                string shuffledIds = "";
                if (question != null && question.Answers != null && question.Answers.Any())
                {
                    var allAnswers = question.Answers.ToList();
                    List<Answer> selectedAnswers;

                    if (allAnswers.Count <= maxOptions)
                    {
                        // Use all answers, just shuffle the order
                        selectedAnswers = allAnswers.OrderBy(x => rng.Next()).ToList();
                    }
                    else
                    {
                        // More answers than allowed: pick random subset, always include correct answer(s)
                        var correctAnswers = allAnswers.Where(a => a.IsCorrect).ToList();
                        var incorrectAnswers = allAnswers.Where(a => !a.IsCorrect).ToList();

                        var picked = new List<Answer>(correctAnswers);

                        // Fill remaining slots with random incorrect answers
                        int remaining = maxOptions - picked.Count;
                        if (remaining > 0)
                        {
                            picked.AddRange(incorrectAnswers.OrderBy(x => rng.Next()).Take(remaining));
                        }

                        // If we somehow have more correct answers than maxOptions, trim
                        if (picked.Count > maxOptions)
                            picked = picked.Take(maxOptions).ToList();

                        // Shuffle the final selection
                        selectedAnswers = picked.OrderBy(x => rng.Next()).ToList();
                    }

                    shuffledIds = string.Join(",", selectedAnswers.Select(a => a.Id));
                }

                var ans = new PracticeExamAnswer
                {
                    PracticeExamId = exam.Id,
                    QuestionId = selectedIds[i],
                    QuestionOrder = i,
                    ShuffledAnswerIds = shuffledIds
                };
                uow.Repository<PracticeExamAnswer>().Insert(ans);
            }
            uow.SaveChanges();

            // Log activity
            var activity = new ActivityLog
            {
                UserId = user.Id,
                Type = ActivityType.Practice,
                Title = title,
                Description = "Bắt đầu thi thử: " + title,
                RelatedId = exam.Id.ToString(),
                Timestamp = DateTime.UtcNow
            };
            uow.Repository<ActivityLog>().Insert(activity);
            uow.SaveChanges();

            return Json(new { success = true, examId = exam.Id });
        }

        // =============================================
        // GET: /Exam/Take/5 — Take or resume exam
        // =============================================
        public ActionResult Take(int id)
        {
            var user = userManager.FindByName(User.Identity.Name);
            var exam = uow.Repository<PracticeExam>().GetSingle(id);

            if (exam == null || exam.UserId != user.Id)
                return HttpNotFound();

            if (exam.Status == PracticeExamStatus.Completed)
                return RedirectToAction("Review", new { id = exam.Id });

            var questionIds = exam.QuestionIds.Split(',').Select(int.Parse).ToList();
            var currentQId = questionIds[exam.CurrentQuestionIndex];

            var question = uow.Repository<Question>()
                .GetSingleIncluding(currentQId, q => q.Answers);

            var existingAnswer = uow.Repository<PracticeExamAnswer>()
                .FindBy(a => a.PracticeExamId == exam.Id && a.QuestionId == currentQId)
                .FirstOrDefault();

            // Progress tracking
            var allExamAnswers = uow.Repository<PracticeExamAnswer>()
                .FindBy(a => a.PracticeExamId == exam.Id)
                .OrderBy(a => a.QuestionOrder)
                .ToList();

            var labels = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var questionAnswers = question.Answers.ToList();

            // Use pre-shuffled answer IDs if available, otherwise fall back to all answers
            List<Answer> orderedAnswers;
            if (!string.IsNullOrEmpty(existingAnswer?.ShuffledAnswerIds))
            {
                var shuffledIds = existingAnswer.ShuffledAnswerIds.Split(',').Select(int.Parse).ToList();
                var answerDict = questionAnswers.ToDictionary(a => a.Id);
                orderedAnswers = shuffledIds
                    .Where(id => answerDict.ContainsKey(id))
                    .Select(id => answerDict[id])
                    .ToList();
            }
            else
            {
                orderedAnswers = questionAnswers.OrderBy(a => a.Order).ToList();
            }

            var model = new PracticeExamTakeViewModel
            {
                ExamId = exam.Id,
                Title = exam.Title,
                TotalQuestions = exam.QuestionCount,
                CurrentIndex = exam.CurrentQuestionIndex,
                StartedAt = exam.StartedAt,
                CurrentQuestion = new PracticeExamQuestionDto
                {
                    QuestionId = question.Id,
                    Title = question.Title,
                    Text = question.Text,
                    Explanation = question.Explanation,
                    SelectedAnswerId = existingAnswer?.SelectedAnswerId,
                    IsCorrect = existingAnswer?.IsCorrect,
                    Options = orderedAnswers.Select((a, idx) => new PracticeExamAnswerOptionDto
                    {
                        AnswerId = a.Id,
                        Text = a.Text,
                        Label = idx < labels.Length ? labels[idx] : (idx + 1).ToString(),
                        IsCorrect = a.IsCorrect,
                        Explanation = a.Explanation
                    }).ToList()
                },
                Progress = allExamAnswers.Select((a, idx) => new QuestionProgressDto
                {
                    Index = idx,
                    IsAnswered = a.SelectedAnswerId.HasValue,
                    IsCurrent = idx == exam.CurrentQuestionIndex,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            return View(model);
        }

        // =============================================
        // POST: /Exam/SubmitAnswer — Save answer and navigate
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAnswer(int examId, int questionId, int? selectedAnswerId, string action)
        {
            var user = userManager.FindByName(User.Identity.Name);
            var exam = uow.Repository<PracticeExam>().GetSingle(examId);

            if (exam == null || exam.UserId != user.Id || exam.Status == PracticeExamStatus.Completed)
                return Json(new { success = false });

            // Save the answer
            bool? answerIsCorrect = null;
            if (selectedAnswerId.HasValue)
            {
                var examAnswer = uow.Repository<PracticeExamAnswer>()
                    .FindBy(a => a.PracticeExamId == examId && a.QuestionId == questionId)
                    .FirstOrDefault();

                if (examAnswer != null)
                {
                    var correctAnswer = uow.Repository<Answer>()
                        .FindBy(a => a.QuestionId == questionId && a.IsCorrect)
                        .FirstOrDefault();

                    examAnswer.SelectedAnswerId = selectedAnswerId.Value;
                    examAnswer.IsCorrect = correctAnswer != null && correctAnswer.Id == selectedAnswerId.Value;
                    examAnswer.AnsweredAt = DateTime.UtcNow;
                    answerIsCorrect = examAnswer.IsCorrect;
                    uow.Repository<PracticeExamAnswer>().Update(examAnswer);
                    uow.SaveChanges();
                }
            }

            // If action is "grade" — just return grading result, don't navigate
            if (action == "grade")
            {
                return Json(new { success = true, isCorrect = answerIsCorrect });
            }

            // Navigate
            var questionIds = exam.QuestionIds.Split(',').Select(int.Parse).ToList();
            int newIndex = exam.CurrentQuestionIndex;

            if (action == "next" && exam.CurrentQuestionIndex < questionIds.Count - 1)
            {
                newIndex = exam.CurrentQuestionIndex + 1;
            }
            else if (action == "prev" && exam.CurrentQuestionIndex > 0)
            {
                newIndex = exam.CurrentQuestionIndex - 1;
            }
            else if (action == "goto" && selectedAnswerId == null)
            {
                // selectedAnswerId is reused as gotoIndex in this case — handle via separate param
            }
            else if (action == "finish")
            {
                return FinishExam(exam);
            }

            exam.CurrentQuestionIndex = newIndex;
            uow.Repository<PracticeExam>().Update(exam);
            uow.SaveChanges();

            return Json(new { success = true, redirectUrl = Url.Action("Take", new { id = examId }) });
        }

        // =============================================
        // POST: /Exam/GoToQuestion — Jump to specific question
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GoToQuestion(int examId, int index)
        {
            var user = userManager.FindByName(User.Identity.Name);
            var exam = uow.Repository<PracticeExam>().GetSingle(examId);

            if (exam == null || exam.UserId != user.Id || exam.Status == PracticeExamStatus.Completed)
                return Json(new { success = false });

            if (index >= 0 && index < exam.QuestionCount)
            {
                exam.CurrentQuestionIndex = index;
                uow.Repository<PracticeExam>().Update(exam);
                uow.SaveChanges();
            }

            return Json(new { success = true, redirectUrl = Url.Action("Take", new { id = examId }) });
        }

        // =============================================
        // POST: /Exam/FinishPractice — Complete the exam
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinishPractice(int examId)
        {
            var user = userManager.FindByName(User.Identity.Name);
            var exam = uow.Repository<PracticeExam>().GetSingle(examId);

            if (exam == null || exam.UserId != user.Id)
                return Json(new { success = false });

            if (exam.Status == PracticeExamStatus.Completed)
                return Json(new { success = true, redirectUrl = Url.Action("Review", new { id = examId }) });

            var result = FinishExam(exam);
            return result;
        }

        private JsonResult FinishExam(PracticeExam exam)
        {
            var answers = uow.Repository<PracticeExamAnswer>()
                .FindBy(a => a.PracticeExamId == exam.Id);

            var correct = answers.Count(a => a.IsCorrect == true);
            var total = answers.Count;

            exam.CorrectCount = correct;
            exam.Score = total > 0 ? Math.Round((decimal)correct / total * 100, 1) : 0;
            exam.Status = PracticeExamStatus.Completed;
            exam.CompletedAt = DateTime.UtcNow;
            exam.DurationSeconds = (int)(DateTime.UtcNow - exam.StartedAt).TotalSeconds;

            uow.Repository<PracticeExam>().Update(exam);
            uow.SaveChanges();

            // Log activity
            var user = userManager.FindByName(User.Identity.Name);
            var activity = new ActivityLog
            {
                UserId = user.Id,
                Type = ActivityType.Practice,
                Title = "Hoàn thành: " + exam.Title,
                Description = string.Format("Kết quả: {0}/{1} ({2}%)", correct, total, exam.Score),
                RelatedId = exam.Id.ToString(),
                Timestamp = DateTime.UtcNow
            };
            uow.Repository<ActivityLog>().Insert(activity);
            uow.SaveChanges();

            return Json(new { success = true, redirectUrl = Url.Action("Review", new { id = exam.Id }) });
        }

        // =============================================
        // GET: /Exam/Review/5 — Review completed exam
        // =============================================
        public ActionResult Review(int id)
        {
            var user = userManager.FindByName(User.Identity.Name);
            var exam = uow.Repository<PracticeExam>().GetSingle(id);

            if (exam == null || exam.UserId != user.Id)
                return HttpNotFound();

            var examAnswers = uow.Repository<PracticeExamAnswer>()
                .FindBy(a => a.PracticeExamId == exam.Id)
                .OrderBy(a => a.QuestionOrder)
                .ToList();

            var questionIds = examAnswers.Select(a => a.QuestionId).ToList();
            var questions = uow.Repository<Question>()
                .GetAllIncluding(q => q.Answers)
                .Where(q => questionIds.Contains(q.Id))
                .ToList()
                .ToDictionary(q => q.Id);

            var labels = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            string durationDisplay = "--";
            if (exam.DurationSeconds.HasValue && exam.DurationSeconds.Value > 0)
            {
                var ts = TimeSpan.FromSeconds(exam.DurationSeconds.Value);
                durationDisplay = ts.TotalHours >= 1
                    ? string.Format("{0}h {1:D2}m", (int)ts.TotalHours, ts.Minutes)
                    : string.Format("{0}m {1:D2}s", ts.Minutes, ts.Seconds);
            }

            var model = new PracticeExamReviewViewModel
            {
                ExamId = exam.Id,
                Title = exam.Title,
                QuestionCount = exam.QuestionCount,
                CorrectCount = exam.CorrectCount,
                Score = exam.Score,
                StartedAt = exam.StartedAt,
                CompletedAt = exam.CompletedAt,
                DurationDisplay = durationDisplay,
                Questions = examAnswers.Select((ea, idx) =>
                {
                    Question q;
                    questions.TryGetValue(ea.QuestionId, out q);
                    var allAns = q != null ? q.Answers.ToList() : new List<Answer>();

                    // Use pre-shuffled answer IDs if available
                    List<Answer> orderedAns;
                    if (!string.IsNullOrEmpty(ea.ShuffledAnswerIds))
                    {
                        var shuffledIds = ea.ShuffledAnswerIds.Split(',').Select(int.Parse).ToList();
                        var ansDict = allAns.ToDictionary(a => a.Id);
                        orderedAns = shuffledIds
                            .Where(id => ansDict.ContainsKey(id))
                            .Select(id => ansDict[id])
                            .ToList();
                    }
                    else
                    {
                        orderedAns = allAns.OrderBy(a => a.Order).ToList();
                    }
                    return new ReviewQuestionDto
                    {
                        Index = idx + 1,
                        QuestionId = ea.QuestionId,
                        QuestionText = q?.Text ?? "",
                        Explanation = q?.Explanation,
                        SelectedAnswerId = ea.SelectedAnswerId,
                        IsCorrect = ea.IsCorrect == true,
                        Answers = orderedAns.Select((a, ai) => new ReviewAnswerDto
                        {
                            AnswerId = a.Id,
                            Text = a.Text,
                            Label = ai < labels.Length ? labels[ai] : (ai + 1).ToString(),
                            IsCorrect = a.IsCorrect,
                            IsSelected = ea.SelectedAnswerId.HasValue && ea.SelectedAnswerId.Value == a.Id,
                            Explanation = a.Explanation
                        }).ToList()
                    };
                }).ToList()
            };

            return View(model);
        }

        // =============================================
        // POST: /Exam/DeletePractice — Delete a practice exam
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePractice(int id)
        {
            var user = userManager.FindByName(User.Identity.Name);
            var exam = uow.Repository<PracticeExam>().GetSingle(id);

            if (exam == null || exam.UserId != user.Id)
                return Json(new { success = false, message = "Không tìm thấy bài thi." });

            // Delete answers first
            var answers = uow.Repository<PracticeExamAnswer>().FindBy(a => a.PracticeExamId == id);
            foreach (var ans in answers)
            {
                uow.Repository<PracticeExamAnswer>().Delete(ans);
            }
            uow.Repository<PracticeExam>().Delete(exam);
            uow.SaveChanges();

            return Json(new { success = true });
        }

        // =============================================
        // POST: /Exam/ResetPractice — Clear answers and restart same exam
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPractice(int examId)
        {
            var user = userManager.FindByName(User.Identity.Name);
            var exam = uow.Repository<PracticeExam>().GetSingle(examId);

            if (exam == null || exam.UserId != user.Id)
                return Json(new { success = false, message = "Không tìm thấy bài thi." });

            // Read max answer options from config
            int maxOptions = 4;
            var maxOptSetting = ConfigurationManager.AppSettings["MaxAnswerOptionsPerQuestion"];
            if (!string.IsNullOrEmpty(maxOptSetting))
            {
                int parsedOpt;
                if (int.TryParse(maxOptSetting, out parsedOpt) && parsedOpt >= 2)
                    maxOptions = parsedOpt;
            }

            // Clear all answers and re-shuffle answer options
            var answers = uow.Repository<PracticeExamAnswer>()
                .FindBy(a => a.PracticeExamId == examId)
                .ToList();

            var questionIds = answers.Select(a => a.QuestionId).Distinct().ToList();
            var questionsDict = uow.Repository<Question>()
                .GetAllIncluding(q => q.Answers)
                .Where(q => questionIds.Contains(q.Id))
                .ToDictionary(q => q.Id);

            var rng = new Random();

            foreach (var ans in answers)
            {
                ans.SelectedAnswerId = null;
                ans.IsCorrect = null;

                // Re-shuffle answer options
                Question q;
                if (questionsDict.TryGetValue(ans.QuestionId, out q) && q.Answers != null && q.Answers.Any())
                {
                    var allAns = q.Answers.ToList();
                    List<Answer> selected;

                    if (allAns.Count <= maxOptions)
                    {
                        selected = allAns.OrderBy(x => rng.Next()).ToList();
                    }
                    else
                    {
                        var correct = allAns.Where(a => a.IsCorrect).ToList();
                        var incorrect = allAns.Where(a => !a.IsCorrect).ToList();
                        var picked = new List<Answer>(correct);
                        int rem = maxOptions - picked.Count;
                        if (rem > 0)
                            picked.AddRange(incorrect.OrderBy(x => rng.Next()).Take(rem));
                        if (picked.Count > maxOptions)
                            picked = picked.Take(maxOptions).ToList();
                        selected = picked.OrderBy(x => rng.Next()).ToList();
                    }

                    ans.ShuffledAnswerIds = string.Join(",", selected.Select(a => a.Id));
                }

                uow.Repository<PracticeExamAnswer>().Update(ans);
            }

            // Reset exam state
            exam.CurrentQuestionIndex = 0;
            exam.StartedAt = DateTime.UtcNow;
            exam.CompletedAt = null;
            exam.DurationSeconds = null;
            exam.CorrectCount = 0;
            exam.Score = null;
            exam.Status = PracticeExamStatus.InProgress;
            uow.Repository<PracticeExam>().Update(exam);
            uow.SaveChanges();

            return Json(new { success = true, redirectUrl = Url.Action("Take", new { id = exam.Id }) });
        }

        // =============================================
        // POST: /Exam/RetakePractice — Retake same config
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RetakePractice(int id)
        {
            var user = userManager.FindByName(User.Identity.Name);
            var original = uow.Repository<PracticeExam>().GetSingle(id);

            if (original == null || original.UserId != user.Id)
                return Json(new { success = false });

            return StartPractice(original.QuestionCount, original.DifficultyFilter, original.CategoryFilter);
        }

        // =============================================
        // GET: /Exam/Start — Start assigned exam (stub)
        // =============================================
        public ActionResult Start(int assignmentId)
        {
            // Placeholder for formal exam — redirect to practice for now
            return RedirectToAction("PracticeExam");
        }
    }
}
