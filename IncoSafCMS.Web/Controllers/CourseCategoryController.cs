using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseCategoryController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CourseCategoryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: CourseCategory/Create?parentId=1
        public ActionResult Create(int? parentId)
        {
            var model = new CourseCategory
            {
                ParentId = parentId,
                IsActive = true
            };
            return PartialView("_CreateEdit", model);
        }

        // POST: CourseCategory/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CourseCategory model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var repo = _uow.Repository<CourseCategory>();
                    // initial insert to get Id
                    model.Path = null;
                    model.Level = 0;
                    model.CreatedAt = DateTime.UtcNow;
                    model.CreatedBy = User?.Identity?.Name;
                    repo.Insert(model);
                    _uow.SaveChanges();

                    // compute path and level
                    if (model.ParentId.HasValue)
                    {
                        var parent = repo.GetSingle(model.ParentId.Value);
                        model.Level = (parent?.Level ?? 0) + 1;
                        var parentPath = parent?.Path ?? "/";
                        model.Path = (parentPath.EndsWith("/") ? parentPath : parentPath + "/") + model.Id + "/";
                    }
                    else
                    {
                        model.Level = 0;
                        model.Path = "/" + model.Id + "/";
                    }
                    repo.Update(model);
                    _uow.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    return Json(new { success = false, message = ex.Message });
                }
            }
            Response.StatusCode = 400;
            return PartialView("_CreateEdit", model);
        }

        // GET: CourseCategory/Edit/5
        public ActionResult Edit(int id)
        {
            var repo = _uow.Repository<CourseCategory>();
            var model = repo.GetSingle(id);
            if (model == null) return HttpNotFound();
            return PartialView("_CreateEdit", model);
        }

        // POST: CourseCategory/Edit/5
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, CourseCategory form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return PartialView("_CreateEdit", form);
            }
            try
            {
                var repo = _uow.Repository<CourseCategory>();
                var entity = repo.GetSingle(id);
                if (entity == null) return HttpNotFound();

                // track old path for children update
                var oldPath = entity.Path;
                var oldLevel = entity.Level;
                var oldParent = entity.ParentId;

                // update basic fields
                entity.Name = form.Name;
                entity.Code = form.Code;
                entity.Description = form.Description;
                entity.SortOrder = form.SortOrder;
                entity.IsActive = form.IsActive;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = User?.Identity?.Name;

                // handle parent change
                entity.ParentId = form.ParentId;

                // compute new level and path
                if (entity.ParentId.HasValue)
                {
                    var parent = repo.GetSingle(entity.ParentId.Value);
                    entity.Level = (parent?.Level ?? 0) + 1;
                    var parentPath = parent?.Path ?? "/";
                    entity.Path = (parentPath.EndsWith("/") ? parentPath : parentPath + "/") + entity.Id + "/";
                }
                else
                {
                    entity.Level = 0;
                    entity.Path = "/" + entity.Id + "/";
                }

                repo.Update(entity);
                _uow.SaveChanges();

                // update descendants paths if path changed
                if (!string.Equals(oldPath, entity.Path, StringComparison.Ordinal))
                {
                    var all = repo.GetAll();
                    var descendants = all.Where(c => c.Path != null && c.Path.StartsWith(oldPath) && c.Id != entity.Id).ToList();
                    foreach (var d in descendants)
                    {
                        // replace oldPath prefix with new entity.Path
                        if (d.Path.StartsWith(oldPath))
                        {
                            d.Path = entity.Path + d.Path.Substring(oldPath.Length);
                            // recompute level by counting slashes
                            d.Level = d.Path.Count(ch => ch == '/') - 2; // since path like /1/2/ has two slashes per level+1
                            repo.Update(d);
                        }
                    }
                    _uow.SaveChanges();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: CourseCategory/Delete/5
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int id)
        {
            try
            {
                var repo = _uow.Repository<CourseCategory>();
                var entity = repo.GetSingle(id);
                if (entity == null) return HttpNotFound();

                // delete descendants first
                var path = entity.Path ?? "/" + entity.Id + "/";
                var all = repo.GetAll();
                var toDelete = all.Where(c => c.Path != null && c.Path.StartsWith(path)).ToList();
                foreach (var d in toDelete)
                {
                    repo.Delete(d);
                }
                // delete self
                repo.Delete(entity);
                _uow.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
