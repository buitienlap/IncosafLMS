using System;
using System.Web;
using System.Web.SessionState;
using IncosafCMS.Data.Identity;
using Microsoft.AspNet.Identity.Owin;
using IncosafCMS.Core.DomainModels.Identity;


namespace IncosafCMS.Web
{
    public class GridViewHelper
    {
        const string OwnerContractGridFilterIndexSessionKey = "OwnerContractGridFilterIndexSessionKey";
        public static string OwnerContractGridFilter
        {
            get
            {
                if (Session[OwnerContractGridFilterIndexSessionKey] == null)
                    Session[OwnerContractGridFilterIndexSessionKey] = "N/A";
                return (string)Session[OwnerContractGridFilterIndexSessionKey];
            }
            set { HttpContext.Current.Session[OwnerContractGridFilterIndexSessionKey] = value; }
        }

        const string ContractGridFilterIndexSessionKey = "ContractGridFilterIndexSessionKey";
        public static int ContractGridFilterIndex
        {
            get
            {
                if (Session[ContractGridFilterIndexSessionKey] == null)
                    Session[ContractGridFilterIndexSessionKey] = 0;
                return (int)Session[ContractGridFilterIndexSessionKey];
            }
            set { HttpContext.Current.Session[ContractGridFilterIndexSessionKey] = value; }
        }
        const string ContractGridDepartmentCodeFilterSessionKey = "ContractGridDepartmentCodeFilterSessionKey";
        public static string ContractGridDepartmentCodeFilter
        {
            get
            {
                if (Session[ContractGridDepartmentCodeFilterSessionKey] == null)
                    Session[ContractGridDepartmentCodeFilterSessionKey] = "";
                return (string)Session[ContractGridDepartmentCodeFilterSessionKey];
            }
            set { HttpContext.Current.Session[ContractGridDepartmentCodeFilterSessionKey] = value; }
        }

        const string UserRoleSessionKey = "UserRoleSessionKey";
        public static string UserRoleFilter
        {
            get
            {
                if (Session[UserRoleSessionKey] == null)
                    Session[UserRoleSessionKey] = "";
                return (string)Session[UserRoleSessionKey];
            }
            set { HttpContext.Current.Session[UserRoleSessionKey] = value; }
        }

        const string UserIdKey = "UserIdKey";
        public static int? UserIdFilter
        {
            get
            {
                if (Session[UserIdKey] == null)
                    Session[UserIdKey] = "";
                return (int?)Session[UserIdKey];
            }
            set { HttpContext.Current.Session[UserIdKey] = value; }
        }

        const string UserDepartmentIdKey = "UserDepartmentIdKey";
        public static int? UserDepartmentIdFilter
        {
            get
            {
                if (Session[UserDepartmentIdKey] == null)
                    Session[UserDepartmentIdKey] = "";
                return (int?)Session[UserDepartmentIdKey];
            }
            set { HttpContext.Current.Session[UserDepartmentIdKey] = value; }
        }

        //20.10.2025 thêm biến Năm tài chính
        public static int FinancialYearFilter { get; set; } = DateTime.Now.Year;
        //23.10.2025 thêm hàm GetCurrentUserName để lưu user theo Session       
        public static string GetCurrentUserName()
        {
            try
            {
                var context = HttpContext.Current;
                if (context?.User?.Identity?.IsAuthenticated == true)
                    return context.User.Identity.Name;
            }
            catch { }
            return null;
        }

        const string ContractInPaymentGridFilterIndexSessionKey = "ContractInPaymentGridFilterIndexSessionKey";
        public static int ContractInPaymentGridFilterIndex
        {
            get
            {
                if (Session[ContractInPaymentGridFilterIndexSessionKey] == null)
                    Session[ContractInPaymentGridFilterIndexSessionKey] = 0;
                return (int)Session[ContractInPaymentGridFilterIndexSessionKey];
            }
            set { HttpContext.Current.Session[ContractInPaymentGridFilterIndexSessionKey] = value; }
        }

        const string SelectedContractIDSessionKey = "SelectedContractIDSessionKey";
        public static int SelectedContractID
        {
            get
            {
                if (Session[SelectedContractIDSessionKey] == null)
                    Session[SelectedContractIDSessionKey] = -1;
                return (int)Session[SelectedContractIDSessionKey];
            }
            set
            {
                HttpContext.Current.Session[SelectedContractIDSessionKey] = value;
            }
        }

        // added by lapbt 10-mar-2025. Lưu taskid chọn ở cửa sổ bên phải, chi tiết hợp đồng, tại home
        const string SelectedTaskIDSessionKey = "SelectedTaskIDSessionKey";
        public static int SelectedTaskID
        {
            get
            {
                if (Session[SelectedTaskIDSessionKey] == null)
                    Session[SelectedTaskIDSessionKey] = -1;
                return (int)Session[SelectedTaskIDSessionKey];
            }
            set
            {
                HttpContext.Current.Session[SelectedTaskIDSessionKey] = value;
            }
        }

        const string SelectedPriceQuotationIDSessionKey = "SelectedPriceQuotationIDSessionKey";
        public static int SelectedPriceQuotationID
        {
            get
            {
                if (Session[SelectedPriceQuotationIDSessionKey] == null)
                    Session[SelectedPriceQuotationIDSessionKey] = -1;
                return (int)Session[SelectedPriceQuotationIDSessionKey];
            }
            set
            {
                HttpContext.Current.Session[SelectedPriceQuotationIDSessionKey] = value;
            }
        }
        const string SelectedCustomerIDSessionKey = "SelectedCustomerIDSessionKey";
        public static int SelectedCustomerID
        {
            get
            {
                if (Session[SelectedCustomerIDSessionKey] == null)
                    Session[SelectedCustomerIDSessionKey] = -1;
                return (int)Session[SelectedCustomerIDSessionKey];
            }
            set
            {
                HttpContext.Current.Session[SelectedCustomerIDSessionKey] = value;
            }
        }
        const string SelectedEquipmentIDSessionKey = "SelectedEquipmentIDSessionKey";
        public static int SelectedEquipmentID
        {
            get
            {
                if (Session[SelectedEquipmentIDSessionKey] == null)
                    Session[SelectedEquipmentIDSessionKey] = -1;
                return (int)Session[SelectedEquipmentIDSessionKey];
            }
            set
            {
                HttpContext.Current.Session[SelectedEquipmentIDSessionKey] = value;
            }
        }
        const string SelectedEquipmentLibIDSessionKey = "SelectedEquipmentLibIDSessionKey";
        public static int SelectedEquipmentLibID
        {
            get
            {
                if (Session[SelectedEquipmentLibIDSessionKey] == null)
                    Session[SelectedEquipmentLibIDSessionKey] = -1;
                return (int)Session[SelectedEquipmentLibIDSessionKey];
            }
            set
            {
                HttpContext.Current.Session[SelectedEquipmentLibIDSessionKey] = value;
            }
        }
        const string SelectedEmployeeIDSessionKey = "SelectedEmployeeIDSessionKey";
        public static int SelectedEmployeeID
        {
            get
            {
                if (Session[SelectedEmployeeIDSessionKey] == null)
                    Session[SelectedEmployeeIDSessionKey] = -1;
                return (int)Session[SelectedEmployeeIDSessionKey];
            }
            set
            {
                HttpContext.Current.Session[SelectedEmployeeIDSessionKey] = value;
            }
        }
        const string SelectedDepartmentIDSessionKey = "SelectedDepartmentIDSessionKey";
        public static int SelectedDepartmentID
        {
            get
            {
                if (Session[SelectedDepartmentIDSessionKey] == null)
                    Session[SelectedDepartmentIDSessionKey] = -1;
                return (int)Session[SelectedDepartmentIDSessionKey];
            }
            set
            {
                HttpContext.Current.Session[SelectedDepartmentIDSessionKey] = value;
            }
        }
        const string EquipmentGridFilterIndexSessionKey = "EquipmentGridFilterIndexSessionKey";
        public static int EquipmentGridFilterIndex
        {
            get
            {
                if (Session[EquipmentGridFilterIndexSessionKey] == null)
                    Session[EquipmentGridFilterIndexSessionKey] = 0;
                return (int)Session[EquipmentGridFilterIndexSessionKey];
            }
            set { HttpContext.Current.Session[EquipmentGridFilterIndexSessionKey] = value; }
        }
        const string FinacialReportFilterTypeSessionKey = "FinacialReportFilterTypeSessionKey";
        public static int FinacialReportFilterType
        {
            get
            {
                if (Session[FinacialReportFilterTypeSessionKey] == null)
                    Session[FinacialReportFilterTypeSessionKey] = 0;
                return (int)Session[FinacialReportFilterTypeSessionKey];
            }
            set { HttpContext.Current.Session[FinacialReportFilterTypeSessionKey] = value; }
        }

        const string FinacialReportFilterNameSessionKey = "FinacialReportFilterNameSessionKey";
        public static string FinacialReportFilterName
        {
            get
            {
                var name = "Báo cáo Tổng hợp";
                switch (FinacialReportFilterType)
                {
                    case 0:
                        name = "Báo cáo Tổng hợp";
                        break;
                    case 1:
                        name = "Báo cáo Hợp đồng đang thực hiện";
                        break;
                    case 2:
                        name = "Báo cáo Công nợ";
                        break;
                    case 3:
                        name = "Báo cáo TIền về";
                        break;
                    case 4:
                        name = "Báo cáo Doanh thu";
                        break;
                    case 5:
                        name = "Báo cáo Sản lượng";
                        break;
                    case 6:
                        name = "Báo cáo Hạn kiểm định";
                        break;
                }
                return name;
            }
        }

        const string FinacialReportFilterFromDateSessionKey = "FinacialReportFilterFromDateSessionKey";
        public static DateTime FinacialReportFilterFromDate
        {
            get
            {
                if (Session[FinacialReportFilterFromDateSessionKey] == null)
                    Session[FinacialReportFilterFromDateSessionKey] = DateTime.Today;
                return (DateTime)Session[FinacialReportFilterFromDateSessionKey];
            }
            set { HttpContext.Current.Session[FinacialReportFilterFromDateSessionKey] = value; }
        }

        const string FinacialReportFilterToDateSessionKey = "FinacialReportFilterToDateSessionKey";
        public static DateTime FinacialReportFilterToDate
        {
            get
            {
                if (Session[FinacialReportFilterToDateSessionKey] == null)
                    Session[FinacialReportFilterToDateSessionKey] = DateTime.Today;
                return (DateTime)Session[FinacialReportFilterToDateSessionKey];
            }
            set { HttpContext.Current.Session[FinacialReportFilterToDateSessionKey] = value; }
        }

        const string FinacialReportFilterDepartmentIDSessionKey = "FinacialReportFilterDepartmentIDSessionKey";
        public static int FinacialReportFilterDepartmentID
        {
            get
            {
                if (Session[FinacialReportFilterDepartmentIDSessionKey] == null)
                    Session[FinacialReportFilterDepartmentIDSessionKey] = -1;
                return (int)Session[FinacialReportFilterDepartmentIDSessionKey];
            }
            set { HttpContext.Current.Session[FinacialReportFilterDepartmentIDSessionKey] = value; }
        }

        const string FinacialReportFilterDepartmentNameSessionKey = "FinacialReportFilterDepartmentNameSessionKey";
        public static string FinacialReportFilterDepartmentName
        {
            get
            {
                if (Session[FinacialReportFilterDepartmentNameSessionKey] == null || (int)Session[FinacialReportFilterDepartmentIDSessionKey] == -1)
                    Session[FinacialReportFilterDepartmentNameSessionKey] = "N/A";
                return (string)Session[FinacialReportFilterDepartmentNameSessionKey];
            }
            set { HttpContext.Current.Session[FinacialReportFilterDepartmentNameSessionKey] = value; }
        }

        const string FinacialReportFilterEmployeeIDSessionKey = "FinacialReportFilterEmployeeIDSessionKey";
        public static int FinacialReportFilterEmployeeID
        {
            get
            {
                if (Session[FinacialReportFilterEmployeeIDSessionKey] == null)
                    Session[FinacialReportFilterEmployeeIDSessionKey] = -1;
                return (int)Session[FinacialReportFilterEmployeeIDSessionKey];
            }
            set { HttpContext.Current.Session[FinacialReportFilterEmployeeIDSessionKey] = value; }
        }

        const string FinacialReportFilterEmployeeNameSessionKey = "FinacialReportFilterEmployeeNameSessionKey";
        public static string FinacialReportFilterEmployeeName
        {
            get
            {
                if (Session[FinacialReportFilterEmployeeNameSessionKey] == null || (int)Session[FinacialReportFilterEmployeeIDSessionKey] == -1)
                    Session[FinacialReportFilterEmployeeNameSessionKey] = "N/A";
                return (string)Session[FinacialReportFilterEmployeeNameSessionKey];
            }
            set { HttpContext.Current.Session[FinacialReportFilterEmployeeNameSessionKey] = value; }
        }




        //////////
        const string IsAdvancedEquipmentFilterFromDateSessionKey = "IsAdvancedEquipmentFilterFromDateSessionKey";
        public static bool IsAdvancedEquipmentFilterFromDate
        {
            get
            {
                if (Session[IsAdvancedEquipmentFilterFromDateSessionKey] == null)
                    Session[IsAdvancedEquipmentFilterFromDateSessionKey] = true;   // 14-apr-2025. Mặc định dùng chế độ adv, để giới hạn DL 2 năm gần nhất
                return (bool)Session[IsAdvancedEquipmentFilterFromDateSessionKey];
            }
            set { HttpContext.Current.Session[IsAdvancedEquipmentFilterFromDateSessionKey] = value; }
        }

        const string AdvancedEquipmentFilterFromDateSessionKey = "AdvancedEquipmentFilterFromDateSessionKey";
        public static DateTime AdvancedEquipmentFilterFromDate
        {
            get
            {
                if (Session[AdvancedEquipmentFilterFromDateSessionKey] == null)
                    Session[AdvancedEquipmentFilterFromDateSessionKey] = DateTime.Today.AddYears(-3);   // 14-apr-2025. Mặc định lấy DL 2 năm gần nhất
                return (DateTime)Session[AdvancedEquipmentFilterFromDateSessionKey];
            }
            set { HttpContext.Current.Session[AdvancedEquipmentFilterFromDateSessionKey] = value; }
        }

        const string AdvancedEquipmentFilterToDateSessionKey = "AdvancedEquipmentFilterToDateSessionKey";
        public static DateTime AdvancedEquipmentFilterToDate
        {
            get
            {
                if (Session[AdvancedEquipmentFilterToDateSessionKey] == null)
                    Session[AdvancedEquipmentFilterToDateSessionKey] = DateTime.Today;
                return (DateTime)Session[AdvancedEquipmentFilterToDateSessionKey];
            }
            set { HttpContext.Current.Session[AdvancedEquipmentFilterToDateSessionKey] = value; }
        }

        const string AdvancedEquipmentFilterDepartmentIDSessionKey = "AdvancedEquipmentFilterDepartmentIDSessionKey";
        public static int AdvancedEquipmentFilterDepartmentID
        {
            get
            {
                if (Session[AdvancedEquipmentFilterDepartmentIDSessionKey] == null)
                    Session[AdvancedEquipmentFilterDepartmentIDSessionKey] = -1;
                return (int)Session[AdvancedEquipmentFilterDepartmentIDSessionKey];
            }
            set { HttpContext.Current.Session[AdvancedEquipmentFilterDepartmentIDSessionKey] = value; }
        }

        const string AdvancedEquipmentFilterDepartmentNameSessionKey = "AdvancedEquipmentFilterDepartmentNameSessionKey";
        public static string AdvancedEquipmentFilterDepartmentName
        {
            get
            {
                if (Session[AdvancedEquipmentFilterDepartmentNameSessionKey] == null || (int)Session[AdvancedEquipmentFilterDepartmentIDSessionKey] == -1)
                    Session[AdvancedEquipmentFilterDepartmentNameSessionKey] = "N/A";
                return (string)Session[AdvancedEquipmentFilterDepartmentNameSessionKey];
            }
            set { HttpContext.Current.Session[AdvancedEquipmentFilterDepartmentNameSessionKey] = value; }
        }

        const string AdvancedEquipmentFilterEmployeeIDSessionKey = "AdvancedEquipmentFilterEmployeeIDSessionKey";
        public static int AdvancedEquipmentFilterEmployeeID
        {
            get
            {
                if (Session[AdvancedEquipmentFilterEmployeeIDSessionKey] == null)
                    Session[AdvancedEquipmentFilterEmployeeIDSessionKey] = -1;
                return (int)Session[AdvancedEquipmentFilterEmployeeIDSessionKey];
            }
            set { HttpContext.Current.Session[AdvancedEquipmentFilterEmployeeIDSessionKey] = value; }
        }

        const string AdvancedEquipmentFilterEmployeeNameSessionKey = "AdvancedEquipmentFilterEmployeeNameSessionKey";
        public static string AdvancedEquipmentFilterEmployeeName
        {
            get
            {
                if (Session[AdvancedEquipmentFilterEmployeeNameSessionKey] == null || (int)Session[AdvancedEquipmentFilterEmployeeIDSessionKey] == -1)
                    Session[AdvancedEquipmentFilterEmployeeNameSessionKey] = "N/A";
                return (string)Session[AdvancedEquipmentFilterEmployeeNameSessionKey];
            }
            set { HttpContext.Current.Session[AdvancedEquipmentFilterEmployeeNameSessionKey] = value; }
        }

        // Thêm sau các filter khác
        const string ContractGridOwnerIdFilterSessionKey = "ContractGridOwnerIdFilterSessionKey";
        public static int? ContractGridOwnerIdFilter
        {
            get
            {
                if (Session[ContractGridOwnerIdFilterSessionKey] == null)
                    Session[ContractGridOwnerIdFilterSessionKey] = null; // mặc định không lọc
                return (int?)Session[ContractGridOwnerIdFilterSessionKey];
            }
            set
            {
                HttpContext.Current.Session[ContractGridOwnerIdFilterSessionKey] = value;
            }
        }

        //4.12.2025
        public static int ContractGridFilterMode { get; set; } = 0; // thêm dòng này
        //9.12.2025
        public static int ContractGridStatusFilter { get; set; } = 0;

        protected static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    }
}