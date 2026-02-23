using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    public class PermissionsController : Controller
    {
        IService<AppPermission> service;
        IUnitOfWork uow;
        public PermissionsController(IService<AppPermission> _service, IUnitOfWork _uow)
        {
            service = _service;
            uow = _uow;
        }
        // GET: Permissions
        public ActionResult Index()
        {
            var model = service.GetAll();
            //ApplicationRole role = new ApplicationRole() { Name = "HeHe" };
            //var pe = model.First();
            //pe.Roles.Add(role);
            //service.Update(pe);
            return View(model);
        }

        // GET: Permissions/Details/5
        public ActionResult Details(int id)
        {
            return View(service.GetById(id));
        }

        // GET: Permissions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Permissions/Create
        [HttpPost]
        public ActionResult Create(AppPermission permission)
        {
            try
            {
                // TODO: Add insert logic here
                service.Add(permission);
                return RedirectToAction("Permission", "Admin");
            }
            catch
            {
                return View();
            }
        }

        // GET: Permissions/Edit/5
        public ActionResult Edit(int id)
        {
            return View(service.GetById(id));
        }

        // POST: Permissions/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AppPermission permission)
        {
            try
            {
                // TODO: Add update logic here
                var model = service.GetById(id);

                return RedirectToAction("Permission", "Admin");
            }
            catch
            {
                return View();
            }
        }

        // GET: Permissions/Delete/5
        public ActionResult Delete(int id)
        {
            return View(service.GetById(id));
        }

        // POST: Permissions/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
