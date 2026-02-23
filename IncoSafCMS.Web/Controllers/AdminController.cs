using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        IUnitOfWork uow;
        public AdminController(IUnitOfWork _uow)
        {
            uow = _uow;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
            //return RedirectToAction("Index", "UsersAdmin");
        }

        public ActionResult UserApprove()
        {
            return View(new ApplicationUser[] { });
        }

        public ActionResult UserGroups()
        {
            //return View(new ApplicationRole[] { });
            return RedirectToAction("Index", "RolesAdmin", new { context = "Admin" });
        }
        public ActionResult Permission()
        {
            return View();
        }

        //public ActionResult EquipLibrary()
        //{
        //    return View(uow.Repository<OriginalEquipment>().GetAll());
        //}

        public ActionResult Customer()
        {
            return View(uow.Repository<Customer>().GetAll());
        }

        public ActionResult Department()
        {
            return View();
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult StandardProcedure()
        {
            return View();
        }
        public ActionResult Backup()
        {
            return View();
        }
    }
}