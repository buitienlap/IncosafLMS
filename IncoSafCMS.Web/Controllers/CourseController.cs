using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Identity;
using System;
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
            _uow.SaveChanges();

            return View(course);
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
