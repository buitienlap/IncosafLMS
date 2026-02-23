using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    public class EquipAdminController : Controller
    {
        IService<OriginalEquipment> service;
        IUnitOfWork uow;
        public EquipAdminController(IService<OriginalEquipment> _service, IUnitOfWork _uow)
        {
            service = _service;
            uow = _uow;
        }
        // GET: EquipAdmin
        public ActionResult Index()
        {
            return View(service.GetAll());
        }

        // GET: EquipAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View(service.GetById(id));
        }

        // GET: EquipAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipAdmin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EquipAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            return View(service.GetById(id));
        }

        // POST: EquipAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EquipAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View(service.GetById(id));
        }

        // POST: EquipAdmin/Delete/5
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
