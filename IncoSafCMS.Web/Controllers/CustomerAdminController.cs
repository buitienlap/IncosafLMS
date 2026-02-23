using DevExpress.Web.Mvc;
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
    public class CustomerAdminController : Controller
    {
        IService<Customer> service;
        IUnitOfWork uow;
        public CustomerAdminController(IService<Customer> _service, IUnitOfWork _uow)
        {
            service = _service;
            uow = _uow;
        }
        // GET: CustomerAdmin
        public ActionResult Index()
        {
            return View(service.GetAll());
        }

        // GET: CustomerAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View(service.GetById(id));
        }

        // GET: CustomerAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerAdmin/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                // TODO: Add insert logic here
                service.Add(customer);
                return RedirectToAction("Customer", "Admin");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            return View(service.GetById(id));
        }

        // POST: CustomerAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Customer customer)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Customer", "Admin");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Customer", "Admin");
            }
            catch
            {
                return View();
            }
        }
    }
}
