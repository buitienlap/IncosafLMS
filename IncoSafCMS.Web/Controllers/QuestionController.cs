using ClosedXML.Excel;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionController : Controller
    {
        private readonly IUnitOfWork uow;
        public QuestionController(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        // GET: QuestionManage
        public ActionResult Index()
        {
            var list = uow.Repository<Question>().GetAllIncluding(q => q.CourseCategory);
            return View(list);
        }

        // Returns question list partial for DevExpress GridView callback
        public ActionResult QuestionGridViewPartial()
        {
            var list = uow.Repository<Question>().GetAllIncluding(q => q.CourseCategory);
            return PartialView("_QuestionListPartial", list);
        }

        // GET: show create form (partial)
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CourseCategories = new SelectList(uow.Repository<CourseCategory>().GetAll(), "Id", "Name");
            return PartialView("_QuestionEdit", new Question());
        }

        // POST: create question + answers via AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question model, List<Answer> Answers, int? CorrectAnswerIndex)
        {
            ModelState.Remove("Answers");
            if (ModelState.IsValid)
            {
                if (Answers != null)
                {
                    for (int i = 0; i < Answers.Count; i++)
                    {
                        Answers[i].IsCorrect = (CorrectAnswerIndex.HasValue && CorrectAnswerIndex.Value == i);
                        Answers[i].Order = i + 1;
                    }
                    model.Answers = Answers;
                }
                uow.Repository<Question>().Insert(model);
                uow.SaveChanges();
                return Json(new { success = true });
            }
            ViewBag.CourseCategories = new SelectList(uow.Repository<CourseCategory>().GetAll(), "Id", "Name", model.CourseCategoryId);
            return PartialView("_QuestionEdit", model);
        }

        // GET: edit form (partial) — load with answers
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var q = uow.Repository<Question>().GetSingleIncluding(id, x => x.Answers);
            if (q == null) return HttpNotFound();
            ViewBag.CourseCategories = new SelectList(uow.Repository<CourseCategory>().GetAll(), "Id", "Name", q.CourseCategoryId);
            return PartialView("_QuestionEdit", q);
        }

        // POST: edit question + answers via AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question model, List<Answer> Answers, int? CorrectAnswerIndex)
        {
            ModelState.Remove("Answers");
            if (ModelState.IsValid)
            {
                // Clear navigation property to avoid referential integrity violation on Attach
                // (model-bound Answers have QuestionId = 0 which mismatches model.Id)
                model.Answers = new List<Answer>();

                // Update question fields
                uow.Repository<Question>().Update(model);

                // Sync answers: delete old, insert new
                var answerRepo = uow.Repository<Answer>();
                var existingAnswers = answerRepo.FindBy(a => a.QuestionId == model.Id);
                foreach (var old in existingAnswers)
                {
                    answerRepo.Delete(old);
                }

                if (Answers != null)
                {
                    for (int i = 0; i < Answers.Count; i++)
                    {
                        var ans = Answers[i];
                        ans.QuestionId = model.Id;
                        ans.IsCorrect = (CorrectAnswerIndex.HasValue && CorrectAnswerIndex.Value == i);
                        ans.Order = i + 1;
                        ans.Id = 0; // force insert
                        answerRepo.Insert(ans);
                    }
                }

                uow.SaveChanges();
                return Json(new { success = true });
            }
            ViewBag.CourseCategories = new SelectList(uow.Repository<CourseCategory>().GetAll(), "Id", "Name", model.CourseCategoryId);
            return PartialView("_QuestionEdit", model);
        }

        // POST: delete question via AJAX
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var answerRepo = uow.Repository<Answer>();
            var answers = answerRepo.FindBy(a => a.QuestionId == id);
            foreach (var a in answers)
            {
                answerRepo.Delete(a);
            }

            var repo = uow.Repository<Question>();
            var q = repo.GetSingle(id);
            if (q == null) return Json(new { success = false, message = "Not found" });
            repo.Delete(q);
            uow.SaveChanges();
            return Json(new { success = true });
        }

        // GET: import form (partial)
        [HttpGet]
        public ActionResult Import()
        {
            return PartialView("_QuestionImport");
        }

        // POST: import questions from JSON or Excel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(HttpPostedFileBase file)
        {
            var result = new ImportResultDto();

            if (file == null || file.ContentLength == 0)
            {
                result.Errors.Add("Vui lòng chọn file để import.");
                return Json(result);
            }

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            List<QuestionImportDto> dtos;

            try
            {
                if (ext == ".json")
                {
                    using (var reader = new StreamReader(file.InputStream))
                    {
                        var json = reader.ReadToEnd();
                        dtos = JsonConvert.DeserializeObject<List<QuestionImportDto>>(json);
                    }
                }
                else if (ext == ".xlsx")
                {
                    dtos = ParseExcel(file.InputStream);
                }
                else
                {
                    result.Errors.Add("Chỉ hỗ trợ file .json hoặc .xlsx.");
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add("Lỗi đọc file: " + ex.Message);
                return Json(result);
            }

            if (dtos == null || dtos.Count == 0)
            {
                result.Errors.Add("File không chứa dữ liệu câu hỏi nào.");
                return Json(result);
            }

            // Cache category lookup
            var categories = uow.Repository<CourseCategory>().GetAll()
                .ToDictionary(c => (c.Code ?? "").Trim().ToLowerInvariant(), c => c.Id);

            var questionRepo = uow.Repository<Question>();
            var answerRepo = uow.Repository<Answer>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                var rowLabel = $"Câu hỏi #{i + 1}";

                // Validate required fields
                if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Text))
                {
                    result.Errors.Add($"{rowLabel}: Thiếu Title hoặc Text.");
                    result.Skipped++;
                    continue;
                }

                // Resolve category
                var catKey = (dto.CourseCategoryCode ?? "").Trim().ToLowerInvariant();
                if (!categories.TryGetValue(catKey, out int catId))
                {
                    result.Errors.Add($"{rowLabel}: Không tìm thấy thể loại có mã \"{dto.CourseCategoryCode}\".");
                    result.Skipped++;
                    continue;
                }

                // Parse difficulty
                if (!Enum.TryParse(dto.Difficulty, true, out DifficultyLevel difficulty))
                {
                    difficulty = DifficultyLevel.Medium;
                }

                var question = new Question
                {
                    Title = dto.Title.Trim(),
                    Text = dto.Text.Trim(),
                    CourseCategoryId = catId,
                    Difficulty = difficulty,
                    Points = dto.Points > 0 ? dto.Points : 1m,
                    Explanation = dto.Explanation?.Trim(),
                    Tags = dto.Tags?.Trim(),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                if (dto.Answers != null && dto.Answers.Count > 0)
                {
                    for (int j = 0; j < dto.Answers.Count; j++)
                    {
                        var a = dto.Answers[j];
                        if (string.IsNullOrWhiteSpace(a.Text)) continue;
                        question.Answers.Add(new Answer
                        {
                            Text = a.Text.Trim(),
                            IsCorrect = a.IsCorrect,
                            Order = j + 1,
                            Explanation = a.Explanation?.Trim(),
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                questionRepo.Insert(question);
                result.Imported++;
            }

            if (result.Imported > 0)
            {
                uow.SaveChanges();
            }

            return Json(result);
        }

        // GET: list of CourseCategoryCode values in the database
        public ActionResult DownloadCategoryCodes()
        {
            var categories = uow.Repository<CourseCategory>().GetAll()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Code)
                .Select(c => new { c.Code, c.Name })
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("CourseCategoryCode");

                ws.Cell(1, 1).Value = "Code";
                ws.Cell(1, 2).Value = "Tên thể loại";
                ws.Range("A1:B1").Style.Font.Bold = true;
                ws.Range("A1:B1").Style.Fill.BackgroundColor = XLColor.FromHtml("#1ab394");
                ws.Range("A1:B1").Style.Font.FontColor = XLColor.White;

                for (int i = 0; i < categories.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = categories[i].Code;
                    ws.Cell(i + 2, 2).Value = categories[i].Name;
                }

                ws.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "danh_sach_course_category.xlsx");
            }
        }

        // GET: list of Difficulty enum values
        public ActionResult DownloadDifficultyLevels()
        {
            var levels = Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>()
                .Select(d => new { Value = d.ToString(), NumericValue = (int)d })
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("DifficultyLevel");

                ws.Cell(1, 1).Value = "Giá trị (dùng khi import)";
                ws.Cell(1, 2).Value = "Mã số";
                ws.Range("A1:B1").Style.Font.Bold = true;
                ws.Range("A1:B1").Style.Fill.BackgroundColor = XLColor.FromHtml("#1ab394");
                ws.Range("A1:B1").Style.Font.FontColor = XLColor.White;

                for (int i = 0; i < levels.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = levels[i].Value;
                    ws.Cell(i + 2, 2).Value = levels[i].NumericValue;
                }

                ws.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "danh_sach_difficulty.xlsx");
            }
        }

        // GET: download JSON template
        public ActionResult DownloadJsonTemplate()
        {
            var templatePath = Server.MapPath("~/App_Data/Templates/questions_template.json");
            if (!System.IO.File.Exists(templatePath))
                return HttpNotFound();
            return File(templatePath, "application/json", "questions_template.json");
        }

        // GET: download Excel template (generated dynamically)
        public ActionResult DownloadExcelTemplate()
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("CauHoi");

                // Headers
                var headers = new[]
                {
                    "STT", "Title", "Text", "CourseCategoryCode", "Difficulty",
                    "Points", "Explanation", "Tags",
                    "AnswerText", "IsCorrect", "AnswerExplanation"
                };

                for (int c = 0; c < headers.Length; c++)
                {
                    var cell = ws.Cell(1, c + 1);
                    cell.Value = headers[c];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1ab394");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Sample data — Question 1 with 4 answers
                var rows = new object[][]
                {
                    new object[] { 1, "Câu hỏi mẫu 1", "Thủ đô của Việt Nam là gì?", "CAT01", "Easy", 1, "Hà Nội là thủ đô", "địa lý", "Hà Nội", true, "" },
                    new object[] { 1, "", "", "", "", "", "", "", "Hồ Chí Minh", false, "" },
                    new object[] { 1, "", "", "", "", "", "", "", "Đà Nẵng", false, "" },
                    new object[] { 1, "", "", "", "", "", "", "", "Huế", false, "" },
                    new object[] { 2, "Câu hỏi mẫu 2", "Ngôn ngữ lập trình chính của .NET?", "CAT01", "Medium", 2, "C# là ngôn ngữ chính", "lập trình", "C#", true, "" },
                    new object[] { 2, "", "", "", "", "", "", "", "Java", false, "" },
                    new object[] { 2, "", "", "", "", "", "", "", "Python", false, "" },
                    new object[] { 2, "", "", "", "", "", "", "", "Ruby", false, "" },
                };

                for (int r = 0; r < rows.Length; r++)
                {
                    for (int c = 0; c < rows[r].Length; c++)
                    {
                        ws.Cell(r + 2, c + 1).Value = rows[r][c]?.ToString() ?? "";
                    }
                }

                // Add instruction sheet
                var wsGuide = workbook.Worksheets.Add("HuongDan");
                wsGuide.Cell("A1").Value = "HƯỚNG DẪN IMPORT CÂU HỎI";
                wsGuide.Cell("A1").Style.Font.Bold = true;
                wsGuide.Cell("A1").Style.Font.FontSize = 14;

                wsGuide.Cell("A3").Value = "Cột STT:";
                wsGuide.Cell("B3").Value = "Số thứ tự câu hỏi. Các dòng cùng STT thuộc cùng 1 câu hỏi. Dòng đầu tiên của mỗi STT chứa thông tin câu hỏi.";

                wsGuide.Cell("A4").Value = "Cột Title:";
                wsGuide.Cell("B4").Value = "Tiêu đề câu hỏi (bắt buộc ở dòng đầu tiên của mỗi câu).";

                wsGuide.Cell("A5").Value = "Cột Text:";
                wsGuide.Cell("B5").Value = "Nội dung đầy đủ câu hỏi (bắt buộc).";

                wsGuide.Cell("A6").Value = "Cột CourseCategoryCode:";
                wsGuide.Cell("B6").Value = "Mã thể loại. Phải khớp với mã đã có trong hệ thống.";

                wsGuide.Cell("A7").Value = "Cột Difficulty:";
                wsGuide.Cell("B7").Value = "Mức độ: VeryEasy, Easy, Medium, Hard, VeryHard";

                wsGuide.Cell("A8").Value = "Cột Points:";
                wsGuide.Cell("B8").Value = "Điểm (mặc định 1).";

                wsGuide.Cell("A9").Value = "Cột AnswerText:";
                wsGuide.Cell("B9").Value = "Nội dung đáp án (bắt buộc). Mỗi dòng là 1 đáp án.";

                wsGuide.Cell("A10").Value = "Cột IsCorrect:";
                wsGuide.Cell("B10").Value = "true hoặc false. Đánh dấu đáp án đúng.";

                wsGuide.Range("A3:A10").Style.Font.Bold = true;
                wsGuide.Column("A").Width = 25;
                wsGuide.Column("B").Width = 80;

                // Auto-fit columns on data sheet
                ws.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "questions_template.xlsx");
            }
        }

        /// <summary>
        /// Parse Excel flat format: rows grouped by STT column.
        /// First row of each group contains question info; all rows have one answer each.
        /// </summary>
        private List<QuestionImportDto> ParseExcel(Stream stream)
        {
            var result = new List<QuestionImportDto>();

            using (var workbook = new XLWorkbook(stream))
            {
                var ws = workbook.Worksheet(1);
                var rows = ws.RangeUsed()?.RowsUsed().Skip(1); // skip header
                if (rows == null) return result;

                QuestionImportDto current = null;
                string lastStt = null;

                foreach (var row in rows)
                {
                    var stt = row.Cell(1).GetString().Trim();
                    var title = row.Cell(2).GetString().Trim();
                    var text = row.Cell(3).GetString().Trim();
                    var catCode = row.Cell(4).GetString().Trim();
                    var difficulty = row.Cell(5).GetString().Trim();
                    var pointsStr = row.Cell(6).GetString().Trim();
                    var explanation = row.Cell(7).GetString().Trim();
                    var tags = row.Cell(8).GetString().Trim();
                    var answerText = row.Cell(9).GetString().Trim();
                    var isCorrectStr = row.Cell(10).GetString().Trim();
                    var answerExplanation = row.Cell(11).GetString().Trim();

                    // New question group
                    if (!string.IsNullOrEmpty(stt) && stt != lastStt)
                    {
                        current = new QuestionImportDto
                        {
                            Title = title,
                            Text = text,
                            CourseCategoryCode = catCode,
                            Difficulty = difficulty,
                            Explanation = explanation,
                            Tags = tags
                        };

                        if (decimal.TryParse(pointsStr, out decimal pts) && pts > 0)
                            current.Points = pts;

                        result.Add(current);
                        lastStt = stt;
                    }

                    // Add answer (every row has an answer)
                    if (current != null && !string.IsNullOrEmpty(answerText))
                    {
                        bool.TryParse(isCorrectStr, out bool isCorrect);
                        current.Answers.Add(new AnswerImportDto
                        {
                            Text = answerText,
                            IsCorrect = isCorrect,
                            Explanation = answerExplanation
                        });
                    }
                }
            }

            return result;
        }
    }
}
