using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.Services;
using IncosafCMS.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IService<Contract> service;
        IService<Department> departmentService;        
        IUnitOfWork uow;
        private IApplicationUserManager userManager;
        public HomeController(IService<Contract> _service, IService<Department> _departmentService, IUnitOfWork _uow, IApplicationUserManager _userManager)
        {
            service = _service;
            uow = _uow;
            userManager = _userManager;
            departmentService = _departmentService;
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            var user = userManager.FindByName(User.Identity.Name);

            var courses = uow.Repository<Course>().FindBy(c => c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .Take(6)
                .ToList();

            var activities = uow.Repository<ActivityLog>()
                .FindBy(a => a.UserId == user.Id)
                .OrderByDescending(a => a.Timestamp)
                .ToList();

            // Load pending / in-progress exam assignments for this user
            var pendingExams = uow.Repository<ExamAssignment>()
                .FindBy(e => e.UserId == user.Id
                    && e.Status != ExamAssignmentStatus.Completed)
                .OrderBy(e => e.Deadline)
                .Select(e => new Models.PendingExamDto
                {
                    Id = e.Id,
                    ExamTitle = e.ExamTitle,
                    QuestionCount = e.QuestionCount,
                    TimeLimitMinutes = e.TimeLimitMinutes,
                    Deadline = e.Deadline,
                    Status = e.Status
                })
                .ToList();

            var model = new HomeDashboardViewModel
            {
                UserDisplayName = user.DisplayName,
                LatestCourses = courses,
                RecentActivities = activities,
                PendingExams = pendingExams,
                // Count distinct courses/exams by RelatedId to avoid duplicates from multiple visits
                TotalCourses = activities.Where(a => a.Type == ActivityType.Learning && a.RelatedId != null)
                    .Select(a => a.RelatedId).Distinct().Count(),
                TotalExams = activities.Where(a => a.Type == ActivityType.Exam && a.RelatedId != null)
                    .Select(a => a.RelatedId).Distinct().Count(),
                TotalPractice = activities.Where(a => a.Type == ActivityType.Practice && a.RelatedId != null)
                    .Select(a => a.RelatedId).Distinct().Count(),
                TotalActivities = activities.Count
            };

            return View(model);

        }
        public ActionResult ContractDetails(Contract model)
        {
            model = service.GetById(model.Id);
            return View(model);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Customer()
        {
            var user = userManager.FindByName(User.Identity.Name);
            var model = User.IsInRole("Admin") ? uow.Repository<Customer>().GetAll() : uow.Repository<Customer>().GetAll().Where(x => x.department == null || x.department.Id == user?.Department?.Id).ToList();
            return View(model);
        }
        public ActionResult CustomerDetails(Customer model)
        {
            model = uow.Repository<Customer>().GetSingle(model.Id);
            return View(model);
        }
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        //public ActionResult Equipment()
        //{
        //    var model = uow.Repository<Equipment>().GetAll();
        //    return View(model);
        //}
        //public ActionResult EquipmentDetails(Equipment model)
        //{
        //    model = uow.Repository<Equipment>().GetSingle(model.Id);
        //    return View(model);
        //}
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        //public ActionResult EquipmentLib()
        //{
        //    var model = uow.Repository<OriginalEquipment>().GetAll();
        //    return View(model);
        //}
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Employee()
        {
            var model = uow.Repository<AppUser>().GetAll();
            return View(model);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Department()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult StandardProcedure()
        {
            return View();
        }
        public ActionResult PriceQuotation()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}