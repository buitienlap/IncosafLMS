using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using System.Linq;
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
            var list = uow.Repository<Question>().GetAll();
            return View(list);
        }

        // Returns question list partial for AJAX refresh
        [HttpGet]
        public ActionResult ListPartial()
        {
            var list = uow.Repository<Question>().GetAll();
            return PartialView("_QuestionListPartial", list);
        }

        // GET: show create form (partial)
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CourseCategories = new SelectList(uow.Repository<CourseCategory>().GetAll(), "Id", "Name");
            return PartialView("_QuestionEdit", new Question());
        }

        // POST: create question via AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question model)
        {
            if (ModelState.IsValid)
            {
                uow.Repository<Question>().Insert(model);
                uow.SaveChanges();
                return Json(new { success = true });
            }
            ViewBag.CourseCategories = new SelectList(uow.Repository<CourseCategory>().GetAll(), "Id", "Name", model.CourseCategoryId);
            return PartialView("_QuestionEdit", model);
        }

        // GET: edit form (partial)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var q = uow.Repository<Question>().GetSingle(id);
            if (q == null) return HttpNotFound();
            ViewBag.CourseCategories = new SelectList(uow.Repository<CourseCategory>().GetAll(), "Id", "Name", q.CourseCategoryId);
            return PartialView("_QuestionEdit", q);
        }

        // POST: edit via AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question model)
        {
            if (ModelState.IsValid)
            {
                uow.Repository<Question>().Update(model);
                uow.SaveChanges();
                return Json(new { success = true });
            }
            ViewBag.CourseCategories = new SelectList(uow.Repository<CourseCategory>().GetAll(), "Id", "Name", model.CourseCategoryId);
            return PartialView("_QuestionEdit", model);
        }

        // POST: delete via AJAX
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var repo = uow.Repository<Question>();
            var q = repo.GetSingle(id);
            if (q == null) return Json(new { success = false, message = "Not found" });
            repo.Delete(q);
            uow.SaveChanges();
            return Json(new { success = true });
        }
    }
}
