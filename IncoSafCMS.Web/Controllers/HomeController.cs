using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.Services;
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
            Session["ShowContractExtraColumns"] = false;
            Session["ShowFilterTabDashboard"] = false;
            ViewData["SubTitle"] = "";
            ViewData["Message"] = "";
            //var contracts = service.GetAll();
            //var model = User.IsInRole("KDV") ? contracts.Where(e => e.own?.UserName == User.Identity.Name) : contracts;
            var user = userManager.FindByName(User.Identity.Name);
            if (User.IsInRole("Admin") || User.IsInRole("TPTH"))
                GridViewHelper.OwnerContractGridFilter = "Công ty";
            else if (User.IsInRole("Accountant") || User.IsInRole("DeptDirector"))
                GridViewHelper.OwnerContractGridFilter = user.Department?.MaDV;
            else
                GridViewHelper.OwnerContractGridFilter = user.DisplayName;

            var departments = departmentService.GetAll();
            if (departments.Count > 0 && User.IsInRole("Admin"))
                departments.Insert(0, new Department { Id = 0, MaDV = "", Name = "Đơn vi: Toàn bộ" });
            
            ViewBag.Departments = departments;           

            // Lấy danh sách nhân viên
            if (User.IsInRole("Admin"))
            {
                var userList = uow.Repository<AppUser>()
                .FindBy(x => x.TwoFactorEnabled)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.DisplayName
                })
                .ToList();

                userList.Insert(0, new SelectListItem { Value = "", Text = "KĐV Toàn bộ" });
                ViewBag.UsersByDepartment = userList;
            }

            if (User.IsInRole("DeptDirector"))
            {
                var userList = uow.Repository<AppUser>()
                .FindBy(x => x.TwoFactorEnabled && x.Department.Id == user.Department.Id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.DisplayName
                })
                .ToList();

                userList.Insert(0, new SelectListItem { Value = "", Text = "KĐV Toàn đơn vị" });
                ViewBag.UsersByDepartment = userList;
            }
            if (!User.IsInRole("Admin") && !User.IsInRole("DeptDirector"))
            {
                var userList = uow.Repository<AppUser>()
                .FindBy(x => x.TwoFactorEnabled && x.Id == user.Id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.DisplayName
                })
                .ToList();
                userList.Insert(0, new SelectListItem { Value = user.Id.ToString(), Text = user.DisplayName.ToString() });
                ViewBag.UserList = userList;
            }

            // Return recent activity logs as the model for the home index (left panel now shows ActivityLog)
            var activities = uow.Repository<ActivityLog>().GetAll()
                .OrderByDescending(x => x.Timestamp)
                .ToList();

            return View(activities);

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