using DevExpress.Web.Mvc;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.Services;
using IncosafCMS.Services;
using IncosafCMS.Web.Helpers;
using IncosafCMS.Web.Models;
using IncosafCMS.Web.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using IncosafCMS.Data;
using System.IO;
using System.Web;
using System.Net;
using ClosedXML.Excel;
using DevExpress.Spreadsheet;
using DevExpress.Web;


namespace IncosafCMS.Web.Controllers
{
    //[DynamicRoleAuthorize]
    //[CustomAuthorize(roleSelector:"CreateContract")]
    [Authorize]
    public class ContractsController : Controller
    {
        private readonly IncosafCMSContext db = new IncosafCMSContext("name=AppContext", null);
        
        IService<Contract> service;
        IUnitOfWork uow;
        private IApplicationUserManager userManager;

        private void EnsureFtpDirectoryExists(string ftpBaseUrl, string folderPath, string ftpUser, string ftpPass)
        {
            // folderPath kiểu: /Hoso001/HN/2025/251032/
            string[] subDirs = folderPath.Trim('/').Split('/');

            string currentPath = "";
            foreach (string subDir in subDirs)
            {
                currentPath += "/" + subDir;
                string url = ftpBaseUrl + currentPath + "/";

                try
                {
                    // thử LIST để xem có tồn tại không
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential(ftpUser, ftpPass);

                    using (var response = (FtpWebResponse)request.GetResponse())
                    {
                        // tồn tại rồi => bỏ qua
                    }
                }
                catch (WebException ex)
                {
                    // nếu lỗi 550 thì nghĩa là thư mục chưa có, cần tạo mới
                    if (ex.Response is FtpWebResponse resp && resp.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        FtpWebRequest mkDirRequest = (FtpWebRequest)WebRequest.Create(url);
                        mkDirRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                        mkDirRequest.Credentials = new NetworkCredential(ftpUser, ftpPass);

                        using (var respMkDir = (FtpWebResponse)mkDirRequest.GetResponse())
                        {
                            // thư mục mới đã tạo
                        }
                    }
                    else
                    {
                        throw; // lỗi khác thì ném ra ngoài
                    }
                }
            }
        }

        private int GetCurrentUserId()
        {
            return Convert.ToInt32(Session["UserId"]);
        }

        public ContractsController(IService<Contract> _service, IUnitOfWork _uow, IApplicationUserManager _userManager)
        {
            service = _service;
            uow = _uow;
            userManager = _userManager;            
        }
        // GET: Contracts
        public ActionResult Index()
        {
            var model = new List<Contract>();            
            return View(model);
        }

        // GET: Contracts/Details/5
        public ActionResult Details(int id)
        {
            Contract model = null;
            if (User.IsInRole("KDV"))
                model = service.FindBy(x => x.Id == id && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault();
            else
                service.GetById(id);

            if (model == null) return RedirectToAction("Error", "Home");
            var user = userManager.FindByName(User.Identity.Name);
            var repCustomer = uow.Repository<Customer>();
            ViewData["Customers"] = User.IsInRole("Admin") ? repCustomer.GetAll() : repCustomer.FindBy(x => x.department == null || (user != null && user.Department != null && x.department.Id == user.Department.Id)).ToList();
            GridViewHelper.SelectedContractID = id;
            return View(model);
        }

        // GET: Contracts/Create
        //[Authorize(Roles = "KDV")]
        public ActionResult Create()
        {
            var provinces = uow.Repository<Province>().GetAll();            
            var customers = IncosafCMS.Web.Providers.CustomerDataProvider.Customers.ToList();
            var user = userManager.FindByName(User.Identity.Name);
            var repProprietor = uow.Repository<Proprietor>();
            AccTaskList.GetAccTasks = new List<AccTask>();
            ViewData["Customers"] = User.IsInRole("Admin") ? customers 
                : customers.Where(x => x.department == null || (user != null && user.Department != null && x.department.Id == user.Department.Id));

            var Proprietors = repProprietor.GetAll();          
            Proprietors.Insert(0, new Proprietor() { Id = -1, PropName = "[Tạo mới người sử dụng]" });
            ViewData["Proprietors"] = Proprietors;
            ViewData["provinces"] = provinces;            
            ViewData["AppUsersKDV1"] = uow.Repository<Core.DomainModels.Identity.AppUser>()
                                        .FindBy(x => x.TwoFactorEnabled == true)
                                        .OrderBy(x => x.DepartmentId)
                                        .ToList();
            ViewData["AppUsersKDV2"] = ViewData["AppUsersKDV1"];
            /*
            ViewData["AppUsersKDV2"] = uow.Repository<Core.DomainModels.Identity.AppUser>()
                                        .FindBy(x => x.TwoFactorEnabled == true)
                                        .OrderBy(x => x.DepartmentId)
                                        .ToList();
            */
            return View(new Contract());
        }

        /// <summary>
        /// Create new contract -> new task (public ActionResult TaskViewCreateActionPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] AccTask task))
        /// Noted by lapbt
        /// 18-mar-2024. Bs tính giá trị khoán TB đưa vào Contract, code cp từ bên Edit sang
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        // POST: Contracts/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create([ModelBinder(typeof(DevExpressEditorsBinder))] Contract contract, FormCollection collection)
        {
            try
            { 
                if (contract.MaHD != null)
                {
                    var existed = uow.Repository<Contract>().FindBy(e => e.MaHD == contract.MaHD).FirstOrDefault();
                    if (existed != null)
                    {
                        ViewData["EditError"] = "Mã hợp đồng đã tồn tại!";
                        return RedirectToAction("Index", "Home");
                    }
                }
                Core.DomainModels.Identity.AppUser own = null;
                if (User.IsInRole("Admin") && !string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbEmployees_Create")))
                {
                    var ownID = EditorExtension.GetValue<int>("cmbEmployees_Create");
                    own = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Id == ownID).FirstOrDefault();
                    contract.own = own;
                }
                else
                {
                    var user = User.Identity.Name;
                    own = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.UserName == user).FirstOrDefault();
                    contract.own = own;
                }

                if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbCustomerID_ContractCreate")))
                {
                    int customerID;
                    if (int.TryParse(EditorExtension.GetValue<string>("cmbCustomerID_ContractCreate"), out customerID))
                        contract.customer = uow.Repository<Customer>().GetSingle(customerID);
                }

                var contractTypeId = EditorExtension.GetValue<int>("cboContractTypeId");
                if (contractTypeId <= 0)
                {                    
                    contract.contractType = 0;
                }
                else
                {
                    contract.contractType = contractTypeId;     // su dung truong nay de luu, k0 fai tao moi
                }

                contract.NguoiLienHe = EditorExtension.GetValue<string>("txtNguoiLienHeCreate");
                contract.DienThoaiNguoiLienHe = EditorExtension.GetValue<string>("txtDienThoaiNguoiLienHeCreate");

                try
                {
                    if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("KDV1")))
                    {
                        int kdv1_id = EditorExtension.GetValue<int>("KDV1");
                        var kdv1 = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Id == kdv1_id).FirstOrDefault();

                        if (kdv1 != null) contract.KDV1 = kdv1;
                    }
                }
                catch { }

                var provinceIdstr = EditorExtension.GetValue<string>("cmbProvinceOfContractCreate");

                var s = int.TryParse(provinceIdstr, out int provinceId);

                if (s)
                {
                    var province = uow.Repository<Province>().GetSingle(provinceId);
                    contract.DiaDiemThucHien = province.ProvinceName;
                }
                else
                    contract.DiaDiemThucHien = provinceIdstr;

                try
                {
                    if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("KDV2")))
                    {
                        int kdv2_id = EditorExtension.GetValue<int>("KDV2");
                        var kdv2 = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Id == kdv2_id).FirstOrDefault();

                        if (kdv2 != null) contract.KDV2 = kdv2;
                    }

                }
                catch { }
                

                foreach (var task in AccTaskList.GetAccTasks)
                {
                    var taskCopy = new AccTask()
                    {
                        Id = 0,
                        Name = task.Name,
                        Unit = task.Unit,
                        Amount = task.Amount,
                        UnitPrice = task.UnitPrice,
                        AccTaskNote = task.AccTaskNote,
                        AccTaskGroup = task.AccTaskGroup,
                        Accreditations = task.Accreditations,
                        vatType = task.vatType,             // added by lapbt, 3 lines to fix bug 
                        VAT = task.VAT,
                        Tyle1 = task.Tyle1,
                        Tyle2 = task.Tyle2,
                        Tyle3 = task.Tyle3,
                        Tyle4 = task.Tyle4,
                        SoLuong = task.SoLuong
                    };
                    contract.Tasks.Add(taskCopy);
                    uow.Repository<AccTask>().Insert(taskCopy);
                    uow.SaveChanges();
                }
                
                // Xử lý thông tin người sử dụng - Hung sua 15.06.2025             
                string strPropName = Request.Form["cmbProprietorsOfContractCreate"]; // Lay Text go moi tu o chứ không phải Value                        
                string strPropAddress = EditorExtension.GetValue<string>("txtPropAddressCreate");
                //Neu DVSD da co:
                int proprietorID;
                if (int.TryParse(EditorExtension.GetValue<string>("cmbProprietorsOfContractCreate"), out proprietorID) && proprietorID > 0)
                {
                    var proprietor = uow.Repository<Proprietor>().GetSingle(proprietorID);
                    //Neu khong thay doi thi giu nguyen
                    if (!string.IsNullOrWhiteSpace(strPropName) && proprietor != null && strPropName == proprietor.PropName)
                    {
                        contract.Proprietor = proprietor;
                    }

                    //Nếu tên ĐVSD không thay đổi, có thay đổi địa chỉ thì update
                    if (!string.IsNullOrWhiteSpace(strPropAddress) && strPropName == proprietor.PropName && strPropAddress != proprietor.PropAddress)
                    {
                        proprietor.PropAddress = strPropAddress;
                        uow.Repository<Proprietor>().Update(proprietor);
                        uow.SaveChanges();
                    }
                    //Nếu sua ô Tên ĐVSD trống: thì đặt ĐVSD ở contract = null
                    if (string.IsNullOrWhiteSpace(strPropName))
                    {
                        contract.Proprietor = null;
                        service.Update(contract);
                    }

                }
                else
                {
                    //Neu dang chua chon DVSD (proprietorID<=0) va O ten DVSD khong trong thi cap nhat moi
                    if (!string.IsNullOrWhiteSpace(strPropName) && proprietorID <= 0)
                    {
                        var newProprietor = new Proprietor()
                        {
                            Id = 0,
                            PropName = strPropName,
                            PropAddress = strPropAddress,
                            PropDepartment = contract.own?.Department
                        };
                        contract.Proprietor = newProprietor;
                        uow.Repository<Proprietor>().Insert(newProprietor);
                        uow.SaveChanges();
                        //service.Update(contract);
                    }
                }

                if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbVATTypeCreate")))
                {
                    if (int.TryParse(EditorExtension.GetValue<string>("cmbVATTypeCreate"), out int vatType))
                    {
                        if (contract.vatType != (VATType)vatType)
                        {
                            contract.vatType = (VATType)vatType;
                        }
                    }
                }

                if (contract.vatType == VATType.cothue)
                {
                    if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbContractVATCreate")))
                    {
                        if (int.TryParse(EditorExtension.GetValue<string>("cmbContractVATCreate"), out int vat))
                        {
                            contract.VAT = vat;
                        }
                    }
                }
                else
                {
                    contract.VAT = 0;
                }

                // 18-mar-2024. Bs tính giá trị khoán TB đưa vào Contract, code cp từ bên Edit sang.
                // đặt ở vị trí cuối cùng, bởi có lq tới tính dựa vào VAT, nên fai để xly VAT ở trên
                if (contract.Tasks.Count > 0)
                {
                    // lapbt edited 30-may-2021. Bo sung tinh lai so tien chua thue
                    contract.Value = contract.Tasks.Sum(x => x.Amount * x.UnitPrice);
                    contract.TienTruThue = contract.Value / (1 + contract.VAT / 100);

                    // lapbt edited 03-jan-2024. Tính toán lại tỷ lệ khoán ở hợp đồng thông qua tính từ chi tiết khoán cá nhân ở đầu việc
                    // Tính tổng giá trị chưa VAT của tất cả các dòng (S1) = Sum(khối lượng * đơn giá /(1+VAT/100))
                    double s1 = contract.Tasks.Sum(x => x.Amount * x.UnitPrice / (1 + x.VAT / 100));

                    // 10/08/2024 thêm tính trung bình khoán công ty
                    double s2_tyle1 = contract.Tasks.Sum(x => x.Amount * x.UnitPrice / (1 + x.VAT / 100) * (x.Tyle1 ?? 0) / 100);

                    // Tính giá trị khoán cá nhân của toàn bộ các dòng trong hợp đồng đó: S2 = Sum(khối lượng * đơn giá / (1 + VAT/100) * khoán cá nhân/100)
                    double s2_tyle2 = contract.Tasks.Sum(x => x.Amount * x.UnitPrice / (1 + x.VAT / 100) * (x.Tyle2 ?? 0) / 100);    // x.AccTaskGroup

                    // Giá trị % tổng khoán cá nhân của cả hợp đồng đó = S2/S1 *100
                    if (s1 != 0)
                    {
                        contract.RatioOfCompany = Math.Round((s2_tyle1 / s1 * 100.0), 2);
                        contract.RatioOfInternal = Math.Round((s2_tyle2 / s1 * 100.0), 7);
                    }
                    else
                    {
                        contract.RatioOfCompany = 0;
                        contract.RatioOfInternal = 0;
                    }
                        
                }
                else
                {
                    // chỗ này có fai set thêm 2 giá trị Value = 0 và TienTruThue = 0. 
                    contract.Value = 0;
                    contract.TienTruThue = 0;
                    contract.RatioOfCompany = 0;
                    contract.RatioOfInternal = 0;
                }


                service.Add(contract);

                // Notification
                var users = uow.Repository<Core.DomainModels.Identity.AppUser>().GetAll();
                var listUser = new List<Core.DomainModels.Identity.AppUser>();
                var directors = users.Where(x => x.Position?.Name.ToLower() == "giám đốc").ToList();
                if (directors != null)
                    listUser.AddRange(directors);

                var manager = users.Where(x => x.Department?.Id == own?.Department?.Id && x.Position?.Name.ToLower() == "trưởng phòng").FirstOrDefault();
                if (manager != null) listUser.Add(manager);

                if (listUser.Count > 0)
                {
                    var notification = new Notification()
                    {
                        content = own?.DisplayName + " đã tạo mới hợp đồng",
                        SentTime = DateTime.Now,
                        read = false,
                        target_url = "/Home/ContractDetails/" + contract.Id,
                        targetUsers = listUser
                    };

                    uow.Repository<Notification>().Insert(notification);
                    uow.SaveChanges();
                    if (own != null)
                    {
                        //own.Notifications.Add(notification);
                        uow.Repository<Core.DomainModels.Identity.AppUser>().Update(own);
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return RedirectToAction("Index", "Home");
        }
        [SetPermissions(Permission = "CapSoHD")]
        public ActionResult EditContractNumber(int id)
        {
            var model = service.GetById(id);
            return View(model);
        }

        [SetPermissions(Permission = "CapSoHD")]
        public ActionResult DeregisterContractNumber(int id)
        {
            var model = service.GetById(id);
            return View(model);
        }


        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "CapSoHD")]
        public ActionResult DeregisterContractNumber(int ContractID, string maHD)
        {
            var contract = service.GetById(ContractID);
            if(contract != null)
            {
                contract.MaHD = "";
                service.Update(contract);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Noted by lapbt: Cap so hop dong
        /// - thực hiện update MaHD
        /// - trc khi đó sẽ ktra xem thực sự chưa tồn tại thì mới update, với t.hop <> ''
        /// - ngày 3-may-2024 thêm 2 biến (ratioCompany, contractTypeId) để lưu ttin bắt buộc khi cấp số
        /// </summary>
        /// <param name="idContract"></param>
        /// <param name="maHD"></param>
        /// <param name="ratioCompany"></param>
        /// <param name="contractTypeId"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "CapSoHD")]
        public ActionResult EditContractNumber(int idContract, string maHD, string ratioCompany, int contractTypeId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // ktra hop le
                    float f_ratioCompany = Convert.ToSingle(ratioCompany.Replace('.',','));     // neu co loi convert thi xem ham nay: ChangeTiLeKhoanCT
                    if (f_ratioCompany < 50 || f_ratioCompany > 99)
                    {
                        return Json("Tỷ lệ khoán công ty từ 50 -> 99. Sử dụng dấu (.) để ngăn cách thập phân. Vd: 77.5", JsonRequestBehavior.AllowGet);
                    }
                    if (contractTypeId <= 0)
                    {
                        return Json("Phải chọn loại hợp đồng", JsonRequestBehavior.AllowGet);
                    }

                    var contract = service.GetById(idContract);
                    if (contract != null)
                    {
                        /* 21-oct-2021. Added by lapbt
                         * khi người cấp số HĐ bấm nút "cấp số" thì:
                            - Nếu hợp đồng chưa có số thì cập nhật (ghi đè) ngày hiện hành vào ô "ngày khởi tạo" 
                                của hợp đồng đó, và đổi label ô đó thành "ngày cấp số"
                            - Nếu hợp đồng đã có số rồi thì thôi không cập nhật vào ô "ngày khởi tạo"
                        */
                        if (string.IsNullOrWhiteSpace(contract.MaHD))
                        {
                            contract.CreateDate = DateTime.Now;
                        }

                        //var maHD = EditorExtension.GetValue<string>("txtMaHD");
                        if (!string.IsNullOrWhiteSpace(maHD))
                        {
                            // Added by Lapbt. Ktra MaHD có tồn tại, ở hợp đồng khác
                            var chkExisted = uow.Repository<Contract>().FindBy(x => x.MaHD == maHD && x.Id != idContract);
                            if (chkExisted.Count > 0)
                            {
                                return Json("Số hợp đồng đã tồn tại.", JsonRequestBehavior.AllowGet);
                            }

                            contract.MaHD = maHD;
                            contract.Status = ApproveStatus.ApprovedLv2;
                            contract.customer = contract.customer;
                            contract.RatioOfCompany = Math.Round(f_ratioCompany, 1);
                            contract.contractType = contractTypeId;     // su dung truong nay de luu, k0 fai tao moi
                            
                            // 11/08/2024 update luôn khoán cty vào tất cả các task của hợp đồng đó luôn
                            for (int i = 0; i < contract.Tasks.Count; i++)
                            {
                                var originalTask = contract.Tasks[i];
                                if (originalTask != null)
                                {
                                    originalTask.Tyle1 = Math.Round(f_ratioCompany, 1);
                                    uow.Repository<AccTask>().Update(originalTask);
                                }
                            }

                            // save contract & task
                            service.Update(contract);
                            uow.SaveChanges();
                        }
                        else
                        {
                            contract.MaHD = string.Empty;
                            contract.Status = ApproveStatus.ApprovedLv1;
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }

                        EquipmentDataProvider.allequipments = null;
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }

                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    return Json("error", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckSoHDAvailable(string soHd)
        {
            if (!string.IsNullOrWhiteSpace(soHd))
            {
                var accre = uow.Repository<Contract>().FindBy(x => x.MaHD == soHd);
                if (accre.Count > 0)
                    return Json("NotAvailable", JsonRequestBehavior.AllowGet);
                else
                    return Json("Available", JsonRequestBehavior.AllowGet);
            }
            else return Json("Available", JsonRequestBehavior.AllowGet);
        }

        [SetPermissions(Permission = "CreateContract")]
        public ActionResult CreateFromContract()
        {            
            var model = IncosafCMS.Web.Providers.ContractDataProvider.Contracts2.Where(x => x.UserName == User.Identity.Name).OrderByDescending(x => x.CreateDate);
            return View(model);
        }
        public ActionResult GetAllContractsForCreateFromContract()
        {
            var model = IncosafCMS.Web.Providers.ContractDataProvider.Contracts2.Where(x => x.UserName == User.Identity.Name).OrderByDescending(x => x.CreateDate);
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "CreateContract")]
        public ActionResult CreateFromContract(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (int.TryParse(EditorExtension.GetValue<string>("cmbContractOld"), out int oldContractID))
                    {
                        var oldContract = service.GetById(oldContractID);
                        if (oldContract != null)
                        {
                            var tenhdmoi = EditorExtension.GetValue<string>("txtTenHDMoi");
                            var newContract = oldContract.Copy();
                            if (!string.IsNullOrWhiteSpace(tenhdmoi)) newContract.Name = tenhdmoi;

                            foreach (var task in newContract.Tasks)
                            {
                                if (task.AccTaskNote == "KĐXD-TNLAS")
                                    task.AccTaskNote = "KĐXD-TNPTN";

                                if (task.AccTaskNote == "KĐXD-TNNOLAS")
                                    task.AccTaskNote = "KĐXD-TNHT";
                                uow.Repository<AccTask>().Insert(task);
                            }

                            uow.SaveChanges();
                            service.Add(newContract);

                            // Notification
                            var own = newContract.own;
                            var users = uow.Repository<Core.DomainModels.Identity.AppUser>().GetAll();
                            var listUser = new List<Core.DomainModels.Identity.AppUser>();
                            var directors = users.Where(x => x.Position?.Name.ToLower() == "giám đốc").ToList();
                            if (directors != null)
                                listUser.AddRange(directors);

                            var manager = users.Where(x => x.Department?.Id == own?.Department?.Id && x.Position?.Name.ToLower() == "trưởng phòng").FirstOrDefault();
                            if (manager != null) listUser.Add(manager);

                            var notification = new Notification()
                            {
                                content = own?.DisplayName + " đã tạo mới hợp đồng",
                                SentTime = DateTime.Now,
                                read = false,
                                target_url = "/Home/ContractDetails/" + newContract.Id,
                                targetUsers = listUser
                            };

                            uow.Repository<Notification>().Insert(notification);
                            uow.SaveChanges();
                            if (own != null)
                            {
                                //own.Notifications.Add(notification);
                                uow.Repository<Core.DomainModels.Identity.AppUser>().Update(own);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }

            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return RedirectToAction("Index", "Home");
        }
        // GET: Contracts/Edit/5. Bắt sự kiện khi click vào menu Chỉnh sửa hợp đồng
        public ActionResult Edit(int id)
        {
            Contract model = new Contract();
            var provinces = new Service<Province>(uow).GetAll();
            //var customers = LargeDatabaseDataProvider.Customers;
            var repProprietor = new Service<Proprietor>(uow);

            if (User.IsInRole("KDV"))
                model = service.FindBy(x => x.Id == id && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault();
            else
                model = service.GetById(id);
            
            var Proprietors = repProprietor.GetAll();
            Proprietors.Insert(0, new Proprietor() { Id = -1, PropName = "[Tạo mới người sử dụng]" });
            ViewData["Proprietors"] = Proprietors;

            ViewData["Proprietor"] = model.Proprietor ?? new Proprietor();
            ViewData["provinces"] = provinces;
            AccTaskList.GetAccTasks = model.Tasks ?? new List<AccTask>();

            /* 07/03/2023 edited by lapbt. Sửa với quyền <> admin, nếu là hđồng chưa cấp số thì vẫn cho sửa chọn lại KDV1, KDV2. Khi đó
             * - sửa ở đây để load lên cả toàn bộ ds
             * - ở giao diện sẽ ktra để locked lại nếu đã cấp số
             */
            ViewData["Employees"] = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.PhoneNumber == "1")
                                        .OrderBy(x => x.DisplayName)
                                        .ToList(); ;
            ViewData["AppUsersKDV2"] = ViewData["Employees"];
            /*
            ViewData["AppUsersKDV2"] = uow.Repository<Core.DomainModels.Identity.AppUser>()
                                        .FindBy(x => x.TwoFactorEnabled == true)
                                        .OrderBy(x => x.DepartmentId)                                        
                                        .ToList();          
            */
            return View(model);
        }

        //Hưng thêm 12.06.2025
        public JsonResult GetProprietorAddress(int id)
        {
            var proprietor = uow.Repository<Proprietor>().FindBy(x => x.Id == id).FirstOrDefault();            
            if (proprietor != null)
            {
                return Json(new { propAddress = proprietor.PropAddress }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { propAddress = "" }, JsonRequestBehavior.AllowGet);
        }

        // POST: Contracts/Edit/5. Nhấn nút Lưu (save) ở cửa sổ Chỉnh sửa hợp đồng
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                bool contractStatusChanged = false;
                var contract = service.GetById(id);
                if (contract != null)
                {
                    if (User.IsInRole("Admin") || User.IsInRole("Accountant")
                        || (User.IsInRole("DeptDirector") && contract.Status != ApproveStatus.ApprovedLv2)
                        || (/*User.IsInRole("KDV") && */contract.Status == ApproveStatus.Waiting))
                    {
                        contract.Name = EditorExtension.GetValue<string>("txtName");
                        contract.SignDate = EditorExtension.GetValue<DateTime>("dteSignDate");

                        contract.CreateDate = EditorExtension.GetValue<DateTime>("dteCreateDate");

                        contract.NgayThucHien = EditorExtension.GetValue<DateTime>("dteNgayThucHien");
                        //10.5.2025 Hung them truong NgayThucHienDen để lưu thông tin ngày thực hiện đến ngày ...                        
                        if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("dteNgayThucHienDen")))
                        {
                            contract.NgayThucHienDen = EditorExtension.GetValue<DateTime>("dteNgayThucHienDen");
                        }
                        else
                            contract.NgayThucHienDen = null;//Kiểm tra nếu ô Đến ngày rỗng thì Lưu vào db NgaythucHienDen = null

                        var provinceIdstr = EditorExtension.GetValue<string>("cmbProvinceOfContractEdit");

                        var s = int.TryParse(provinceIdstr, out int provinceId);

                        if (s)
                        {
                            var province = uow.Repository<Province>().GetSingle(provinceId);
                            contract.DiaDiemThucHien = province.ProvinceName;
                        }
                        else
                            contract.DiaDiemThucHien = provinceIdstr;

                        if (User.IsInRole("Admin") && !string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbEmployees_Edit")))
                        {
                            var ownID = EditorExtension.GetValue<int>("cmbEmployees_Edit");
                            var own = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Id == ownID).FirstOrDefault();
                            if (own != null) contract.own = own;
                        }

                        if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbCustomerID_ContractEdit")))
                        {
                            int customerID;
                            if (int.TryParse(EditorExtension.GetValue<string>("cmbCustomerID_ContractEdit"), out customerID))
                                contract.customer = uow.Repository<Customer>().GetSingle(customerID);
                        }

                        var strIsGiayDeNghi = EditorExtension.GetValue<string>("cbIsGiayDeNghiEdit");
                        bool isGiayDeNghi = strIsGiayDeNghi == "True";
                        contract.IsGiayDeNghi = isGiayDeNghi;

                        var contractTypeId = EditorExtension.GetValue<int>("cboContractTypeId");
                        if (contractTypeId <= 0)
                        {
                            // return Json("Phải chọn loại hợp đồng", JsonRequestBehavior.AllowGet);
                        } else if (User.IsInRole("Admin") || User.IsInRole("Accountant") || string.IsNullOrEmpty(contract.MaHD))
                        {
                            contract.contractType = contractTypeId;     // su dung truong nay de luu, k0 fai tao moi
                        }

                        contract.NguoiLienHe = EditorExtension.GetValue<string>("txtNguoiLienHe");
                        contract.DienThoaiNguoiLienHe = EditorExtension.GetValue<string>("txtDienThoaiNguoiLienHe");

                        // Xử lý Task
                        for (int i = 0; i < contract.Tasks.Count; i++)
                        {
                            var originalTask = contract.Tasks[i];
                            var modifiedTask = AccTaskList.GetAccTasks.Find(x => x.Id == originalTask.Id);
                            if (modifiedTask != null)
                            {
                                originalTask.Name = modifiedTask.Name;
                                originalTask.Unit = modifiedTask.Unit;
                                originalTask.Amount = modifiedTask.Amount;
                                originalTask.AccTaskGroup = modifiedTask.AccTaskGroup;
                                originalTask.AccTaskNote = modifiedTask.AccTaskNote;
                                originalTask.UnitPrice = modifiedTask.UnitPrice;
                                originalTask.vatType = modifiedTask.vatType;
                                originalTask.VAT = modifiedTask.VAT;
                                originalTask.Tyle1 = Math.Round(modifiedTask.Tyle1 ?? 0,2);
                                // Tyle2 k0 xly ở client đc, nên ktra ở đây để k0 thực hiện update
                                if (User.IsInRole("Admin") || User.IsInRole("Accountant") || string.IsNullOrEmpty(contract.MaHD))
                                    originalTask.Tyle2 = Math.Round(modifiedTask.Tyle2 ?? 0, 4);
                                originalTask.Tyle3 = Math.Round(modifiedTask.Tyle3 ?? 0, 2);
                                originalTask.Tyle4 = Math.Round(modifiedTask.Tyle4 ?? 0, 4);

                                uow.Repository<AccTask>().Update(originalTask);
                                //uow.SaveChanges();
                            }
                            else
                            {
                                var accreditations = originalTask.Accreditations;
                                var rems = new List<Accreditation>();
                                foreach (var accreditation in accreditations)
                                {
                                    if (accreditation.equiment != null)
                                    {
                                        var equipment = accreditation.equiment;
                                        equipment.contract = null;
                                        var partions = equipment.Partions;
                                        for (int k = 0; k < partions.Count; k++)
                                        {
                                            var partion = partions[k];
                                            equipment.Partions.Remove(partion);
                                            uow.Repository<EquipmentPartion>().Delete(partion);
                                            k--;
                                        }

                                        var loadTests = equipment.LoadTests;
                                        for (int g = 0; g < loadTests.Count; g++)
                                        {
                                            var loadTest = loadTests[g];
                                            equipment.LoadTests.Remove(loadTest);
                                            uow.Repository<LoadTest>().Delete(loadTest);
                                            g--;
                                        }

                                        var specifications = equipment.specifications;
                                        for (int h = 0; h < specifications.Count; h++)
                                        {
                                            var specification = specifications[h];
                                            equipment.specifications.Remove(specification);
                                            uow.Repository<Specifications>().Delete(specification);
                                            h--;
                                        }

                                        var technicaldocuments = equipment.TechnicalDocuments;
                                        for (int m = 0; m < technicaldocuments.Count; m++)
                                        {
                                            var technicaldocument = technicaldocuments[m];
                                            equipment.TechnicalDocuments.Remove(technicaldocument);
                                            uow.Repository<TechnicalDocument>().Delete(technicaldocument);
                                            m--;
                                        }
                                    }
                                    rems.Add(accreditation);
                                }

                                foreach (var ac in rems)
                                {
                                    originalTask.Accreditations.Remove(ac);
                                    uow.Repository<Accreditation>().Delete(ac);
                                }

                                contract.Tasks.Remove(originalTask);
                                uow.Repository<AccTask>().Delete(originalTask);
                                i--;
                            }
                        }

                        foreach (var taskNew in AccTaskList.GetAccTasks)
                        {
                            var oriTask = contract.Tasks.Find(x => x.Id == taskNew.Id);
                            if (oriTask != null) continue;
                            var taskNewCopy = new AccTask()
                            {
                                Id = 0,
                                Name = taskNew.Name,
                                Unit = taskNew.Unit,
                                Amount = taskNew.Amount,
                                AccTaskGroup = taskNew.AccTaskGroup,
                                AccTaskNote = taskNew.AccTaskNote,
                                UnitPrice = taskNew.UnitPrice,
                                vatType = taskNew.vatType,
                                VAT = taskNew.VAT,
                                Tyle1 = Math.Round(taskNew.Tyle1 ?? 0,2),
                                Tyle2 = Math.Round(taskNew.Tyle2 ?? 0, 4),
                                Tyle3 = Math.Round(taskNew.Tyle3 ?? 0, 2),
                                Tyle4 = Math.Round(taskNew.Tyle4 ?? 0, 4),
                                Accreditations = taskNew.Accreditations
                            };
                            contract.Tasks.Add(taskNewCopy);
                            uow.Repository<AccTask>().Insert(taskNewCopy);
                            //uow.SaveChanges();
                        }

                        
                        if (contract.Tasks.Count > 0)
                        {
                            // lapbt edited 30-may-2021. Bo sung tinh lai so tien chua thue
                            contract.Value = contract.Tasks.Sum(x => x.Amount * x.UnitPrice);
                            contract.TienTruThue = contract.Value / (1 + contract.VAT / 100);

                            // lapbt edited 03-jan-2024. Tính toán lại tỷ lệ khoán ở hợp đồng thông qua tính từ chi tiết khoán cá nhân ở đầu việc
                            // Tính tổng giá trị chưa VAT của tất cả các dòng (S1) = Sum(khối lượng * đơn giá /(1+VAT/100))
                            double s1 = contract.Tasks.Sum(x => x.Amount * x.UnitPrice / (1 + x.VAT / 100));

                            // 10/08/2024 tính trung bình khoán công ty
                            double s2_tyle1 = contract.Tasks.Sum(x => x.Amount * x.UnitPrice / (1 + x.VAT / 100) * (x.Tyle1 ?? 0) / 100);
                            // Tính giá trị khoán cá nhân của toàn bộ các dòng trong hợp đồng đó: S2 = Sum(khối lượng * đơn giá / (1 + VAT/100) * khoán cá nhân/100)
                            double s2_tyle2 = contract.Tasks.Sum(x => x.Amount * x.UnitPrice / (1 + x.VAT / 100) * (x.Tyle2 ?? 0)/100); // x.AccTaskGroup

                            // Giá trị % tổng khoán cá nhân của cả hợp đồng đó = S2/S1 *100
                            if (s1 != 0)
                            {
                                contract.RatioOfCompany = Math.Round((s2_tyle1 / s1 * 100.0), 2);
                                contract.RatioOfInternal = Math.Round((s2_tyle2 / s1 * 100.0), 7);
                            }
                            else
                            {
                                contract.RatioOfCompany = 0;
                                contract.RatioOfInternal = 0;
                            }
                                
                        }
                        else
                        {
                            // chỗ này có fai set thêm 2 giá trị Value = 0 và TienTruThue = 0. 
                            contract.Value = 0;
                            contract.TienTruThue = 0;
                            contract.RatioOfCompany = 0;
                            contract.RatioOfInternal = 0;
                        }

                        contract.ResponsibilityA = EditorExtension.GetValue<string>("meResponsibilityA");
                        contract.ResponsibilityB = EditorExtension.GetValue<string>("meResponsibilityB");
                        contract.PaymentMethod = EditorExtension.GetValue<string>("mePaymentMethod");
                        contract.Commitments = EditorExtension.GetValue<string>("meCommitments");
                        contract.Effective = EditorExtension.GetValue<string>("meEffective");

                        if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbVATType")))
                        {
                            if (int.TryParse(EditorExtension.GetValue<string>("cmbVATType"), out int vatType))
                            {
                                if (contract.vatType != (VATType)vatType)
                                {
                                    contract.vatType = (VATType)vatType;
                                }
                            }
                        }

                        if (contract.vatType == VATType.cothue)
                        {
                            if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbContractVAT")))
                            {
                                if (int.TryParse(EditorExtension.GetValue<string>("cmbContractVAT"), out int vat))
                                {
                                    contract.VAT = vat;
                                }
                            }
                        }
                        else
                        {
                            contract.VAT = 0;
                        }

                        if (!string.IsNullOrWhiteSpace(contract.MaHD)) { contract.Status = ApproveStatus.ApprovedLv2; }
                        else if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("cmbStatus")))
                        {
                            if (int.TryParse(EditorExtension.GetValue<string>("cmbStatus"), out int statusID))
                            {
                                if (string.IsNullOrEmpty(contract.MaHD) && (ApproveStatus)statusID == ApproveStatus.ApprovedLv2)
                                {
                                    contract.Status = contract.Status;
                                    contractStatusChanged = false;
                                }
                                else if (contract.Status != (ApproveStatus)statusID)
                                {
                                    contract.Status = (ApproveStatus)statusID;
                                    contractStatusChanged = true;
                                }
                            }
                        }

                        // Added by Lapbt 23/08/2022. Save KDV1 & KDV2
                        if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("KDV1")))
                        {
                            int kdv1_id = EditorExtension.GetValue<int>("KDV1");
                            var kdv1 = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Id == kdv1_id).FirstOrDefault();

                            if (kdv1 != null) contract.KDV1 = kdv1;
                        }
                        if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("KDV2")))
                        {
                            int kdv2_id = EditorExtension.GetValue<int>("KDV2");
                            var kdv2 = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Id == kdv2_id).FirstOrDefault();

                            if (kdv2 != null) contract.KDV2 = kdv2;
                        }

                        // Xử lý thông tin người sử dụng                        
                        string strPropName = Request.Form["cmbProprietorsOfContractEdit"]; // Lay Text go moi tu o chứ không phải Value                        
                        string strPropAddress = EditorExtension.GetValue<string>("txtPropAddressEdit");
                        //Neu DVSD da co:
                        int proprietorID;
                        if (int.TryParse(EditorExtension.GetValue<string>("cmbProprietorsOfContractEdit"), out proprietorID) && proprietorID > 0)
                        {
                            var proprietor = uow.Repository<Proprietor>().GetSingle(proprietorID);
                            //Neu khong thay doi thi giu nguyen
                            if (!string.IsNullOrWhiteSpace(strPropName) && proprietor != null && strPropName == proprietor.PropName)
                            {
                                contract.Proprietor = proprietor;
                            }
                                                        
                            //Nếu tên ĐVSD không thay đổi, có thay đổi địa chỉ thì update
                            if (!string.IsNullOrWhiteSpace(strPropAddress) && strPropName == proprietor.PropName && strPropAddress != proprietor.PropAddress)
                            {  
                                proprietor.PropAddress = strPropAddress;                                
                                uow.Repository<Proprietor>().Update(proprietor);
                                uow.SaveChanges();
                            }
                            //Nếu sua ô Tên ĐVSD trống: thì đặt ĐVSD ở contract = null
                            if (string.IsNullOrWhiteSpace(strPropName))
                            {
                                contract.Proprietor = null;
                                service.Update(contract);
                            }
                            
                        }
                        else
                        {
                            //Neu dang chua chon DVSD (proprietorID<=0) va O ten DVSD khong trong thi cap nhat moi
                            if (!string.IsNullOrWhiteSpace(strPropName) && proprietorID <= 0)
                            {
                                var newProprietor = new Proprietor()
                                {
                                    Id = 0,
                                    PropName = strPropName,
                                    PropAddress = strPropAddress,
                                    PropDepartment = contract.own?.Department
                                };
                                contract.Proprietor = newProprietor;
                                uow.Repository<Proprietor>().Insert(newProprietor);
                                uow.SaveChanges();
                                service.Update(contract);
                            }                           
                        }    

                        uow.SaveChanges();
                        service.Update(contract);

                        // Notification
                        if (contractStatusChanged)
                        {
                            var own = contract.own;
                            var users = uow.Repository<Core.DomainModels.Identity.AppUser>().GetAll();
                            var listUser = new List<Core.DomainModels.Identity.AppUser>();
                            var directors = users.Where(x => x.Position?.Name.ToLower() == "giám đốc" && x.UserName != User.Identity.Name).ToList();
                            if (directors != null)
                                listUser.AddRange(directors);

                            var manager = users.Where(x => x.Department?.Id == own?.Department?.Id && x.Position?.Name.ToLower() == "trưởng phòng").FirstOrDefault();
                            if (manager != null) listUser.Add(manager);

                            if (own != null)
                                listUser.Add(own);

                            if (listUser.Count > 0)
                            {
                                var notification = new Notification()
                                {
                                    content = own?.DisplayName + " đã thay đổi trạng thái hợp đồng",
                                    SentTime = DateTime.Now,
                                    read = false,
                                    target_url = "/Home/ContractDetails/" + contract.Id,
                                    targetUsers = listUser
                                };

                                uow.Repository<Notification>().Insert(notification);
                                uow.SaveChanges();
                                if (own != null)
                                {
                                    //own.Notifications.Add(notification);
                                    uow.Repository<Core.DomainModels.Identity.AppUser>().Update(own);
                                }
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return RedirectToAction("Index", "Home");

        }

        //thêm 27.12.2025
        public ActionResult GetEmployees(ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var query = uow.Repository<Core.DomainModels.Identity.AppUser>().GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(e.Filter))
            {
                query = query.Where(x => x.DisplayName.Contains(e.Filter));
            }

            var data = query
                .OrderBy(x => x.DisplayName)
                .Skip(e.BeginIndex)
                .Take(e.EndIndex - e.BeginIndex + 1)
                .Select(x => new
                {
                    x.Id,
                    x.DisplayName
                })
                .ToList();

            return PartialView("ComboBoxItems", data);
        }
       
        public ActionResult GetAllCustomersForContractEdit()
        {
            var user = userManager.FindByName(User.Identity.Name);
            var allUser = uow.Repository<Customer>().GetAll();
            var model = GridViewHelper.SelectedContractID > 0 ? service.GetById(GridViewHelper.SelectedContractID) : null;
            ViewBag.CustomerId = model != null ? model.customer?.Id.ToString() : string.Empty;
            ViewData["Customers"] = User.IsInRole("Admin") ? allUser : allUser.Where(x => x.department == null || x.department.Id == user?.Department?.Id).ToList();
            return PartialView(model);
        }        

        public ActionResult GetAllCustomersForContractCreate()
        {
            var user = userManager.FindByName(User.Identity.Name);
            var allUser = uow.Repository<Customer>().GetAll();
            ViewBag.CustomerId = allUser.LastOrDefault()?.Id;
            ViewData["Customers"] = User.IsInRole("Admin") ? allUser : allUser.Where(x => x.department == null || x.department.Id == user?.Department?.Id).ToList();
            return PartialView();
        }       

        // GET: Contracts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contracts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var result = "false";
            string Message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    var contract = User.IsInRole("KDV") ? service.GetAll().Where(x => x.Id == id && x.own?.UserName == User.Identity.Name).FirstOrDefault() : service.GetById(id);
                    if (contract != null)
                    {                                                
                        //Hưng sửa 25.2.2025 - Chỉ xóa Hợp đồng khi là Admin hoặc HĐ chưa cấp số
                        if (!User.IsInRole("Admin") && (!string.IsNullOrEmpty(contract.MaHD)))
                        {                           
                           Message = "HĐ đã cấp số, Không được xóa. Liên hệ Admin! - Số HĐ: " + contract.MaHD;
                           return Json(new object[] { result, Message }, JsonRequestBehavior.AllowGet);                        
                        }
                        //Hưng sửa 25.2.2025 - Chỉ xóa Hợp đồng khi là Admin hoặc HĐ chưa lấy số KQKĐ
                        var equipments = uow.Repository<Equipment>().FindBy(x => x.contract.Id == contract.Id);
                        foreach (var equipment in equipments)
                        {                            
                            if (!User.IsInRole("Admin") && (equipment.contract != null))
                            {
                                Message = "HĐ đã lấy số KQ kỹ thuật, Số lượng: " + equipments.Count + " " + equipment.Name + ". Không được xóa. Liên hệ Admin!" ;
                                return Json(new object[] { result, Message }, JsonRequestBehavior.AllowGet);
                            }
                            var accreditation = uow.Repository<Accreditation>().FindBy(x => x.equiment.Id == equipment.Id);                            
                            foreach (var accred in accreditation)
                            {
                                if (User.IsInRole("Admin") && (accred.NumberAcc != null))
                                {
                                    accred.StampNumber = null;
                                    accred.SerialNumber = null;
                                    accred.NumberAcc = null;
                                    uow.Repository<Accreditation>().Delete(accred);//Xóa ở bảng Accreditation
                                }
                            }
                            if (User.IsInRole("Admin") && (equipment != null))
                            {
                                uow.Repository<Equipment>().Delete(equipment);//Xóa ở bảng Equipment
                            }
                        }       

                        uow.Repository<Contract>().Delete(contract);//xóa ở bảng Contract
                        uow.SaveChanges();
                        result = "true";
                        Message = "Đã xóa!";
                        return Json(new object[] { result, Message }, JsonRequestBehavior.AllowGet);                           
                            
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    Message = "Đã có lỗi xảy ra!";
                    return Json(new object[] { result, Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
                Message = "Đã có lỗi xảy ra!";
                return Json(new object[] { result, Message }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", "Home");
        }         

        public ActionResult GetContractById(int id)
        {
            var contract = LargeDatabaseDataProvider.GetContractById(id);
           
            return Json(contract, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeTiLeKhoanCT(int contractId, string tilekhoanCT)
        {
            var contract = service.GetById(contractId);
            if (contract != null)
            {
                double tilekhoan = 0;
                if (double.TryParse(tilekhoanCT.Replace('.', ','), out tilekhoan))        //  fai thay dau o day
                {
                    if ((User.IsInRole("Admin") || User.IsInRole("Accountant") || string.IsNullOrEmpty(contract.MaHD)) 
                        && (tilekhoan >= 50 && tilekhoan <= 99))
                    {
                        contract.RatioOfCompany = tilekhoan;
                        service.Update(contract);
                        return Json("Valid", JsonRequestBehavior.AllowGet);
                    } else
                    {
                        return Json("NotValid", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("NotValid", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeTiLeKhoanCN(int contractId, string tilekhoanCN)
        {
            var contract = service.GetById(contractId);
            if (contract != null)
            {
                double tilekhoan = 0;
                if (double.TryParse(tilekhoanCN, out tilekhoan))
                {
                    contract.RatioOfInternal = tilekhoan;
                    service.Update(contract);
                    return Json("Valid", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("NotValid", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cập nhật IsGiayDeNghi tại trang home, chi tiết bên phải
        /// Added by lapbt, 23-jan-2024
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="contractIsGiayDeNghi"></param>
        /// <returns></returns>
        public ActionResult ChangeIsGiayDeNghi(int contractId, bool contractIsGiayDeNghi)
        {
            var contract = service.GetById(contractId);
            if (contract != null)
            {
                if (User.IsInRole("Admin") || string.IsNullOrEmpty(contract.MaHD))   // chk right here 22/02/24 chưa cấp số thoải mái, đã cấp chỉ admin sửa. User.IsInRole("DeptDirector") || User.IsInRole("Accountant")
                {
                    contract.IsGiayDeNghi = contractIsGiayDeNghi;
                    service.Update(contract);
                    return Json("Valid", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("NotValid", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult TaskViewCreateActionPartial()
        {
            var user = userManager.FindByName(User.Identity.Name);
            ViewBag.departmentId = user.Department.Id;
            return PartialView("_TaskViewCreateActionPartial", AccTaskList.GetAccTasks);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TaskViewCreateActionPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] AccTask task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccTaskList.AddAccTask(task);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TaskViewCreateActionPartial", AccTaskList.GetAccTasks);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TaskViewCreateActionPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] AccTask task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccTaskList.UpdateAccTask(task);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TaskViewCreateActionPartial", AccTaskList.GetAccTasks);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TaskViewCreateActionPartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    AccTaskList.DeleteAccTask(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_TaskViewCreateActionPartial", AccTaskList.GetAccTasks);
        }
        [ValidateInput(false)]
        public ActionResult TaskViewEditActionPartial()
        {
            var user = userManager.FindByName(User.Identity.Name);
            ViewBag.departmentId = user.Department.Id;
            return PartialView("_TaskViewEditActionPartial", AccTaskList.GetAccTasks);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TaskViewEditActionPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] AccTask task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccTaskList.AddAccTask(task);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TaskViewEditActionPartial", AccTaskList.GetAccTasks);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TaskViewEditActionPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] AccTask task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccTaskList.UpdateAccTask(task);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TaskViewEditActionPartial", AccTaskList.GetAccTasks);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TaskViewEditActionPartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    AccTaskList.DeleteAccTask(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_TaskViewEditActionPartial", AccTaskList.GetAccTasks);
        }
        public ActionResult CustomCallBackTasksOfContractAction(int? selectedcontract)
        {
            //if (string.IsNullOrEmpty(selectedcontract.ToString()) || selectedcontract < 0) GridViewHelper.SelectedContractID = -1;
            if (!selectedcontract.HasValue || selectedcontract < 0)
            {
                GridViewHelper.SelectedContractID = -1;
            }
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return TasksOfContractPartial();
        }        
        [ValidateInput(false)]
        public ActionResult TasksOfContractPartial()
        {
            if (GridViewHelper.SelectedContractID < 0)
            {
                var model = new List<AccTask>();
                return PartialView("_TasksOfContractPartial", model);
            }
            else
            {
                var contract = service.GetById(GridViewHelper.SelectedContractID);                
                var model = contract?.Tasks ?? new List<AccTask>();

                //var accTask = uow.Repository<AccTask>().FindBy(x => x.Contract_Id == GridViewHelper.SelectedContractID);
                //var model = accTask ?? new List<AccTask>();

                return PartialView("_TasksOfContractPartial", model);
            }
        }

        /// <summary>
        /// Added by Lapbt 10-mar-2025
        /// 2 hàm dưới thêm vào để load các TB KĐ từ 1 task đc chọn ở cửa sổ bên phải, chi tiết hợp đồng, tại home
        /// </summary>
        /// <param name="selectedtaskofcontract"></param>
        /// <returns></returns>
        public ActionResult CustomCallBackAccreditationOfTaskAction(int? selectedtaskofcontract)
        {
            if (!selectedtaskofcontract.HasValue || selectedtaskofcontract < 0)
            {
                GridViewHelper.SelectedTaskID = -1;
            }
            else GridViewHelper.SelectedTaskID = selectedtaskofcontract.Value;
            return AccreditationOfTaskContractPartial();
        }
        [ValidateInput(false)]

        public ActionResult AccreditationOfTaskContractPartial()
        {
            if (GridViewHelper.SelectedTaskID < 0)
            {
                var model = new List<Accreditation>();
                return PartialView("_AccreditationOfTaskContractPartial", model);
            }
            else
            {
                // cần lấy ds các Tbi kđ, trong task ở đây                
                var accreditation = uow.Repository<Accreditation>().FindBy(x => x.AccTask_Id == GridViewHelper.SelectedTaskID);               
                var model = accreditation ?? new List<Accreditation>(); 
                return PartialView("_AccreditationOfTaskContractPartial", model);
            }
        }
      
        public ActionResult CustomCallBackTurnOversOfDetailContractAction(int? selectedcontract)
        {
            if (!selectedcontract.HasValue || string.IsNullOrEmpty(selectedcontract.ToString()) || selectedcontract < 0) GridViewHelper.SelectedContractID = -1;
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return TurnOversOfDetailContractPartial();
        }
        [ValidateInput(false)]
        public ActionResult TurnOversOfDetailContractPartial()
        {
            if (GridViewHelper.SelectedContractID < 0)
            {
                var model = new List<TurnOver>();
                return PartialView("_TurnOversOfDetailContractPartial", model);
            }
            else
            {
                var contract = service.GetById(GridViewHelper.SelectedContractID);
                var model = contract?.TurnOvers ?? new List<TurnOver>();

                return PartialView("_TurnOversOfDetailContractPartial", model);
            }
        }

        public ActionResult TurnOversOfInvoiceContractPartial()
        {
            var model = ReportHelper.InvoiceReportDataSource.InvoiceTurnOvers;
            return PartialView("_TurnOversOfInvoiceContractPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult TurnOversOfContractPartialAddNewViaContract([ModelBinder(typeof(DevExpressEditorsBinder))] TurnOver TurnOver)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    if (contract != null)
                    {
                        contract.TurnOvers.Add(TurnOver);
                        contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                        contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TurnOversOfDetailContractPartial", contract.TurnOvers);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult TurnOversOfContractPartialUpdateViaContract([ModelBinder(typeof(DevExpressEditorsBinder))] TurnOver TurnOver)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    if (contract != null)
                    {
                        var originalTurnOver = contract.TurnOvers.Where(x => x.Id == TurnOver.Id).FirstOrDefault();
                        if (originalTurnOver != null)
                        {
                            originalTurnOver.TurnOverName = TurnOver.TurnOverName;
                            originalTurnOver.TurnOverDate = TurnOver.TurnOverDate;
                            originalTurnOver.BLNumber = TurnOver.BLNumber;
                            originalTurnOver.BLValue = TurnOver.BLValue;
                            originalTurnOver.HDNumber = TurnOver.HDNumber;
                            originalTurnOver.HDValue = TurnOver.HDValue;
                            originalTurnOver.VAT = TurnOver.VAT;
                            originalTurnOver.TurnOverNote = TurnOver.TurnOverNote;
                        }

                        contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                        contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TurnOversOfDetailContractPartial", contract.TurnOvers);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult TurnOversOfContractPartialDeleteViaContract(System.Int32 Id)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (Id >= 0)
            {
                try
                {
                    if (contract != null)
                    {
                        var TurnOver = contract.TurnOvers.Where(x => x.Id == Id).FirstOrDefault();
                        if (TurnOver != null)
                        {
                            var count = TurnOver.Payments.Count;
                            for (int i = 0; i < count; i++)
                            {
                                var payment = TurnOver.Payments[i];
                                TurnOver.Payments.Remove(payment);
                            }
                            contract.TurnOvers.Remove(TurnOver);
                            uow.Repository<TurnOver>().Delete(TurnOver);
                            uow.SaveChanges();
                        }

                        contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                        contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_TurnOversOfDetailContractPartial", contract.TurnOvers);
        }
        public ActionResult CustomCallBackPaymentsOfDetailContractAction(int? selectedcontract)
        {
            if (!selectedcontract.HasValue || string.IsNullOrEmpty(selectedcontract.ToString()) || selectedcontract < 0) GridViewHelper.SelectedContractID = -1;
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return PaymentsOfDetailContractPartial();
        }
        [ValidateInput(false)]
        public ActionResult PaymentsOfDetailContractPartial()
        {
            var model = new List<Payment>();
            if (GridViewHelper.SelectedContractID < 0)
            {
                ViewData["TurnOvers"] = new List<TurnOver>() { new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" } };
                return PartialView("_PaymentsOfDetailContractPartial", model);
            }
            else
            {
                var contract = service.GetById(GridViewHelper.SelectedContractID);
                model = contract?.Payments;

                if (model == null)
                {
                    ViewData["TurnOvers"] = new List<TurnOver>() { new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" } };
                    model = new List<Payment>();
                }
                else
                {
                    var turnOvers = contract?.TurnOvers;
                    turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                    ViewData["TurnOvers"] = turnOvers;
                }

                return PartialView("_PaymentsOfDetailContractPartial", model);
            }
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult PaymentsOfContractPartialAddNewViaContract([ModelBinder(typeof(DevExpressEditorsBinder))] Payment payment)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(payment.turnOver.HDNumber) || int.Parse(payment.turnOver.HDNumber) <= 0)
                    {
                        if (contract != null)
                        {
                            payment.turnOver = null;                            
                            contract.Payments.Add(payment);
                            contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                            contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                            contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }
                    }
                    else
                    {
                        if (contract != null)
                        {
                            var idHoaDon = int.Parse(payment.turnOver.HDNumber);
                            var HoaDon = uow.Repository<TurnOver>().GetSingle(idHoaDon);
                            payment.turnOver = HoaDon;
                            HoaDon.Payments.Add(payment);
                            contract.Payments.Add(payment);
                            contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                            contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                            contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }
                    }
                }
                catch (Exception e)
                {
                    var turnOvers = contract == null ? new List<TurnOver>() : contract.TurnOvers;
                    turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                    ViewData["TurnOvers"] = turnOvers;
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var turnOvers = contract == null ? new List<TurnOver>() : contract.TurnOvers;
                turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                ViewData["TurnOvers"] = turnOvers;
                ViewData["EditError"] = "Please, correct all errors.";
            }
            return PartialView("_PaymentsOfDetailContractPartial", contract.Payments);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult PaymentsOfContractPartialUpdateViaContract([ModelBinder(typeof(DevExpressEditorsBinder))] Payment payment)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(payment.turnOver.HDNumber) || int.Parse(payment.turnOver.HDNumber) <= 0)
                    {
                        if (contract != null)
                        {
                            var originalPayment = contract.Payments.Where(x => x.Id == payment.Id).FirstOrDefault();
                            if (originalPayment != null)
                            {
                                if (originalPayment.turnOver != null)
                                    originalPayment.turnOver.Payments.Remove(originalPayment);

                                originalPayment.turnOver = null;
                                originalPayment.PaymentName = payment.PaymentName;
                                originalPayment.PaymentDate = payment.PaymentDate;
                                originalPayment.PaymentValue = payment.PaymentValue;
                                originalPayment.PaymentNumber = payment.PaymentNumber;
                                originalPayment.PaymentNote = payment.PaymentNote;
                                originalPayment.PaymentMethod = payment.PaymentMethod;
                            }

                            contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                            contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                            contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }
                    }
                    else
                    {
                        if (contract != null)
                        {
                            var originalPayment = contract.Payments.Where(x => x.Id == payment.Id).FirstOrDefault();
                            if (originalPayment != null)
                            {
                                var idHoaDon = int.Parse(payment.turnOver.HDNumber);
                                var HoaDon = uow.Repository<TurnOver>().GetSingle(idHoaDon);

                                if (originalPayment.turnOver == null)
                                {
                                    HoaDon.Payments.Add(originalPayment);
                                    originalPayment.turnOver = HoaDon;
                                }
                                else
                                {
                                    originalPayment.turnOver.Payments.Remove(originalPayment);
                                    HoaDon.Payments.Add(originalPayment);
                                    originalPayment.turnOver = HoaDon;
                                }

                                originalPayment.PaymentName = payment.PaymentName;
                                originalPayment.PaymentDate = payment.PaymentDate;
                                originalPayment.PaymentValue = payment.PaymentValue;
                                originalPayment.PaymentNumber = payment.PaymentNumber;
                                originalPayment.PaymentNote = payment.PaymentNote;
                                originalPayment.PaymentMethod = payment.PaymentMethod;
                            }

                            contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                            contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                            contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }
                    }
                }
                catch (Exception e)
                {
                    var turnOvers = contract == null ? new List<TurnOver>() : contract.TurnOvers;
                    turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                    ViewData["TurnOvers"] = turnOvers;
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var turnOvers = contract == null ? new List<TurnOver>() : contract.TurnOvers;
                turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                ViewData["TurnOvers"] = turnOvers;
                ViewData["EditError"] = "Please, correct all errors.";
            }
            return PartialView("_PaymentsOfDetailContractPartial", contract.Payments);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult PaymentsOfContractPartialDeleteViaContract(System.Int32 Id)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (Id >= 0)
            {
                try
                {
                    if (contract != null)
                    {
                        var payment = contract.Payments.Where(x => x.Id == Id).FirstOrDefault();
                        if (payment != null)
                        {
                            payment.turnOver?.Payments.Remove(payment);
                            contract.Payments.Remove(payment);
                            uow.Repository<Payment>().Delete(payment);
                            uow.SaveChanges();
                        }

                        contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                        contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                        contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_PaymentsOfDetailContractPartial", contract.Payments);
        }
        public ActionResult CustomCallBackInternalPaymentsOfDetailContractAction(int? selectedcontract)
        {
            if (!selectedcontract.HasValue || string.IsNullOrEmpty(selectedcontract.ToString()) || selectedcontract < 0) 
                GridViewHelper.SelectedContractID = -1;
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return InternalPaymentsOfDetailContractPartial();
        }
        [ValidateInput(false)]
        public ActionResult InternalPaymentsOfDetailContractPartial()
        {
            if (GridViewHelper.SelectedContractID < 0)
            {
                var model = new List<InternalPayment>();
                return PartialView("_InternalPaymentsOfDetailContractPartial", model);
            }
            else
            {
                var contract = service.GetById(GridViewHelper.SelectedContractID);
                var model = contract?.InternalPayments ?? new List<InternalPayment>();

                if (model == null) model = new List<InternalPayment>();
                return PartialView("_InternalPaymentsOfDetailContractPartial", model);
            }
        }

        [ValidateInput(false)]
        public ActionResult Payment()
        {
            //var model = User.IsInRole("KDV") ? service.FindBy(e => e.own != null && e.own.UserName == User.Identity.Name) : service.GetAll();

            return View();
        }
        [ValidateInput(false)]
        public ActionResult ContractPaymentViewPartial()
        {           
            var contracts = ContractDataProvider.ContractsPayment.ToList();
            var model = new List<ContractPaymentViewModel>();
            if (User.IsInRole("Admin") || User.IsInRole("TPTH"))
            {
                switch (GridViewHelper.ContractInPaymentGridFilterIndex)
                {
                    case 0:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD)).ToList();
                        break;
                    case 1:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD) && x.TurnOverCount > 0 && x.CongNo == 0).ToList();
                        break;
                    case 2:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD) && x.TurnOverCount > 0 && x.CongNo != 0).ToList();
                        break;
                }
            }
            else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
            {
                var user = userManager.FindByName(User.Identity.Name);
                switch (GridViewHelper.ContractInPaymentGridFilterIndex)
                {
                    case 0:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD) && x.DepartmentId == user?.Department?.Id).ToList();
                        break;
                    case 1:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD) && x.DepartmentId == user?.Department?.Id && x.TurnOverCount > 0 && x.CongNo == 0).ToList();
                        break;
                    case 2:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD) && x.DepartmentId == user?.Department?.Id && x.TurnOverCount > 0 && x.CongNo > 0).ToList();
                        break;
                }
            }
            else
            {
                switch (GridViewHelper.ContractInPaymentGridFilterIndex)
                {
                    case 0:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD) && x.UserName == User.Identity.Name).ToList();
                        break;
                    case 1:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD) && x.UserName == User.Identity.Name && x.TurnOverCount > 0 && x.CongNo == 0).ToList();
                        break;
                    case 2:
                        model = contracts.Where(x => !string.IsNullOrEmpty(x.MaHD) && x.UserName == User.Identity.Name && x.TurnOverCount > 0 && x.CongNo > 0).ToList();
                        break;
                }
            }
            return PartialView("_ContractPaymentViewPartial", model);
        }
        public ActionResult ChangeContractGridFilterModeInPaymentPartial(int filtermode)
        {
            GridViewHelper.ContractInPaymentGridFilterIndex = filtermode;
            return ContractPaymentViewPartial();
        }
        #region Payment of Contract
        public ActionResult CustomCallBackPaymentsOfContractAction(int? selectedcontract)
        {
            if (!selectedcontract.HasValue || string.IsNullOrEmpty(selectedcontract.ToString()) || selectedcontract < 0) GridViewHelper.SelectedContractID = -1;
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return PaymentsOfContractPartial();
        }
        [ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult PaymentsOfContractPut(int id)
        {
            GridViewHelper.SelectedContractID = id;
            return PaymentsOfContractPartial();
        }
        [ValidateInput(false)]
        public ActionResult PaymentsOfContractPartial()
        {
            var model = new List<Payment>();
            if (GridViewHelper.SelectedContractID < 0)
            {
                ViewData["TurnOvers"] = new List<TurnOver>() { new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" } };
                return PartialView("_PaymentsOfContractPartial", model);
            }
            else
            {
                var contract = service.GetById(GridViewHelper.SelectedContractID);
                if (User.IsInRole("KDV"))
                    model = service.FindBy(x => x.Id == GridViewHelper.SelectedContractID && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault()?.Payments;
                else
                    model = contract?.Payments;

                if (model == null)
                {
                    model = new List<Payment>();
                    ViewData["TurnOvers"] = new List<TurnOver>() { new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" } };
                }
                else
                {
                    var turnOvers = contract?.TurnOvers;
                    turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                    ViewData["TurnOvers"] = turnOvers;
                }
                return PartialView("_PaymentsOfContractPartial", model);
            }
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult PaymentsOfContractPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Payment payment)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(payment.turnOver.HDNumber) || int.Parse(payment.turnOver.HDNumber) <= 0)
                    {
                        if (contract != null)
                        {
                            payment.turnOver = null;                            
                            contract.Payments.Add(payment);
                            contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                            contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                            contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }
                    }
                    else
                    {
                        if (contract != null)
                        {
                            var idHoaDon = int.Parse(payment.turnOver.HDNumber);
                            var HoaDon = uow.Repository<TurnOver>().GetSingle(idHoaDon);
                            payment.turnOver = HoaDon;
                            HoaDon.Payments.Add(payment);
                            contract.Payments.Add(payment);
                            contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                            contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                            contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }
                    }
                }
                catch (Exception e)
                {
                    var turnOvers = contract == null ? new List<TurnOver>() : contract.TurnOvers;
                    turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                    ViewData["TurnOvers"] = turnOvers;
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var turnOvers = contract == null ? new List<TurnOver>() : contract.TurnOvers;
                turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                ViewData["TurnOvers"] = turnOvers;
                ViewData["EditError"] = "Please, correct all errors.";
            }
            return PartialView("_PaymentsOfContractPartial", contract.Payments);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult PaymentsOfContractPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Payment payment)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(payment.turnOver.HDNumber) || int.Parse(payment.turnOver.HDNumber) <= 0)
                    {
                        if (contract != null)
                        {
                            var originalPayment = contract.Payments.Where(x => x.Id == payment.Id).FirstOrDefault();
                            if (originalPayment != null)
                            {
                                if (originalPayment.turnOver != null)
                                    originalPayment.turnOver.Payments.Remove(originalPayment);

                                originalPayment.turnOver = null;
                                originalPayment.PaymentName = payment.PaymentName;
                                originalPayment.PaymentDate = payment.PaymentDate;
                                originalPayment.PaymentValue = payment.PaymentValue;
                                originalPayment.PaymentNumber = payment.PaymentNumber;
                                originalPayment.PaymentNote = payment.PaymentNote;
                                originalPayment.PaymentMethod = payment.PaymentMethod;
                            }

                            contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                            contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                            contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }
                    }
                    else
                    {
                        if (contract != null)
                        {
                            var originalPayment = contract.Payments.Where(x => x.Id == payment.Id).FirstOrDefault();
                            if (originalPayment != null)
                            {
                                var idHoaDon = int.Parse(payment.turnOver.HDNumber);
                                var HoaDon = uow.Repository<TurnOver>().GetSingle(idHoaDon);

                                if (originalPayment.turnOver == null)
                                {
                                    HoaDon.Payments.Add(originalPayment);
                                    originalPayment.turnOver = HoaDon;
                                }
                                else
                                {
                                    originalPayment.turnOver.Payments.Remove(originalPayment);
                                    HoaDon.Payments.Add(originalPayment);
                                    originalPayment.turnOver = HoaDon;
                                }

                                originalPayment.PaymentName = payment.PaymentName;
                                originalPayment.PaymentDate = payment.PaymentDate;
                                originalPayment.PaymentValue = payment.PaymentValue;
                                originalPayment.PaymentNumber = payment.PaymentNumber;
                                originalPayment.PaymentNote = payment.PaymentNote;
                                originalPayment.PaymentMethod = payment.PaymentMethod;
                            }

                            contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                            contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                            contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                            contract.customer = contract.customer;
                            service.Update(contract);
                        }
                    }
                }
                catch (Exception e)
                {
                    var turnOvers = contract == null ? new List<TurnOver>() : contract.TurnOvers;
                    turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                    ViewData["TurnOvers"] = turnOvers;
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var turnOvers = contract == null ? new List<TurnOver>() : contract.TurnOvers;
                turnOvers.Insert(0, new TurnOver() { Id = 0, HDNumber = "Chưa có HĐ" });
                ViewData["TurnOvers"] = turnOvers;
                ViewData["EditError"] = "Please, correct all errors.";
            }
            return PartialView("_PaymentsOfContractPartial", contract.Payments);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult PaymentsOfContractPartialDelete(System.Int32 Id)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (Id >= 0)
            {
                try
                {
                    if (contract != null)
                    {
                        var payment = contract.Payments.Where(x => x.Id == Id).FirstOrDefault();
                        if (payment != null)
                        {
                            payment.turnOver?.Payments.Remove(payment);
                            contract.Payments.Remove(payment);
                            uow.Repository<Payment>().Delete(payment);
                            uow.SaveChanges();
                        }

                        contract.TongTienVe = contract.Payments.Sum(x => x.PaymentValue);
                        contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                        contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_PaymentsOfContractPartial", contract.Payments);
        }
        #endregion

        #region Internal Payment of Contract
        public ActionResult CustomCallBackInternalPaymentsOfContractAction(int? selectedcontract)
        {
            if (!selectedcontract.HasValue || string.IsNullOrEmpty(selectedcontract.ToString()) || selectedcontract < 0) GridViewHelper.SelectedContractID = -1;
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return InternalPaymentsOfContractPartial();
        }
        [ValidateInput(false)]
        public ActionResult InternalPaymentsOfContractPartial()
        {
            var model = new List<InternalPayment>();
            if (GridViewHelper.SelectedContractID < 0)
            {
                return PartialView("_InternalPaymentsOfContractPartial", model);
            }
            else
            {
                if (User.IsInRole("KDV"))
                    model = service.FindBy(x => x.Id == GridViewHelper.SelectedContractID && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault()?.InternalPayments;
                else
                    model = service.FindBy(x => x.Id == GridViewHelper.SelectedContractID).FirstOrDefault()?.InternalPayments;

                if (model == null) model = new List<InternalPayment>();
                return PartialView("_InternalPaymentsOfContractPartial", model);
            }
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult InternalPaymentsOfContractPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] InternalPayment internalPayment)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    //if (string.IsNullOrWhiteSpace(internalPayment.InternalPaymentName))
                    //{
                    //    ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    //}
                    //else
                    //{
                    if (contract != null)
                    {
                        contract.InternalPayments.Add(internalPayment);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_InternalPaymentsOfContractPartial", contract.InternalPayments);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult InternalPaymentsOfContractPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] InternalPayment internalPayment)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    //if (string.IsNullOrWhiteSpace(internalPayment.InternalPaymentName))
                    //{
                    //    ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    //}
                    //else
                    //{
                    if (contract != null)
                    {
                        var originalPayment = contract.InternalPayments.Where(x => x.Id == internalPayment.Id).FirstOrDefault();
                        if (originalPayment != null)
                        {
                            originalPayment.InternalPaymentName = internalPayment.InternalPaymentName;
                            originalPayment.InternalPaymentDate = internalPayment.InternalPaymentDate;
                            originalPayment.InternalPaymentValue = internalPayment.InternalPaymentValue;
                            originalPayment.InternalPaymentNumber = internalPayment.InternalPaymentNumber;
                            originalPayment.InternalPaymentNote = internalPayment.InternalPaymentNote;
                            originalPayment.IPType = internalPayment.IPType;
                        }

                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_InternalPaymentsOfContractPartial", contract.InternalPayments);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult InternalPaymentsOfContractPartialDelete(System.Int32 Id)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (Id >= 0)
            {
                try
                {
                    if (contract != null)
                    {
                        var interpayment = contract.InternalPayments.Where(x => x.Id == Id).FirstOrDefault();
                        if (interpayment != null)
                        {
                            uow.Repository<InternalPayment>().Delete(interpayment);
                            contract.InternalPayments.Remove(interpayment);
                        }

                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_InternalPaymentsOfContractPartial", contract.InternalPayments);
        }

        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult InternalPaymentsOfContractPartialAddNewViaContract([ModelBinder(typeof(DevExpressEditorsBinder))] InternalPayment internalPayment)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    //if (string.IsNullOrWhiteSpace(internalPayment.InternalPaymentName))
                    //{
                    //    ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    //}
                    //else
                    //{
                    if (contract != null)
                    {
                        contract.InternalPayments.Add(internalPayment);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_InternalPaymentsOfDetailContractPartial", contract.InternalPayments);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult InternalPaymentsOfContractPartialUpdateViaContract([ModelBinder(typeof(DevExpressEditorsBinder))] InternalPayment internalPayment)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    //if (string.IsNullOrWhiteSpace(internalPayment.InternalPaymentName))
                    //{
                    //    ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    //}
                    //else
                    //{
                    if (contract != null)
                    {
                        var originalPayment = contract.InternalPayments.Where(x => x.Id == internalPayment.Id).FirstOrDefault();
                        if (originalPayment != null)
                        {
                            originalPayment.InternalPaymentName = internalPayment.InternalPaymentName;
                            originalPayment.InternalPaymentDate = internalPayment.InternalPaymentDate;
                            originalPayment.InternalPaymentValue = internalPayment.InternalPaymentValue;
                            originalPayment.InternalPaymentNumber = internalPayment.InternalPaymentNumber;
                            originalPayment.InternalPaymentNote = internalPayment.InternalPaymentNote;
                            originalPayment.IPType = internalPayment.IPType;
                        }

                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_InternalPaymentsOfDetailContractPartial", contract.InternalPayments);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult InternalPaymentsOfContractPartialDeleteViaContract(System.Int32 Id)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (Id >= 0)
            {
                try
                {
                    if (contract != null)
                    {
                        var interpayment = contract.InternalPayments.Where(x => x.Id == Id).FirstOrDefault();
                        if (interpayment != null)
                        {
                            uow.Repository<InternalPayment>().Delete(interpayment);
                            contract.InternalPayments.Remove(interpayment);
                        }

                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_InternalPaymentsOfDetailContractPartial", contract.InternalPayments);
        }
        #endregion

        #region TurnOver of Contract
        public ActionResult CustomCallBackTurnOversOfContractAction(int? selectedcontract)
        {
            if (!selectedcontract.HasValue || string.IsNullOrEmpty(selectedcontract.ToString()) || selectedcontract < 0) GridViewHelper.SelectedContractID = -1;
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return TurnOversOfContractPartial();
        }

        [ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult TurnOversOfContractPut(int id)
        {
            GridViewHelper.SelectedContractID = id;
            return TurnOversOfContractPartial();
        }
        [ValidateInput(false)]
        public ActionResult TurnOversOfContractPartial()
        {
            var model = new List<TurnOver>();
            if (GridViewHelper.SelectedContractID < 0)
            {
                return PartialView("_TurnOversOfContractPartial", model);
            }
            else
            {
                if (User.IsInRole("KDV"))
                    model = service.FindBy(x => x.Id == GridViewHelper.SelectedContractID && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault()?.TurnOvers;
                else
                    model = service.FindBy(x => x.Id == GridViewHelper.SelectedContractID).FirstOrDefault()?.TurnOvers;

                if (model == null) model = new List<TurnOver>();
                return PartialView("_TurnOversOfContractPartial", model);
            }
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult TurnOversOfContractPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] TurnOver TurnOver)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    //if (string.IsNullOrWhiteSpace(TurnOver.TurnOverName))
                    //{
                    //    ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    //}
                    //else
                    //{
                    if (contract != null)
                    {
                        contract.TurnOvers.Add(TurnOver);
                        contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                        contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TurnOversOfContractPartial", contract.TurnOvers);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult TurnOversOfContractPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] TurnOver TurnOver)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (ModelState.IsValid)
            {
                try
                {
                    //if (string.IsNullOrWhiteSpace(TurnOver.TurnOverName))
                    //{
                    //    ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
                    //}
                    //else
                    //{
                    if (contract != null)
                    {
                        var originalTurnOver = contract.TurnOvers.Where(x => x.Id == TurnOver.Id).FirstOrDefault();
                        if (originalTurnOver != null)
                        {
                            originalTurnOver.TurnOverName = TurnOver.TurnOverName;
                            originalTurnOver.TurnOverDate = TurnOver.TurnOverDate;
                            originalTurnOver.BLNumber = TurnOver.BLNumber;
                            originalTurnOver.BLValue = TurnOver.BLValue;
                            originalTurnOver.HDNumber = TurnOver.HDNumber;
                            originalTurnOver.HDValue = TurnOver.HDValue;
                            originalTurnOver.VAT = TurnOver.VAT;
                            originalTurnOver.TurnOverNote = TurnOver.TurnOverNote;
                        }

                        contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                        contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TurnOversOfContractPartial", contract.TurnOvers);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult TurnOversOfContractPartialDelete(System.Int32 Id)
        {
            var contract = service.GetById(GridViewHelper.SelectedContractID);
            if (Id >= 0)
            {
                try
                {
                    if (contract != null)
                    {
                        var TurnOver = contract.TurnOvers.Where(x => x.Id == Id).FirstOrDefault();
                        if (TurnOver != null)
                        {
                            var count = TurnOver.Payments.Count;
                            for (int i = 0; i < count; i++)
                            {
                                var payment = TurnOver.Payments[i];
                                TurnOver.Payments.Remove(payment);
                            }
                            contract.TurnOvers.Remove(TurnOver);
                            uow.Repository<TurnOver>().Delete(TurnOver);
                            uow.SaveChanges();
                        }

                        contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                        contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_TurnOversOfContractPartial", contract.TurnOvers);
        }
        #endregion
        [ValidateInput(false)]
        public ActionResult TurnOverAndPaymentView(int id)
        {
            GridViewHelper.SelectedContractID = id;
            var model = User.IsInRole("KDV") ? service.GetAll().Where(x => x.Id == id && x.own?.UserName == User.Identity.Name).FirstOrDefault() : service.GetById(id);
            if (model == null) model = new Contract();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult TurnOverAndPaymentView(Contract contract)
        {
            var orgContract = service.GetById(contract.Id);

            if (orgContract != null)
            {
                orgContract.RatioOfCompany = contract.RatioOfCompany;
                orgContract.RatioOfInternal = contract.RatioOfInternal;
                orgContract.RatioOfGroup = contract.RatioOfGroup;
                orgContract.Finished = contract.Finished;

                orgContract.customer = orgContract.customer;
                uow.SaveChanges();
                service.Update(orgContract);
            }

            return RedirectToAction("Payment", "Contracts");
        }
        [ValidateInput(false)]
        public ActionResult InternalPaymentView(int id)
        {
            GridViewHelper.SelectedContractID = id;
            var contract = User.IsInRole("KDV") ? service.GetAll().Where(x => x.Id == id && x.own?.UserName == User.Identity.Name).FirstOrDefault() : service.GetById(id);
            if (contract == null) contract = new Contract();

            var model = new InternalPaymentViewModel(contract);
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "NhapTTTC")]
        public ActionResult InternalPaymentView(InternalPaymentViewModel InternalPayment)
        {
            var contract = InternalPayment.Contract;
            var orgContract = service.GetById(contract.Id);
            uow.SaveChanges();
            service.Update(orgContract);

            return RedirectToAction("Payment", "Contracts");
        }
        public ActionResult ApplyContractStatus(int idContract, int approvedType)
        {
            var result = "true";
            var contract = service.GetById(idContract);
            if (contract != null)
            {
                if (User.IsInRole("Admin") || User.IsInRole("DeptDirector"))
                {
                    if (contract.Status != ApproveStatus.ApprovedLv2)
                    {
                        contract.Status = approvedType == 0 ? ApproveStatus.Waiting : approvedType == 1 ? ApproveStatus.ApprovedLv1 : contract.Status;
                        contract.customer = contract.customer;
                        service.Update(contract);
                    }
                    else result = "false";
                }
                else result = "false";
            }
            else result = "false";
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Noted by Lapbt. Cap so hop dong
        /// 1. Yêu cầu lấy số
        ///     - lấy vào vị trí trống để đảm bảo liền mạch số
        ///     - nếu đã liền mạch, lấy tăng dần +1
        /// 2. Thực hiện hơi trâu
        ///     - lấy toàn bộ hđồng của năm -> lấy ra cột MaHD -> đưa vào mảng
        ///     - thực hiện so sánh giá trị của 2 số liền nhau có hiệu của nó > 1, tức là bị hổng
        ///     - tại điểm đầu tiên có sự khác biệt đó, lấy ra số đứng trc để + 1 thì chính là vị trí hổng
        /// </summary>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        [SetPermissions(Permission = "CapSoHD")]
        public ActionResult GetAutoContractNumber(int ContractID)
        {
            var prefix = DateTime.Today.ToString("yy"); // + ".";
            //prefix = "21";

            var thisYearContracts = uow.Repository<Contract>().FindBy(e => !string.IsNullOrEmpty(e.MaHD) && e.MaHD.StartsWith(prefix)).OrderBy(e => e.MaHD);

            //var hds = thisYearContracts.Where(e => e.MaHD.EndsWith("6793"));
            //var mahdss = hds.Select(e => e.MaHD.TrimStart(prefix.ToCharArray()).TrimStart(new[] { '.' })).ToList();
            //var mahdsss = hds.Select(e => e.MaHD.Substring(2).TrimStart(new[] { '.' })).ToList();
            //var mahds = thisYearContracts.Select(e => e.MaHD.Replace(prefix, "")).Where(e => !e.Contains(".")).ToList();


            // last code closed by lapbt 12/12/2020. One line below
            //var mahds = thisYearContracts.Select(e => e.MaHD.Substring(2).TrimStart(new[] { '.', '0' })).Where(e => e.Length == 4 && e.All(char.IsDigit)).ToList();
            //mahds.Sort();

            // added by lapbt. No remove 0 char, for using format: 000N
            var remchar = (prefix == "20") ? '0' : ' ';
            var mahds = thisYearContracts.Select(e => e.MaHD.Substring(2).TrimStart(new[] { '.', remchar })).Where(e => e.Length == 4 && e.All(char.IsDigit)).ToList();
  
            /* Đưa ds các MaHD của năm vào mảng khi đó mảng biểu diễn là 
             * chỉ số:  0 1 2 3 4 5
             * giá trị: 1 2 3 4 5 6
             * xly:
             * - lấy chỉ số + 1 thì sẽ fai = giá trị
             * - nếu có sự khác biệt thì số (chỉ số + 1) chính là giá trị cần tìm
             */
            var sohds = mahds.Select(e => int.Parse(e)).ToList();
            sohds.Sort();

            var a = 0;                                          // nhận số đứng trc vị trí hổng, hoặc là số cuối cùng của mảng khi mà k0 bị hổng
            if (sohds.Count > 0)
            {
                a = sohds.First(e =>
                {
                    var nextindx = sohds.IndexOf(e) + 1;        // chỉ số của số liền sau, số hiện tại (e)
                    if (nextindx == sohds.Count) return true;   // nếu chỉ số đó là là vị trí cuối cùng rồi thì có nghĩa k0 có lỗ hổng
                    else
                        return (sohds[nextindx] - e > 1);       // tìm ra điểm đầu tiên có lỗ hổng
                });
            }
            

            //var result = (from e in sohds

            //              let nextindex = sohds.IndexOf(e) + 1

            //              let nextelement = sohds.ElementAt(nextindex == sohds.Count ? nextindex - 1 : nextindex)

            //              select nextelement - e).ToList();

            //result.RemoveAt(sohds.Count - 1);

            //var lastContract = thisYearContracts.LastOrDefault();

            //var soHD = lastContract != null ? lastContract.MaHD.Split(new char[] { '.' })[1] : "0";
            //var soHdInt = int.Parse(soHD);

            //var soHdInt = int.Parse(mahds.LastOrDefault());
            //var soHdInt = sohds.LastOrDefault();

            var newShd = (a + 1).ToString("#0000");         // thực hiện + 1 cho số tìm đc, sẽ ra MaHD cần tìm
            while (mahds.Contains(newShd))
            {
                a += 1;
                newShd = (a + 1).ToString("#0000");
            }
            
            return Json(prefix + newShd, JsonRequestBehavior.AllowGet);

            var autoMaHD = DataProvider.DB.Database.SqlQuery<AutoMaHD>("GetLastAutoMaHD").FirstOrDefault();
            AutoMaHD nextMaHD = null;
            if (autoMaHD == null || autoMaHD.Prefix != prefix)
            {
                nextMaHD = new AutoMaHD()
                {
                    Id = 0,
                    Prefix = DateTime.Today.ToString("yy") + ".",
                    ContractNumber = 1
                };

                uow.Repository<AutoMaHD>().Insert(nextMaHD);
                uow.SaveChanges();
            }
            else
            {
                nextMaHD = new AutoMaHD()
                {
                    Id = 0,
                    Prefix = DateTime.Today.ToString("yy") + ".",
                    ContractNumber = autoMaHD.ContractNumber + 1
                };

                uow.Repository<AutoMaHD>().Insert(nextMaHD);
                uow.SaveChanges();
            }

            var result = nextMaHD.Prefix + nextMaHD.ContractNumber.ToString("#00000");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RefreshContractData()
        {
            ContractDataProvider.contracts = null;
            ContractDataProvider.contracts1 = null;
            ContractDataProvider.contractsPayment = null;
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployeesCallbackRouteValuesForCreate()
        {
            if (User.IsInRole("Admin"))
            {
                ViewData["Employees"] = uow.Repository<Core.DomainModels.Identity.AppUser>()
                                        .GetAll()
                                        .OrderBy(x => x.DisplayName)
                                        .ToList();            }
            else
            {
                //ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
                ViewData["Employees"] = uow.Repository<Core.DomainModels.Identity.AppUser>()
                                        .FindBy(x => x.PhoneNumber == "1")
                                        .OrderBy(x => x.DisplayName)
                                        .ToList();
            }
            return PartialView();
        }
        public ActionResult EmployeesCallbackRouteValuesForCreateKDV1()
        {
            ViewData["Employees"] = uow.Repository<Core.DomainModels.Identity.AppUser>()
                                        .FindBy(x => x.PhoneNumber == "1")
                                        .OrderBy(x => x.DisplayName)
                                        .ToList();
           
            return PartialView();
        }
        public ActionResult EmployeesCallbackRouteValuesForCreateKDV2()
        {
            ViewData["Employees"] = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.PhoneNumber == "1");
            ViewData["AppUsersKDV2"] = uow.Repository<Core.DomainModels.Identity.AppUser>()
                                        .FindBy(x => x.TwoFactorEnabled == true)
                                        .OrderBy(x => x.DisplayName)
                                        .ToList();
            return PartialView();
        }
        public ActionResult EmployeesCallbackRouteValuesForEditKDV1()
        {
            ViewData["Employees"] = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.PhoneNumber == "1");
            
            return PartialView();
        }
        public ActionResult EmployeesCallbackRouteValuesForEditKDV2()
        {
            ViewData["Employees"] = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.PhoneNumber == "1");
            
            return PartialView();
        }
        public ActionResult EmployeesCallbackRouteValuesForEdit()
        {
            var model = GridViewHelper.SelectedContractID > 0 ? service.GetById(GridViewHelper.SelectedContractID) : null;
            //11.5.2025 Hưng sửa gộp. Xử lý KDV không chỉnh sửa tên chủ trì bằng cách settings.Enabled = !User.IsInRole("KDV");
            // ở EmployeesCallbackRouteValuesForCreate
            //và EmployeesCallbackRouteValues
            ViewData["Employees"] = uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.PhoneNumber == "1");
            
            return PartialView(model);
        }
        [SetPermissions(Permission = "InHoaDon")]
        public ActionResult InvoiceCreate(int id)
        {
            var contract = service.GetById(id);
            if (contract == null) return Json("error", JsonRequestBehavior.AllowGet);

            var model = new InvoiceViewModel()
            {
                ContractID = contract.Id,
                ContractNumber = contract.MaHD,
                ContractSignDate = contract.SignDate,
                CustomerAccountNumber = contract.customer?.AccountNumber,
                CustomerAddress = contract.customer?.Address,
                CustomerName = contract.customer?.Name,
                Representative = contract?.customer.Representative,
                TaxID = contract.customer?.TaxID,
                VATType = contract.vatType,
                VAT = contract.vatType == VATType.cothue ? contract.VAT : 0
            };

            foreach (var turnOver in contract.TurnOvers)
            {
                var newTurnOver = new TurnOver()
                {
                    Id = turnOver.Id,
                    BLNumber = turnOver.BLNumber,
                    BLValue = turnOver.BLValue,
                    HDNumber = turnOver.HDNumber,
                    HDValue = turnOver.HDValue,
                    TurnOverDate = turnOver.TurnOverDate,
                    TurnOverName = turnOver.TurnOverName,
                    TurnOverNote = turnOver.TurnOverNote,
                    VAT = turnOver.VAT
                };

                model.InvoiceTurnOvers.Add(newTurnOver);
            }

            foreach (var task in contract.Tasks)
            {
                var invoiceProduct = new InvoiceProduct()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Unit = task.Unit,
                    Amount = task.Amount,
                    VATType = task.vatType,
                    VAT = task.VAT,
                    Price = task.UnitPrice / (1 + task.VAT / 100)
                };

                model.InvoiceProducts.Add(invoiceProduct);
            }

            InvoiceProductList.GetInvoiceProducts = model.InvoiceProducts.ToList();
            ReportHelper.InvoiceReportDataSource = model;

            return PartialView("_InvoiceCreatePartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [SetPermissions(Permission = "InHoaDon")]
        public ActionResult InvoiceCreate([ModelBinder(typeof(DevExpressEditorsBinder))] InvoiceViewModel invoice)
        {
            DevExpress.Web.Office.DocumentManager.CloseAllDocuments();
            invoice.InvoiceProducts = InvoiceProductList.GetInvoiceProducts;
            var contract = service.GetById(invoice.ContractID);
            if (contract != null)
            {
                var turnOver = new TurnOver()
                {
                    Id = 0,
                    HDNumber = invoice.HDNumber,
                    TurnOverDate = invoice.CreateDate,
                    VAT = invoice.VAT,
                    HDValue = invoice.InvoiceProducts.Sum(x => x.Value),
                    contract = contract
                };

                contract.TurnOvers.Add(turnOver);
                contract.TienTruThue = contract.TurnOvers.Sum(x => x.TienTruThue);
                contract.TongTienXuatHoaDon = contract.TurnOvers.Sum(x => x.TotalValue);
                contract.customer = contract.customer;
                service.Update(contract);

                ReportHelper.InvoiceReportDataSource = invoice;

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
                return Json("error", JsonRequestBehavior.AllowGet);
        }

        public ActionResult InvoiceReport()
        {
            return View(ReportHelper.InvoiceReportDataSource);
        }
        public ActionResult InvoiceReportPartialView()
        {
            return PartialView("_InvoiceReportPartialView", ReportHelper.InvoiceReportDataSource);
        }
        [ValidateInput(false)]
        public ActionResult InvoiceProductCreateActionPartial()
        {
            return PartialView("_InvoiceProductCreateActionPartial", InvoiceProductList.GetInvoiceProducts);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceProductCreateActionPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] InvoiceProduct invoiceProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    InvoiceProductList.AddInvoiceProduct(invoiceProduct);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_InvoiceProductCreateActionPartial", InvoiceProductList.GetInvoiceProducts);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceProductCreateActionPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] InvoiceProduct invoiceProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    InvoiceProductList.UpdateInvoiceProduct(invoiceProduct);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_InvoiceProductCreateActionPartial", InvoiceProductList.GetInvoiceProducts);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceProductCreateActionPartialDelete(System.Int32 Id)
        {
            if (Id >= 0)
            {
                try
                {
                    InvoiceProductList.DeleteInvoiceProduct(Id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_InvoiceProductCreateActionPartial", InvoiceProductList.GetInvoiceProducts);
        }

        public ActionResult GetCurrentUser()
        {
            var usn = User.Identity.Name;
            var user = userManager.FindByName(usn);

            return Json(user.Id, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckHDNumberValid(int ContractID, string HDNumber)
        {
            var contract = service.GetById(ContractID);
            if (contract != null)
            {
                var hd = contract.TurnOvers.FirstOrDefault(x => x.HDNumber?.ToLower() == HDNumber.ToLower() && !string.IsNullOrWhiteSpace(HDNumber));
                if (hd != null) return Json("NotValid", JsonRequestBehavior.AllowGet);
                else return Json("Valid", JsonRequestBehavior.AllowGet);
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckHDValue(int ContractID)
        {
            var contract = service.GetById(ContractID);
            if (contract != null)
            {
                var tongtienhoadon = contract.TurnOvers.Sum(x => x.HDValue);
                var tonghoadonmoi = InvoiceProductList.GetInvoiceProducts.Sum(x => x.Value);

                if ((tonghoadonmoi + tongtienhoadon) > contract.Value)
                    return Json("over", JsonRequestBehavior.AllowGet);
                else
                    return Json("ok", JsonRequestBehavior.AllowGet);
            }
            return Json("contractnotfound", JsonRequestBehavior.AllowGet);
        }

        // Paging
        public ActionResult AdvancedCustomBindingPagingAction(GridViewPagerState pager)
        {
            var viewModel = GridViewExtension.GetViewModel("grvContracts");
            viewModel.ApplyPagingState(pager);
            return AdvancedCustomBindingCore(viewModel);
        }
        // Filtering
        public ActionResult AdvancedCustomBindingFilteringAction(GridViewFilteringState filteringState)
        {
            //filteringState.SearchPanelFilter = "\"" + filteringState.SearchPanelFilter + "\"";
            filteringState.SearchPanelFilter = filteringState.SearchPanelFilter;
            var viewModel = GridViewExtension.GetViewModel("grvContracts");
            viewModel.ApplyFilteringState(filteringState);
            return AdvancedCustomBindingCore(viewModel);
        }
        // Sorting
        public ActionResult AdvancedCustomBindingSortingAction(GridViewColumnState column, bool reset)
        {
            var viewModel = GridViewExtension.GetViewModel("grvContracts");
            viewModel.ApplySortingState(column, reset);
            return AdvancedCustomBindingCore(viewModel);
        }
        // Grouping
        public ActionResult AdvancedCustomBindingGroupingAction(GridViewColumnState column)
        {
            var viewModel = GridViewExtension.GetViewModel("grvContracts");
            viewModel.ApplyGroupingState(column);
            return AdvancedCustomBindingCore(viewModel);
        }

        PartialViewResult AdvancedCustomBindingCore(GridViewModel viewModel)
        {
            try
            {                
                // 1. Đoạn xử lý gán năm tài chính đã chọn theo từng user
                var userName = GridViewHelper.GetCurrentUserName();
                string prefix = "ContractTotals_";
                if (!string.IsNullOrEmpty(userName))
                    prefix += userName + "_";
                // --- Lấy lại từ Session ---
                ViewBag.Total_Value = Session[prefix + "Value"] ?? 0;
                ViewBag.Total_XuatHoaDon = Session[prefix + "XuatHoaDon"] ?? 0;
                ViewBag.Total_ThuNo = Session[prefix + "ThuNo"] ?? 0;
                ViewBag.Total_CongNo = Session[prefix + "CongNo"] ?? 0;
                ViewBag.Total_DoDang = Session[prefix + "DoDang"] ?? 0;
                ViewBag.Total_TotalEquipment = Session[prefix + "Equipment"] ?? 0;
                // 1. End

                // Cập nhật GridViewHelper
                viewModel.ProcessCustomBinding(
                GridViewCustomBindingHandlers.GetDataRowCountAdvanced,
                GridViewCustomBindingHandlers.GetDataAdvanced,
                GridViewCustomBindingHandlers.GetSummaryValuesAdvanced,
                GridViewCustomBindingHandlers.GetGroupingInfoAdvanced,
                GridViewCustomBindingHandlers.GetUniqueHeaderFilterValuesAdvanced

            );                

            }
            catch 
            {
                
            }
            return PartialView("_ContractViewPartial", viewModel);

        }

        static GridViewModel CreateGridViewModelWithSummary()
        {
            var viewModel = new GridViewModel();
            viewModel.Pager.PageSize = 40;          // added by lapbt 13-mar-2025. Do lần đầu load trang lên, GetViewModel("grvContracts") == null, nên khởi tạo mặc định vào đây
            viewModel.SearchPanel.ColumnNames = "MaHD; Name; OwnerDisplayName; KDV1DisplayName; KDV2DisplayName; CustomerName; ";
            viewModel.KeyFieldName = "Id";
            viewModel.Columns.Add("MaHD");
            viewModel.Columns.Add("Name");
            viewModel.Columns.Add("SignDate");
            viewModel.Columns.Add("OwnerDisplayName");
            viewModel.Columns.Add("KDV1DisplayName");
            viewModel.Columns.Add("KDV2DisplayName");
            viewModel.Columns.Add("CustomerName");
            viewModel.Columns.Add("CustomerRepresentative");
            viewModel.Columns.Add("CustomerAddress");
            viewModel.Columns.Add("Value");
            viewModel.Columns.Add("XuatHoaDon");
            viewModel.Columns.Add("ThuNo");
            viewModel.Columns.Add("CongNo");
            viewModel.Columns.Add("DoDang");
            viewModel.Columns.Add("RatioOfCompany");
            viewModel.Columns.Add("ValueRoC");
            viewModel.Columns.Add("IsCompletedIPoC");
            viewModel.Columns.Add("RatioOfInternal");
            viewModel.Columns.Add("IsCompletedIPoI");
            viewModel.Columns.Add("ValueRoI");
            viewModel.Columns.Add("Status");
            //viewModel.Columns.Add("TotalEquipment");
            //viewModel.Columns.Add("DocCount");

            return viewModel;
        }
        
        [ValidateInput(false)]
        public ActionResult ContractViewPartial()
        {
            // 1. Luôn tạo GridViewModel mới để không bị cache            
            var viewModel = GridViewExtension.GetViewModel("grvContracts");
            if (viewModel == null)
            {
                viewModel = CreateGridViewModelWithSummary();
            }
            
            //2. Lọc dữ liệu theo user
            var user = userManager.FindByName(User.Identity.Name);            
            GridViewHelper.UserIdFilter = user?.Id;
            GridViewHelper.UserDepartmentIdFilter = user?.Department?.Id;            
            GridViewHelper.UserRoleFilter =
                (User.IsInRole("Admin") || User.IsInRole("TPTH")) ? "Admin" :
                (User.IsInRole("Accountant") || User.IsInRole("DeptDirector")) ? "Director" : "User";
            
            if (!User.IsInRole("Admin") && !User.IsInRole("DeptDirector"))
                GridViewHelper.ContractGridOwnerIdFilter = user.Id;

            // 3. Tạo Session lưu trạng thái mở rộng cửa sổ danh sách HĐ            
            if (Session["ShowContractExtraColumns"] == null)
            {
                Session["ShowContractExtraColumns"] = false;// Nếu chưa có thì gán mặc định = false
            }
            // Lấy trạng thái từ Session để truyền sang Partial
            ViewBag.ShowContractExtraColumns = Session["ShowContractExtraColumns"] as bool? ?? false;

            // --- Xử lý search text
            if (Session["ContractSearchText"] == null)
                Session["ContractSearchText"] = "";

            string search = Request.Params["search"];
            if (!string.IsNullOrEmpty(search))
                Session["ContractSearchText"] = search;
            
            if (search == "") Session["ContractSearchText"] = ""; // Khi xoá trống

            //4. Đoạn xử lý chọn năm tài chính
            var userName = GridViewHelper.GetCurrentUserName();
            string yearKey = "ContractView_Year_" + userName;            
            if (!string.IsNullOrEmpty(Request.Params["year"]))
            {
                // Nếu có param 'year' → cập nhật session theo user
                if (int.TryParse(Request.Params["year"], out int newYear))
                    Session[yearKey] = newYear;
            }
            // Lấy year từ Session hoặc mặc định năm hiện tại
            int year = Session[yearKey] != null ? (int)Session[yearKey] : DateTime.Now.Year;            
            string prefix = "ContractTotals_";
            if (!string.IsNullOrEmpty(userName))
                prefix += userName + "_";
            var totals = LargeDatabaseDataProvider.GetTotalsByYear(year);
            Session[prefix + "Value"] = totals.Total_Value;
            Session[prefix + "XuatHoaDon"] = totals.Total_XuatHoaDon;
            Session[prefix + "ThuNo"] = totals.Total_ThuNo;
            Session[prefix + "CongNo"] = totals.Total_CongNo;
            Session[prefix + "DoDang"] = totals.Total_DoDang;
            Session[prefix + "Equipment"] = totals.Total_Equipment;

            return AdvancedCustomBindingCore(viewModel);
        }
       
        [HttpPost]
        public ActionResult ContractToggleColumns()
        {
            bool current = Session["ShowContractExtraColumns"] as bool? ?? false;
            string search = Session["ContractSearchText"] as string ?? "";
            Session["ShowContractExtraColumns"] = !current;
            Session["ContractSearchText"] = search;
            // 👉 Không cần GetModel nữa, gọi lại ContractViewPartial để render grid mới
            return ContractViewPartial();
        }       

        [HttpPost]
        public ActionResult ContractGridRouter(int? filtermode, string departmentCode, int? ownerId, bool? toggle = null)
        {
            // Lưu filter trạng thái (quan trọng)
            if (filtermode.HasValue)
                GridViewHelper.ContractGridStatusFilter = filtermode.Value;
            // Lưu selectedIndex để combobox nhớ trạng thái           
            if (filtermode.HasValue && filtermode.Value > -1)
                GridViewHelper.ContractGridFilterIndex = filtermode.Value;
            // Lưu department
            if (!string.IsNullOrEmpty(departmentCode))
                GridViewHelper.ContractGridDepartmentCodeFilter = departmentCode;
            // Lưu ownerId (nhân viên)
            if (ownerId.HasValue)
                GridViewHelper.ContractGridOwnerIdFilter = ownerId.Value;
            else
                GridViewHelper.ContractGridOwnerIdFilter = null;

            // Toggle ẩn/hiện cột
            if (toggle.HasValue && toggle.Value)
            {
                bool current = Session["ShowContractExtraColumns"] as bool? ?? false;
                Session["ShowContractExtraColumns"] = !current;
            }

            return ContractViewPartial(); // render lại grid với cấu hình mới
        }


        [ValidateInput(false)]
        public ActionResult DoanhThuOfContractPartial(int? contractId)
        {
            if (contractId == null || contractId < 0)
                return PartialView("_DoanhThuOfContractPartial", new List<v_ActTurnOver>());

            using (var db = new IncosafCMSContext())
            {
                var contract = db.Contracts.Find(contractId);
                if (contract == null || string.IsNullOrEmpty(contract.MaHD))
                    return PartialView("_DoanhThuOfContractPartial", new List<v_ActTurnOver>());

                //Bắt buộc phải lấy DL bằng sql trực tiếp do bảng v_ActTurnOver không có cột Id
                string sql = @"SELECT * FROM v_ActTurnOver 
                   WHERE ma_hd = @p0 AND ma_hd <> '' AND ma_hd IS NOT NULL AND so_ct IS NOT NULL 
                   ORDER BY ngay_ct DESC";

                var model = db.Database.SqlQuery<v_ActTurnOver>(sql, contract.MaHD).ToList();

                return PartialView("_DoanhThuOfContractPartial", model);
            }
        }

        public ActionResult CustomCallBackTasksOfDoanhThuOfContractAction(int? selectedcontract)
        {            
            if (!selectedcontract.HasValue || selectedcontract < 0)
            {
                GridViewHelper.SelectedContractID = -1;
            }
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return DoanhThuOfContractPartial(GridViewHelper.SelectedContractID);
        }

        [ValidateInput(false)]
        public ActionResult ThuNoOfContractPartial(int? contractId)
        {
            if (contractId == null || contractId < 0)
                return PartialView("_ThuNoOfContractPartial", new List<v_ActPayment>());

            using (var db = new IncosafCMSContext())
            {
                var contract = db.Contracts.Find(contractId);
                if (contract == null || string.IsNullOrEmpty(contract.MaHD))
                    return PartialView("_ThuNoOfContractPartial", new List<v_ActPayment>());

                //Bắt buộc phải lấy DL bằng sql trực tiếp do bảng v_ActPayment không có cột Id
                string sql = @"SELECT * FROM v_ActPayment 
                   WHERE ma_hd = @p0 AND ma_hd <> '' AND ma_hd IS NOT NULL AND so_ct IS NOT NULL
                   ORDER BY ngay_ct DESC";

                var model = db.Database.SqlQuery<v_ActPayment>(sql, contract.MaHD).ToList();

                return PartialView("_ThuNoOfContractPartial", model);
            }
        }

        public ActionResult CustomCallBackTasksOfThuNoOfContractAction(int? selectedcontract)
        {            
            if (!selectedcontract.HasValue || selectedcontract < 0)
            {
                GridViewHelper.SelectedContractID = -1;
            }
            else GridViewHelper.SelectedContractID = selectedcontract.Value;
            return ThuNoOfContractPartial(GridViewHelper.SelectedContractID);
        }


        //Xem file hồ sơ lưu 28.9.2025
        public ActionResult ViewDocuments(int id)
        {            
            var docs = uow.Repository<Contract_Doc>()
                  .FindBy(d => d.Contract.Id == id)
                  .ToList();
            if (docs == null || !docs.Any())
            {
                return Json(new { success = false, message = "Hợp đồng này chưa có hồ sơ nào." }, JsonRequestBehavior.AllowGet);
            }

            // Build absolute URLs (chú ý chuẩn hóa path_save)
            var fileUrls = docs.Select(d => {
                var path = d.path_save ?? "";
                // đảm bảo bắt đầu bằng "/"
                if (!path.StartsWith("/")) path = "/" + path;
                // đảm bảo kết thúc bằng "/"
                if (!path.EndsWith("/")) path = path + "/";
                return "https://hoso.incosaf.com" + path + d.filename_save;
            }).ToList();
            return PartialView("_ViewDocuments", docs);            
        }

       
        public ActionResult UploadDocument(int id)
        {
            var contract = db.Contracts.Find(id);
            ViewBag.ContractId = id;
            ViewBag.MaHD = contract.MaHD;
            return PartialView("_UploadContractDocumentPartial");
        }

        [HttpPost]
        public ActionResult UploadDocument(int id, HttpPostedFileBase file)
        {
            var contract = db.Contracts.Find(id);
            var branchId = System.Configuration.ConfigurationManager.AppSettings["BranchID"];

            string deptCode = "";
            switch (branchId)
            {
                case "01":
                    deptCode = "HN";
                    break;
                case "02":
                    deptCode = "HCM";
                    break;
                case "03":
                    deptCode = "DN";
                    break;
                default:
                    deptCode = "HN"; // fallback
                    break;
            }
            
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExt = Path.GetExtension(file.FileName);

                // tránh trùng lặp: dùng Guid + ext
                string newFileName = Guid.NewGuid().ToString("N") + fileExt;

                // VD: /Hoso001/HN/2025/251032/
                string folderPath = $"/Hoso0{branchId}/{deptCode}/{DateTime.Now.Year}/{contract.MaHD}/";
                string ftpBaseUrl = "ftp://a.incosaf.com:212";
                string ftpFullPath = ftpBaseUrl + folderPath + newFileName;

                // đảm bảo thư mục tồn tại
                EnsureFtpDirectoryExists(ftpBaseUrl, folderPath, "hoso_user", "d3Hy3#w{w)du2D");

                // sau đó mới upload
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential("hoso_user", "d3Hy3#w{w)du2D");

                using (Stream fileStream = file.InputStream)
                using (Stream ftpStream = request.GetRequestStream())
                {
                    fileStream.CopyTo(ftpStream);
                }

                // ---- SAVE DB ----
                var doc = new Contract_Doc
                {
                    docType = 1,
                    filename_text = fileName,
                    filename_save = newFileName,
                    path_save = folderPath,
                    doc_note = null,
                    Contract_Id = contract.Id,
                    upload_by_cms_uid = GetCurrentUserId(),
                    upload_date = DateTime.Now,
                    delete_by_cms_uid = null,
                    delete_date = null,
                    status = 1
                };

                db.Contract_Docs.Add(doc);               
                db.SaveChanges();
            }            
            return RedirectToAction("Index", "Home");
        }

        //Xóa hồ sơ nếu không quá 2 ngày up lên
        [HttpPost]
        public ActionResult DeleteDocument(int id)
        {
            var doc = db.Contract_Docs.Find(id);
            if (doc == null)
                return Json(new { success = false, message = "Không tìm thấy hồ sơ." });

            // Kiểm tra thời gian upload, cho phép xóa trong vòng 2 ngày, ngoại trừ admin xóa được tất
            if ((DateTime.Now - doc.upload_date).TotalDays > 2 && !User.IsInRole("Admin"))
            {
                return Json(new { success = false, message = "Hồ sơ đã upload > 2 ngày, không thể xóa." });
            }

            try
            {
                // Xóa file trên FTP
                string ftpBaseUrl = "ftp://a.incosaf.com:212";
                string ftpFullPath = ftpBaseUrl + doc.path_save + doc.filename_save;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential("hoso_user", "d3Hy3#w{w)du2D");
                using (var response = (FtpWebResponse)request.GetResponse()) { }

                // Xóa DB
                int contractId = doc.Contract_Id;
                db.Contract_Docs.Remove(doc);
                db.SaveChanges();
                //return RedirectToAction("Index", "Home");
                return Json(new { success = true, contractId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi xóa hồ sơ: " + ex.Message });
            }
           
        }

        //30.10.2025 cập nhật trạng thái Finished của hợp đồng (chỉ cho Admin)
        [HttpPost]
        public JsonResult UpdateFinishedStatus(int id, bool finished)
        {
            try
            {
                using (var db = new IncosafCMSContext())
                {
                    var contract = db.Contracts.FirstOrDefault(x => x.Id == id);
                    if (contract == null)
                        return Json(new { success = false, message = "Không tìm thấy hợp đồng!" });

                    // Chỉ admin được sửa                    
                    if (!User.IsInRole("Admin"))
                        return Json(new { success = false, message = "Bạn không có quyền cập nhật!" });

                    contract.Finished = finished;
                    db.SaveChanges();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //3.11.2025 thêm Dashboard Cty       
        // Lấy danh sách đơn vị      
        public JsonResult GetDepartments()
        {
            // Lấy danh sách đầy đủ (cho Admin)
            var list = db.Departments
                .Select(x => new {
                    Value = x.Id,
                    Text = x.Name,
                    MaDV = x.MaDV
                }).ToList();

            var directorDept = (object)null;            

            // Lọc theo Trưởng Đơn vị
            var user = userManager.FindByName(User.Identity.Name);            
            
            if (User.IsInRole("DeptDirector") || (User.IsInRole("Admin") && user.Department.Id == 8))
            {
                list = db.Departments
                    .Where(x => x.MaDV == user.Department.MaDV)
                    .Select(x => new {
                        Value = x.Id,
                        Text = x.Name,
                        MaDV = x.MaDV
                    }).ToList();
                
                // Lấy thông tin đơn vị của user đăng nhập
                directorDept = new
                {
                    Id = user.Department.Id,
                    Name = user.Department.Name,
                    MaDV = user.Department.MaDV
                };
                ViewBag.MaDV = user.Department.MaDV;
            }

            // Trả về JSON gồm 2 phần
            return Json(new
            {
                departments = list,
                directorDept = directorDept
            }, JsonRequestBehavior.AllowGet);
        }
       
        // DashboardCompanyPartial
        public ActionResult DashboardCompanyPartial(int? year, string reportType = "company", int? departmentId = null, string tabId = "main")
        {
            int yNow = DateTime.Now.Year;
            int mNow = DateTime.Now.Month;

            // 🔹 Logic năm tài chính mặc định
            int defaultYear = mNow < 2 ? yNow - 1 : yNow;
            year ??= defaultYear;

            ViewBag.CurrentYear = year;
            ViewBag.ReportType = reportType;
            ViewBag.TabId = tabId;

            /*
            year ??= DateTime.Now.Year;
            ViewBag.CurrentYear = year;
            ViewBag.ReportType = reportType;
            ViewBag.TabId = tabId;
            */
            bool isChild = Session["ShowFilterTabDashboard"] as bool? ?? false;

            if (Session["ShowFilterTabDashboard"] != null)
            {
                Session["ShowFilterTabDashboard"] = true;// Nếu chưa có thì gán mặc định = false
                ViewBag.IsChild = isChild;
            }

            if (reportType == "department" && departmentId == null)
            {
                var user = userManager.FindByName(User.Identity.Name);
                departmentId = user.Department.Id;
            }

            if (reportType == "department" && departmentId.HasValue)
            {
                var deptModel = LargeDatabaseDataProvider.GetDepartmentDetailFinance(year.Value, departmentId.Value);
                // 🔹 Lấy thông tin đơn vị từ dữ liệu (có thể tùy chỉnh theo structure model của bạn)
                var firstItem = deptModel?.FirstOrDefault();
                if (firstItem != null)
                {
                    // Giả sử trong model có các field MaDV và DepartmentName
                    ViewBag.DepartmentName = firstItem.DepartmentName ?? "";
                    ViewBag.MaDV = firstItem.MaDV ?? "";                    
                }
                else
                {
                    // fallback
                    ViewBag.DepartmentName = "";
                    ViewBag.MaDV = "";
                }

                return PartialView("_DashboardCompanyPartial", deptModel);

            }
            else
            {
                var companyModel = LargeDatabaseDataProvider.GetCompanyFinanceSummary(year.Value);
                ViewBag.DepartmentName = "Công ty";
                //ViewBag.MaDV = "CTY";
                return PartialView("_DashboardCompanyPartial", companyModel);
            }

        }

        public ActionResult DashboardCompanyGrid(int? year, string reportType = "company", int? departmentId = null)
        {
            year ??= DateTime.Now.Year;           

            ViewBag.ReportType = reportType;
            ViewBag.CurrentYear = year;

            if (reportType == "department" && departmentId.HasValue)
            {
                var deptModel = LargeDatabaseDataProvider.GetDepartmentDetailFinance(year.Value, departmentId.Value);
                return PartialView("_DashboardCompanyGrid", deptModel);
            }
            else
            {
                var companyModel = LargeDatabaseDataProvider.GetCompanyFinanceSummary(year.Value);
                return PartialView("_DashboardCompanyGrid", companyModel);
            }
        }

        public ActionResult DashboardCompanyGridLH(int? year, string reportType = "company", int? departmentId = null)
        {
            //year ??= DateTime.Now.Year;
            int yNow = DateTime.Now.Year;
            int mNow = DateTime.Now.Month;
            // 🔹 Logic năm tài chính mặc định
            int defaultYear = mNow < 2 ? yNow - 1 : yNow;
            year ??= defaultYear;

            ViewBag.CurrentYear = year;
            ViewBag.ReportType = reportType;            

            if (reportType == "department" && departmentId.HasValue)
            {
                var deptModel = LargeDatabaseDataProvider.GetDepartmentDetailFinance(year.Value, departmentId.Value);
                return PartialView("_DashboardCompanyGridLoaiHinh", deptModel);
            }
            else
            {
                var companyModel = LargeDatabaseDataProvider.GetCompanyFinanceSummary(year.Value);
                return PartialView("_DashboardCompanyGridLoaiHinh", companyModel);
            }
        }

        public ActionResult ExportDashboardToExcel(string type, int year, int? departmentId)
        {
            List<LargeDatabaseDataProvider.FinanceSummary> data = null;
            string departmentName = "";
            string departmentDV = "";
            string fromdate = "Từ ngày 01 / 01 / " + year;
            string todate = "đến ngày " + DateTime.Now.ToString("dd/MM/yyyy");

            if (year < DateTime.Now.Year)
                todate = "đến ngày 31/12/" + year;

            if (type == "company")
            {
                data = LargeDatabaseDataProvider.GetCompanyFinanceSummary(year);
                departmentName = "CÔNG TY";
                departmentDV = "Cty";
            }
                
            else if (type == "department" && departmentId.HasValue)
            {
                data = LargeDatabaseDataProvider.GetDepartmentDetailFinance(year, departmentId.Value);                
                departmentName = db.Departments.Where(x => x.Id == departmentId.Value).FirstOrDefault().Name.ToUpper();
                departmentDV = db.Departments.Where(x => x.Id == departmentId.Value).FirstOrDefault().MaDV;
            }  
            else
                return new HttpStatusCodeResult(400, "Invalid parameters");

            var settings = DashboardCompanyExportSettings(type, year, data.Cast<object>().ToList());

            // BƯỚC 1: DevExpress xuất ra MemoryStream
            using (var devExpressStream = new MemoryStream())   // ←←← ĐỔI TÊN ĐỂ KHÔNG TRÙNG
            {
                GridViewExtension.WriteXlsx(settings, data, devExpressStream);
                devExpressStream.Position = 0;

                // BƯỚC 2: ClosedXML đọc file + chèn 3 dòng + tăng font
                using (var workbook = new XLWorkbook(devExpressStream))
                {
                    var ws = workbook.Worksheets.First();

                    // Chèn 3 dòng ở đầu
                    ws.Row(1).InsertRowsAbove(3);

                    int lastCol = ws.LastColumnUsed()?.ColumnNumber() ?? 15;
                    int lastRow = ws.LastRowUsed().RowNumber() + 2; // ←←← Dòng cuối cùng của bảng
                    // Dòng 1
                    ws.Cell("A1").Value = "CÔNG TY CP INCOSAF";
                    ws.Cell("A1").Style.Font.FontSize = 13;
                    ws.Cell("A1").Style.Font.Bold = true;
                    ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(ws.Cell("A1"), ws.Cell(1, lastCol)).Merge();
                    ws.Row(1).Height = 15;

                    // Dòng 2                    
                    ws.Cell("A2").Value = $"BÁO CÁO TỔNG HỢP {departmentName} - NĂM {year}";
                    ws.Cell("A2").Style.Font.FontSize = 15;
                    ws.Cell("A2").Style.Font.Bold = true;
                    ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(ws.Cell("A2"), ws.Cell(2, lastCol)).Merge();
                    ws.Row(2).Height = 20;

                    // Dòng 3
                    //ws.Cell("A2").Value = type == "company" ? "Theo đơn vị" : "Chi tiết nhân viên";
                    ws.Cell(3, lastCol).Value = "(Đơn vị tính: 1.000đ)";
                    ws.Cell(3, lastCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Cell(3, 6).Value = "    " + fromdate + " " + todate;
                    ws.Cell(3, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    //ws.Range(ws.Cell("A3"), ws.Cell(3, lastCol)).Merge();
                    ws.Row(3).Height = 12;
                    ws.Row(3).Style.Font.FontSize = 11;

                    // Dòng cuối
                    string lastColLetter = XLHelper.GetColumnLetterFromNumber(lastCol - 1); 
                    ws.Cell($"A{lastRow}").Value = "Ngày báo cáo " + DateTime.Now.ToString("dd/MM/yyyy");
                    ws.Cell($"A{lastRow}").Style.Font.FontSize = 10;                   
                    ws.Cell($"A{lastRow}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Range(ws.Cell($"A{lastRow}"), ws.Cell(lastRow, lastCol)).Merge();
                    
                    // 4. HEADER CỘT (dòng 4): Wrap Text + Font 12 + Đậm                    
                    ws.Row(4).Style.Alignment.WrapText = true;
                    ws.Row(4).Style.Font.FontSize = 11;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Row(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Row(4).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 230, 241);                    
                    ws.Row(lastRow).Style.Alignment.WrapText = true;
                    // Cột "Tên đơn vị" hoặc "Tên nhân viên" – Wrap Text (cột B)
                    ws.Column(2).Style.Alignment.WrapText = true;

                    // 5. ĐẶT CỨNG WIDTH = .... CHO MỘT SỐ CỘT    
                    
                    ws.Column(1).Width = 3;
                    if (type == "department")
                    {
                        ws.Column(2).Style.Alignment.WrapText = false;
                        ws.Column(2).Style.Alignment.ShrinkToFit = true;
                    }
                                            

                    ws.Column(2).Width = 18;
                    ws.Column(3).Width = 10;
                    ws.Column(4).Width = 10;
                    ws.Column(5).Width = 5;
                    ws.Column(6).Width = 10;
                    ws.Column(7).Width = 10;
                    ws.Column(8).Width = 5;
                    ws.Column(9).Width = 10;
                    ws.Column(10).Width = 10;
                    ws.Column(11).Width = 5;
                    ws.Column(12).Width = 9;     // Dở dang trước 2025
                    ws.Column(13).Width = 9; // Dở dang 2025
                    ws.Column(14).Width = 9; // Công nợ trước 2025
                    ws.Column(15).Width = 9; // Công nợ 2025
                    
                    // Font toàn bảng
                    var dataRange = ws.Range(4, 1, ws.LastRowUsed().RowNumber(), lastCol);
                    dataRange.Style.Font.FontSize = 11;
                    dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Range(ws.Cell("A5"), ws.Cell(lastRow - 1, 1)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.LastRowUsed().Style.Font.Bold = true;
                    ws.LastRowUsed().Style.Alignment.WrapText = false;
                    ws.LastRowUsed().Style.Alignment.ShrinkToFit = true;

                    ws.Range(ws.Cell("A1"), ws.Cell(3, lastCol)).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                    ws.Range(ws.Cell("A1"), ws.Cell(3, lastCol)).Style.Border.InsideBorder = XLBorderStyleValues.None;                    
                    
                    ws.Range(ws.Cell($"A{lastRow - 1}"), ws.Cell(lastRow, lastCol)).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                    ws.Range(ws.Cell($"A{lastRow - 1}"), ws.Cell(lastRow, lastCol)).Style.Border.InsideBorder = XLBorderStyleValues.None;
                    
                    ws.Range(ws.Cell("A4"), ws.Cell(lastRow - 2, lastCol)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(ws.Cell("A4"), ws.Cell(lastRow - 2, lastCol)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // ĐẶT KHỔ A4 NGANG + FIT 1 TRANG
                    ws.PageSetup.PageOrientation = ClosedXML.Excel.XLPageOrientation.Landscape;
                    ws.PageSetup.PaperSize = ClosedXML.Excel.XLPaperSize.A4Paper;
                    ws.PageSetup.FitToPages(1, 1);
                    ws.PageSetup.Margins.Top = ws.PageSetup.Margins.Bottom = 0.5;
                    ws.PageSetup.Margins.Left = ws.PageSetup.Margins.Right = 0.0;
                    ws.PageSetup.CenterHorizontally = true;

                    // BƯỚC 3: Xuất ra Response
                    using (var finalStream = new MemoryStream())   // ←←← DÙNG STREAM MỚI
                    {
                        workbook.SaveAs(finalStream);
                        finalStream.Position = 0;

                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", $"attachment; filename=BC_{departmentDV}_{year}_{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
                        finalStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return new EmptyResult();
        }

        // GridViewSettings cho Export
        public static GridViewSettings DashboardCompanyExportSettings(string type, int year, List<object> data)        
        {
            var settings = new GridViewSettings();            
            settings.Name = "gvDashboardCompanyExport";
            settings.SettingsBehavior.AllowSort = false;
            settings.SettingsPager.Visible = false;
            settings.Settings.ShowFooter = true;
            settings.SettingsBehavior.AllowFocusedRow = false;

            settings.Columns.Add(column =>
            {
                column.Caption = "TT";
                column.FieldName = "STT"; // vẫn cần gán FieldName
                column.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
                column.VisibleIndex = 0;
                column.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            });
            settings.CustomUnboundColumnData = (sender, e) =>
            {
                if (e.Column.FieldName == "STT")
                {
                    e.Value = e.ListSourceRowIndex + 1; // thứ tự dòng
                }
            };
            // Cột tên đv/nv
            if (type == "company")
            {
                settings.Columns.Add(c =>
                {
                    c.FieldName = "DepartmentName";
                    c.Caption = "Tên đơn vị";
                    c.Width = 185;
                    c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                });
            }
            else
            {
                settings.Columns.Add(c =>
                {
                    c.FieldName = "EmployeeName";
                    c.Caption = "Tên nhân viên";
                    c.Width = 185;
                    c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                });
            }            

            settings.Columns.Add(c =>
            {
                c.FieldName = "SLKeHoach";
                c.Caption = "SL kế hoạch";
                //c.Width = 95;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SLThucHien";
                c.Caption = "SL thực hiện";
                //c.Width = 95;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeSL";
                c.Caption = "% SL";
                //c.Width = 50;
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "DTKeHoach";
                c.Caption = "DT kế hoạch";
                //c.Width = 95;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "DTThucHien";
                c.Caption = "DT thực hiện";
                //c.Width = 95;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeDT";
                c.Caption = "% DT";
                //c.Width = 50;
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "TNKeHoach";
                c.Caption = "Thu nợ KH";
                //c.Width = 95;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "TNThucHien";
                c.Caption = "Thu nợ TH";
                //c.Width = 95;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeTN";
                c.Caption = "% TN";
                //c.Width = 50;
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "CongNo";
                c.Caption = "Công nợ " + year;
                //c.Width = 80;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "CongNo_old";
                c.Caption = "Nợ trước " + year;
                //c.Width = 80;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "DoDang";
                c.Caption = "Dở dang " + year;
               // c.Width = 80;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "DoDang_old";
                c.Caption = "Dở dang trước " + year;
                //c.Width = 80;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            // Tổng cộng cuối bảng
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SLKeHoach").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SLThucHien").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "DTKeHoach").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "DTThucHien").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TNKeHoach").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TNThucHien").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CongNo").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CongNo_old").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "DoDang").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "DoDang_old").DisplayFormat = "{0:N0}";

            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeSL").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeDT").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeTN").DisplayFormat = "{0:P0}";

            double totalSLKH = 0, totalSLTH = 0;
            double totalDTKH = 0, totalDTTH = 0;
            double totalTNKH = 0, totalTNTH = 0;

            var list = data.Cast<LargeDatabaseDataProvider.FinanceSummary>().ToList();

            totalSLKH = list.Sum(x => x.SLKeHoach);
            totalSLTH = list.Sum(x => x.SLThucHien);

            totalDTKH = list.Sum(x => x.DTKeHoach);
            totalDTTH = list.Sum(x => x.DTThucHien);

            totalTNKH = list.Sum(x => x.TNKeHoach);
            totalTNTH = list.Sum(x => x.TNThucHien);

            settings.CustomSummaryCalculate = (sender, e) =>
            {
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                {                    
                    string fieldName = null;

                    var prop = e.Item.GetType().GetProperty("FieldName");
                    if (prop != null)
                    {
                        var val = prop.GetValue(e.Item, null);
                        fieldName = val != null ? val.ToString() : null;
                    }
                    else
                    {
                        fieldName = null;
                    }

                    if (fieldName == "TyLeSL")
                    {
                        e.TotalValue = (totalSLKH == 0.0) ? 0.0 : (totalSLTH / totalSLKH);
                    }
                    else if (fieldName == "TyLeDT")
                    {
                        e.TotalValue = (totalDTKH == 0.0) ? 0.0 : (totalDTTH / totalDTKH);
                    }
                    else if (fieldName == "TyLeTN")
                    {
                        e.TotalValue = (totalTNKH == 0.0) ? 0.0 : (totalTNTH / totalTNKH);
                    }

                }
            };
            
            //settings.SettingsExport.FileName = $"BC_{type}_{year}.xlsx";
            // 2. TĂNG CỠ CHỮ TOÀN BỘ FILE EXCEL
            
            settings.SettingsExport.Styles.Default.Font.Size = 11;  // Chữ trong ô dữ liệu
            settings.SettingsExport.Styles.Header.Font.Size = 11;
            settings.SettingsExport.Styles.Header.Font.Bold = true;            
            settings.SettingsExport.Styles.Footer.Font.Size = 11;
            settings.SettingsExport.Styles.Footer.Font.Bold = true;  
            
            return settings;
        }

        //17.12.2025 thêm vẽ biểu đồ cho Dashboard 5 năm: SL; DT; TN; Các loại hình theo SL
        public JsonResult GetProduction5Years(string type, int? departmentId = null)
        {
            int currentYear = DateTime.Now.Year;
            //Nếu ngày hiện tại thuộc tháng 1 thì lấy báo cáo và vẽ biểu đồ của năm trước đó
            if (DateTime.Now.Month < 2)
                currentYear = currentYear - 1;

            var result = new List<object>();
            var user = userManager.FindByName(User.Identity.Name);            

            if (type == "department" && departmentId == null) departmentId = user.Department.Id;

            for (int y = currentYear - 4; y <= currentYear; y++)
            {
                double sl = 0, dt = 0, tn = 0;
                double slTBN = 0, slTBAL = 0, slTNHT = 0, slTNPTN = 0, slTBTC = 0, slHL = 0;                

                if (type == "company")
                {
                    var data = LargeDatabaseDataProvider.GetCompanyFinanceSummary(y);
                    sl = data.Sum(x => x.SLThucHien);
                    dt = data.Sum(x => x.DTThucHien);
                    tn = data.Sum(x => x.TNThucHien);
                    slTBN = data.Sum(x => x.SL_TBN);
                    slTBAL = data.Sum(x => x.SL_TBAL);
                    slTNHT = data.Sum(x => x.SL_TNHT);
                    slTNPTN = data.Sum(x => x.SL_TNPTN);
                    slTBTC = data.Sum(x => x.SL_TBTC);
                    slHL = data.Sum(x => x.SL_HL);
                }
                else if (type == "department" && departmentId.HasValue)
                {
                    var data = LargeDatabaseDataProvider.GetDepartmentDetailFinance(y, departmentId.Value);
                    sl = data.Sum(x => x.SLThucHien);
                    dt = data.Sum(x => x.DTThucHien);
                    tn = data.Sum(x => x.TNThucHien);
                    slTBN = data.Sum(x => x.SL_TBN);
                    slTBAL = data.Sum(x => x.SL_TBAL);
                    slTNHT = data.Sum(x => x.SL_TNHT);
                    slTNPTN = data.Sum(x => x.SL_TNPTN);
                    slTBTC = data.Sum(x => x.SL_TBTC);
                    slHL = data.Sum(x => x.SL_HL);
                }

                result.Add(new
                {
                    Year = y,
                    SanLuong = sl,
                    DoanhThu = dt,
                    ThuNo = tn,
                    SL_TBN = slTBN,
                    SL_TBAL = slTBAL,
                    SL_TNHT = slTNHT,
                    SL_TNPTN = slTNPTN,
                    SL_TBTC = slTBTC,
                    SL_HL = slHL
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionByLoaiHinh(int year, string type, int? departmentId = null)
        {
            var user = userManager.FindByName(User.Identity.Name);

            if (type == "department" && departmentId == null)
                departmentId = user.Department.Id;

            var result = new
            {
                SL_TBN = 0.0,
                SL_TBAL = 0.0,
                SL_HQ = 0.0,
                SL_HL = 0.0,
                SL_AK = 0.0,
                SL_TNHT = 0.0,
                SL_TNPTN = 0.0,
                SL_TBTC = 0.0,
                SL_TVXD = 0.0,
                SL_KHAC = 0.0
            };

            if (type == "company")
            {
                var data = LargeDatabaseDataProvider.GetCompanyFinanceSummary(year);

                result = new
                {
                    SL_TBN = data.Sum(x => x.SL_TBN),
                    SL_TBAL = data.Sum(x => x.SL_TBAL),
                    SL_HQ = data.Sum(x => x.SL_HQ),
                    SL_HL = data.Sum(x => x.SL_HL),
                    SL_AK = data.Sum(x => x.SL_AK),
                    SL_TNHT = data.Sum(x => x.SL_TNHT),
                    SL_TNPTN = data.Sum(x => x.SL_TNPTN),
                    SL_TBTC = data.Sum(x => x.SL_TBTC),
                    SL_TVXD = data.Sum(x => x.SL_TVXD),
                    SL_KHAC = data.Sum(x => x.SL_KHAC)
                };
            }
            else if (type == "department" && departmentId.HasValue)
            {
                var data = LargeDatabaseDataProvider.GetDepartmentDetailFinance(year, departmentId.Value);

                result = new
                {
                    SL_TBN = data.Sum(x => x.SL_TBN),
                    SL_TBAL = data.Sum(x => x.SL_TBAL),
                    SL_HQ = data.Sum(x => x.SL_HQ),
                    SL_HL = data.Sum(x => x.SL_HL),
                    SL_AK = data.Sum(x => x.SL_AK),
                    SL_TNHT = data.Sum(x => x.SL_TNHT),
                    SL_TNPTN = data.Sum(x => x.SL_TNPTN),
                    SL_TBTC = data.Sum(x => x.SL_TBTC),
                    SL_TVXD = data.Sum(x => x.SL_TVXD),
                    SL_KHAC = data.Sum(x => x.SL_KHAC)
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportDashboardToExcelLH(string type, int year, int? departmentId)
        {
            List<LargeDatabaseDataProvider.FinanceSummary> data = null;
            string departmentName = "";
            string departmentDV = "";
            string fromdate = "Từ ngày 01/01/" + year;
            string todate = "đến ngày " + DateTime.Now.ToString("dd/MM/yyyy");

            if (year < DateTime.Now.Year)
                todate = "đến ngày 31/12/" + year;

            if (type == "company")
            {
                data = LargeDatabaseDataProvider.GetCompanyFinanceSummary(year);
                departmentName = "CÔNG TY";
                departmentDV = "Cty";
            }

            else if (type == "department" && departmentId.HasValue)
            {
                data = LargeDatabaseDataProvider.GetDepartmentDetailFinance(year, departmentId.Value);
                departmentName = db.Departments.Where(x => x.Id == departmentId.Value).FirstOrDefault().Name.ToUpper();
                departmentDV = db.Departments.Where(x => x.Id == departmentId.Value).FirstOrDefault().MaDV;
            }
            else
                return new HttpStatusCodeResult(400, "Invalid parameters");

            var settings = DashboardCompanyExportSettingsLH(type, year, data.Cast<object>().ToList());

            // BƯỚC 1: DevExpress xuất ra MemoryStream
            using (var devExpressStream = new MemoryStream())   // ←←← ĐỔI TÊN ĐỂ KHÔNG TRÙNG
            {
                GridViewExtension.WriteXlsx(settings, data, devExpressStream);
                devExpressStream.Position = 0;

                // BƯỚC 2: ClosedXML đọc file + chèn 3 dòng + tăng font
                using (var workbook = new XLWorkbook(devExpressStream))
                {
                    var ws = workbook.Worksheets.First();

                    // Chèn 3 dòng ở đầu
                    ws.Row(1).InsertRowsAbove(3);

                    int lastCol = ws.LastColumnUsed()?.ColumnNumber() ?? 15;
                    int lastRow = ws.LastRowUsed().RowNumber() + 2; // ←←← Dòng cuối cùng của bảng
                    // Dòng 1
                    ws.Cell("A1").Value = "CÔNG TY CP INCOSAF";
                    ws.Cell("A1").Style.Font.FontSize = 13;
                    ws.Cell("A1").Style.Font.Bold = true;
                    ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(ws.Cell("A1"), ws.Cell(1, lastCol)).Merge();
                    ws.Row(1).Height = 15;

                    // Dòng 2                    
                    ws.Cell("A2").Value = $"BÁO CÁO SẢN LƯỢNG THEO LOẠI HÌNH {departmentName} - NĂM {year}";
                    ws.Cell("A2").Style.Font.FontSize = 15;
                    ws.Cell("A2").Style.Font.Bold = true;
                    ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(ws.Cell("A2"), ws.Cell(2, lastCol)).Merge();
                    ws.Row(2).Height = 20;

                    // Dòng 3
                    //ws.Cell("A2").Value = type == "company" ? "Theo đơn vị" : "Chi tiết nhân viên";
                    ws.Cell(3, lastCol).Value = "(Đơn vị tính: 1.000đ)";
                    ws.Cell(3, lastCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Cell(3, 6).Value = "    " + fromdate + " " + todate;
                    ws.Cell(3, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    //ws.Range(ws.Cell("A3"), ws.Cell(3, lastCol)).Merge();
                    ws.Row(3).Height = 12;
                    ws.Row(3).Style.Font.FontSize = 11;

                    // Dòng cuối
                    string lastColLetter = XLHelper.GetColumnLetterFromNumber(lastCol - 1);
                    ws.Cell($"A{lastRow}").Value = "Ngày báo cáo " + DateTime.Now.ToString("dd/MM/yyyy");
                    ws.Cell($"A{lastRow}").Style.Font.FontSize = 10;
                    ws.Cell($"A{lastRow}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Range(ws.Cell($"A{lastRow}"), ws.Cell(lastRow, lastCol)).Merge();

                    // 4. HEADER CỘT (dòng 4): Wrap Text + Font 12 + Đậm                    
                    ws.Row(4).Style.Alignment.WrapText = true;
                    ws.Row(4).Style.Font.FontSize = 11;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Row(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Row(4).Style.Fill.BackgroundColor = XLColor.FromArgb(220, 230, 241);
                    ws.Row(lastRow).Style.Alignment.WrapText = true;
                    // Cột "Tên đơn vị" hoặc "Tên nhân viên" – Wrap Text (cột B)
                    ws.Column(2).Style.Alignment.WrapText = true;

                    // 5. ĐẶT CỨNG WIDTH = .... CHO MỘT SỐ CỘT
                    ws.Column(1).Width = 3;
                    if (type == "department")
                    {
                        ws.Column(2).Style.Alignment.WrapText = false;
                        ws.Column(2).Style.Alignment.ShrinkToFit = true;
                    }

                    ws.Column(2).Width = 16;
                    ws.Column(3).Width = 9.5;
                    ws.Column(4).Width = 4;
                    //ws.Column(4).Style.Font.FontSize = 8;
                    ws.Column(5).Width = 9.5;
                    ws.Column(6).Width = 4;                    
                    ws.Column(7).Width = 9;
                    ws.Column(8).Width = 4;
                    ws.Column(9).Width = 9;
                    ws.Column(10).Width = 4;
                    ws.Column(11).Width = 9;
                    ws.Column(12).Width = 4;
                    ws.Column(13).Width = 9.5; 
                    ws.Column(14).Width = 4; 
                    ws.Column(15).Width = 9.5;
                    ws.Column(16).Width = 4;
                    ws.Column(17).Width = 9;
                    ws.Column(18).Width = 4;
                    ws.Column(19).Width = 9;
                    ws.Column(20).Width = 4;
                    ws.Column(21).Width = 9;
                    ws.Column(22).Width = 11; // Tổng cộng

                    // Font toàn bảng
                    var dataRange = ws.Range(4, 1, ws.LastRowUsed().RowNumber(), lastCol);
                    dataRange.Style.Font.FontSize = 11;
                    dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Range(ws.Cell("A5"), ws.Cell(lastRow - 1, 1)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.LastRowUsed().Style.Font.Bold = true;
                    ws.LastRowUsed().Style.Alignment.WrapText = false;
                    ws.LastRowUsed().Style.Alignment.ShrinkToFit = true;

                    ws.Range(ws.Cell("A1"), ws.Cell(3, lastCol)).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                    ws.Range(ws.Cell("A1"), ws.Cell(3, lastCol)).Style.Border.InsideBorder = XLBorderStyleValues.None;

                    ws.Range(ws.Cell($"A{lastRow - 1}"), ws.Cell(lastRow, lastCol)).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                    ws.Range(ws.Cell($"A{lastRow - 1}"), ws.Cell(lastRow, lastCol)).Style.Border.InsideBorder = XLBorderStyleValues.None;

                    ws.Range(ws.Cell("A4"), ws.Cell(lastRow - 2, lastCol)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(ws.Cell("A4"), ws.Cell(lastRow - 2, lastCol)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // ĐẶT KHỔ A4 NGANG + FIT 1 TRANG
                    ws.PageSetup.PageOrientation = ClosedXML.Excel.XLPageOrientation.Landscape;
                    ws.PageSetup.PaperSize = ClosedXML.Excel.XLPaperSize.A4Paper;
                    ws.PageSetup.FitToPages(1, 1);
                    ws.PageSetup.Margins.Top = ws.PageSetup.Margins.Bottom = 0.5;
                    ws.PageSetup.Margins.Left = ws.PageSetup.Margins.Right = 0.0;
                    ws.PageSetup.CenterHorizontally = true;

                    // BƯỚC 3: Xuất ra Response
                    using (var finalStream = new MemoryStream())   // ←←← DÙNG STREAM MỚI
                    {
                        workbook.SaveAs(finalStream);
                        finalStream.Position = 0;

                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", $"attachment; filename=BCLoaiHinh_{departmentDV}_{year}_{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
                        finalStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return new EmptyResult();
        }

        public static GridViewSettings DashboardCompanyExportSettingsLH(string type, int year, List<object> data)
        {
            var settings = new GridViewSettings();
            settings.Name = "gvDashboardCompanyExportLH";
            settings.SettingsBehavior.AllowSort = false;
            settings.SettingsPager.Visible = false;
            settings.Settings.ShowFooter = true;
            settings.SettingsBehavior.AllowFocusedRow = false;

            settings.Columns.Add(column =>
            {
                column.Caption = "TT";
                column.FieldName = "STT"; // vẫn cần gán FieldName
                column.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
                column.VisibleIndex = 0;
                column.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            });
            settings.CustomUnboundColumnData = (sender, e) =>
            {
                if (e.Column.FieldName == "STT")
                {
                    e.Value = e.ListSourceRowIndex + 1; // thứ tự dòng
                }
            };
            // Cột tên đv/nv
            if (type == "company")
            {
                settings.Columns.Add(c =>
                {
                    c.FieldName = "DepartmentName";
                    c.Caption = "Tên đơn vị";
                    c.Width = 185;
                    c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                });
            }
            else
            {
                settings.Columns.Add(c =>
                {
                    c.FieldName = "EmployeeName";
                    c.Caption = "Tên nhân viên";
                    c.Width = 185;
                    c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                });
            }

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_TBN";
                c.Caption = "Thiết bị nâng";
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeTBN";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_TBAL";
                c.Caption = "TB Áp lực";
                //c.Width = 95;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeTBAL";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_HQ";
                c.Caption = "Hợp quy - Giám định";
                //c.Width = 95;
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeHQ";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_HL";
                c.Caption = "Huấn luyện";
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeHL";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_AK";
                c.Caption = "Áp kế";                
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeAK";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_TNHT";
                c.Caption = "TN hiện trường";
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeTNHT";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_TNPTN";
                c.Caption = "TN Phòng TN";
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeTNPTN";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_TBTC";
                c.Caption = "TB Công nghệ";
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeTBTC";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_TVXD";
                c.Caption = "Tư vấn XD";
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "TyLeTVXD";
                c.Caption = "%";
                c.PropertiesEdit.DisplayFormatString = "P0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "SL_KHAC";
                c.Caption = "SL Khác";
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "TongDonVi";
                c.Caption = "Tổng đơn vị";
                c.PropertiesEdit.DisplayFormatString = "N0";
                c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            });            
            // Tổng cộng cuối bảng
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_TBN").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_TBAL").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_HQ").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_HL").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_AK").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_TNHT").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_TNPTN").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_TBTC").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_TVXD").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "SL_KHAC").DisplayFormat = "{0:N0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "TongDonVi").DisplayFormat = "{0:N0}";

            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeTBN").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeTBAL").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeHQ").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeHL").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeTNHT").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeTNPTN").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeTBTC").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeTVXD").DisplayFormat = "{0:P0}";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "TyLeAK").DisplayFormat = "{0:P0}";

            double totalSLAK = 0, totalSLHL = 0;
            double totalSLHQ = 0, totalSLKHAC = 0;
            double totalSLTBAL = 0, totalSLTBN = 0;
            double totalSLTBTC = 0, totalSLTNHT = 0;
            double totalSLTNPTN = 0, totalSLTVXD = 0;
            double totalAll = 0;

            var list = data.Cast<LargeDatabaseDataProvider.FinanceSummary>().ToList();

            totalSLAK = list.Sum(x => x.SL_AK);
            totalSLHL = list.Sum(x => x.SL_HL);

            totalSLHQ = list.Sum(x => x.SL_HQ);
            totalSLKHAC = list.Sum(x => x.SL_KHAC);

            totalSLTBAL = list.Sum(x => x.SL_TBAL);
            totalSLTBN = list.Sum(x => x.SL_TBN);

            totalSLTBTC = list.Sum(x => x.SL_TBTC);
            totalSLTNHT = list.Sum(x => x.SL_TNHT);

            totalSLTNPTN = list.Sum(x => x.SL_TNPTN);
            totalSLTVXD = list.Sum(x => x.SL_TVXD);
            totalAll = totalSLTBN + totalSLTBAL + totalSLHQ + totalSLHL + totalSLAK +
                        totalSLTNHT + totalSLTNPTN + totalSLTVXD + totalSLTBTC + totalSLKHAC;
            settings.CustomSummaryCalculate = (sender, e) =>
            {
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                {
                    string fieldName = null;

                    var prop = e.Item.GetType().GetProperty("FieldName");
                    if (prop != null)
                    {
                        var val = prop.GetValue(e.Item, null);
                        fieldName = val != null ? val.ToString() : null;
                    }
                    else
                    {
                        fieldName = null;
                    }
                    if (fieldName == "TyLeTBN")
                    {
                        e.TotalValue = (totalSLTBN == 0.0) ? 0.0 : (totalSLTBN / totalAll);
                    }
                    else if (fieldName == "TyLeTBAL")
                    {
                        e.TotalValue = (totalSLTBAL == 0.0) ? 0.0 : (totalSLTBAL / totalAll);
                    }
                    else if (fieldName == "TyLeHQ")
                    {
                        e.TotalValue = (totalSLHQ == 0.0) ? 0.0 : (totalSLHQ / totalAll);
                    }
                    else if (fieldName == "TyLeHL")
                    {
                        e.TotalValue = (totalSLHL == 0.0) ? 0.0 : (totalSLHL / totalAll);
                    }
                    else if (fieldName == "TyLeAK")
                    {
                        e.TotalValue = (totalSLAK == 0.0) ? 0.0 : (totalSLAK / totalAll);
                    }
                    else if (fieldName == "TyLeTNHT")
                    {
                        e.TotalValue = (totalSLTNHT == 0.0) ? 0.0 : (totalSLTNHT / totalAll);
                    }
                    else if (fieldName == "TyLeTNPTN")
                    {
                        e.TotalValue = (totalSLTNPTN == 0.0) ? 0.0 : (totalSLTNPTN / totalAll);
                    }
                    else if (fieldName == "TyLeTVXD")
                    {
                        e.TotalValue = (totalSLTVXD == 0.0) ? 0.0 : (totalSLTVXD / totalAll);
                    }
                    else if (fieldName == "TyLeTBTC")
                    {
                        e.TotalValue = (totalSLTBTC == 0.0) ? 0.0 : (totalSLTBTC / totalAll);
                    }

                }
            };
           
            // 2. TĂNG CỠ CHỮ TOÀN BỘ FILE EXCEL
            settings.SettingsExport.Styles.Default.Font.Size = 11;  // Chữ trong ô dữ liệu
            settings.SettingsExport.Styles.Header.Font.Size = 11;
            settings.SettingsExport.Styles.Header.Font.Bold = true;
            settings.SettingsExport.Styles.Footer.Font.Size = 11;
            settings.SettingsExport.Styles.Footer.Font.Bold = true;

            return settings;
        }


    }
}
