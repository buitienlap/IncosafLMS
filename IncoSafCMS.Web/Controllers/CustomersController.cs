using DevExpress.Web.Mvc;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    public class CustomersController : Controller
    {
        IService<Customer> service;
        IUnitOfWork uow;
        private IApplicationUserManager userManager;
        public CustomersController(IService<Customer> _service, IUnitOfWork _uow, IApplicationUserManager _userManager)
        {
            service = _service;
            uow = _uow;
            userManager = _userManager;
        }
        // GET: Customers
        public ActionResult Index()
        {
            return View(service.GetAll());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            var model = service.GetById(id);
            GridViewHelper.SelectedCustomerID = id;
            return View(model);
        }

        // GET: Customers/Create
        //[Authorize(Roles = "KDV")]
        public ActionResult Create()
        {
            return View(new Customer());
        }

        // POST: Customers/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create([ModelBinder(typeof(DevExpressEditorsBinder))] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = userManager.FindByName(User.Identity.Name);
                    customer.department = null; //user?.department;
                    service.Add(customer);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            if (User.IsInRole("Admin"))
                return RedirectToAction("Customer", "Admin");
            else
                return RedirectToAction("Customer", "Home");
        }
        //// GET: Customers/Create
        ////[Authorize(Roles = "KDV")]
        //public ActionResult EditViaContractEdit(int id)
        //{
        //    var model = service.GetById(id);
        //    if (model == null) model = new Customer();
        //    return View(model);
        //}

        //[HttpPost, ValidateInput(false)]
        //[SetPermissions(Permission = "SuaTTKH")]
        //public ActionResult EditViaContractEdit(string id, string ten, string diachi, string daidien, string chucvu, string dienthoai, string fax, string sotaikhoan, string nganhang, string masothue)
        //{
        //    string Error = "false";
        //    int selectCustomer = -1;
        //    if (string.IsNullOrWhiteSpace(ten) || ten == "Nhập tên khách hàng") { Error = "true"; return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet); }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (int.TryParse(id, out selectCustomer))
        //            {
        //                var customer = service.GetById(selectCustomer);
        //                if (customer != null)
        //                {
        //                    // 03-may-2024. added by lapbt. Chỉ cho admin sửa hoặc KH chưa có hợp đồng nào
        //                    if (!User.IsInRole("Admin"))
        //                    {
        //                        var chkExisted = uow.Repository<Contract>().FindBy(x => x.customer.Id == selectCustomer);
        //                        if (chkExisted.Count > 0)
        //                        {
        //                            Error = "true_1";
        //                            return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }

        //                    customer.Name = ten;
        //                    customer.Address = diachi;
        //                    customer.Representative = daidien;
        //                    customer.RepresentativePosition = chucvu;
        //                    customer.Phone = dienthoai;
        //                    customer.Fax = fax;
        //                    customer.AccountNumber = sotaikhoan;
        //                    customer.BankName = nganhang;
        //                    customer.TaxID = masothue;

        //                    service.Update(customer);                            
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Error = "true";
        //            ViewData["EditError"] = e.Message;
        //            return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        Error = "true";
        //        ViewData["EditError"] = "Please, correct all errors.";
        //        return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult EditViaContractCreate(int id)
        //{
        //    var model = service.GetById(id);
        //    if (model == null) model = new Customer();
        //    return View(model);
        //}
        //[HttpPost, ValidateInput(false)]
        //[SetPermissions(Permission = "SuaTTKH")]
        //public ActionResult EditViaContractCreate(string id, string ten, string diachi, string daidien, string chucvu, string dienthoai, string fax, string sotaikhoan, string nganhang, string masothue)
        //{
        //    string Error = "false";
        //    int selectCustomer = -1;
        //    if (string.IsNullOrWhiteSpace(ten) || ten == "Nhập tên khách hàng") { Error = "true"; return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet); }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (int.TryParse(id, out selectCustomer))
        //            {
        //                var customer = service.GetById(selectCustomer);
        //                if (customer != null)
        //                {
        //                    // 03-may-2024. added by lapbt. Chỉ cho admin sửa hoặc KH chưa có hợp đồng nào
        //                    if (!User.IsInRole("Admin"))
        //                    {
        //                        var chkExisted = uow.Repository<Contract>().FindBy(x => x.customer.Id == selectCustomer);
        //                        if (chkExisted.Count > 0)
        //                        {
        //                            Error = "true_1";
        //                            return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }

        //                    customer.Name = ten;
        //                    customer.Address = diachi;
        //                    customer.Representative = daidien;
        //                    customer.RepresentativePosition = chucvu;
        //                    customer.Phone = dienthoai;
        //                    customer.Fax = fax;
        //                    customer.AccountNumber = sotaikhoan;
        //                    customer.BankName = nganhang;
        //                    customer.TaxID = masothue;

        //                    service.Update(customer);
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Error = "true";
        //            ViewData["EditError"] = e.Message;
        //            return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        Error = "true";
        //        ViewData["EditError"] = "Please, correct all errors.";
        //        return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult CreateViaContractEdit()
        //{
        //    return View(new Customer());
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreateViaContractEdit(string ten, string diachi, string daidien, string chucvu, string dienthoai, string fax, string sotaikhoan, string nganhang, string masothue)
        //{
        //    string Error = "false";
        //    int selectCustomer = -1;
        //    if (string.IsNullOrWhiteSpace(ten) || ten == "Nhập tên khách hàng") { Error = "true"; return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet); }

        //    if (GridViewHelper.SelectedContractID > 0)
        //    {
        //        var contract = uow.Repository<Contract>().GetSingle(GridViewHelper.SelectedContractID);
        //        if (contract != null && contract.customer != null) selectCustomer = contract.customer.Id;
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var customer = new Customer()
        //            {
        //                Name = string.IsNullOrWhiteSpace(ten) || ten == "Nhập tên khách hàng" ? string.Empty : ten,
        //                Address = string.IsNullOrWhiteSpace(diachi) || diachi == "Nhập địa chỉ khách hàng" ? string.Empty : diachi,
        //                Representative = string.IsNullOrWhiteSpace(daidien) || daidien == "Nhập tên người đại diện" ? string.Empty : daidien,
        //                RepresentativePosition = string.IsNullOrWhiteSpace(chucvu) || chucvu == "Nhập chức vụ của người đại diện" ? string.Empty : chucvu,
        //                Phone = string.IsNullOrWhiteSpace(dienthoai) || dienthoai == "Nhập số điện thoại của khách hàng" ? string.Empty : dienthoai,
        //                Fax = string.IsNullOrWhiteSpace(fax) || fax == "Nhập số Fax khách hàng" ? string.Empty : fax,
        //                AccountNumber = string.IsNullOrWhiteSpace(sotaikhoan) || sotaikhoan == "Nhập số tài khoản của khách hàng" ? string.Empty : sotaikhoan,
        //                BankName = string.IsNullOrWhiteSpace(nganhang) || nganhang == "Nhập tên ngân hàng" ? string.Empty : nganhang,
        //                TaxID = string.IsNullOrWhiteSpace(masothue) || masothue == "Nhập mã số thuế của khách hàng" ? string.Empty : masothue,
        //            };
        //            if (!string.IsNullOrWhiteSpace(customer.Name))
        //            {
        //                var user = userManager.FindByName(User.Identity.Name);
        //                customer.department = null; //user?.department;
        //                service.Add(customer);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Error = "true";
        //            ViewData["EditError"] = e.Message;
        //            return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        Error = "true";
        //        ViewData["EditError"] = "Please, correct all errors.";
        //        return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //}
        // GET: Customers/Create
        //[Authorize(Roles = "KDV")]
        //public ActionResult CreateViaContractCreate()
        //{
        //    return View(new Customer());
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreateViaContractCreate(string ten, string diachi, string daidien, string chucvu, string dienthoai, string fax, string sotaikhoan, string nganhang, string masothue)
        //{
        //    string Error = "false";
        //    int selectCustomer = -1;
        //    if (string.IsNullOrWhiteSpace(ten) || ten == "Nhập tên khách hàng") { Error = "true"; return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet); }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var customer = new Customer()
        //            {
        //                Name = string.IsNullOrWhiteSpace(ten) || ten == "Nhập tên khách hàng" ? string.Empty : ten,
        //                Address = string.IsNullOrWhiteSpace(diachi) || diachi == "Nhập địa chỉ khách hàng" ? string.Empty : diachi,
        //                Representative = string.IsNullOrWhiteSpace(daidien) || daidien == "Nhập tên người đại diện" ? string.Empty : daidien,
        //                RepresentativePosition = string.IsNullOrWhiteSpace(chucvu) || chucvu == "Nhập chức vụ của người đại diện" ? string.Empty : chucvu,
        //                Phone = string.IsNullOrWhiteSpace(dienthoai) || dienthoai == "Nhập số điện thoại của khách hàng" ? string.Empty : dienthoai,
        //                Fax = string.IsNullOrWhiteSpace(fax) || fax == "Nhập số Fax khách hàng" ? string.Empty : fax,
        //                AccountNumber = string.IsNullOrWhiteSpace(sotaikhoan) || sotaikhoan == "Nhập số tài khoản của khách hàng" ? string.Empty : sotaikhoan,
        //                BankName = string.IsNullOrWhiteSpace(nganhang) || nganhang == "Nhập tên ngân hàng" ? string.Empty : nganhang,
        //                TaxID = string.IsNullOrWhiteSpace(masothue) || masothue == "Nhập mã số thuế của khách hàng" ? string.Empty : masothue,
        //            };
        //            if (!string.IsNullOrWhiteSpace(customer.Name))
        //            {
        //                var user = userManager.FindByName(User.Identity.Name);
        //                customer.department = null; //user?.department;
        //                service.Add(customer);

        //                selectCustomer = customer.Id;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Error = "true";
        //            selectCustomer = -1;
        //            ViewData["EditError"] = e.Message;
        //            return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        Error = "true";
        //        selectCustomer = -1;
        //        ViewData["EditError"] = "Please, correct all errors.";
        //        return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new object[] { Error, selectCustomer }, JsonRequestBehavior.AllowGet);
        //}
        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            var model = service.GetById(id);
            return View(model);
        }

        // POST: Customers/Edit/5
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = service.GetById(id);
                    if (customer != null)
                    {
                        customer.MaKH = EditorExtension.GetValue<string>("MaKH");
                        customer.Name = EditorExtension.GetValue<string>("Name");
                        customer.Address = EditorExtension.GetValue<string>("Address");
                        customer.Phone = EditorExtension.GetValue<string>("Phone");
                        customer.Fax = EditorExtension.GetValue<string>("Fax");
                        customer.AccountNumber = EditorExtension.GetValue<string>("AccountNumber");
                        customer.BankName = EditorExtension.GetValue<string>("BankName");
                        customer.TaxID = EditorExtension.GetValue<string>("TaxID");
                        customer.Representative = EditorExtension.GetValue<string>("Representative");
                        customer.RepresentativePosition = EditorExtension.GetValue<string>("RepresentativePosition");
                        customer.department = null;

                        service.Update(customer);
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
                return RedirectToAction("Customer", "Admin");
            else
                return RedirectToAction("Customer", "Home");
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customers/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    var result = "error";
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var customer = service.GetAll().Where(x => x.Id == id).FirstOrDefault();
        //            if (customer != null)
        //            {
        //                var contracts = uow.Repository<Contract>().GetAll().Where(x => x.customer.Id == id).ToList();
        //                for (int i = 0; i < contracts.Count; i++)
        //                {
        //                    var contract = contracts[i];
        //                    //var tasks = contract.Tasks;
        //                    //for (int j = 0; j < tasks.Count; j++)
        //                    //{
        //                    //    var task = tasks[j];
        //                    //    contract.Tasks.Remove(task);
        //                    //    uow.Repository<AccTask>().Delete(task);
        //                    //    uow.SaveChanges();
        //                    //    j--;
        //                    //}
        //                    uow.Repository<Contract>().Delete(contract);
        //                    uow.SaveChanges();
        //                    i--;
        //                }
        //                service.Delete(customer);

        //                result = "success";
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";

        //    return Json(new object[] { result }, JsonRequestBehavior.AllowGet);
        //    //if (User.IsInRole("Admin"))
        //    //    return RedirectToAction("Customer", "Admin");
        //    //else
        //    //    return RedirectToAction("Customer", "Home");
        //}

        [ValidateInput(false)]
        public ActionResult CustomerViewPartial()
        {
            if (User.IsInRole("Admin"))
            {
                var model = service.GetAll();
                return PartialView("_CustomerViewPartial", model);
            }
            else
            {
                var model = service.FindBy(x => x.Id == 0);
                return PartialView("_CustomerViewPartial", model);
            }
        }

        //[ValidateInput(false)]
        //public ActionResult ContractOfCustomerPartial()
        //{
        //    if (GridViewHelper.SelectedCustomerID < 0)
        //    {
        //        var model = new List<Contract>();
        //        return PartialView("_ContractOfCustomerPartial", model);
        //    }
        //    else
        //    {
        //        var model = new List<Contract>();
        //        var contractRepo = uow.Repository<Contract>();
        //        if (User.IsInRole("KDV"))
        //        {
        //            model = contractRepo.FindBy(x => x.customer != null && x.customer.Id == GridViewHelper.SelectedCustomerID && x.own != null && x.own.UserName == User.Identity.Name);
        //        }
        //        else
        //        {
        //            model = contractRepo.FindBy(x => x.customer != null && x.customer.Id == GridViewHelper.SelectedCustomerID);
        //        }
        //        //var model = User.IsInRole("KDV") ? 
        //        //    uow.Repository<Contract>().GetAll().Where(x => x.customer?.Id == GridViewHelper.SelectedCustomerID && x.own?.UserName == User.Identity.Name) : 
        //        //    uow.Repository<Contract>().GetAll().Where(x => x.customer?.Id == GridViewHelper.SelectedCustomerID);

        //        return PartialView("_ContractOfCustomerPartial", model);
        //    }
        //}
        //public ActionResult CustomCallBackContractOfCustomerAction(int selectedcustomer)
        //{
        //    if (string.IsNullOrEmpty(selectedcustomer.ToString()) || selectedcustomer < 0) GridViewHelper.SelectedCustomerID = -1;
        //    else GridViewHelper.SelectedCustomerID = selectedcustomer;
        //    return ContractOfCustomerPartial();
        //}
    }
}
