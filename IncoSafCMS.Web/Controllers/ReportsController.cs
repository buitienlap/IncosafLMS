using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using IncosafCMS.Web.Helpers;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using IncosafCMS.Web.Dto;
using IncosafCMS.Core.DomainModels.Identity;
using System.Text;
using System.IO;
using DevExpress.XtraReports.UI;
using System.Data.Entity;


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


namespace IncosafCMS.Web.Controllers
{
    public class ReportsController : Controller
    {
        IService<Accreditation> service;
        IUnitOfWork uow;
        public ReportsController(IService<Accreditation> _service, IUnitOfWork _uow)
        {
            service = _service;
            uow = _uow;
        }

        //Hàm dùng chung lấy thông tin TB khi in đơn lẻ 1 TB hoặc in nhiều TB cùng lúc
        private AccreditationDto BuildAccreditationDto(int equipmentId)
        {
            // --- Xử lý QRCode
            string qr_link = "";
            string qrcode = GetEquipment_QRCode_GCN(equipmentId, false);
            if (!string.IsNullOrEmpty(qrcode))
            {
                string cQR_WebQuery_gcn = "gcn";
                qr_link = BuildQRLink(cQR_WebQuery_gcn, qrcode);
            }

            // --- Lấy model Accreditation
            Accreditation model;
            if (User.IsInRole("Admin"))
            {
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == equipmentId).FirstOrDefault();
            }
            else
            {
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == equipmentId).FirstOrDefault();
            }

            if (model == null) return null;

            // --- Kiểm tra đủ thông số kỹ thuật
            if (model.equiment?.specifications == null || model.equiment.specifications.Count == 0)
                return null;

            // --- Tạo DTO
            var des = new AccreditationDto
            {
                Id = model.Id,
                equiment = model.equiment,
                NumberAcc = model.NumberAcc,
                AccrDate = model.AccrDate,
                DateOfNext = model.DateOfNext,
                StampNumber = model.StampNumber,
                TypeAcc = model.TypeAcc,
                AccrResultDate = model.AccrResultDate,
                Location = model.Location,
                PartionsNotice = model.PartionsNotice,
                LoadTestNotice = model.LoadTestNotice,
                QRCode_GCN = qrcode,
                QRCodeLink = qr_link
            };

            // --- Thông tin kiểm định viên
            if (des.Tester1Id.HasValue && des.Tester1Id.Value > 0)
            {
                des.Tester1 = uow.Repository<AppUser>().GetSingle(des.Tester1Id.Value);
            }
            if (des.Tester2Id.HasValue && des.Tester2Id.Value > 0)
            {
                des.Tester2 = uow.Repository<AppUser>().GetSingle(des.Tester2Id.Value);
            }

            return des;
        }


        // GET: Reports
        public ActionResult AccreditationCertificateReport(int id)
        {
            var model = new Accreditation();            
            if (User.IsInRole("Admin"))
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id).FirstOrDefault() ?? new Accreditation();
            else
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id
                                        && x.equiment.contract != null
                                        && x.equiment.contract.own != null
                                        && x.equiment.contract.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Accreditation();

            var config = new AutoMapperConfig().Configure();
            IMapper mapper = config.CreateMapper();
            var des = new AccreditationDto();
            mapper.Map(model, des);
            if(des.Tester1Id.HasValue && des.Tester1Id.Value > 0)
            {
                des.Tester1 = uow.Repository<AppUser>().GetSingle(des.Tester1Id.Value);
            }
            if (des.Tester2Id.HasValue && des.Tester2Id.Value > 0)
            {
                des.Tester2 = uow.Repository<AppUser>().GetSingle(des.Tester2Id.Value);
            }
            ReportHelper.AccreditationReportDataSource = des;

            return View(new AccreditationCertificateReport());
        }
        //Mới thay 5.7.2025
        // GET: Reports        
        public ActionResult AccreditationCertificateReportA5(int id)
        {
            var dto = BuildAccreditationDto(id);
            if (dto == null)
                return Json(new object[] { "error", "KHÔNG ĐỦ DỮ LIỆU HOẶC KHÔNG TÌM THẤY THIẾT BỊ!" }, JsonRequestBehavior.AllowGet);

            ReportHelper.AccreditationReportDataSource = dto;
            return View(new AccreditationCertificateReportA5());
        }

        //16.10.2025 gộp in GCN
       [HttpGet]
        public ActionResult AccreditationCertificateReportA5_Multi(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return Json(new { error = "Không có danh sách thiết bị để in!" }, JsonRequestBehavior.AllowGet);

            var idList = ids.Split(',').Select(int.Parse).ToList();
            var mainReport = new XtraReport();

            foreach (var equipmentId in idList)
            {
                var dto = BuildAccreditationDto(equipmentId);
                if (dto == null) continue; // bỏ qua thiết bị lỗi hoặc thiếu TSKT

                ReportHelper.AccreditationReportDataSource = dto;

                var subReport = new AccreditationCertificateReportA5();
                subReport.CreateDocument();
                mainReport.Pages.AddRange(subReport.Pages);
            }

            if (mainReport.Pages.Count == 0)
                return Json(new { error = "Không có thiết bị nào hợp lệ để in!" }, JsonRequestBehavior.AllowGet);

            using (var ms = new MemoryStream())
            {
                mainReport.ExportToPdf(ms);
                ms.Seek(0, SeekOrigin.Begin);
                // 👉 Cách mở trực tiếp PDF trên trình duyệt
                Response.AppendHeader("Content-Disposition", "inline; filename=GCN_Gop.pdf");
                return File(ms.ToArray(), "application/pdf");
                
            }
        }




        /*Đóng lại 5.7.2025
         public ActionResult AccreditationCertificateReportA5(int id)
        {            
            // chèn vào đây đoạn xly QRCode
            string qr_link = "";
            string qrcode = GetEquipment_QRCode_GCN(id, false);     // lấy hoặc tạo mã QR
            if (!string.IsNullOrEmpty(qrcode))
            {
                // tạo link QR
                string cQR_WebQuery_gcn = "gcn";
                qr_link = BuildQRLink(cQR_WebQuery_gcn, qrcode);
            }

            // code sau vẫn đưa DL sang report
            var model = new Accreditation();
            if (User.IsInRole("Admin"))
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id).FirstOrDefault() ?? new Accreditation();
            else
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id
                                        && x.equiment.contract != null
                                        && x.equiment.contract.own != null
                                        && x.equiment.contract.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Accreditation();

            // Kiểm tra TB khác KĐAT thì không in
            if (model.equiment.specifications.Count == 0)
            {
                return Json("CHƯA ĐỦ THÔNG TIN IN KẾT QUẢ THIẾT BỊ NÀY!", JsonRequestBehavior.AllowGet);
            }
            var config = new AutoMapperConfig().Configure();
            IMapper mapper = config.CreateMapper();
            var des = new AccreditationDto();
            mapper.Map(model, des);
            if (des.Tester1Id.HasValue && des.Tester1Id.Value > 0)
            {
                des.Tester1 = uow.Repository<AppUser>().GetSingle(des.Tester1Id.Value);
            }
            if (des.Tester2Id.HasValue && des.Tester2Id.Value > 0)
            {
                des.Tester2 = uow.Repository<AppUser>().GetSingle(des.Tester2Id.Value);
            }

            // mapping QR info
            des.QRCodeLink = qr_link;
            des.QRCode_GCN = qrcode;

            ReportHelper.AccreditationReportDataSource = des;
            //ReportHelper.AccreditationReportDataSource = model;

            return View(new AccreditationCertificateReportA5());
        }         
         */

        // GET: Reports
        public ActionResult AccreditationCertificateReportML(int id)
        {
            var model = new Accreditation();
            if (User.IsInRole("Admin"))
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id).FirstOrDefault() ?? new Accreditation();
            else
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id
                                        && x.equiment.contract != null
                                        && x.equiment.contract.own != null
                                        && x.equiment.contract.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Accreditation();

            var config = new AutoMapperConfig().Configure();
            IMapper mapper = config.CreateMapper();
            var des = new AccreditationDto();
            mapper.Map(model, des);
            if (des.Tester1Id.HasValue && des.Tester1Id.Value > 0)
            {
                des.Tester1 = uow.Repository<AppUser>().GetSingle(des.Tester1Id.Value);
            }
            if (des.Tester2Id.HasValue && des.Tester2Id.Value > 0)
            {
                des.Tester2 = uow.Repository<AppUser>().GetSingle(des.Tester2Id.Value);
            }
            ReportHelper.AccreditationReportDataSource = des;
            //ReportHelper.AccreditationReportDataSource = model;
            return View(new AccreditationCertificateReportML());
        }

        // GET: Reports
        public ActionResult AccreditationReport(int id)
        {
            var model = new Accreditation();
            if (User.IsInRole("Admin"))
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id).FirstOrDefault() ?? new Accreditation();
            else
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id
                                        && x.equiment.contract != null
                                        && x.equiment.contract.own != null
                                        && x.equiment.contract.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Accreditation();
            var config = new AutoMapperConfig().Configure();
            IMapper mapper = config.CreateMapper();
            var des = new AccreditationDto();
            mapper.Map(model, des);
            if (des.Tester1Id.HasValue && des.Tester1Id.Value > 0)
            {
                des.Tester1 = uow.Repository<AppUser>().GetSingle(des.Tester1Id.Value);
            }
            if (des.Tester2Id.HasValue && des.Tester2Id.Value > 0)
            {
                des.Tester2 = uow.Repository<AppUser>().GetSingle(des.Tester2Id.Value);
            }
            ReportHelper.AccreditationReportDataSource = des;
            //ReportHelper.AccreditationReportDataSource = model;
            return View(new AccreditationReport());
        }

        // GET: Reports
        public ActionResult AccreditationReportTM(int id)
        {
            var model = new Accreditation();
            if (User.IsInRole("Admin"))
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id).FirstOrDefault() ?? new Accreditation();
            else
                model = service.FindBy(x => x.equiment != null && x.equiment.Id == id
                                        && x.equiment.contract != null
                                        && x.equiment.contract.own != null
                                        && x.equiment.contract.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Accreditation();
            var config = new AutoMapperConfig().Configure();
            IMapper mapper = config.CreateMapper();
            var des = new AccreditationDto();
            mapper.Map(model, des);
            if (des.Tester1Id.HasValue && des.Tester1Id.Value > 0)
            {
                des.Tester1 = uow.Repository<AppUser>().GetSingle(des.Tester1Id.Value);
            }
            if (des.Tester2Id.HasValue && des.Tester2Id.Value > 0)
            {
                des.Tester2 = uow.Repository<AppUser>().GetSingle(des.Tester2Id.Value);
            }
            ReportHelper.AccreditationReportDataSource = des;
            //ReportHelper.AccreditationReportDataSource = model;
            return View(new AccreditationReportTM());
        }

        // GET: Reports
        public ActionResult ContractReport(int id)
        {
            var model = new Contract();
            var contractRepo = uow.Repository<Contract>();

            if (User.IsInRole("Admin"))
                model = contractRepo.FindBy(x => x.Id == id).FirstOrDefault() ?? new Contract();
            else
                model = contractRepo.FindBy(x => x.Id == id && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Contract();

            ReportHelper.ContractReportDataSource = model;
            return View(new ContractReport());
        }

        // GET: Reports
        public ActionResult ContractSuggestReport(int id)
        {
            var model = new Contract();
            var contractRepo = uow.Repository<Contract>();

            /* edited by lapbt 16-mar-2022. Allow role: Admin & Accountant to print Giay de nghi
             * Khi in report se:
             * - ContractSuggestReport(): khoi tao thong tin can de in, trong do
             * - Se lay gia tri tu: ReportHelper.ContractReportDataSource
             * - ContractReportDataSource: se duoc khoi tao tai day
             */
            if (User.IsInRole("Admin") || User.IsInRole("Accountant"))
                model = contractRepo.FindBy(x => x.Id == id).FirstOrDefault() ?? new Contract();
            else
                model = contractRepo.FindBy(x => x.Id == id && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Contract();

            ReportHelper.ContractReportDataSource = model;
            return View(new ContractSuggestReport());
        }
        // GET: Reports
        public ActionResult TurnOverSuggestReport(int id)
        {
            var model = new Contract();
            var contractRepo = uow.Repository<Contract>();
            if (User.IsInRole("Admin") || User.IsInRole("Accountant")) // edited by lapbt 16-mar-2022.Allow role: Admin & Accountant to print Giay de nghi xuat chung tu
                model = contractRepo.FindBy(x => x.Id == id).FirstOrDefault() ?? new Contract();
            else
                model = contractRepo.FindBy(x => x.Id == id && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Contract();

            ReportHelper.TurnOverReportDataSource = model;
            return View(new TurnOverSuggestReport());
        }
        // GET: Reports
        public ActionResult PriceQuotationReport(int id)
        {
            var model = new PriceQuotation();
            var pricequotationRepo = uow.Repository<PriceQuotation>();

            if (User.IsInRole("Admin"))
                model = pricequotationRepo.FindBy(x => x.Id == id).FirstOrDefault() ?? new PriceQuotation();
            else model = pricequotationRepo.FindBy(x => x.Id == id && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault() ?? new PriceQuotation();

            ReportHelper.PriceQuotationReportDataSource = model;
            return View(new PriceQuotationReport());
        }
        // GET: Reports
        public ActionResult ContractAcceptanceReport(int id)
        {
            var model = new Contract();
            var contractRepo = uow.Repository<Contract>();

            /* added by lapbt 19-jan-2025. Clone from ContractSuggestReport
             * Khi in report se:
             * - ContractAcceptanceReport(): khoi tao thong tin can de in, trong do
             * - Se lay gia tri tu: ReportHelper.ContractReportDataSource
             * - ContractReportDataSource: se duoc khoi tao tai day
             */
            if (User.IsInRole("Admin") || User.IsInRole("Accountant"))
                model = contractRepo.FindBy(x => x.Id == id).FirstOrDefault() ?? new Contract();
            else
                model = contractRepo.FindBy(x => x.Id == id && x.own != null && x.own.UserName == User.Identity.Name).FirstOrDefault() ?? new Contract();

            ReportHelper.ContractReportDataSource = model;
            return View(new ContractAcceptanceReport());
        }

        #region QRCode generate. Chưa tạo thành class riêng, nên để vào đây, có thể đưa ra helper để tạo đc ở nhiều chỗ

        public string GetEquipment_QRCode_GCN(int vEquip_id, bool isAutoLock = false)
        {
            var equip = uow.Repository<Equipment>().GetSingle(vEquip_id);
            if (equip != null)
            {
                if (!string.IsNullOrEmpty(equip.qr_gcn))
                {
                    return equip.qr_gcn;
                }
                else
                {
                    // gen & update QR code here
                    return GenUpdate_Equipment_QRCode_GCN(vEquip_id, isAutoLock);
                }
            } else
            {
                return "";      // k0 tim thay thiet bi
            }
            
        }

        private string GenUpdate_Equipment_QRCode_GCN(int vEquip_id, bool isAutoLock = false)
        {
            string vBranchID = System.Configuration.ConfigurationManager.AppSettings["BranchID"] ?? "";
            if (string.IsNullOrEmpty(vBranchID)) return "";

            short qr_br = Convert.ToInt16(vBranchID); // convert 01,02,03 to 1,2,3
            short qr_year = (short)(DateTime.Now.Year % 100);
            string qr_random = genRandomString();

            string qr_gcn = qr_br.ToString() + qr_year.ToString() + qr_random;
            if (isQRCodeExists_GCN(qr_gcn))
            {
                qr_random = genRandomString();
                qr_gcn = qr_br.ToString() + qr_year.ToString() + qr_random;
                if (isQRCodeExists_GCN(qr_gcn))
                {
                    qr_random = genRandomString();
                    qr_gcn = qr_br.ToString() + qr_year.ToString() + qr_random;
                    if (isQRCodeExists_GCN(qr_gcn))
                    {
                        return ""; // cho nay lam hoi do hoi, nhg dang ban nen chua xly them
                    }
                }
            }

            // ghi lai QRCode
            var equip = uow.Repository<Equipment>().GetSingle(vEquip_id);

            equip.isPrintGcn = true;
            equip.qr_year = qr_year;
            equip.qr_gcn = qr_gcn;
            if (isAutoLock) equip.status = 1;

            uow.Repository<Equipment>().Update(equip);
            uow.SaveChanges();
            return qr_gcn;
        }

        public bool isQRCodeExists_GCN(string qrcode)
        {
            if (qrcode.Trim().Length == 0) return false;
            short qr_year = Convert.ToInt16(qrcode.Substring(1, 2)); // 123abc. Lay ra vi tri 2-3 la nam cua GR. Muc dich de index cho nhanh trong tim kiem trong nam

            var existed = uow.Repository<Equipment>().FindBy(e => e.qr_year == qr_year && e.qr_gcn == qrcode);
            if (existed != null && existed.Count > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private string genRandomString(short len = 6)
        {
            Random rnd = new Random();
            string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789"; // bỏ đi chữ I, l vì nhìn bị lẫn
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                int index = rnd.Next(allowedChars.Length);
                result.Append(allowedChars[index]);
            }
            return result.ToString();
        }


        public string BuildQRLink(string doc_type, string qr_code)
        {
            string cQR_WebDomain_Net = "https://tracuu.incosaf.com";
            if (qr_code != "")
            {
                return string.Format("{0}/{1}?id={2}", cQR_WebDomain_Net, doc_type, qr_code);
            }
            else
            {
                return "";
            }
        }

        //Thêm code 21.7.2025 để in ảnh kèm GCN 
        public ActionResult AccreditationCertificateReportA5WithImage(int id, string imagePath = null)
        {
           
            try
            {
                // --- Truy xuất model & load dữ liệu cần thiết               
                var model = uow.Repository<Accreditation>().FindBy(x => x.equiment != null && x.equiment.Id == id).FirstOrDefault();
                              
                if (model == null)
                    return Json("KHÔNG TÌM THẤY BIÊN BẢN CỦA THIẾT BỊ!", JsonRequestBehavior.AllowGet);

                if (model.equiment?.specifications == null || model.equiment.specifications.Count == 0)
                    return Json(new object[] { "error", " CHƯA ĐỦ THÔNG TIN IN KẾT QUẢ THIẾT BỊ NÀY!" }, JsonRequestBehavior.AllowGet);
                //return Json("CHƯA ĐỦ THÔNG TIN IN KẾT QUẢ THIẾT BỊ NÀY!", JsonRequestBehavior.AllowGet);

                // --- Tạo QRCode
                string qrcode = GetEquipment_QRCode_GCN(id, false);
                string qr_link = string.IsNullOrEmpty(qrcode) ? "" : BuildQRLink("gcn", qrcode);

                // --- Tạo bản sao Equipment tách rời DbContext
                var equipmentDto = new Equipment
                {
                    Id = model.equiment.Id,
                    Code = model.equiment.Code,
                    Name = model.equiment.Name,
                    mahieu = model.equiment.mahieu,
                    ManuFacturer = model.equiment.ManuFacturer,
                    No = model.equiment.No,
                    YearOfProduction = model.equiment.YearOfProduction,
                    specifications = model.equiment.specifications?.Select(sp => new Specifications
                    {
                        Id = sp.Id,
                        Name = sp.Name,
                        Value = sp.Value,
                        f_unit = sp.f_unit
                    }).ToList(),
                    
                    contract = model.equiment.contract != null ? new Contract
                    {
                        Id = model.equiment.contract.Id,
                        MaHD = model.equiment.contract.MaHD,
                        customer = model.equiment.contract.customer != null ? new Customer
                        {
                            Id = model.equiment.contract.customer.Id,
                            Name = model.equiment.contract.customer.Name
                        } : null,
                        own = model.equiment.contract.own != null ? new AppUser
                        {
                            Id = model.equiment.contract.own.Id,
                            UserName = model.equiment.contract.own.UserName,
                            Department = model.equiment.contract.own.Department != null ? new Department
                            {
                                MaDV = model.equiment.contract.own.Department.MaDV
                            } : null
                        } : null
                    } : null
                    
                };

                // --- Tạo DTO
                var des = new AccreditationDto
                {
                    Id = model.Id,
                    equiment = equipmentDto,
                    NumberAcc = model.NumberAcc,
                    AccrDate = model.AccrDate,
                    DateOfNext = model.DateOfNext,
                    StampNumber = model.StampNumber,
                    TypeAcc = model.TypeAcc,
                    AccrResultDate = model.AccrResultDate,
                    Location = model.Location,
                    PartionsNotice = model.PartionsNotice,
                    LoadTestNotice = model.LoadTestNotice,
                    //Tester1Id = model.Tester1Id,
                    //Tester2Id = model.Tester2Id,
                    QRCode_GCN = qrcode,
                    QRCodeLink = qr_link
                };

                // --- Gán vào Session cho report sử dụng
                ReportHelper.AccreditationReportDataSource = des;
                // --- Tạo report
                var report = new AccreditationCertificateReportA5();

                // --- Gán đường dẫn ảnh (nếu có)
                if (!string.IsNullOrEmpty(imagePath))
                {                    
                    if (imagePath.StartsWith("~"))
                        imagePath = VirtualPathUtility.ToAbsolute(imagePath); // chuyển thành /UploadedImages/...

                    // Lấy base URL như http://192.168.0.202:820
                    var baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}";

                    // Gán đường dẫn đầy đủ vào report
                    var fullUrl = baseUrl + imagePath;

                    report.Parameters["SelectedImagePath"].Value = fullUrl;
                    report.Parameters["SelectedImagePath"].Visible = false;
                }                 

                
                // --- Trả về view chứa report
                return View("AccreditationCertificateReportA5", report);
                //return View(new AccreditationCertificateReportA5());
            }
            catch (Exception ex)
            {
                return Content("LỖI: " + ex.Message);
            }
        }

        

        #endregion

    }
}