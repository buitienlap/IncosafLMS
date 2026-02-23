using IncosafCMS.Core.Services;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Data;
using System.Linq;
using System.Web.Mvc;

namespace IncoSafCMS.Web.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IService<ActivityLog> _service;
        private readonly IUnitOfWork _uow;

        public ActivityController(IService<ActivityLog> service, IUnitOfWork uow)
        {
            _service = service;
            _uow = uow;
        }

        // GET: Activity
        public ActionResult Index()
        {
            var list = _service.GetAll();
            return View(list);
        }

        public ActionResult GetByUser(int userId)
        {
            var items = _uow.Repository<ActivityLog>().FindBy(x => x.UserId == userId).OrderByDescending(x => x.Timestamp).ToList();
            return PartialView("_ActivityGridPartial", items);
        }

        public ActionResult Details(int id)
        {
            var model = _service.GetById(id);
            return PartialView("_ActivityDetailsPartial", model);
        }
    }
}
