using DevExpress.Web.Mvc;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Data.Extensions;
using IncosafCMS.Data.Identity;
using IncosafCMS.Services;
using IncosafCMS.Web.Models;
using IncosafCMS.Web.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    public class EmployeesController : Controller
    {
        IService<AppUser> service;
        IUnitOfWork uow;
        private IApplicationUserManager _userManager;
        private IApplicationRoleManager _roleManager;
        public EmployeesController(IService<AppUser> _service, IUnitOfWork _uow, IApplicationUserManager userManager, IApplicationRoleManager roleManager)
        {
            service = _service;
            uow = _uow;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        // GET: Employees/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewData["Departments"] = uow.Repository<Department>().GetAll();
            ViewData["EmployeePositions"] = uow.Repository<EmployeePosition>().GetAll();
            ViewData["Roles"] = _roleManager.GetRoles();
            //EmployeeSanLuongDKList.GetEmployeeSanLuongDK = new List<SanLuongDK>();
            return View(new AppUser());
        }

        // POST: Employees/Create
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([ModelBinder(typeof(DevExpressEditorsBinder))] AppUser user, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.Email;

                IncosafCMSContext context = new IncosafCMSContext("AppContext", new DebugLogger());
                EntityRepository<EmployeePosition> posRepo = new EntityRepository<EmployeePosition>(context);
                EntityRepository<Department> depRepo = new EntityRepository<Department>(context);
                var applicationRoleManager = IdentityFactory.CreateRoleManager(context);
                var applicationUserManager = IdentityFactory.CreateUserManager(context);
                var id = 0;

                var iduser = user.ToApplicationUser();

                if (user.Position != null)
                {
                    iduser.Position = await posRepo.GetSingleAsync(user.Position.Id);
                }

                if (user.Department != null)
                {
                    iduser.Department = await depRepo.GetSingleAsync(user.Department.Id);
                }

                //iduser.SanLuongDK = new List<SanLuongDK>();
                //// Xử lý sản lượng đăng ký
                //foreach (var sanluongdk in EmployeeSanLuongDKList.GetEmployeeSanLuongDK)
                //{
                //    var sanluongdkNew = new SanLuongDK()
                //    {
                //        NamDK = sanluongdk.NamDK,
                //        SanLuong = sanluongdk.SanLuong
                //    };

                //    iduser.SanLuongDK.Add(sanluongdkNew);
                //    uow.Repository<SanLuongDK>().Insert(sanluongdkNew);
                //    uow.SaveChanges();
                //}

                var adminresult = await applicationUserManager.CreateAsync(iduser, user.UserName); // password trùng với user name

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    id = iduser.Id;
                    if (selectedRole != null)
                    {
                        var result = await applicationUserManager.AddToRolesAsync(iduser.Id, selectedRole);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewData["Roles"] = _roleManager.GetRoles();
                            return RedirectToAction("Index", "Admin");
                        }
                    }

                    var idenUser = await applicationUserManager.FindByIdAsync(id);
                    if (idenUser != null)
                    {
                        var appuser = idenUser.ToAppUser();
                        context.Set<AppUser>().Add(appuser);
                    }
                    context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewData["Roles"] = _roleManager.GetRoles();
                    return RedirectToAction("Index", "Admin");

                }
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
                ViewData["Roles"] = _roleManager.GetRoles();
                var message = string.Join(" | ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
                Exception exception = new Exception(message.ToString());
                return RedirectToAction("Index", "Admin");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            //ViewData["Roles"] = _roleManager.GetRoles();
            //return RedirectToAction("Index", "Admin");

        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            ViewData["Departments"] = uow.Repository<Department>().GetAll();
            ViewData["EmployeePositions"] = uow.Repository<EmployeePosition>().GetAll();
            ViewData["Roles"] = _roleManager.GetRoles();
            var model = _userManager.FindById(id);
            return View(model);
        }

        // POST: Employees/Edit/5
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id, FormCollection collection, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                using (var context = new IncosafCMSContext("AppContext", new DebugLogger()))
                {
                    EntityRepository<EmployeePosition> posRepo = new EntityRepository<EmployeePosition>(context);
                    EntityRepository<Department> depRepo = new EntityRepository<Department>(context);
                    var applicationRoleManager = IdentityFactory.CreateRoleManager(context);
                    var applicationUserManager = IdentityFactory.CreateUserManager(context);

                    try
                    {
                        var user = await applicationUserManager.FindByIdAsync(id);
                        if (user == null)
                        {
                            return HttpNotFound();
                        }

                        user.MaNV = EditorExtension.GetValue<string>("MaNV");
                        user.UserName = EditorExtension.GetValue<string>("Email");
                        user.Email = EditorExtension.GetValue<string>("Email");
                        user.DisplayName = EditorExtension.GetValue<string>("DisplayName");
                        user.Address = EditorExtension.GetValue<string>("Address");
                        user.PhoneNumber = EditorExtension.GetValue<string>("PhoneNumber");

                        try
                        {
                            var resetPassword = EditorExtension.GetValue<bool>("cbResetPassword");
                            if (resetPassword)
                            {
                                if (user.PasswordHash != null)
                                {
                                    await applicationUserManager.RemovePasswordAsync(user.Id);
                                }

                                await applicationUserManager.AddPasswordAsync(user.Id, user.Email);
                            }
                        }
                        catch (Exception)
                        {

                        }


                        if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("Position.Id")))
                        {
                            if (user.Position == null)
                            {
                                int positiontID;
                                if (int.TryParse(EditorExtension.GetValue<string>("Position.Id"), out positiontID))
                                    user.Position = await posRepo.GetSingleAsync(positiontID);
                            }
                            else
                            {
                                int positiontID;
                                if (int.TryParse(EditorExtension.GetValue<string>("Position.Id"), out positiontID))
                                    user.Position = await posRepo.GetSingleAsync(positiontID);
                            }
                        }

                        if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("department.Id")))
                        {
                            if (user.Department == null)
                            {
                                int departmentID;
                                if (int.TryParse(EditorExtension.GetValue<string>("department.Id"), out departmentID))
                                    user.Department = await depRepo.GetSingleAsync(departmentID);
                            }
                            else
                            {
                                int departmentID;
                                if (int.TryParse(EditorExtension.GetValue<string>("department.Id"), out departmentID))
                                    if (user.Department.Id != departmentID)
                                        user.Department = await depRepo.GetSingleAsync(departmentID);
                            }
                        }

                        //var userRoles = await _userManager.GetRolesAsync(user.Id);
                        var userRoles = await applicationUserManager.GetRolesAsync(user.Id).ConfigureAwait(false);

                        selectedRole = selectedRole ?? new string[] { };

                        //var result = await _userManager.AddUserToRolesAsync(user.Id, selectedRole.Except(userRoles).ToList());
                        ApplicationIdentityResult result = null;

                        foreach (var role in selectedRole.Except(userRoles).ToList())
                        {
                            var iresult = await applicationUserManager.AddToRoleAsync(user.Id, role);
                            result = iresult.ToApplicationIdentityResult();
                            if (!result.Succeeded)
                            {
                                ModelState.AddModelError("", result.Errors.First());
                                return View();
                            }
                        }

                        foreach (var role in userRoles.Except(selectedRole).ToList())
                        {
                            var iresult = await applicationUserManager.RemoveFromRoleAsync(user.Id, role);
                            result = iresult.ToApplicationIdentityResult();
                            if (!result.Succeeded)
                            {
                                ModelState.AddModelError("", result.Errors.First());
                                return View();
                            }
                        }

                        await applicationUserManager.UpdateAsync(user);
                        uow = new UnitOfWork(context);
                        service = new Service<AppUser>(uow);

                        var appU = service.FindBy(e => e.Email == user.Email).FirstOrDefault();

                        if (appU == null)
                        {
                            appU = user.ToAppUser();
                            if (user.Department != null)
                                appU.DepartmentId = user.Department.Id;
                            if (user.Position != null)
                                appU.PositionId = user.Position.Id;
                            context.Set<AppUser>().Add(appU);
                            //service.Add(appU);
                        }
                        else
                        {
                            //appU = context.Set<AppUser>().Where(e => e.Id == user.Id).FirstOrDefault();

                            appU.CopyApplicationIdentityUserProperties(user);
                            appU.MaNV = user.MaNV;
                            appU.PhoneNumber = user.PhoneNumber;
                            appU.Address = user.Address;
                            appU.DisplayName = user.DisplayName;
                            if (user.Department != null)
                                appU.DepartmentId = user.Department.Id;
                            if (user.Position != null)
                                appU.PositionId = user.Position.Id;
                            appU.Roles = new HashSet<ApplicationUserRole>();
                            foreach (var role in user.Roles)
                            {
                                appU.Roles.Add(role.ToApplicationUserRole());
                            }
                            //context.Entry(appU).State = System.Data.Entity.EntityState.Modified;
                            //context.Set<AppUser>().Add(appU);
                            //service.Update(appU);
                        }


                        context.SaveChanges();


                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] = e.Message;
                    }
                }
                //IncosafCMSContext context = new IncosafCMSContext("AppContext", new DebugLogger());

            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return RedirectToAction("Index", "Admin");
        }

        //// GET: Employees/Delete/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Employees/Delete/5
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userManager.FindById(id);
                    if (user == null)
                    {
                        ViewData["EditError"] = "Không tìm thấy tài khoản người dùng!";
                        var appuser = await service.GetByIdAsync(id);
                        await service.DeleteAsync(appuser);
                    }
                    else if (User.Identity.Name != user.UserName)
                    {
                        await _userManager.DeleteAsync(id);
                        var appuser = await service.GetByIdAsync(id);
                        await service.DeleteAsync(appuser);
                    }
                    else
                    {
                        ViewData["EditError"] = "Không thể xóa tài khoản hiện đang đăng nhập!";
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return RedirectToAction("Index", "Admin");
        }

        [ValidateInput(false)]
        public ActionResult EmployeeViewPartial()
        {
            var model = IncosafCMS.Web.Providers.AppUserDataProvider.AppUsers; //_userManager.GetUsers();
            return PartialView("_EmployeeViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult ActivityOfEmployeePartial()
        {
            if (GridViewHelper.SelectedEmployeeID < 0)
            {
                var model = new List<ActivityLog>();
                return PartialView("_ActivityOfEmployeePartial", model);
            }
            else
            {
                //var contracts = IncosafCMS.Web.Providers.ActivityDataProvider.Contracts1.ToList();
                //var model = contracts.Where(x => x.own?.Id == GridViewHelper.SelectedEmployeeID);
                var model = ActivityDataProvider.GetActivityByOwnerID(GridViewHelper.SelectedEmployeeID);
                return PartialView("_ActivityOfEmployeePartial", model);
            }
        }
        public ActionResult CustomCallBackActivityOfEmployeeAction(int selectedemployee)
        {
            if (string.IsNullOrEmpty(selectedemployee.ToString()) || selectedemployee < 0) GridViewHelper.SelectedEmployeeID = -1;
            else GridViewHelper.SelectedEmployeeID = selectedemployee;
            return ActivityOfEmployeePartial();
        }
        //public ActionResult SanLuongDKOfEmployeeEditPartial()
        //{
        //    if (GridViewHelper.SelectedEmployeeID < 0)
        //    {
        //        var model = new List<SanLuongDK>();
        //        return PartialView("_SanLuongDKOfEmployeeEditPartial", model);
        //    }
        //    else
        //    {
        //        var model = service.GetById(GridViewHelper.SelectedEmployeeID)?.SanLuongDK;

        //        if (model == null) model = new List<SanLuongDK>();
        //        return PartialView("_SanLuongDKOfEmployeeEditPartial", model);
        //    }
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult SanLuongDKOfEmployeeEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] SanLuongDK sanluongDK)
        //{
        //    var employee = service.GetById(GridViewHelper.SelectedEmployeeID);
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrWhiteSpace(sanluongDK.NamDK))
        //            {
        //                ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
        //            }
        //            else
        //            {
        //                if (employee != null)
        //                {
        //                    employee.SanLuongDK.Add(sanluongDK);
        //                    service.Update(employee);
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    return PartialView("_SanLuongDKOfEmployeeEditPartial", employee.SanLuongDK);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult SanLuongDKOfEmployeeEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] SanLuongDK sanluongDK)
        //{
        //    var employee = service.GetById(GridViewHelper.SelectedEmployeeID);
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrWhiteSpace(sanluongDK.NamDK))
        //            {
        //                ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
        //            }
        //            else
        //            {
        //                if (employee != null)
        //                {
        //                    var originalSanLuongDK = employee.SanLuongDK.Where(x => x.Id == sanluongDK.Id).FirstOrDefault();
        //                    if (originalSanLuongDK != null)
        //                    {
        //                        originalSanLuongDK.NamDK = sanluongDK.NamDK;
        //                        originalSanLuongDK.SanLuong = sanluongDK.SanLuong;
        //                    }

        //                    service.Update(employee);
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    return PartialView("_SanLuongDKOfEmployeeEditPartial", employee.SanLuongDK);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult SanLuongDKOfEmployeeEditPartialDelete(System.Int32 Id)
        //{
        //    var employee = service.GetById(GridViewHelper.SelectedEmployeeID);
        //    if (Id >= 0)
        //    {
        //        try
        //        {
        //            if (employee != null)
        //            {
        //                var interpayment = employee.SanLuongDK.Where(x => x.Id == Id).FirstOrDefault();
        //                if (interpayment != null) employee.SanLuongDK.Remove(interpayment);

        //                service.Update(employee);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    return PartialView("_SanLuongDKOfEmployeeEditPartial", employee.SanLuongDK);
        //}

        //public ActionResult SanLuongDKOfEmployeeCreatePartial()
        //{
        //    EmployeeSanLuongDKList.GetEmployeeSanLuongDK = new List<SanLuongDK>();
        //    return PartialView("_SanLuongDKOfEmployeeCreatePartial", EmployeeSanLuongDKList.GetEmployeeSanLuongDK);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult SanLuongDKOfEmployeeCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] SanLuongDK sanluongDK)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrWhiteSpace(sanluongDK.NamDK))
        //            {
        //                ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
        //            }
        //            else
        //            {
        //                EmployeeSanLuongDKList.AddEmployeeSanLuongDK(sanluongDK);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    return PartialView("_SanLuongDKOfEmployeeCreatePartial", EmployeeSanLuongDKList.GetEmployeeSanLuongDK);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult SanLuongDKOfEmployeeCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] SanLuongDK sanluongDK)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrWhiteSpace(sanluongDK.NamDK))
        //            {
        //                ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
        //            }
        //            else
        //            {
        //                EmployeeSanLuongDKList.UpdateEmployeeSanLuongDK(sanluongDK);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    return PartialView("_SanLuongDKOfEmployeeCreatePartial", EmployeeSanLuongDKList.GetEmployeeSanLuongDK);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult SanLuongDKOfEmployeeCreatePartialDelete(System.Int32 Id)
        //{
        //    if (Id >= 0)
        //    {
        //        try
        //        {
        //            EmployeeSanLuongDKList.DeleteEmployeeSanLuongDK(Id);
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    return PartialView("_SanLuongDKOfEmployeeCreatePartial", EmployeeSanLuongDKList.GetEmployeeSanLuongDK);
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_roleManager != null)
                {
                    _roleManager.Dispose();
                    _roleManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
