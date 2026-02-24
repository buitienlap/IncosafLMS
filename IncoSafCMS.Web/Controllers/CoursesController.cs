using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CoursesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Courses
        public ActionResult Index(string q = null, int? categoryId = null)
        {
            var repo = _uow.Repository<Course>();
            // include CourseCategory to ensure navigation property is available in views
            var query = repo.GetAllIncluding(c => c.CourseCategory).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c => c.Title.Contains(q) || (c.Code != null && c.Code.Contains(q)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(c => c.CourseCategoryId == categoryId.Value);
            }

            var list = query.OrderByDescending(c => c.CreatedAt).ToList();
            ViewBag.Categories = _uow.Repository<CourseCategory>().GetAll().ToList();
            ViewBag.Search = q;
            ViewBag.CategoryId = categoryId;
            return View(list);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int id)
        {
            var repo = _uow.Repository<Course>();
            var course = repo.GetSingleIncluding(id, c => c.CourseCategory);
            if (course == null) return HttpNotFound();
            // If requested via AJAX load partial for modal
            if (Request.IsAjaxRequest())
            {
                // return partial without layout
                return PartialView("_Details", course);
            }
            return View(course);
        }

        // GET: Courses/Create (returns partial for AJAX)
        public ActionResult Create(int? categoryId)
        {
            var model = new Course
            {
                CourseCategoryId = categoryId ?? 0,
                IsActive = true
            };
            ViewBag.Categories = new SelectList(_uow.Repository<CourseCategory>().GetAll(), "Id", "Name", model.CourseCategoryId);
            return PartialView("_CreateEdit", model);
        }

        // POST: Courses/Create
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                ViewBag.Categories = new SelectList(_uow.Repository<CourseCategory>().GetAll(), "Id", "Name", model.CourseCategoryId);
                return PartialView("_CreateEdit", model);
            }

            try
            {
                model.CreatedAt = DateTime.UtcNow;
                var repo = _uow.Repository<Course>();
                repo.Insert(model);
                _uow.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Courses/Edit/5 (returns partial for AJAX)
        public ActionResult Edit(int id)
        {
            var repo = _uow.Repository<Course>();
            var course = repo.GetSingleIncluding(id, c => c.CourseCategory);
            if (course == null) return HttpNotFound();
            ViewBag.Categories = new SelectList(_uow.Repository<CourseCategory>().GetAll(), "Id", "Name", course.CourseCategoryId);
            return PartialView("_CreateEdit", course);
        }

        // POST: Courses/Edit/5
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Course form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                ViewBag.Categories = new SelectList(_uow.Repository<CourseCategory>().GetAll(), "Id", "Name", form.CourseCategoryId);
                return PartialView("_CreateEdit", form);
            }

            try
            {
                var repo = _uow.Repository<Course>();
                var entity = repo.GetAll().FirstOrDefault(c => c.Id == id);
                if (entity == null) return HttpNotFound();

                entity.Title = form.Title;
                entity.Code = form.Code;
                entity.Description = form.Description;
                entity.CourseCategoryId = form.CourseCategoryId;
                entity.IsActive = form.IsActive;
                entity.UpdatedAt = DateTime.UtcNow;

                repo.Update(entity);
                _uow.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int id)
        {
            var repo = _uow.Repository<Course>();
            var course = repo.GetSingleIncluding(id, c => c.CourseCategory);
            if (course == null) return HttpNotFound();
            return View(course);
        }

        // POST: Courses/Delete/5 (ajax)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var repo = _uow.Repository<Course>();
                var course = repo.GetAll().FirstOrDefault(c => c.Id == id);
                if (course == null) return HttpNotFound();
                repo.Delete(course);
                _uow.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
