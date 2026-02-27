using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    /// <summary>
    /// User-facing controller for browsing and learning courses.
    /// Admin CRUD lives in CoursesController.
    /// </summary>
    [Authorize]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationUserManager _userManager;

        public CourseController(IUnitOfWork uow, IApplicationUserManager userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        // GET: Course/Browse — browse and search all active courses
        public ActionResult Browse(string search, int? categoryId, string sort, string contentType)
        {
            var query = _uow.Repository<Course>()
                .GetAllIncluding(c => c.CourseCategory)
                .Where(c => c.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(c => c.Title.ToLower().Contains(term)
                    || (c.Description != null && c.Description.ToLower().Contains(term))
                    || (c.Code != null && c.Code.ToLower().Contains(term)));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(c => c.CourseCategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                CourseContentType ct;
                if (Enum.TryParse(contentType, out ct))
                {
                    query = query.Where(c => c.ContentType == ct);
                }
            }

            switch (sort)
            {
                case "title":
                    query = query.OrderBy(c => c.Title);
                    break;
                case "oldest":
                    query = query.OrderBy(c => c.CreatedAt);
                    break;
                case "popular":
                    query = query.OrderByDescending(c => c.ParticipantCount);
                    break;
                case "rating":
                    query = query.OrderByDescending(c => c.AverageRating ?? 0);
                    break;
                default: // "newest"
                    query = query.OrderByDescending(c => c.CreatedAt);
                    break;
            }

            var courses = query.ToList();

            var categories = _uow.Repository<CourseCategory>()
                .FindBy(cc => cc.IsActive)
                .OrderBy(cc => cc.SortOrder)
                .ThenBy(cc => cc.Name)
                .ToList();

            // Current user's ratings for these courses
            var user = _userManager.FindByName(User.Identity.Name);
            var userRatings = new Dictionary<int, int>();
            if (user != null)
            {
                userRatings = _uow.Repository<CourseRating>()
                    .FindBy(r => r.UserId == user.Id)
                    .ToDictionary(r => r.CourseId, r => r.Stars);
            }

            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Sort = sort ?? "newest";
            ViewBag.ContentType = contentType;
            ViewBag.Categories = categories;
            ViewBag.UserRatings = userRatings;
            ViewBag.TotalCourseCount = _uow.Repository<Course>().FindBy(c => c.IsActive).Count();

            return View(courses);
        }

        // GET: Course/Detail/5 — returns partial for popup on Home page
        public ActionResult Detail(int id)
        {
            var course = _uow.Repository<Course>().GetSingleIncluding(id, c => c.CourseCategory);
            if (course == null || !course.IsActive) return HttpNotFound();
            return PartialView("_DetailPopup", course);
        }

        // GET: Course/Learn/5 — actual learning page
        public ActionResult Learn(int id)
        {
            var course = _uow.Repository<Course>().GetSingleIncluding(id, c => c.CourseCategory);
            if (course == null || !course.IsActive) return HttpNotFound();

            // Record activity
            var user = _userManager.FindByName(User.Identity.Name);
            var activity = new ActivityLog
            {
                UserId = user.Id,
                Type = ActivityType.Learning,
                Title = course.Title,
                Description = "Tham gia khóa học: " + course.Title,
                Timestamp = DateTime.UtcNow,
                RelatedId = course.Id.ToString(),
                Duration = 0
            };
            _uow.Repository<ActivityLog>().Insert(activity);

            // Update course statistics
            course.ViewCount += 1;
            var courseIdStr = course.Id.ToString();
            var distinctParticipants = _uow.Repository<ActivityLog>()
                .FindBy(a => a.Type == ActivityType.Learning && a.RelatedId == courseIdStr)
                .Select(a => a.UserId)
                .Distinct()
                .Count();
            // +1 for current user in case this is their first visit (not yet committed)
            var alreadyJoined = _uow.Repository<ActivityLog>()
                .FindBy(a => a.Type == ActivityType.Learning && a.RelatedId == courseIdStr && a.UserId == user.Id)
                .Any();
            course.ParticipantCount = alreadyJoined ? distinctParticipants : distinctParticipants + 1;

            _uow.Repository<Course>().Update(course);
            _uow.SaveChanges();

            // Pass user's rating for this course
            var userRating = _uow.Repository<CourseRating>()
                .FindBy(r => r.CourseId == id && r.UserId == user.Id)
                .FirstOrDefault();
            ViewBag.UserRating = userRating != null ? userRating.Stars : 0;

            return View(course);
        }

        // POST: Course/RateCourse — AJAX rating
        [HttpPost]
        public ActionResult RateCourse(int courseId, int stars)
        {
            if (stars < 1 || stars > 5)
                return Json(new { success = false, message = "Đánh giá phải từ 1 đến 5 sao." });

            var user = _userManager.FindByName(User.Identity.Name);
            if (user == null)
                return Json(new { success = false, message = "Không tìm thấy người dùng." });

            var existing = _uow.Repository<CourseRating>()
                .FindBy(r => r.CourseId == courseId && r.UserId == user.Id)
                .FirstOrDefault();

            if (existing != null)
            {
                existing.Stars = stars;
                existing.CreatedAt = DateTime.UtcNow;
                _uow.Repository<CourseRating>().Update(existing);
            }
            else
            {
                var rating = new CourseRating
                {
                    CourseId = courseId,
                    UserId = user.Id,
                    Stars = stars,
                    CreatedAt = DateTime.UtcNow
                };
                _uow.Repository<CourseRating>().Insert(rating);
            }
            _uow.SaveChanges();

            // Recalculate course average
            var allRatings = _uow.Repository<CourseRating>()
                .FindBy(r => r.CourseId == courseId)
                .ToList();

            var course = _uow.Repository<Course>().GetSingle(courseId);
            if (course != null)
            {
                course.RatingCount = allRatings.Count;
                course.AverageRating = allRatings.Count > 0 ? allRatings.Average(r => r.Stars) : (double?)null;
                _uow.Repository<Course>().Update(course);
                _uow.SaveChanges();
            }

            return Json(new
            {
                success = true,
                averageRating = course?.AverageRating ?? 0,
                ratingCount = course?.RatingCount ?? 0,
                userStars = stars
            });
        }

        // POST: Course/RecordProgress — AJAX heartbeat to update duration
        [HttpPost]
        public ActionResult RecordProgress(int courseId, int elapsedSeconds)
        {
            var user = _userManager.FindByName(User.Identity.Name);

            // Find the latest learning activity for this course+user today
            var today = DateTime.UtcNow.Date;
            var activity = _uow.Repository<ActivityLog>()
                .FindBy(a => a.UserId == user.Id
                    && a.Type == ActivityType.Learning
                    && a.RelatedId == courseId.ToString()
                    && a.Timestamp >= today)
                .OrderByDescending(a => a.Timestamp)
                .FirstOrDefault();

            if (activity != null)
            {
                activity.Duration = elapsedSeconds;
                _uow.Repository<ActivityLog>().Update(activity);
                _uow.SaveChanges();
            }

            return Json(new { success = true });
        }

        // GET: Course/ServeFile/5 — serve PDF/PPTX inline for viewer embedding
        public ActionResult ServeFile(int id)
        {
            var course = _uow.Repository<Course>().GetAll().FirstOrDefault(c => c.Id == id);
            if (course == null || string.IsNullOrEmpty(course.ContentUrl))
                return HttpNotFound();

            var path = course.ContentUrl;
            if (path.StartsWith("~"))
            {
                path = Server.MapPath(path);
            }

            if (!System.IO.File.Exists(path))
                return HttpNotFound("File not found.");

            var contentType = "application/octet-stream";
            var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            switch (ext)
            {
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".pptx":
                    contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case ".ppt":
                    contentType = "application/vnd.ms-powerpoint";
                    break;
            }

            // Serve inline so Office Online / Google Docs viewers can fetch it
            var fileName = System.IO.Path.GetFileName(path);
            Response.Headers["Content-Disposition"] = "inline; filename=\"" + fileName + "\"";
            Response.Headers["Access-Control-Allow-Origin"] = "*";

            return File(path, contentType);
        }
    }
}
