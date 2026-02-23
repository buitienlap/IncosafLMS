using DevExpress.Web.Mvc;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        IService<Department> service;
        IUnitOfWork uow;
        IApplicationUserManager userManager;
        public DepartmentsController(IService<Department> _service, IUnitOfWork _uow, IApplicationUserManager _userManager)
        {
            service = _service;
            uow = _uow;
            userManager = _userManager;
        }
        // GET: Departments
        public ActionResult Index()
        {
            return View(service.GetAll());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Departments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewData["Users"] = uow.Repository<AppUser>().GetAll();
            return View(new Department());
        }

        // POST: Departments/Create
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Add(department);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return RedirectToAction("Department", "Admin");
        }

        // GET: Departments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var model = service.GetById(id);
            return View(model);
        }

        // POST: Departments/Edit/5
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var department = service.GetById(id);
                    if (department != null)
                    {
                        department.MaDV = collection.Get("MaDV");
                        department.Name = collection.Get("Name");
                        department.Phone = collection.Get("Phone");
                        department.Email = collection.Get("Email");

                        service.Update(department);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return RedirectToAction("Department", "Home");
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Departments/Delete/5
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var department = service.GetById(id);
                    var employees = department.Employees;
                    for (int i = 0; i < employees.Count; i++)
                    {
                        var employee = employees[i];
                        employee.Department = null;
                        uow.SaveChanges();
                    }
                    if (department != null)
                    {
                        service.Delete(department);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return RedirectToAction("Department", "Home");
        }

        [ValidateInput(false)]
        public ActionResult DepartmentViewPartial()
        {
            var model = service.GetAll();
            return PartialView("_DepartmentViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult EmployeesOfDepartmentPartial()
        {
            if (GridViewHelper.SelectedDepartmentID < 0)
            {
                var model = new List<AppUser>();
                return PartialView("_EmployeesOfDepartmentPartial", model);
            }
            else
            {
                var users = IncosafCMS.Web.Providers.AppUserDataProvider.AppUsers.ToList();
                var model = users.Where(x => x.Department?.Id == GridViewHelper.SelectedDepartmentID).ToList();
                if (model == null) model = new List<AppUser>();
                return PartialView("_EmployeesOfDepartmentPartial", model);
            }
        }
        public ActionResult CustomCallBackEmployeesOfDepartmentAction(int selecteddepartment)
        {
            if (string.IsNullOrEmpty(selecteddepartment.ToString()) || selecteddepartment < 0) GridViewHelper.SelectedDepartmentID = -1;
            else GridViewHelper.SelectedDepartmentID = selecteddepartment;
            return EmployeesOfDepartmentPartial();
        }
    }
}
