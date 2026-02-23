using IncosafCMS.Web.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Data.Identity;
using IncosafCMS.Data.Extensions;
using IncosafCMS.Core.Data;

namespace IncosafCMS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        private IApplicationUserManager _userManager;
        private IApplicationRoleManager _roleManager;
        private IService<AppPermission> service;
        private IUnitOfWork uow;
        public RolesAdminController(IService<AppPermission> _service, IUnitOfWork _uow, IApplicationUserManager userManager, IApplicationRoleManager roleManager)
        {
            service = _service;
            uow = _uow;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //
        // GET: /Roles/
        public ActionResult Index()
        {
            return View(_roleManager.GetRoles());
        }

        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await _roleManager.FindByIdAsync(id.Value);
            if (role == null)
            {
                return HttpNotFound();
            }
            // Get the list of Users in this Role
            var users = new List<AppUser>();

            // Get the list of Users in this Role
            foreach (var user in _userManager.GetUsers())
            {
                if (await _userManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole { Name = roleViewModel.Name };
                var roleresult = await _roleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await _roleManager.FindByIdAsync(id.Value);
            if (role == null)
            {
                return HttpNotFound();
            }
            var roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            ViewData["Permissions"] = service.GetAll();
            return View(roleModel);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] RoleViewModel roleModel, params int[] selectedPermission)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.UpdateAsync(roleModel.Id, roleModel.Name);
                var role = await _roleManager.FindByNameAsync(roleModel.Name);

                var rolePermRep = uow.Repository<ApplicationPermissionRole>();

                var permissions = service.GetAll();

                foreach (var permission in permissions)
                {
                    var roleperms = permission.Roles.Where(e => e.RoleId == role.Id).ToList();

                    for (int i = 0; i < roleperms.Count; i++)
                    {
                        var r = roleperms[i];
                        permission.Roles.Remove(r);
                        rolePermRep.Delete(r);
                    }

                    //if (roleperms?.Count() > 0)
                    //{
                    //    foreach (var per in roleperms)
                    //    {
                    //        permission.Roles.Remove(per);
                    //    }
                    //}

                    service.Update(permission);
                }

                permissions = service.GetAll();
                foreach (var permission in permissions)
                {
                    var rolePerm = new ApplicationPermissionRole() { PermissionId = permission.Id, RoleId = role.Id };
                    if (selectedPermission.Contains(permission.Id))
                    {
                        permission.Roles.Add(rolePerm);
                        //_service.Update(permission);
                        service.Update(permission);
                    }
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await _roleManager.FindByIdAsync(id.Value);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ApplicationIdentityResult result;
                if (deleteUser != null)
                {
                    result = await _roleManager.DeleteAsync(id.Value);
                }
                else
                {
                    result = await _roleManager.DeleteAsync(id.Value);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

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
