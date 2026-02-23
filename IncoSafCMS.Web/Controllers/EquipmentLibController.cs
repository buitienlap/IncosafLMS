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
    public class EquipmentLibController : Controller
    {
        IService<OriginalEquipment> service;
        IUnitOfWork uow;
        public EquipmentLibController(IService<OriginalEquipment> _service, IUnitOfWork _uow)
        {
            service = _service;
            uow = _uow;
        }
        // GET: EquipmentLib
        public ActionResult Index()
        {
            return View();
        }

        // GET: EquipmentLib/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EquipmentLib/Create
        public ActionResult Create()
        {
            EquipmentSpecificationsList.GetEquipmentSpecifications = new List<Specifications>();
            EquipmentLoadTestsList.GetEquipmentLoadTests = new List<LoadTest>();
            EquipmentPartionList.GetEquipmentPartion = new List<EquipmentPartion>();
            EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument = new List<TechnicalDocument>();
            EquipmentLoadTestsList.GetEquipmentLoadTests = new List<LoadTest>();
            var equipGroups = service.GetAll().Where(e => e.IsGroup == 1);
            ViewData["equipGroups"] = equipGroups;
            return View(new OriginalEquipment());
        }

        // POST: EquipmentLib/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] OriginalEquipment equipment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var specification in EquipmentSpecificationsList.GetEquipmentSpecifications)
                    {
                        var specificationCopy = new Specifications()
                        {
                            Id = 0,
                            Name = specification.Name,
                            Value = specification.Value,
                            f_key = specification.f_key,
                            f_unit = specification.f_unit
                        };
                        equipment.specifications.Add(specificationCopy);
                        uow.Repository<Specifications>().Insert(specificationCopy);
                    }

                    foreach (var loadTest in EquipmentLoadTestsList.GetEquipmentLoadTests)
                    {
                        var loadTestCopy = new LoadTest()
                        {
                            Id = 0,
                            LocalTest = loadTest.LocalTest,
                            Radius = loadTest.Radius,
                            Passed = true,
                            CorrespondingLoad = loadTest.CorrespondingLoad,
                            DynamicLoad = loadTest.DynamicLoad,
                            StaticLoad = loadTest.StaticLoad
                        };

                        equipment.LoadTests.Add(loadTestCopy);

                        uow.Repository<LoadTest>().Insert(loadTestCopy);
                    }

                    foreach (var partion in EquipmentPartionList.GetEquipmentPartion)
                    {
                        var partionCopy = new EquipmentPartion()
                        {
                            Id = 0,
                            Name = partion.Name,
                            Note = partion.Note,
                            Passed1 = true,
                            Passed2 = partion.Passed2
                        };
                        equipment.Partions.Add(partionCopy);
                        uow.Repository<EquipmentPartion>().Insert(partionCopy);
                    }

                    foreach (var technicaldocument in EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument)
                    {
                        var technicaldocumentCopy = new TechnicalDocument()
                        {
                            Id = 0,
                            Name = technicaldocument.Name,
                            Note = technicaldocument.Note,
                            Passed = true
                        };

                        equipment.TechnicalDocuments.Add(technicaldocumentCopy);
                        uow.Repository<TechnicalDocument>().Insert(technicaldocumentCopy);
                    }

                    uow.SaveChanges();
                    service.Add(equipment);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            if (User.IsInRole("Admin"))
                return RedirectToAction("EquipLibrary", "Admin");
            else
                return RedirectToAction("EquipmentLib", "Home");
        }

        public ActionResult Copy(int OriginalEquipmentID)
        {
            var originalEquipment = service.GetById(OriginalEquipmentID);
            return View(originalEquipment);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Copy(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var originalEquipment = service.GetById(id);
                    if (originalEquipment != null)
                    {
                        var newOriginalEquipment = originalEquipment.Copy();

                        newOriginalEquipment.Code = string.IsNullOrEmpty(EditorExtension.GetValue<string>("Code")) ? originalEquipment.Code : EditorExtension.GetValue<string>("Code");
                        newOriginalEquipment.Name = string.IsNullOrEmpty(EditorExtension.GetValue<string>("Name")) ? originalEquipment.Name : EditorExtension.GetValue<string>("Name");

                        foreach (var loadtest in newOriginalEquipment.LoadTests)
                            uow.Repository<LoadTest>().Insert(loadtest);
                        foreach (var partion in newOriginalEquipment.Partions)
                            uow.Repository<EquipmentPartion>().Insert(partion);
                        foreach (var specification in newOriginalEquipment.specifications)
                            uow.Repository<Specifications>().Insert(specification);
                        foreach (var technicaldocument in newOriginalEquipment.TechnicalDocuments)
                            uow.Repository<TechnicalDocument>().Insert(technicaldocument);


                        uow.SaveChanges();
                        service.Add(newOriginalEquipment);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            if (User.IsInRole("Admin"))
                return RedirectToAction("EquipLibrary", "Admin");
            else
                return RedirectToAction("EquipmentLib", "Home");
        }

        // GET: EquipmentLib/Edit/5
        public ActionResult Edit(int id)
        {
            var model = service.GetAll().Where(x => x.Id == id).FirstOrDefault();
            if (model == null) model = new OriginalEquipment();
            EquipmentSpecificationsList.GetEquipmentSpecifications = model.specifications ?? new List<Specifications>();
            EquipmentPartionList.GetEquipmentPartion = model.Partions ?? new List<EquipmentPartion>();
            EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument = model.TechnicalDocuments ?? new List<TechnicalDocument>();
            EquipmentLoadTestsList.GetEquipmentLoadTests = model.LoadTests ?? new List<LoadTest>();
            var equipGroups = service.GetAll().Where(e => e.IsGroup == 1);
            ViewData["equipGroups"] = equipGroups;
            return View(model);
        }

        // POST: EquipmentLib/Edit/5
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipmentlib = service.GetById(id);
                    if (equipmentlib != null)
                    {
                        equipmentlib.Code = collection.Get("Code") == "Nhập mã hiệu thiết bị" ? "" : collection.Get("Code");
                        equipmentlib.Name = collection.Get("Name") == "Nhập tên thiết bị" ? "" : collection.Get("Name");
                        equipmentlib.Uses = collection.Get("Uses") == "Nhập công dụng của thiết bị" ? "" : collection.Get("Uses");
                        equipmentlib.No = collection.Get("No") == "Nhập số quản lý của thiết bị" ? "" : collection.Get("No");
                        equipmentlib.YearOfProduction = collection.Get("YearOfProduction") == "Nhập năm sản xuất của thiết bị" ? "" : collection.Get("YearOfProduction");
                        equipmentlib.ManuFacturer = collection.Get("ManuFacturer") == "Nhập tên nhà chế tạo của thiết bị" ? "" : collection.Get("ManuFacturer");
                        //equipmentlib.ParentCode = collection.GetValue("ParentCode").RawValue.ToString();
                        equipmentlib.ParentCode = EditorExtension.GetValue<string>("ParentCode");

                        // Xử lý Partions
                        for (int i = 0; i < equipmentlib.Partions.Count; i++)
                        {
                            var originalPartion = equipmentlib.Partions[i];
                            var modifiedPartion = EquipmentPartionList.GetEquipmentPartion.Find(x => x.Id == originalPartion.Id);
                            if (modifiedPartion != null)
                            {
                                originalPartion.Name = modifiedPartion.Name;
                                originalPartion.Note = modifiedPartion.Note;
                                originalPartion.Passed1 = modifiedPartion.Passed1;
                                originalPartion.Passed2 = modifiedPartion.Passed2;

                                uow.Repository<EquipmentPartion>().Update(originalPartion);
                                //uow.SaveChanges();
                            }
                            else
                            {
                                equipmentlib.Partions.Remove(originalPartion);

                                uow.Repository<EquipmentPartion>().Delete(originalPartion);
                                //uow.SaveChanges();
                                i--;
                            }
                        }

                        foreach (var partionNew in EquipmentPartionList.GetEquipmentPartion)
                        {
                            var oriPartion = equipmentlib.Partions.Find(x => x.Id == partionNew.Id);
                            if (oriPartion != null) continue;
                            var partionNewCopy = new EquipmentPartion()
                            {
                                Id = 0,
                                Name = partionNew.Name,
                                Passed1 = partionNew.Passed1,
                                Passed2 = partionNew.Passed2,
                                Note = partionNew.Note
                            };
                            equipmentlib.Partions.Add(partionNewCopy);
                            uow.Repository<EquipmentPartion>().Insert(partionNewCopy);
                            //uow.SaveChanges();
                        }

                        // Xử lý LoadTest
                        for (int i = 0; i < equipmentlib.LoadTests.Count; i++)
                        {
                            var originalLoadTest = equipmentlib.LoadTests[i];
                            var modifiedLoadTest = EquipmentLoadTestsList.GetEquipmentLoadTests.Find(x => x.Id == originalLoadTest.Id);
                            if (modifiedLoadTest != null)
                            {
                                originalLoadTest.LocalTest = modifiedLoadTest.LocalTest;
                                originalLoadTest.Radius = modifiedLoadTest.Radius;
                                originalLoadTest.Passed = modifiedLoadTest.Passed;
                                originalLoadTest.CorrespondingLoad = modifiedLoadTest.CorrespondingLoad;
                                originalLoadTest.DynamicLoad = modifiedLoadTest.DynamicLoad;
                                originalLoadTest.StaticLoad = modifiedLoadTest.StaticLoad;

                                uow.Repository<LoadTest>().Update(originalLoadTest);
                                //uow.SaveChanges();
                            }
                            else
                            {
                                equipmentlib.LoadTests.Remove(originalLoadTest);

                                uow.Repository<LoadTest>().Delete(originalLoadTest);
                                //uow.SaveChanges();
                                i--;
                            }
                        }

                        foreach (var loadTestNew in EquipmentLoadTestsList.GetEquipmentLoadTests)
                        {
                            var oriLoadTest = equipmentlib.LoadTests.Find(x => x.Id == loadTestNew.Id);
                            if (oriLoadTest != null) continue;
                            var loadTestNewCopy = new LoadTest()
                            {
                                Id = 0,
                                LocalTest = loadTestNew.LocalTest,
                                Radius = loadTestNew.Radius,
                                Passed = loadTestNew.Passed,
                                CorrespondingLoad = loadTestNew.CorrespondingLoad,
                                DynamicLoad = loadTestNew.DynamicLoad,
                                StaticLoad = loadTestNew.StaticLoad
                            };
                            equipmentlib.LoadTests.Add(loadTestNewCopy);
                            uow.Repository<LoadTest>().Insert(loadTestNewCopy);
                            //uow.SaveChanges();
                        }

                        // Xử lý Specifications
                        for (int i = 0; i < equipmentlib.specifications.Count; i++)
                        {
                            var originalSpecifications = equipmentlib.specifications[i];
                            var modifiedSpecifications = EquipmentSpecificationsList.GetEquipmentSpecifications.Find(x => x.Id == originalSpecifications.Id);
                            if (modifiedSpecifications != null)
                            {
                                originalSpecifications.Name = modifiedSpecifications.Name;
                                originalSpecifications.Value = modifiedSpecifications.Value;
                                originalSpecifications.f_unit = modifiedSpecifications.f_unit;
                                originalSpecifications.f_key = modifiedSpecifications.f_key;

                                uow.Repository<Specifications>().Update(originalSpecifications);
                                //uow.SaveChanges();
                            }
                            else
                            {
                                equipmentlib.specifications.Remove(originalSpecifications);

                                uow.Repository<Specifications>().Delete(originalSpecifications);
                                //uow.SaveChanges();
                                i--;
                            }
                        }

                        foreach (var specificationsNew in EquipmentSpecificationsList.GetEquipmentSpecifications)
                        {
                            var oriSpecifications = equipmentlib.specifications.Find(x => x.Id == specificationsNew.Id);
                            if (oriSpecifications != null) continue;
                            var specificationsNewCopy = new Specifications()
                            {
                                Id = 0,
                                Name = specificationsNew.Name,
                                Value = specificationsNew.Value,
                                f_key = specificationsNew.f_key,
                                f_unit = specificationsNew.f_unit
                            };
                            equipmentlib.specifications.Add(specificationsNewCopy);
                            uow.Repository<Specifications>().Insert(specificationsNewCopy);
                            //uow.SaveChanges();
                        }

                        // Xử lý TechnicalDocument
                        for (int i = 0; i < equipmentlib.TechnicalDocuments.Count; i++)
                        {
                            var originalTechnicalDocument = equipmentlib.TechnicalDocuments[i];
                            var modifiedTechnicalDocument = EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument.Find(x => x.Id == originalTechnicalDocument.Id);
                            if (modifiedTechnicalDocument != null)
                            {
                                originalTechnicalDocument.Name = modifiedTechnicalDocument.Name;
                                originalTechnicalDocument.Note = modifiedTechnicalDocument.Note;
                                originalTechnicalDocument.Passed = modifiedTechnicalDocument.Passed;

                                uow.Repository<TechnicalDocument>().Update(originalTechnicalDocument);
                                //uow.SaveChanges();
                            }
                            else
                            {
                                equipmentlib.TechnicalDocuments.Remove(originalTechnicalDocument);

                                uow.Repository<TechnicalDocument>().Delete(originalTechnicalDocument);
                                //uow.SaveChanges();
                                i--;
                            }
                        }

                        foreach (var technicalDocumentNew in EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument)
                        {
                            var oriTechnicalDocument = equipmentlib.TechnicalDocuments.Find(x => x.Id == technicalDocumentNew.Id);
                            if (oriTechnicalDocument != null) continue;
                            var technicalDocumentNewCopy = new TechnicalDocument()
                            {
                                Id = 0,
                                Name = technicalDocumentNew.Name,
                                Note = technicalDocumentNew.Note,
                                Passed = technicalDocumentNew.Passed
                            };
                            equipmentlib.TechnicalDocuments.Add(technicalDocumentNewCopy);
                            uow.Repository<TechnicalDocument>().Insert(technicalDocumentNewCopy);
                            //uow.SaveChanges();
                        }

                        uow.SaveChanges();
                        service.Update(equipmentlib);

                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            if (User.IsInRole("Admin"))
                return RedirectToAction("EquipLibrary", "Admin");
            else
                return RedirectToAction("EquipmentLib", "Home");
        }

        // GET: EquipmentLib/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EquipmentLib/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipmentlib = service.GetAll().Where(x => x.Id == id).FirstOrDefault();
                    if (equipmentlib != null)
                    {
                        var partions = equipmentlib.Partions;
                        for (int i = 0; i < partions.Count; i++)
                        {
                            var partion = partions[i];
                            equipmentlib.Partions.Remove(partion);
                            uow.Repository<EquipmentPartion>().Delete(partion);
                            uow.SaveChanges();
                            i--;
                        }

                        var loadTests = equipmentlib.LoadTests;
                        for (int i = 0; i < loadTests.Count; i++)
                        {
                            var loadTest = loadTests[i];
                            equipmentlib.LoadTests.Remove(loadTest);
                            uow.Repository<LoadTest>().Delete(loadTest);
                            i--;
                        }

                        var specifications = equipmentlib.specifications;
                        for (int i = 0; i < specifications.Count; i++)
                        {
                            var specification = specifications[i];
                            equipmentlib.specifications.Remove(specification);
                            uow.Repository<Specifications>().Delete(specification);
                            uow.SaveChanges();
                            i--;
                        }

                        var technicaldocuments = equipmentlib.TechnicalDocuments;
                        for (int i = 0; i < technicaldocuments.Count; i++)
                        {
                            var technicaldocument = technicaldocuments[i];
                            equipmentlib.TechnicalDocuments.Remove(technicaldocument);
                            uow.Repository<TechnicalDocument>().Delete(technicaldocument);
                            i--;
                        }

                        service.Delete(equipmentlib);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            if (User.IsInRole("Admin"))
                return RedirectToAction("EquipLibrary", "Admin");
            else
                return RedirectToAction("EquipmentLib", "Home");

        }
        [ValidateInput(false)]
        public ActionResult EquipmentLibViewPartial()
        {
            var model = IncosafCMS.Web.Providers.EquipmentLibDataProvider.EquipmentLibs.ToList();
            return PartialView("_EquipmentLibViewPartial", model);
        }

        #region Details View
        [ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibPartial()
        {
            if (GridViewHelper.SelectedEquipmentLibID < 0)
            {
                var model = new OriginalEquipment();
                return PartialView("_SpecificationsOfEquipmentLibPartial", model);
            }
            else
            {
                var equipmentlibs = IncosafCMS.Web.Providers.EquipmentLibDataProvider.EquipmentLibs.ToList();
                var model = equipmentlibs.FirstOrDefault(x => x.Id == GridViewHelper.SelectedEquipmentLibID);
                if (model?.specifications == null) model = new OriginalEquipment();
                return PartialView("_SpecificationsOfEquipmentLibPartial", model);
            }
        }
        [ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibPartial()
        {
            if (GridViewHelper.SelectedEquipmentID < 0)
            {
                var model = new OriginalEquipment();
                return PartialView("_LoadTestsOfEquipmentLibPartial", model);
            }
            else
            {
                var equipmentlibs = IncosafCMS.Web.Providers.EquipmentLibDataProvider.EquipmentLibs.ToList();
                var model = equipmentlibs.FirstOrDefault(x => x.Id == GridViewHelper.SelectedEquipmentLibID);
                if (model?.LoadTests == null) model = new OriginalEquipment();
                return PartialView("_LoadTestsOfEquipmentLibPartial", model);
            }
        }
        public ActionResult CustomCallBackLoadTestsOfEquipmentLibAction(int selectedequipmentlib)
        {
            if (string.IsNullOrEmpty(selectedequipmentlib.ToString()) || selectedequipmentlib < 0) GridViewHelper.SelectedEquipmentID = -1;
            else GridViewHelper.SelectedEquipmentID = selectedequipmentlib;
            return LoadTestsOfEquipmentLibPartial();
        }
        public ActionResult CustomCallBackSpecificationsOfEquipmentLibAction(int selectedequipmentlib)
        {
            if (string.IsNullOrEmpty(selectedequipmentlib.ToString()) || selectedequipmentlib < 0) GridViewHelper.SelectedEquipmentLibID = -1;
            else GridViewHelper.SelectedEquipmentLibID = selectedequipmentlib;
            return SpecificationsOfEquipmentLibPartial();
        }
        [ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibPartial()
        {
            if (GridViewHelper.SelectedEquipmentLibID < 0)
            {
                var model = new OriginalEquipment();
                return PartialView("_PartionsOfEquipmentLibPartial", model);
            }
            else
            {
                var equipmentlibs = IncosafCMS.Web.Providers.EquipmentLibDataProvider.EquipmentLibs.ToList();
                var model = equipmentlibs.FirstOrDefault(x => x.Id == GridViewHelper.SelectedEquipmentLibID);
                if (model?.Partions == null) model = new OriginalEquipment();
                return PartialView("_PartionsOfEquipmentLibPartial", model);
            }
        }
        public ActionResult CustomCallBackPartionsOfEquipmentLibAction(int selectedequipmentlib)
        {
            if (string.IsNullOrEmpty(selectedequipmentlib.ToString()) || selectedequipmentlib < 0) GridViewHelper.SelectedEquipmentLibID = -1;
            else GridViewHelper.SelectedEquipmentLibID = selectedequipmentlib;
            return PartionsOfEquipmentLibPartial();
        }

        [ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibPartial()
        {
            if (GridViewHelper.SelectedEquipmentLibID < 0)
            {
                var model = new OriginalEquipment();
                return PartialView("_TechnicalDocumentOfEquipmentLibPartial", model);
            }
            else
            {
                var equipmentlibs = IncosafCMS.Web.Providers.EquipmentLibDataProvider.EquipmentLibs.ToList();
                var model = equipmentlibs.FirstOrDefault(x => x.Id == GridViewHelper.SelectedEquipmentLibID);
                if (model?.TechnicalDocuments == null) model = new OriginalEquipment();
                return PartialView("_TechnicalDocumentOfEquipmentLibPartial", model);
            }
        }
        public ActionResult CustomCallBackTechnicalDocumentOfEquipmentLibAction(int selectedequipmentlib)
        {
            if (string.IsNullOrEmpty(selectedequipmentlib.ToString()) || selectedequipmentlib < 0) GridViewHelper.SelectedEquipmentLibID = -1;
            else GridViewHelper.SelectedEquipmentLibID = selectedequipmentlib;
            return TechnicalDocumentOfEquipmentLibPartial();
        }
        #endregion

        #region Create View
        [ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibCreatePartial()
        {
            return PartialView("_SpecificationsOfEquipmentLibCreatePartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Specifications specifications)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(specifications.Name) || string.IsNullOrWhiteSpace(specifications.Value))
                    {
                        ViewData["EditableSpecifications"] = specifications;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentSpecificationsList.AddSpecifications(specifications);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableSpecifications"] = specifications;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_SpecificationsOfEquipmentLibCreatePartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Specifications specifications)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(specifications.Name) || string.IsNullOrWhiteSpace(specifications.Value))
                    {
                        ViewData["EditableSpecifications"] = specifications;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentSpecificationsList.UpdateSpecifications(specifications);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableSpecifications"] = specifications;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_SpecificationsOfEquipmentLibCreatePartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibCreatePartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    EquipmentSpecificationsList.DeleteSpecifications(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_SpecificationsOfEquipmentLibCreatePartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
        }

        //----------------------------------------//
        [ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibCreatePartial()
        {
            return PartialView("_LoadTestsOfEquipmentLibCreatePartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] LoadTest loadTest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(loadTest.LocalTest))
                    {
                        ViewData["EditableLoadTests"] = loadTest;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentLoadTestsList.AddLoadTests(loadTest);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableLoadTests"] = loadTest;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_LoadTestsOfEquipmentLibCreatePartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] LoadTest loadTest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(loadTest.LocalTest))
                    {
                        ViewData["EditableLoadTests"] = loadTest;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentLoadTestsList.UpdateLoadTests(loadTest);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableLoadTests"] = loadTest;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_LoadTestsOfEquipmentLibCreatePartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibCreatePartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    EquipmentLoadTestsList.DeleteLoadTests(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_LoadTestsOfEquipmentLibCreatePartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
        }

        //----------------------------------------//
        [ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibCreatePartial()
        {
            return PartialView("_PartionsOfEquipmentLibCreatePartial", EquipmentPartionList.GetEquipmentPartion);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] EquipmentPartion equipmentPartion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(equipmentPartion.Name))
                    {
                        ViewData["EditablePartion"] = equipmentPartion;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentPartionList.AddEquipmentPartion(equipmentPartion);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditablePartion"] = equipmentPartion;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_PartionsOfEquipmentLibCreatePartial", EquipmentPartionList.GetEquipmentPartion);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] EquipmentPartion equipmentPartion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(equipmentPartion.Name))
                    {
                        ViewData["EditablePartion"] = equipmentPartion;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentPartionList.UpdateEquipmentPartion(equipmentPartion);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditablePartion"] = equipmentPartion;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_PartionsOfEquipmentLibCreatePartial", EquipmentPartionList.GetEquipmentPartion);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibCreatePartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    EquipmentPartionList.DeleteEquipmentPartion(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_PartionsOfEquipmentLibCreatePartial", EquipmentPartionList.GetEquipmentPartion);
        }
        //--------------------------------------------//
        [ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibCreatePartial()
        {
            return PartialView("_TechnicalDocumentOfEquipmentLibCreatePartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] TechnicalDocument technicalDocument)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(technicalDocument.Name))
                    {
                        ViewData["EditableTechnicalDocument"] = technicalDocument;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentTechnicalDocumentList.AddEquipmentTechnicalDocument(technicalDocument);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableTechnicalDocument"] = technicalDocument;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TechnicalDocumentOfEquipmentLibCreatePartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] TechnicalDocument technicalDocument)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(technicalDocument.Name))
                    {
                        ViewData["EditableTechnicalDocument"] = technicalDocument;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentTechnicalDocumentList.UpdateEquipmentTechnicalDocument(technicalDocument);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditablePartion"] = technicalDocument;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TechnicalDocumentOfEquipmentLibCreatePartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibCreatePartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    EquipmentTechnicalDocumentList.DeleteEquipmentTechnicalDocument(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_TechnicalDocumentOfEquipmentLibCreatePartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
        }
        #endregion

        #region Edit View
        [ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibEditPartial()
        {
            return PartialView("_SpecificationsOfEquipmentLibEditPartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Specifications specifications)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(specifications.Name) || string.IsNullOrWhiteSpace(specifications.Value))
                    {
                        ViewData["EditableSpecifications"] = specifications;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentSpecificationsList.AddSpecifications(specifications);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableSpecifications"] = specifications;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_SpecificationsOfEquipmentLibEditPartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Specifications specifications)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(specifications.Name) || string.IsNullOrWhiteSpace(specifications.Value))
                    {
                        ViewData["EditableSpecifications"] = specifications;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentSpecificationsList.UpdateSpecifications(specifications);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableSpecifications"] = specifications;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_SpecificationsOfEquipmentLibEditPartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SpecificationsOfEquipmentLibEditPartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    EquipmentSpecificationsList.DeleteSpecifications(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_SpecificationsOfEquipmentLibEditPartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
        }

        //----------------------------------------//
        [ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibEditPartial()
        {
            return PartialView("_LoadTestsOfEquipmentLibEditPartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] LoadTest loadTest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(loadTest.LocalTest))
                    {
                        ViewData["EditableLoadTests"] = loadTest;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentLoadTestsList.AddLoadTests(loadTest);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableLoadTests"] = loadTest;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_LoadTestsOfEquipmentLibEditPartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] LoadTest loadTest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(loadTest.LocalTest))
                    {
                        ViewData["EditableLoadTests"] = loadTest;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentLoadTestsList.UpdateLoadTests(loadTest);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableLoadTests"] = loadTest;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_LoadTestsOfEquipmentLibEditPartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult LoadTestsOfEquipmentLibEditPartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    EquipmentLoadTestsList.DeleteLoadTests(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_LoadTestsOfEquipmentLibEditPartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
        }

        //----------------------------------------//
        [ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibEditPartial()
        {
            return PartialView("_PartionsOfEquipmentLibEditPartial", EquipmentPartionList.GetEquipmentPartion);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] EquipmentPartion equipmentPartion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(equipmentPartion.Name))
                    {
                        ViewData["EditablePartion"] = equipmentPartion;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentPartionList.AddEquipmentPartion(equipmentPartion);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditablePartion"] = equipmentPartion;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_PartionsOfEquipmentLibEditPartial", EquipmentPartionList.GetEquipmentPartion);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] EquipmentPartion equipmentPartion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(equipmentPartion.Name))
                    {
                        ViewData["EditablePartion"] = equipmentPartion;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentPartionList.UpdateEquipmentPartion(equipmentPartion);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditablePartion"] = equipmentPartion;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_PartionsOfEquipmentLibEditPartial", EquipmentPartionList.GetEquipmentPartion);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PartionsOfEquipmentLibEditPartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    EquipmentPartionList.DeleteEquipmentPartion(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_PartionsOfEquipmentLibEditPartial", EquipmentPartionList.GetEquipmentPartion);
        }
        //--------------------------------------//
        [ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibEditPartial()
        {
            return PartialView("_TechnicalDocumentOfEquipmentLibEditPartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] TechnicalDocument technicalDocument)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(technicalDocument.Name))
                    {
                        ViewData["EditableTechnicalDocument"] = technicalDocument;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentTechnicalDocumentList.AddEquipmentTechnicalDocument(technicalDocument);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableTechnicalDocument"] = technicalDocument;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TechnicalDocumentOfEquipmentLibEditPartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] TechnicalDocument technicalDocument)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(technicalDocument.Name))
                    {
                        ViewData["EditableTechnicalDocument"] = technicalDocument;
                        ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    }
                    else
                        EquipmentTechnicalDocumentList.UpdateEquipmentTechnicalDocument(technicalDocument);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    ViewData["EditableTechnicalDocument"] = technicalDocument;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TechnicalDocumentOfEquipmentLibEditPartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TechnicalDocumentOfEquipmentLibEditPartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    EquipmentTechnicalDocumentList.DeleteEquipmentTechnicalDocument(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_TechnicalDocumentOfEquipmentLibEditPartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
        }
        #endregion
    }
}
