using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Extensions;
using IncosafCMS.Data;
using IncosafCMS.Services;
using IncosafCMS.Web.Models;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using IncosafCMS.Web.Providers;
using System.Data;
using DevExpress.Spreadsheet;



namespace IncosafCMS.Web.Providers
{
    public static class LargeDatabaseDataProvider
    {
        const string LargeDatabaseDataContextKey = "DXLargeDatabaseDataContext";
        public static IncosafCMSContext DB
        {
            get
            {
                if (HttpContext.Current.Items[LargeDatabaseDataContextKey] == null)
                {
                    var newContext = new IncosafCMSContext("name=AppContext", new DebugLogger());
                    //newContext.Database.CommandTimeout = 0;
                    HttpContext.Current.Items[LargeDatabaseDataContextKey] = newContext;
                }
                return (IncosafCMSContext)HttpContext.Current.Items[LargeDatabaseDataContextKey];
            }
        }

        public static IQueryable<ContractViewModel> Contracts
        {
            get
            {
                var ctx = new IncosafCMSContext("name=AppContext", null);
                var contracts = ctx.Contracts;                

                var result = from contract in contracts
                             //join t in ctx.v_ActTurnOver
                             //    on contract.MaHD equals t.ma_hd into gj1
                             //join r in ctx.v_ActPayment
                             //    on contract.MaHD equals r.ma_hd into gj2
                             //join eq in ctx.Equipments
                             //    on contract.Id equals eq.contract.Id into gj3
                             //join d in ctx.Contract_Docs
                             //    on contract.Id equals d.Contract.Id into gj4
                             select new ContractViewModel
                             {
                                 Id = contract.Id,
                                 MaHD = contract.MaHD,
                                 Name = contract.Name,
                                 SignDate = contract.SignDate,
                                 CreateDate = contract.CreateDate,
                                 Value = contract.Value,
                                 Finished = contract.Finished,
                                 RatioOfCompany = contract.RatioOfCompany,
                                 ValueRoC = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfCompany / 100,
                                 RatioOfInternal = contract.RatioOfInternal,
                                 ValueRoI = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfInternal / 100,
                                 Status = contract.Status,
                                 OwnerId = contract.own.Id,
                                 UserName = contract.own.UserName,
                                 OwnerDisplayName = contract.own.DisplayName,
                                 KDV1DisplayName = contract.KDV1.DisplayName ?? contract.own.DisplayName,
                                 KDV2DisplayName = contract.KDV2.DisplayName,
                                 DepartmentId = contract.own.DepartmentId,
                                 MaDV = contract.own.Department.MaDV,
                                 CustomerName = contract.customer.Name,
                                 CustomerAddress = contract.customer.Address,
                                 //TotalValueIPoC = contract.InternalPayments
                                 //    .Where(x => x.IPType == InternalPaymentType.ThanhToanTrungTam)
                                 //    .Select(x => x.InternalPaymentValue)
                                 //    .DefaultIfEmpty(0).Sum(),
                                 //TotalValueIPoI = contract.InternalPayments
                                 //    .Where(x => x.IPType == InternalPaymentType.ThanhToanChuTri)
                                 //    .Select(x => x.InternalPaymentValue)
                                 //    .DefaultIfEmpty(0).Sum(),

                                 //TotalEquipment = gj3.Where(x => x.contract.Id == contract.Id).Count(),
                                 //// 👉 thêm 2 cột mới
                                 //XuatHoaDon = gj1.Where(x => x.ma_hd == contract.MaHD 
                                 //        && !string.IsNullOrEmpty(x.so_ct) && !string.IsNullOrEmpty(x.ma_hd))
                                 //                .Sum(x => (double?)x.DT) ?? 0,
                                 //ThuNo = gj2.Where(x => x.ma_hd == contract.MaHD 
                                 //        && !string.IsNullOrEmpty(x.so_ct) && !string.IsNullOrEmpty(x.ma_hd))
                                 //           .Sum(x => (double?)(x.TM + x.CK)) ?? 0,
                                 //DocCount = gj4.Where(x => x.Contract.Id == contract.Id).Count()
                             };

                // Giữ nguyên WhereIf như cũ
                result = result
                    .WhereIf(!string.IsNullOrEmpty(GridViewHelper.ContractGridDepartmentCodeFilter),
                        x => x.MaDV.Equals(GridViewHelper.ContractGridDepartmentCodeFilter))
                    .WhereIf(GridViewHelper.ContractGridOwnerIdFilter.HasValue,
                        x => x.OwnerId == GridViewHelper.ContractGridOwnerIdFilter.Value)                  
                   .WhereIf(GridViewHelper.ContractGridFilterIndex != 0,
                        x => x.Status == (GridViewHelper.ContractGridFilterIndex == 1
                            ? ApproveStatus.Waiting
                            : GridViewHelper.ContractGridFilterIndex == 2
                                ? ApproveStatus.ApprovedLv2
                                : ApproveStatus.Waiting))

                    .WhereIf(GridViewHelper.UserRoleFilter.Equals("Director"),
                        x => x.DepartmentId == GridViewHelper.UserDepartmentIdFilter)
                    .WhereIf(GridViewHelper.UserRoleFilter.Equals("User"),
                        x => x.OwnerId == GridViewHelper.UserIdFilter)  
                    ;

                return result;
            }
        }

        // 👉 Hàm mới để lấy tổng cho toàn bộ danh sách trong năm
        public static (double Total_Value, double Total_XuatHoaDon, double Total_ThuNo,
               double Total_CongNo, double Total_DoDang, int Total_Equipment) GetTotalsByYear(int year)
        {
            using (var ctx = new IncosafCMSContext("name=AppContext", null))
            {
                // DÙNG CỘT NAM / CreateYear → NHANH + DỄ BẢO TRÌ
                var contracts = ctx.Contracts
                    .Where(c => c.CreateDate != null && c.MaHD != null && c.CreateYear == year); // DÙNG CreateYear

                // Áp dụng bộ lọc như cũ
                contracts = contracts
                    .WhereIf(!string.IsNullOrEmpty(GridViewHelper.ContractGridDepartmentCodeFilter),
                        c => c.own.Department.MaDV.Equals(GridViewHelper.ContractGridDepartmentCodeFilter))
                    .WhereIf(GridViewHelper.ContractGridOwnerIdFilter.HasValue,
                        c => c.own.Id == GridViewHelper.ContractGridOwnerIdFilter.Value)
                    .WhereIf(GridViewHelper.ContractGridFilterIndex != 0,
                        c => c.Status == (GridViewHelper.ContractGridFilterIndex == 1
                            ? ApproveStatus.Waiting
                            : GridViewHelper.ContractGridFilterIndex == 2
                                ? ApproveStatus.ApprovedLv2
                                : ApproveStatus.Waiting))
                    .WhereIf(GridViewHelper.UserRoleFilter.Equals("Director"),
                        c => c.own.DepartmentId == GridViewHelper.UserDepartmentIdFilter)
                    .WhereIf(GridViewHelper.UserRoleFilter.Equals("User"),
                        c => c.own.Id == GridViewHelper.UserIdFilter);

                // --- 1. Tổng giá trị hợp đồng trong năm (dùng CreateYear) ---
                var total_Value = contracts.Sum(c => (double?)c.Value) ?? 0.0;

                //// --- 2. Doanh thu (xuất hóa đơn) trong năm (dùng nam) ---
                //var turnoverByContract = ctx.v_ActTurnOver
                //    .Where(v => v.nam == year && !string.IsNullOrEmpty(v.ma_hd)) // DÙNG nam
                //    .GroupBy(v => v.ma_hd)
                //    .Select(g => new
                //    {
                //        MaHD = g.Key,
                //        TotalDT = (double?)g.Sum(x => x.DT)
                //    })
                //    .ToList();

                //// --- 3. Thu nợ theo MaHD trong năm (dùng nam) ---
                //var paymentByContract = ctx.v_ActPayment
                //    .Where(p => p.nam == year && !string.IsNullOrEmpty(p.ma_hd)) // DÙNG nam
                //    .GroupBy(p => p.ma_hd)
                //    .Select(g => new
                //    {
                //        MaHD = g.Key,
                //        TotalThu = (double?)g.Sum(x => x.TM + x.CK)
                //    })
                //    .ToList();

                //// --- 4. Tổng Xuất hóa đơn & Tổng Thu nợ ---
                //var total_XuatHoaDon = turnoverByContract.Sum(x => x.TotalDT) ?? 0.0;
                //var total_ThuNo = paymentByContract.Sum(x => x.TotalThu) ?? 0.0;

                //// Tạo dictionary
                //var turnoverDict = turnoverByContract.ToDictionary(x => x.MaHD, x => x.TotalDT ?? 0.0);
                //var paymentDict = paymentByContract.ToDictionary(x => x.MaHD, x => x.TotalThu ?? 0.0);

                // Danh sách hợp đồng trong năm
                var contractList = contracts
                    .Select(c => new { c.Id, c.MaHD, c.Value })
                    .ToList();

                //// --- 5. Tổng Công nợ ---
                //var total_CongNo = contractList
                //    .Select(c =>
                //    {
                //        turnoverDict.TryGetValue(c.MaHD, out var daXuat);
                //        paymentDict.TryGetValue(c.MaHD, out var daThu);
                //        return daXuat > 0 && daThu < daXuat ? (daXuat - daThu) : 0.0;
                //    })
                //    .Sum();

                //// --- 6. Tổng Hợp đồng dở dang ---
                //var total_DoDang = contractList
                //    .Select(c =>
                //    {
                //        turnoverDict.TryGetValue(c.MaHD, out var daXuat);
                //        return daXuat < c.Value ? (c.Value - daXuat) : 0.0;
                //    })
                //    .Sum();

                //// --- 7. Tổng thiết bị đã kiểm định trong năm ---
                //var total_Equipment = ctx.Accreditations
                //    .Where(ac => ac.AccrDate != null && ac.AccrDate.Value.Year == year) // Nếu chưa có cột năm ở đây
                //    .Join(ctx.Equipments, ac => ac.equiment.Id, eq => eq.Id, (ac, eq) => eq.contract.Id)
                //    .Join(contracts, eqContractId => eqContractId, c => c.Id, (eqContractId, c) => eqContractId)
                //    .Distinct()
                //    .Count();

                // Nếu bảng Accreditations có cột ngày → thêm cột AccrYear tương tự (khuyến nghị)

                //return (total_Value, total_XuatHoaDon, total_ThuNo, total_CongNo, total_DoDang, total_Equipment);
                return (0,0,0,0,0,0);
            }
        }
       
        public static ContractViewModel GetContractById(int id)
        {
            var ctx = new IncosafCMSContext("name=AppContext", null);
            var contracts = ctx.Contracts;
            var result = from contract in contracts
                         //join t in ctx.v_ActTurnOver
                         //    on contract.MaHD equals t.ma_hd into gj1
                         //join r in ctx.v_ActPayment
                         //    on contract.MaHD equals r.ma_hd into gj2
                         //join d in ctx.Contract_Docs
                             //on contract.Id equals d.Contract.Id into gj4
                         select new ContractViewModel
                         {
                             Id = contract.Id,
                             MaHD = contract.MaHD,
                             Name = contract.Name,
                             SignDate = contract.SignDate,
                             CreateDate = contract.CreateDate,
                             Value = contract.Value,
                             Finished = contract.Finished,
                             RatioOfCompany = contract.RatioOfCompany,
                             ValueRoC = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfCompany / 100,
                             RatioOfInternal = contract.RatioOfInternal,
                             ValueRoI = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfInternal / 100,
                             Status = contract.Status,
                             OwnerId = contract.own.Id,
                             UserName = contract.own.UserName,
                             OwnerDisplayName = contract.own.DisplayName,
                             KDV1DisplayName = contract.KDV1.DisplayName ?? contract.own.DisplayName,
                             KDV2DisplayName = contract.KDV2.DisplayName,
                             DepartmentId = contract.own.DepartmentId,
                             MaDV = contract.own.Department.MaDV,
                             CustomerName = contract.customer.Name,
                             CustomerAddress = contract.customer.Address,
                             CustomerTaxID = contract.customer.TaxID,
                             IsGiayDeNghi = contract.IsGiayDeNghi,         // 23-jan-2024 by Lapbt. using in index.js để hiển thị thêm ở home bên chi tiết
                             RatioOfGroup = contract.RatioOfGroup,
                             contractType = contract.contractType,         // 13-may-2024. using in index.js hiển thị loại hình hợp đồng
                             //TotalValueIPoC = contract.InternalPayments.Where(x => x.IPType == InternalPaymentType.ThanhToanTrungTam).Select(x => x.InternalPaymentValue).DefaultIfEmpty(0).Sum(),
                             //TotalValueIPoI = contract.InternalPayments.Where(x => x.IPType == InternalPaymentType.ThanhToanChuTri).Select(x => x.InternalPaymentValue).DefaultIfEmpty(0).Sum(),

                             //XuatHoaDon = gj1.Where(x => x.ma_hd == contract.MaHD
                             //            && !string.IsNullOrEmpty(x.so_ct) && !string.IsNullOrEmpty(x.ma_hd))
                             //                    .Sum(x => (double?)x.DT) ?? 0,                             
                             //ThuNo = gj2.Where(x => x.ma_hd == contract.MaHD
                             //        && !string.IsNullOrEmpty(x.so_ct) && !string.IsNullOrEmpty(x.ma_hd))
                             //               .Sum(x => (double?)(x.TM + x.CK)) ?? 0,
                             //DocCount = gj4.Where(x => x.Contract.Id == contract.Id).Count()
                             // danh sách hóa đơn chi tiết
                             /*
                             vActTurnOvers = gj1
                                .Where(x => x.ma_hd == contract.MaHD && !string.IsNullOrEmpty(x.so_ct))
                                .AsEnumerable()
                                .Select(x => new v_ActTurnOver
                                {
                                    //ma_hd = x.ma_hd,
                                    ngay_ct = x.ngay_ct,
                                    so_ct = x.so_ct,
                                    DT = x.DT,
                                    VAT = x.VAT
                                }).ToList()
                             */

                         };
            // Giữ nguyên WhereIf như cũ
            result = result
            .Where(x => x.Id == id);
            return result.FirstOrDefault();
        }

        public static IQueryable<Customer> Customers
        {
            get
            {
                var uow = new UnitOfWork(DB);
                var customerService = new Service<Customer>(uow);
                return customerService.GetAll(x => x.Id > 0);
            }
        }

        public static AppUser GetAppUserById(int ownId)
        {
            var uow = new UnitOfWork(DB);
            var service = new Service<AppUser>(uow);
            return service.GetById(ownId);
        }

        //6.11.2025 thêm hàm này để lấy DL ở 2 db chi nhánh
        private static string GetConnectionStringByDepartment(int departmentId)
        {            
            if (departmentId == 8) // CN HCM
                return ConfigurationManager.ConnectionStrings["AppContext_SG"].ConnectionString;
            else if (departmentId == 10) // CN Đà Nẵng
                return ConfigurationManager.ConnectionStrings["AppContext_DN"].ConnectionString;
            else
                return ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString;
        }


        //4.11.2025 thêm class FinanceSummary và hàm GetCompanyFinanceSummary dưới đây        //
        //để lấy dữ liệu báo cáo tổng hợp Cty theo từng đơn vị
        public class FinanceSummary
        {
            public int DepartmentId { get; set; }
            public string DepartmentName { get; set; }
            public int EmployeeId { get; set; }
            public string MaDV { get; set; }            
            public string EmployeeName { get; set; }
            public double SLKeHoach { get; set; }
            public double SLThucHien { get; set; }
            public double TyLeSL => SLKeHoach == 0 ? 0 : (SLThucHien / SLKeHoach);
            public double DTKeHoach => SLKeHoach * 0.88;
            public double DTThucHien { get; set; }
            public double TyLeDT => DTKeHoach == 0 ? 0 : (DTThucHien / DTKeHoach);
            public double TNKeHoach => SLKeHoach * 0.93;
            public double TNThucHien { get; set; }
            public double TyLeTN => TNKeHoach == 0 ? 0 : (TNThucHien / TNKeHoach);
            public double CongNo { get; set; }
            public double CongNo_old { get; set; }
            public double DoDang { get; set; }
            public double DoDang_old { get; set; }            
            public double SL_TBN { get; set; }
            public double TyLeTBN => TongDonVi == 0 ? 0 : (SL_TBN / TongDonVi);
            public double SL_TBAL { get; set; }
            public double TyLeTBAL => TongDonVi == 0 ? 0 : (SL_TBAL / TongDonVi);
            public double SL_HQ { get; set; }
            public double TyLeHQ => TongDonVi == 0 ? 0 : (SL_HQ / TongDonVi);
            public double SL_HL { get; set; }
            public double TyLeHL => TongDonVi == 0 ? 0 : (SL_HL / TongDonVi);
            public double SL_AK { get; set; }
            public double TyLeAK => TongDonVi == 0 ? 0 : (SL_AK / TongDonVi);
            public double SL_TNHT { get; set; }
            public double TyLeTNHT => TongDonVi == 0 ? 0 : (SL_TNHT / TongDonVi);
            public double SL_TNPTN { get; set; }
            public double TyLeTNPTN => TongDonVi == 0 ? 0 : (SL_TNPTN / TongDonVi);
            public double SL_TVXD { get; set; }
            public double TyLeTVXD => TongDonVi == 0 ? 0 : (SL_TVXD / TongDonVi);
            public double SL_TBTC { get; set; }
            public double TyLeTBTC => TongDonVi == 0 ? 0 : (SL_TBTC / TongDonVi);
            public double SL_KHAC { get; set; }
            public double TongDonVi { get; set; }            

        }

        public static List<FinanceSummary> GetCompanyFinanceSummary(int year)
        {
            var userId = GridViewHelper.UserIdFilter;
            var connections = new[]
            {
                "name=AppContext", //bản live đặt: "name=AppContext", bản test đặt "name=AppContext_HN"
                "name=AppContext_SG",
                "name=AppContext_DN"
            };
            var result = new List<FinanceSummary>();    

            foreach (var connName in connections)
            {
                try
                {
                    /*
                    if (connName == "name=AppContext_DN")
                    {
                        using (var ctx_check = new IncosafCMSContext("name=AppContext_DN", null))
                        {
                            // LẤY THỜI GIAN RESTORE LOG MỚI NHẤT – CHÍNH XÁC 100%
                            var lastRestore = ctx_check.Database
                                .SqlQuery<DateTime?>(@" 
                                SELECT MAX(restore_date) AS LastLogRestoreDate
                                FROM msdb.dbo.restorehistory
                                WHERE destination_database_name = 'IncoSafCMS_DN' AND restore_type = 'L'; ")
                                .FirstOrDefault();

                            if (lastRestore.HasValue && lastRestore.Value.Date == DateTime.Today)
                            {
                                //connName = "name=AppContext_DN_real";
                                // ĐÚNG DỮ LIỆU TRONG NGÀY → DÙNG LUÔN SERVER NÀY

                                //result = FetchDataFromContext(ctx, year);
                                //usedConnection = sv.Replica;
                                System.Diagnostics.Debug.WriteLine($"SRV {connName} - Cập nhật log lúc: {lastRestore:dd/MM/yyyy HH:mm}");
                                //break; // thoát vòng lặp luôn
                            }

                           
                        }
                    }
                    */

                    using (var ctx = new IncosafCMSContext(connName, null))
                    {                       
                        // 1. DỮ LIỆU CHÍNH – ĐÃ CHẠY ĐÚNG
                        var mainData = ctx.Database
                            .SqlQuery<FinanceSummary>("EXEC GetCompanyFinanceSummary_Year @Year", new SqlParameter("@Year", year))
                            .OrderBy(x => x.DepartmentId)
                            .ToList();

                        //// 2. GỘP DỮ LIỆU
                        //foreach (var row in mainData)
                        //{
                        //    var dept = ctx.Departments.FirstOrDefault(d => d.Id == row.DepartmentId);
                        //    if (dept == null) continue;
                        //    row.DepartmentName = dept.Name;
                        //    row.MaDV = dept.MaDV;
                        //    if (row.SLKeHoach > 0 || row.SLThucHien > 0 ||
                        //        row.DTThucHien > 0 || row.TNThucHien > 0 ||
                        //        row.CongNo > 0 || row.DoDang > 0 || row.CongNo_old > 0)
                        //    {
                        //        result.Add(row);
                        //    }
                                    
                        //}
                        
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Bỏ qua DB {connName}: {ex.Message}");
                }
            }

            // GỘP THEO TÊN PHÒNG BAN
            return result
                .GroupBy(x => x.DepartmentName)
                .Select(g => new FinanceSummary
                {
                    DepartmentName = g.Key,
                    MaDV = g.First().MaDV,
                    SLKeHoach = g.Sum(x => x.SLKeHoach),
                    SLThucHien = g.Sum(x => x.SLThucHien),
                    DTThucHien = g.Sum(x => x.DTThucHien),
                    TNThucHien = g.Sum(x => x.TNThucHien),
                    CongNo = g.Sum(x => x.CongNo),
                    CongNo_old = g.Sum(x => x.CongNo_old),
                    DoDang = g.Sum(x => x.DoDang),
                    DoDang_old = g.Sum(x => x.DoDang_old),                    
                    SL_TBN = g.Sum(x => x.SL_TBN),                    
                    SL_TBAL = g.Sum(x => x.SL_TBAL),
                    SL_HQ = g.Sum(x => x.SL_HQ),
                    SL_HL = g.Sum(x => x.SL_HL),
                    SL_AK = g.Sum(x => x.SL_AK),
                    SL_TNHT = g.Sum(x => x.SL_TNHT),
                    SL_TNPTN = g.Sum(x => x.SL_TNPTN),
                    SL_TVXD = g.Sum(x => x.SL_TVXD),
                    SL_TBTC = g.Sum(x => x.SL_TBTC),
                    SL_KHAC = g.Sum(x => x.SL_KHAC),
                    TongDonVi = g.Sum(x => x.SL_TBN) + g.Sum(x => x.SL_TBAL) + g.Sum(x => x.SL_HQ)
                    + g.Sum(x => x.SL_HL) + g.Sum(x => x.SL_AK) + g.Sum(x => x.SL_TNHT)
                    + g.Sum(x => x.SL_TNPTN) + g.Sum(x => x.SL_TVXD) + g.Sum(x => x.SL_TBTC) + g.Sum(x => x.SL_KHAC),
                    
                })
                .OrderBy(x => x.DepartmentId)
                .ToList();
        }

        //5.11.2025 thêm hàm GetDepartmentDetailFinance dưới đây
        //để lấy dữ liệu báo cáo tổng hợp đơn vị theo từng cá nhân
        public static List<FinanceSummary> GetDepartmentDetailFinance(int year, int departmentId)
        {
            var result = new List<FinanceSummary>();

            // Xác định chuỗi kết nối theo đơn vị
            string name_Connect = "name=AppContext";//bản live sẽ đặt: "name=AppContext" bản test đặt "name=AppContext_HN";
            if (departmentId == 8)
                name_Connect = "name=AppContext_SG";
            if (departmentId == 10)
                name_Connect = "name=AppContext_DN";

            try
            {
                // Thử kết nối DB tương ứng
                result = GetDepartmentFinanceData(year, departmentId, name_Connect);
            }
            catch (Exception ex1)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi khi kết nối DB " + name_Connect + ": " + ex1.Message);

                try
                {
                    // ⚙️ Fallback sang DB mặc định "AppContext"
                    name_Connect = "name=AppContext";
                    result = GetDepartmentFinanceData(year, departmentId, name_Connect);
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine("Fallback AppContext cũng lỗi: " + ex2.Message);
                    // Trả về rỗng nếu cả 2 DB đều lỗi
                    result = new List<FinanceSummary>();
                }
            }

            return result;
        }

        private static List<FinanceSummary> GetDepartmentFinanceData(int year, int departmentId, string name_Connect)
        {
            var result = new List<FinanceSummary>();

            try
            {
                using (var ctx = new IncosafCMSContext(name_Connect, null))
                {
                    // 1. DỮ LIỆU CHÍNH
                    var data = ctx.Database
                        .SqlQuery<FinanceSummary>(
                            "EXEC GetEmployeeFinanceSummary_Year @Year, @DepartmentId",
                            new SqlParameter("@Year", year),
                            new SqlParameter("@DepartmentId", departmentId)
                        )
                        .ToList();                   

                    // 2. GỘP DỮ LIỆU
                    var employeesInOrder = ctx.AppUser
                        .Where(e => e.DepartmentId == departmentId)
                        .OrderBy(e => e.Id)
                        .Select(e => new { e.Id, e.DisplayName })
                        .ToList();

                    var empNameDict = employeesInOrder.ToDictionary(e => e.Id, e => e.DisplayName ?? "Không tên");
                    //var dept = ctx.Departments.FirstOrDefault(d => d.Id == departmentId);

                    //foreach (var row in data)
                    //{
                    //    row.EmployeeName = empNameDict.TryGetValue(row.EmployeeId, out var name) ? name : "Không xác định";
                    //    row.DepartmentName = dept?.Name ?? "";
                    //    row.MaDV = dept?.MaDV ?? "";
                    //    row.TongDonVi = row.SL_TBN + row.SL_TBAL + row.SL_HQ + row.SL_AK + row.SL_HL
                    //        + row.SL_TBTC + row.SL_TNHT + row.SL_TNPTN + row.SL_TVXD + row.SL_KHAC;
                    //    result.Add(row);
                    //}

                    // GIỮ NGUYÊN THỨ TỰ NHÂN VIÊN
                    var orderedResult = employeesInOrder
                        .Select(e => result.FirstOrDefault(r => r.EmployeeId == e.Id) ?? new FinanceSummary
                        {
                            EmployeeId = e.Id,
                            EmployeeName = e.DisplayName ?? "Không tên",
                            //DepartmentName = dept?.Name ?? "",
                            //MaDV = dept?.MaDV ?? ""
                        })
                        .Where(x => x.SLKeHoach > 0 || x.SLThucHien > 0 || x.DTThucHien > 0 || x.TNThucHien > 0 || x.CongNo > 0 || x.DoDang > 0 || x.CongNo_old > 0 || x.DoDang_old > 0)
                        .ToList();

                    return orderedResult;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi kết nối DB {name_Connect}: {ex.Message}");
                if (name_Connect != "name=AppContext")
                {
                    try { return GetDepartmentFinanceData(year, departmentId, "name=AppContext"); }
                    catch { /* ignore */ }
                }
                return result;
            }
        }



        /* Code cũ lấy theo cách UnitOfWork(DB)
        public static IQueryable<ContractViewModel> Contracts
        {
            get
            {
                var uow = new UnitOfWork(DB);
                var contractService = new Service<Contract>(uow);
                var contracts = contractService.GetAll(x => x.Id > 0);
                var result = (from contract in contracts
                              select new ContractViewModel
                              {
                                  Id = contract.Id,
                                  MaHD = contract.MaHD,
                                  Name = contract.Name,
                                  SignDate = contract.SignDate,
                                  CreateDate = contract.CreateDate,
                                  Value = contract.Value,
                                  Finished = contract.Finished,
                                  RatioOfCompany = contract.RatioOfCompany,
                                  ValueRoC = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfCompany / 100,
                                  RatioOfInternal = contract.RatioOfInternal,
                                  ValueRoI = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfInternal / 100,
                                  Status = contract.Status,
                                  OwnerId = contract.own.Id,
                                  UserName = contract.own.UserName,
                                  OwnerDisplayName = contract.own.DisplayName,
                                  KDV1DisplayName = contract.KDV1.DisplayName ?? contract.own.DisplayName,
                                  KDV2DisplayName = contract.KDV2.DisplayName,
                                  DepartmentId = contract.own.DepartmentId,
                                  MaDV = contract.own.Department.MaDV,
                                  CustomerName = contract.customer.Name,
                                  CustomerRepresentative = contract.customer.Representative,
                                  CustomerAddress = contract.customer.Address,
                                  CustomerPhone = contract.customer.Phone,
                                  CustomerFax = contract.customer.Fax,
                                  CustomerAccountNumber = contract.customer.AccountNumber,
                                  TotalValueIPoC = contract.InternalPayments.Where(x => x.IPType == InternalPaymentType.ThanhToanTrungTam).Select(x => x.InternalPaymentValue).DefaultIfEmpty(0).Sum(),
                                  TotalValueIPoI = contract.InternalPayments.Where(x => x.IPType == InternalPaymentType.ThanhToanChuTri).Select(x => x.InternalPaymentValue).DefaultIfEmpty(0).Sum(),
                                  //TongTienXuatHoaDon = contract.TurnOvers.Where(x => string.IsNullOrEmpty(x.HDNumber)).Select(x => x.HDValue).DefaultIfEmpty(0).Sum(),
                                  //TongTienVe = contract.Payments.Select(x => x.PaymentValue).DefaultIfEmpty(0).Sum(),
                                  //TongTienXuatHoaDon = (double)contract.vActTurnOvers.Where(x => string.IsNullOrEmpty(x.so_ct) && x.ma_hd == contract.MaHD).Select(x => x.DT).DefaultIfEmpty(0).Sum(),
                                  //TongTienVe = (double)(contract.vActPayment.Where(x => string.IsNullOrEmpty(x.so_ct) && x.ma_hd == contract.MaHD).Select(x => x.TM).DefaultIfEmpty(0).Sum() +
                                  //                      contract.vActPayment.Where(x => string.IsNullOrEmpty(x.so_ct) && x.ma_hd == contract.MaHD).Select(x => x.TM).DefaultIfEmpty(0).Sum()),
                                  //XuatHoaDon = contract.XuatHoaDon,
                                  //ThuNo = contract.ThuNo,
                              }).WhereIf(!string.IsNullOrEmpty(GridViewHelper.ContractGridDepartmentCodeFilter), x => x.MaDV.Equals(GridViewHelper.ContractGridDepartmentCodeFilter))                              
                              .WhereIf(GridViewHelper.ContractGridFilterIndex != 0, x => x.Status == (GridViewHelper.ContractGridFilterIndex == 1 ? ApproveStatus.Waiting : GridViewHelper.ContractGridFilterIndex == 2 ? ApproveStatus.ApprovedLv2 : ApproveStatus.Waiting))
                              .WhereIf(GridViewHelper.UserRoleFilter.Equals("Director"), x => x.DepartmentId == GridViewHelper.UserDepartmentIdFilter)
                              .WhereIf(GridViewHelper.UserRoleFilter.Equals("User"), x => x.OwnerId == GridViewHelper.UserIdFilter);

                return result;
                //return contractService.GetAll(x => x.Id > 0);
            }
        }

        public static ContractViewModel GetContractById(int id)
        {
            var uow = new UnitOfWork(DB);
            var contractService = new Service<Contract>(uow);
            var contracts = contractService.GetAll(x => x.Id > 0);
            var result = (from contract in contracts
                          select new ContractViewModel
                          {
                              Id = contract.Id,
                              MaHD = contract.MaHD,
                              Name = contract.Name,
                              SignDate = contract.SignDate,
                              CreateDate = contract.CreateDate,
                              Value = contract.Value,
                              Finished = contract.Finished,
                              RatioOfCompany = contract.RatioOfCompany,
                              ValueRoC = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfCompany / 100,
                              RatioOfInternal = contract.RatioOfInternal,
                              ValueRoI = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfInternal / 100,
                              Status = contract.Status,
                              OwnerId = contract.own.Id,
                              UserName = contract.own.UserName,
                              OwnerDisplayName = contract.own.DisplayName,
                              KDV1DisplayName = contract.KDV1.DisplayName ?? contract.own.DisplayName,
                              KDV2DisplayName = contract.KDV2.DisplayName,
                              DepartmentId = contract.own.DepartmentId,
                              MaDV = contract.own.Department.MaDV,
                              CustomerName = contract.customer.Name,
                              //CustomerRepresentative = contract.customer.Representative,
                              CustomerAddress = contract.customer.Address,
                              //CustomerPhone = contract.customer.Phone,
                              //CustomerFax = contract.customer.Fax,
                              CustomerTaxID = contract.customer.TaxID,      // 25-aug-2023. added by lapbt. index.js gọi ajax để lấy ttin hiển thị ở index.cshtml trang chính, bên phải
                              //CustomerAccountNumber = contract.customer.AccountNumber,
                              IsGiayDeNghi = contract.IsGiayDeNghi,         // 23-jan-2024 by Lapbt. using in index.js để hiển thị thêm ở home bên chi tiết
                              RatioOfGroup = contract.RatioOfGroup,
                              contractType = contract.contractType,         // 13-may-2024. using in index.js hiển thị loại hình hợp đồng
                              TotalValueIPoC = contract.InternalPayments.Where(x => x.IPType == InternalPaymentType.ThanhToanTrungTam).Select(x => x.InternalPaymentValue).DefaultIfEmpty(0).Sum(),
                              TotalValueIPoI = contract.InternalPayments.Where(x => x.IPType == InternalPaymentType.ThanhToanChuTri).Select(x => x.InternalPaymentValue).DefaultIfEmpty(0).Sum(),
                              //TongTienXuatHoaDon = contract.TurnOvers.Where(x => string.IsNullOrEmpty(x.HDNumber)).Select(x => x.HDValue).DefaultIfEmpty(0).Sum(),
                              //TongTienVe = contract.Payments.Select(x => x.PaymentValue).DefaultIfEmpty(0).Sum(),
                              //TongTienXuatHoaDon = (double)contract.vActTurnOvers.Where(x => string.IsNullOrEmpty(x.so_ct) && x.ma_hd == contract.MaHD).Select(x => x.DT).DefaultIfEmpty(0).Sum(),
                              //TongTienVe = (double)(contract.vActPayment.Where(x => string.IsNullOrEmpty(x.so_ct) && x.ma_hd == contract.MaHD).Select(x => x.TM).DefaultIfEmpty(0).Sum() +
                              //                     contract.vActPayment.Where(x => string.IsNullOrEmpty(x.so_ct) && x.ma_hd == contract.MaHD).Select(x => x.TM).DefaultIfEmpty(0).Sum()),
                              //XuatHoaDon = contract.XuatHoaDon,
                              //ThuNo = contract.ThuNo,
                          }).Where(x => x.Id == id);

            return result.FirstOrDefault();
        }
        */


        /*
        public static List<FinanceSummary> GetCompanyFinanceSummary_old(int year)
        {
            DateTime fromDate = new DateTime(year, 1, 1);
            DateTime toDate = fromDate.AddYears(1);
            var result = new List<FinanceSummary>();

            var dbConnections = new List<(string ConnName, int DepartmentId)>
    {
        ("name=AppContext", 1),       // Hà Nội
        ("name=AppContext_SG", 8),    // HCM
        ("name=AppContext_DN", 10)    // Đà Nẵng
    };

            foreach (var brand in dbConnections)
            {
                try
                {
                    using (var ctx = new IncosafCMSContext(brand.ConnName, null))
                    {
                        // 1️⃣ Lấy danh sách đơn vị
                        var departments = ctx.Departments.ToList();

                        // 2️⃣ Sản lượng kế hoạch
                        var sanLuongDict = ctx.SanLuongDK
                            .Where(x => x.NamDK == year.ToString() && x.NhanVien.DepartmentId != null && x.SanLuong > 0)
                            .GroupBy(x => x.NhanVien.DepartmentId)
                            .ToDictionary(g => g.Key.Value, g => g.Sum(x => (double?)x.SanLuong) ?? 0);
                       
                        // 3️⃣ Lấy hợp đồng thực hiện trong năm                       
                        var contracts = ctx.Contracts
                            .Where(c => c.CreateYear == year
                                     && !string.IsNullOrEmpty(c.MaHD)
                                     && c.own != null && c.own.DepartmentId != null)
                            .Select(c => new { c.Value, c.own.DepartmentId, c.MaHD })
                            .ToList();

                        var contractByDeptSL = contracts
                            .GroupBy(c => c.DepartmentId)
                            .ToDictionary(g => g.Key.Value, g => new
                            {
                                TotalValue = g.Sum(x => (double?)x.Value) ?? 0,
                                MaHDs = g.Select(x => x.MaHD).ToList()
                            });

                        // 3️⃣ Lấy tất cả hợp đồng có phát sinh trong năm (DT hoặc TN)                       
                        var maHdTurnOver = ctx.v_ActTurnOver
                            .Where(v => v.nam == year && !string.IsNullOrEmpty(v.ma_hd))
                            .Select(v => v.ma_hd)
                            .Distinct();
                       
                        var maHdPayment = ctx.v_ActPayment
                            .Where(p => p.nam == year && !string.IsNullOrEmpty(p.ma_hd))
                            .Select(p => p.ma_hd)
                            .Distinct();

                        var allMaHDs = maHdTurnOver
                            .Union(maHdPayment)
                            .Distinct()
                            .ToList();

                        // 4️⃣ Lấy thông tin hợp đồng tương ứng các mã này để xác định phòng ban
                        var contractByDept = ctx.Contracts
                            .Where(c => allMaHDs.Contains(c.MaHD) && c.own != null && c.own.DepartmentId != null)
                            .Select(c => new { c.Value, c.own.DepartmentId, c.MaHD })
                            .ToList()
                            .GroupBy(c => c.DepartmentId)
                            .ToDictionary(
                                g => g.Key.Value,
                                g => new
                                {
                                    TotalValue = g.Sum(x => (double?)x.Value) ?? 0,
                                    MaHDs = g.Select(x => x.MaHD).ToList()
                                });

                        // 5️⃣ Doanh thu và Thu nợ theo mã hợp đồng                       
                        var turnoverDict = (from v in ctx.v_ActTurnOver
                                            where v.nam == year && allMaHDs.Contains(v.ma_hd)
                                            group v by v.ma_hd into g
                                            select new { MaHD = g.Key, TotalDT = (double?)g.Sum(x => x.DTnoV) ?? 0.0 })
                                            .ToDictionary(x => x.MaHD, x => x.TotalDT);
                       
                        var paymentDict = (from p in ctx.v_ActPayment
                                           where p.nam == year && allMaHDs.Contains(p.ma_hd)
                                           group p by p.ma_hd into g
                                           select new { MaHD = g.Key, TotalTN = (double?)g.Sum(x => x.TM + x.CK) ?? 0.0 })
                                           .ToDictionary(x => x.MaHD, x => x.TotalTN);                                               

                        // 6️⃣ Tổng hợp theo phòng ban
                        foreach (var dept in departments)
                        {
                            var id = dept.Id;
                            double slKH = sanLuongDict.ContainsKey(id) ? sanLuongDict[id] : 0.0;
                            double slTH = contractByDeptSL.ContainsKey(id) ? contractByDeptSL[id].TotalValue : 0.0;

                            double dtTH = 0.0;
                            double tnTH = 0.0;

                            if (contractByDept.ContainsKey(id))
                            {
                                foreach (var mahd in contractByDept[id].MaHDs)
                                {
                                    if (turnoverDict.TryGetValue(mahd, out double valDT)) dtTH += valDT;
                                    if (paymentDict.TryGetValue(mahd, out double valTN)) tnTH += valTN;
                                }
                            }

                            // --- Tích lũy: Công nợ & HĐ dở dang (từ SP) ---
                            double congNo = 0.0;
                            double doDang = 0.0;

                            if (slTH > 0 || dtTH > 0 || tnTH > 0)
                            {
                                result.Add(new FinanceSummary
                                {
                                    DepartmentName = dept.Name,
                                    MaDV = dept.MaDV,
                                    SLKeHoach = slKH,
                                    SLThucHien = slTH,
                                    DTThucHien = dtTH,
                                    TNThucHien = tnTH,
                                    CongNo = congNo,
                                    DoDang = doDang
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Bỏ qua DB {brand.ConnName}: {ex.Message}");
                }
            }

            // 7️⃣ Cộng gộp theo tên phòng ban
            var finalResult = result
                .GroupBy(r => r.DepartmentName)
                .Select(g => new FinanceSummary
                {
                    DepartmentName = g.Key,
                    SLKeHoach = g.Sum(x => x.SLKeHoach),
                    SLThucHien = g.Sum(x => x.SLThucHien),
                    DTThucHien = g.Sum(x => x.DTThucHien),
                    TNThucHien = g.Sum(x => x.TNThucHien),
                    CongNo = g.Sum(x => x.CongNo),
                    DoDang = g.Sum(x => x.DoDang)
                })
                .ToList();

            return finalResult;
        }
        */
        /*
        private static List<FinanceSummary> GetDepartmentFinanceData_old(int year, int departmentId, string name_Connect)
        {
            var result = new List<FinanceSummary>();

            try
            {
                using (var ctx = new IncosafCMSContext(name_Connect, null))
                {
                    DateTime fromDate = new DateTime(year, 1, 1);
                    DateTime toDate = fromDate.AddYears(1);

                    // 1️⃣ Danh sách nhân viên
                    var employees = ctx.AppUser
                        .Where(e => e.Department.Id == departmentId)
                        .ToList();

                    // 2️⃣ Sản lượng kế hoạch theo nhân viên
                    var sanLuongDict = ctx.SanLuongDK
                        .Where(x => x.NamDK == year.ToString() && x.NhanVien.DepartmentId == departmentId)
                        .GroupBy(x => x.NhanVien)
                        .ToDictionary(g => g.Key.Id, g => g.Sum(x => (double?)x.SanLuong) ?? 0);
                                        
                    // 3️⃣ Lấy các mã hợp đồng có phát sinh DT hoặc TN trong năm
                    var maHdTurnOver = ctx.v_ActTurnOver
                        .Where(v => v.ngay_ct >= fromDate && v.ngay_ct < toDate && !string.IsNullOrEmpty(v.ma_hd))
                        .Select(v => v.ma_hd)
                        .Distinct();

                    var maHdPayment = ctx.v_ActPayment
                        .Where(p => p.ngay_ct >= fromDate && p.ngay_ct < toDate && !string.IsNullOrEmpty(p.ma_hd))
                        .Select(p => p.ma_hd)
                        .Distinct();

                    var allMaHDs = maHdTurnOver
                        .Union(maHdPayment)
                        .Distinct()
                        .ToList();

                    // 4️⃣ Lấy toàn bộ hợp đồng thuộc phòng ban này
                    var contracts = ctx.Contracts
                        .Where(c => allMaHDs.Contains(c.MaHD)
                                 && c.own != null
                                 && c.own.DepartmentId == departmentId)
                        .Select(c => new { c.Value, c.own, c.MaHD })
                        .ToList();

                    var contractByEmp = contracts
                        .GroupBy(c => c.own.Id)
                        .ToDictionary(g => g.Key, g => new
                        {
                            TotalValue = g.Sum(x => (double?)x.Value) ?? 0,
                            MaHDs = g.Select(x => x.MaHD).ToList()
                        });

                    // 3️⃣ Hợp đồng thực hiện trong năm
                    var contractsYear = ctx.Contracts
                        .Where(c => c.CreateDate >= fromDate && c.CreateDate < toDate
                            && !string.IsNullOrEmpty(c.MaHD)
                            && c.own != null
                            && c.own.DepartmentId == departmentId)
                        .Select(c => new { c.Value, c.own, c.MaHD })
                        .ToList();

                    var contractByEmpSL = contractsYear
                        .GroupBy(c => c.own.Id)
                        .ToDictionary(g => g.Key, g => new
                        {
                            TotalValue = g.Sum(x => (double?)x.Value) ?? 0,
                            MaHDs = g.Select(x => x.MaHD).ToList()
                        });


                    // 5️⃣ Lấy DT & TN theo mã hợp đồng (theo năm)
                    var turnoverDict = (from v in ctx.v_ActTurnOver
                                        where v.ngay_ct >= fromDate && v.ngay_ct < toDate && allMaHDs.Contains(v.ma_hd)
                                        group v by v.ma_hd into g
                                        select new { MaHD = g.Key, TotalDT = (double?)g.Sum(x => x.DTnoV) ?? 0.0 })
                                        .ToDictionary(x => x.MaHD, x => x.TotalDT);

                    var paymentDict = (from p in ctx.v_ActPayment
                                       where p.ngay_ct >= fromDate && p.ngay_ct < toDate && allMaHDs.Contains(p.ma_hd)
                                       group p by p.ma_hd into g
                                       select new { MaHD = g.Key, TotalTN = (double?)g.Sum(x => x.TM + x.CK) ?? 0.0 })
                                       .ToDictionary(x => x.MaHD, x => x.TotalTN);

                    // 6️⃣ Tổng hợp theo nhân viên
                    foreach (var emp in employees)
                    {
                        var id = emp.Id;
                        double slKH = sanLuongDict.ContainsKey(id) ? sanLuongDict[id] : 0.0;
                        double slTH = contractByEmpSL.ContainsKey(id) ? contractByEmpSL[id].TotalValue : 0.0;

                        double dtTH = 0.0;
                        double tnTH = 0.0;

                        if (contractByEmp.ContainsKey(id))
                        {
                            foreach (var mahd in contractByEmp[id].MaHDs)
                            {
                                if (turnoverDict.TryGetValue(mahd, out double valDT)) dtTH += valDT;
                                if (paymentDict.TryGetValue(mahd, out double valTN)) tnTH += valTN;
                            }
                        }

                        if (slTH > 0 || dtTH > 0 || tnTH > 0)
                        {
                            result.Add(new FinanceSummary
                            {
                                EmployeeName = emp.DisplayName,
                                SLKeHoach = slKH,
                                SLThucHien = slTH,
                                DTThucHien = dtTH,
                                TNThucHien = tnTH
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ⚠️ Nếu kết nối thất bại (vd: chi nhánh không có DB riêng), fallback về DB mặc định
                System.Diagnostics.Debug.WriteLine($"⚠️ Lỗi kết nối DB {name_Connect}: {ex.Message}");
                if (name_Connect != "name=AppContext")
                {
                    try
                    {
                        return GetDepartmentFinanceData(year, departmentId, "name=AppContext");
                    }
                    catch (Exception subEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"⚠️ Fallback DB cũng lỗi: {subEx.Message}");
                    }
                }
            }

            return result;
        }
        */
        /*
        private static List<FinanceSummary> GetDepartmentFinanceData(int year, int departmentId, string name_Connect)
        {
            using (var ctx = new IncosafCMSContext(name_Connect, null))
            {
                DateTime fromDate = new DateTime(year, 1, 1);
                DateTime toDate = fromDate.AddYears(1);

                // 1️⃣ Danh sách nhân viên
                var employees = ctx.AppUser
                    .Where(e => e.Department.Id == departmentId)
                    .ToList();

                // 2️⃣ Sản lượng kế hoạch
                var sanLuongDict = ctx.SanLuongDK
                    .Where(x => x.NamDK == year.ToString() && x.NhanVien.DepartmentId == departmentId)
                    .GroupBy(x => x.NhanVien)
                    .ToDictionary(g => g.Key.Id, g => g.Sum(x => (double?)x.SanLuong) ?? 0);

                // 3️⃣ Hợp đồng thực hiện
                var contracts = ctx.Contracts
                    .Where(c => c.CreateDate >= fromDate && c.CreateDate < toDate
                        && !string.IsNullOrEmpty(c.MaHD)
                        && c.own != null
                        && c.own.DepartmentId == departmentId)
                    .Select(c => new { c.Value, c.own, c.MaHD })
                    .ToList();

                var contractByEmp = contracts
                    .GroupBy(c => c.own.Id)
                    .ToDictionary(g => g.Key, g => new
                    {
                        TotalValue = g.Sum(x => (double?)x.Value) ?? 0,
                        MaHDs = g.Select(x => x.MaHD).ToList()
                    });

                // 4️⃣ DT và TN
                var allMaHDs = contractByEmp.Values.SelectMany(v => v.MaHDs).Distinct().ToList();

                var turnoverDict = (from c in ctx.Contracts
                                    join v in ctx.v_ActTurnOver on c.MaHD equals v.ma_hd
                                    where !string.IsNullOrEmpty(v.ma_hd) && v.ngay_ct >= fromDate && v.ngay_ct < toDate
                                    group v by v.ma_hd into g
                                    select new { MaHD = g.Key, TotalDT = (double?)g.Sum(x => x.DTnoV) ?? 0 })
                                    .ToDictionary(x => x.MaHD, x => x.TotalDT);

                var paymentDict = (from c in ctx.Contracts
                                   join p in ctx.v_ActPayment on c.MaHD equals p.ma_hd
                                   where !string.IsNullOrEmpty(p.ma_hd) && p.ngay_ct >= fromDate && p.ngay_ct < toDate
                                   group p by p.ma_hd into g
                                   select new { MaHD = g.Key, TotalTN = (double?)g.Sum(x => x.TM + x.CK) ?? 0 })
                                   .ToDictionary(x => x.MaHD, x => x.TotalTN);

                // 5️⃣ Gom theo nhân viên
                var result = new List<FinanceSummary>();
                foreach (var emp in employees)
                {
                    var id = emp.Id;
                    double slKH = sanLuongDict.ContainsKey(id) ? sanLuongDict[id] : 0.0;
                    double slTH = contractByEmp.ContainsKey(id) ? contractByEmp[id].TotalValue : 0.0;

                    double dtTH = 0.0;
                    double tnTH = 0.0;
                    if (contractByEmp.ContainsKey(id))
                    {
                        foreach (var mahd in contractByEmp[id].MaHDs)
                        {
                            if (turnoverDict.TryGetValue(mahd, out double valDT)) dtTH += valDT;
                            if (paymentDict.TryGetValue(mahd, out double valTN)) tnTH += valTN;
                        }
                    }

                    if (slTH > 0 || dtTH > 0 || tnTH > 0)
                    {
                        result.Add(new FinanceSummary
                        {
                            EmployeeName = emp.DisplayName,
                            SLKeHoach = slKH,
                            SLThucHien = slTH,
                            DTThucHien = dtTH,
                            TNThucHien = tnTH
                        });
                    }
                }

                return result;
            }
        }
        */




    }
}