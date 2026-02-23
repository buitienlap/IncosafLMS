using DevExpress.Web.Mvc;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.Services;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncosafCMS.Web.Controllers
{
    [Authorize]
    public class FinacialReportsController : Controller
    {
        IService<Department> service;
        //FinacialService Fservice;
        IUnitOfWork uow;
        IApplicationUserManager userManager;
        public FinacialReportsController(IService<Department> _service, IUnitOfWork _uow, IApplicationUserManager _userManager)
        {
            service = _service;
            uow = _uow;
            userManager = _userManager;
            //Fservice = new FinacialService();
        }
        // GET: FinacialReports
        public ActionResult ViewIndex()
        {
            return View();
        }
        public ActionResult EmployeesCallbackRouteValues()
        {
            int departmentID = (!string.IsNullOrWhiteSpace(Request.Params["DepartmentID"])) ? int.Parse(Request.Params["DepartmentID"]) : -1;
            if (departmentID > 0)
            {
                var department = service.GetById(departmentID);

                if (User.IsInRole("Admin") || User.IsInRole("TPTH") || User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
                    ViewData["Employees"] = department != null ? uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Department != null && x.Department.Id == department.Id).ToList() : new List<Core.DomainModels.Identity.AppUser>();
                else
                {
                    var user = userManager.FindByName(User.Identity.Name);
                    ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>() { user };
                }
            }
            else
            {
                ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
            }
            return PartialView();
        }
        //public ActionResult ChangeFinacialReportsSLGridFilterModePartial(string FromDate, string ToDate, int DepartmentID, int EmployeeID)
        //{
        //    if (DateTime.TryParseExact(FromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _fromDate))
        //        GridViewHelper.FinacialReportFilterFromDate = _fromDate;
        //    if (DateTime.TryParseExact(ToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _toDate))
        //        GridViewHelper.FinacialReportFilterToDate = _toDate;

        //    GridViewHelper.FinacialReportFilterDepartmentID = DepartmentID;
        //    GridViewHelper.FinacialReportFilterEmployeeID = EmployeeID;
        //    return SLOfContractViewPartial();
        //}

        //public ActionResult SLOfContractViewPartial()
        //{
        //    var FViewModal = Fservice.QuerySL(GridViewHelper.FinacialReportFilterFromDate, GridViewHelper.FinacialReportFilterToDate, GridViewHelper.FinacialReportFilterEmployeeID, GridViewHelper.FinacialReportFilterDepartmentID);
        //    return PartialView("_SLOfContractViewPartial", FViewModal.Finacials);
        //}
        //public ActionResult ChangeFinacialReportsDTGridFilterModePartial(string FromDate, string ToDate, int DepartmentID, int EmployeeID)
        //{
        //    if (DateTime.TryParseExact(FromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _fromDate))
        //        GridViewHelper.FinacialReportFilterFromDate = _fromDate;
        //    if (DateTime.TryParseExact(ToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _toDate))
        //        GridViewHelper.FinacialReportFilterToDate = _toDate;

        //    GridViewHelper.FinacialReportFilterDepartmentID = DepartmentID;
        //    GridViewHelper.FinacialReportFilterEmployeeID = EmployeeID;
        //    return DTOfContractViewPartial();
        //}
        //public ActionResult DTOfContractViewPartial()
        //{
        //    var FViewModal = Fservice.QueryDT(GridViewHelper.FinacialReportFilterFromDate, GridViewHelper.FinacialReportFilterToDate, GridViewHelper.FinacialReportFilterEmployeeID, GridViewHelper.FinacialReportFilterDepartmentID);
        //    return PartialView("_DTOfContractViewPartial", FViewModal.Finacials);
        //}
        
        public ActionResult ApplyReportSpreadsheetFilterModePartial()
        {
            if (User.IsInRole("Admin") || User.IsInRole("TPTH"))
            {
                ViewData["Departments"] = service.GetAll();
                if (GridViewHelper.FinacialReportFilterDepartmentID > 0)
                {
                    var department = service.GetById(GridViewHelper.FinacialReportFilterDepartmentID);

                    ViewData["Employees"] = department != null ? uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Department != null && x.Department.Id == department.Id).ToList() : new List<Core.DomainModels.Identity.AppUser>();
                }
                else
                {
                    ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
                }
            }
            else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
            {
                var user = userManager.FindByName(User.Identity.Name);
                ViewData["Departments"] = service.GetAll().Where(x => user != null && user.Department != null && x.Id == user.Department.Id).ToList();

                if (GridViewHelper.FinacialReportFilterDepartmentID > 0)
                {
                    var department = service.GetById(GridViewHelper.FinacialReportFilterDepartmentID);

                    ViewData["Employees"] = department != null ? uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Department != null && x.Department.Id == department.Id).ToList() : new List<Core.DomainModels.Identity.AppUser>();
                }
                else
                {
                    ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
                }
            }
            else
            {
                var user = userManager.FindByName(User.Identity.Name);
                ViewData["Departments"] = service.GetAll().Where(x => user != null && user.Department != null && x.Id == user.Department.Id).ToList();

                if (GridViewHelper.FinacialReportFilterDepartmentID > 0)
                {
                    var department = service.GetById(GridViewHelper.FinacialReportFilterDepartmentID);

                    ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>() { user };
                }
                else
                {
                    ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
                }
            }

            return PartialView("_ApplyReportSpreadsheetFilterModePartial");
        }
        [HttpPost]
        public ActionResult ApplyReportSpreadsheetFilterModePartial(int FilterType, string FromDate, string ToDate, int DepartmentID, int EmployeeID, string DepartmentName, string EmployeeName)
        {
            GridViewHelper.FinacialReportFilterType = FilterType < 0 ? 0 : FilterType;
            if (DateTime.TryParseExact(FromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _fromDate))
                GridViewHelper.FinacialReportFilterFromDate = _fromDate;
            if (DateTime.TryParseExact(ToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _toDate))
                GridViewHelper.FinacialReportFilterToDate = _toDate;

            if (User.IsInRole("Admin") || User.IsInRole("TPTH"))
            {
                GridViewHelper.FinacialReportFilterDepartmentID = DepartmentID;
                GridViewHelper.FinacialReportFilterEmployeeID = EmployeeID;
                GridViewHelper.FinacialReportFilterDepartmentName = string.IsNullOrEmpty(DepartmentName) ? "N/A" : DepartmentName;
                GridViewHelper.FinacialReportFilterEmployeeName = string.IsNullOrEmpty(EmployeeName) ? "N/A" : EmployeeName;
            }
            else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
            {
                var user = userManager.FindByName(User.Identity.Name);
                GridViewHelper.FinacialReportFilterDepartmentID = user.Department != null ? user.Department.Id : 0;
                GridViewHelper.FinacialReportFilterEmployeeID = EmployeeID;
                GridViewHelper.FinacialReportFilterDepartmentName = user != null && user.Department != null ? user.Department.Name : "N/A";
                GridViewHelper.FinacialReportFilterEmployeeName = string.IsNullOrEmpty(EmployeeName) ? "N/A" : EmployeeName;
            }
            else
            {
                var user = userManager.FindByName(User.Identity.Name);
                GridViewHelper.FinacialReportFilterDepartmentID = user.Department != null ? user.Department.Id : 0;
                GridViewHelper.FinacialReportFilterEmployeeID = user != null ? user.Id : 0;
                GridViewHelper.FinacialReportFilterDepartmentName = user != null && user.Department != null ? user.Department.Name : "N/A";
                GridViewHelper.FinacialReportFilterEmployeeName = user != null ? user.DisplayName : "N/A";
            }

            return Json("success");
        }
    }
}