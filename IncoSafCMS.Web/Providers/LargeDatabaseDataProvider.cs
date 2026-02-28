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

        //public static IQueryable<ContractViewModel> Contracts
        //{
        //    get
        //    {
        //        var ctx = new IncosafCMSContext("name=AppContext", null);
        //        var contracts = ctx.Contracts;                

        //        var result = from contract in contracts
        //                     //join t in ctx.v_ActTurnOver
        //                     //    on contract.MaHD equals t.ma_hd into gj1
        //                     //join r in ctx.v_ActPayment
        //                     //    on contract.MaHD equals r.ma_hd into gj2
        //                     //join eq in ctx.Equipments
        //                     //    on contract.Id equals eq.contract.Id into gj3
        //                     //join d in ctx.Contract_Docs
        //                     //    on contract.Id equals d.Contract.Id into gj4
        //                     select new ContractViewModel
        //                     {
        //                         Id = contract.Id,
        //                         MaHD = contract.MaHD,
        //                         Name = contract.Name,
        //                         SignDate = contract.SignDate,
        //                         CreateDate = contract.CreateDate,
        //                         Value = contract.Value,
        //                         Finished = contract.Finished,
        //                         RatioOfCompany = contract.RatioOfCompany,
        //                         ValueRoC = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfCompany / 100,
        //                         RatioOfInternal = contract.RatioOfInternal,
        //                         ValueRoI = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfInternal / 100,
        //                         Status = contract.Status,
        //                         OwnerId = contract.own.Id,
        //                         UserName = contract.own.UserName,
        //                         OwnerDisplayName = contract.own.DisplayName,
        //                         KDV1DisplayName = contract.KDV1.DisplayName ?? contract.own.DisplayName,
        //                         KDV2DisplayName = contract.KDV2.DisplayName,
        //                         DepartmentId = contract.own.DepartmentId,
        //                         MaDV = contract.own.Department.MaDV,
        //                         CustomerName = contract.customer.Name,
        //                         CustomerAddress = contract.customer.Address,
        //                         //TotalValueIPoC = contract.InternalPayments
        //                         //    .Where(x => x.IPType == InternalPaymentType.ThanhToanTrungTam)
        //                         //    .Select(x => x.InternalPaymentValue)
        //                         //    .DefaultIfEmpty(0).Sum(),
        //                         //TotalValueIPoI = contract.InternalPayments
        //                         //    .Where(x => x.IPType == InternalPaymentType.ThanhToanChuTri)
        //                         //    .Select(x => x.InternalPaymentValue)
        //                         //    .DefaultIfEmpty(0).Sum(),

        //                         //TotalEquipment = gj3.Where(x => x.contract.Id == contract.Id).Count(),
        //                         //// 👉 thêm 2 cột mới
        //                         //XuatHoaDon = gj1.Where(x => x.ma_hd == contract.MaHD 
        //                         //        && !string.IsNullOrEmpty(x.so_ct) && !string.IsNullOrEmpty(x.ma_hd))
        //                         //                .Sum(x => (double?)x.DT) ?? 0,
        //                         //ThuNo = gj2.Where(x => x.ma_hd == contract.MaHD 
        //                         //        && !string.IsNullOrEmpty(x.so_ct) && !string.IsNullOrEmpty(x.ma_hd))
        //                         //           .Sum(x => (double?)(x.TM + x.CK)) ?? 0,
        //                         //DocCount = gj4.Where(x => x.Contract.Id == contract.Id).Count()
        //                     };

        //        // Giữ nguyên WhereIf như cũ
        //        result = result
        //            .WhereIf(!string.IsNullOrEmpty(GridViewHelper.ContractGridDepartmentCodeFilter),
        //                x => x.MaDV.Equals(GridViewHelper.ContractGridDepartmentCodeFilter))
        //            .WhereIf(GridViewHelper.ContractGridOwnerIdFilter.HasValue,
        //                x => x.OwnerId == GridViewHelper.ContractGridOwnerIdFilter.Value)                  
        //           .WhereIf(GridViewHelper.ContractGridFilterIndex != 0,
        //                x => x.Status == (GridViewHelper.ContractGridFilterIndex == 1
        //                    ? ApproveStatus.Waiting
        //                    : GridViewHelper.ContractGridFilterIndex == 2
        //                        ? ApproveStatus.ApprovedLv2
        //                        : ApproveStatus.Waiting))

        //            .WhereIf(GridViewHelper.UserRoleFilter.Equals("Director"),
        //                x => x.DepartmentId == GridViewHelper.UserDepartmentIdFilter)
        //            .WhereIf(GridViewHelper.UserRoleFilter.Equals("User"),
        //                x => x.OwnerId == GridViewHelper.UserIdFilter)  
        //            ;

        //        return result;
        //    }
        //}

        //// 👉 Hàm mới để lấy tổng cho toàn bộ danh sách trong năm
        //public static (double Total_Value, double Total_XuatHoaDon, double Total_ThuNo,
        //       double Total_CongNo, double Total_DoDang, int Total_Equipment) GetTotalsByYear(int year)
        //{
        //    using (var ctx = new IncosafCMSContext("name=AppContext", null))
        //    {
        //        // DÙNG CỘT NAM / CreateYear → NHANH + DỄ BẢO TRÌ
        //        var contracts = ctx.Contracts
        //            .Where(c => c.CreateDate != null && c.MaHD != null && c.CreateYear == year); // DÙNG CreateYear

        //        // Áp dụng bộ lọc như cũ
        //        contracts = contracts
        //            .WhereIf(!string.IsNullOrEmpty(GridViewHelper.ContractGridDepartmentCodeFilter),
        //                c => c.own.Department.MaDV.Equals(GridViewHelper.ContractGridDepartmentCodeFilter))
        //            .WhereIf(GridViewHelper.ContractGridOwnerIdFilter.HasValue,
        //                c => c.own.Id == GridViewHelper.ContractGridOwnerIdFilter.Value)
        //            .WhereIf(GridViewHelper.ContractGridFilterIndex != 0,
        //                c => c.Status == (GridViewHelper.ContractGridFilterIndex == 1
        //                    ? ApproveStatus.Waiting
        //                    : GridViewHelper.ContractGridFilterIndex == 2
        //                        ? ApproveStatus.ApprovedLv2
        //                        : ApproveStatus.Waiting))
        //            .WhereIf(GridViewHelper.UserRoleFilter.Equals("Director"),
        //                c => c.own.DepartmentId == GridViewHelper.UserDepartmentIdFilter)
        //            .WhereIf(GridViewHelper.UserRoleFilter.Equals("User"),
        //                c => c.own.Id == GridViewHelper.UserIdFilter);

        //        // --- 1. Tổng giá trị hợp đồng trong năm (dùng CreateYear) ---
        //        var total_Value = contracts.Sum(c => (double?)c.Value) ?? 0.0;

        //        //// --- 2. Doanh thu (xuất hóa đơn) trong năm (dùng nam) ---
        //        //var turnoverByContract = ctx.v_ActTurnOver
        //        //    .Where(v => v.nam == year && !string.IsNullOrEmpty(v.ma_hd)) // DÙNG nam
        //        //    .GroupBy(v => v.ma_hd)
        //        //    .Select(g => new
        //        //    {
        //        //        MaHD = g.Key,
        //        //        TotalDT = (double?)g.Sum(x => x.DT)
        //        //    })
        //        //    .ToList();

        //        //// --- 3. Thu nợ theo MaHD trong năm (dùng nam) ---
        //        //var paymentByContract = ctx.v_ActPayment
        //        //    .Where(p => p.nam == year && !string.IsNullOrEmpty(p.ma_hd)) // DÙNG nam
        //        //    .GroupBy(p => p.ma_hd)
        //        //    .Select(g => new
        //        //    {
        //        //        MaHD = g.Key,
        //        //        TotalThu = (double?)g.Sum(x => x.TM + x.CK)
        //        //    })
        //        //    .ToList();

        //        //// --- 4. Tổng Xuất hóa đơn & Tổng Thu nợ ---
        //        //var total_XuatHoaDon = turnoverByContract.Sum(x => x.TotalDT) ?? 0.0;
        //        //var total_ThuNo = paymentByContract.Sum(x => x.TotalThu) ?? 0.0;

        //        //// Tạo dictionary
        //        //var turnoverDict = turnoverByContract.ToDictionary(x => x.MaHD, x => x.TotalDT ?? 0.0);
        //        //var paymentDict = paymentByContract.ToDictionary(x => x.MaHD, x => x.TotalThu ?? 0.0);

        //        // Danh sách hợp đồng trong năm
        //        var contractList = contracts
        //            .Select(c => new { c.Id, c.MaHD, c.Value })
        //            .ToList();

        //        //// --- 5. Tổng Công nợ ---
        //        //var total_CongNo = contractList
        //        //    .Select(c =>
        //        //    {
        //        //        turnoverDict.TryGetValue(c.MaHD, out var daXuat);
        //        //        paymentDict.TryGetValue(c.MaHD, out var daThu);
        //        //        return daXuat > 0 && daThu < daXuat ? (daXuat - daThu) : 0.0;
        //        //    })
        //        //    .Sum();

        //        //// --- 6. Tổng Hợp đồng dở dang ---
        //        //var total_DoDang = contractList
        //        //    .Select(c =>
        //        //    {
        //        //        turnoverDict.TryGetValue(c.MaHD, out var daXuat);
        //        //        return daXuat < c.Value ? (c.Value - daXuat) : 0.0;
        //        //    })
        //        //    .Sum();

        //        //// --- 7. Tổng thiết bị đã kiểm định trong năm ---
        //        //var total_Equipment = ctx.Accreditations
        //        //    .Where(ac => ac.AccrDate != null && ac.AccrDate.Value.Year == year) // Nếu chưa có cột năm ở đây
        //        //    .Join(ctx.Equipments, ac => ac.equiment.Id, eq => eq.Id, (ac, eq) => eq.contract.Id)
        //        //    .Join(contracts, eqContractId => eqContractId, c => c.Id, (eqContractId, c) => eqContractId)
        //        //    .Distinct()
        //        //    .Count();

        //        // Nếu bảng Accreditations có cột ngày → thêm cột AccrYear tương tự (khuyến nghị)

        //        //return (total_Value, total_XuatHoaDon, total_ThuNo, total_CongNo, total_DoDang, total_Equipment);
        //        return (0,0,0,0,0,0);
        //    }
        //}
       
        //public static ContractViewModel GetContractById(int id)
        //{
        //    var ctx = new IncosafCMSContext("name=AppContext", null);
        //    var contracts = ctx.Contracts;
        //    var result = from contract in contracts
        //                 //join t in ctx.v_ActTurnOver
        //                 //    on contract.MaHD equals t.ma_hd into gj1
        //                 //join r in ctx.v_ActPayment
        //                 //    on contract.MaHD equals r.ma_hd into gj2
        //                 //join d in ctx.Contract_Docs
        //                     //on contract.Id equals d.Contract.Id into gj4
        //                 select new ContractViewModel
        //                 {
        //                     Id = contract.Id,
        //                     MaHD = contract.MaHD,
        //                     Name = contract.Name,
        //                     SignDate = contract.SignDate,
        //                     CreateDate = contract.CreateDate,
        //                     Value = contract.Value,
        //                     Finished = contract.Finished,
        //                     RatioOfCompany = contract.RatioOfCompany,
        //                     ValueRoC = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfCompany / 100,
        //                     RatioOfInternal = contract.RatioOfInternal,
        //                     ValueRoI = (contract.Value / (1 + contract.VAT / 100)) * contract.RatioOfInternal / 100,
        //                     Status = contract.Status,
        //                     OwnerId = contract.own.Id,
        //                     UserName = contract.own.UserName,
        //                     OwnerDisplayName = contract.own.DisplayName,
        //                     KDV1DisplayName = contract.KDV1.DisplayName ?? contract.own.DisplayName,
        //                     KDV2DisplayName = contract.KDV2.DisplayName,
        //                     DepartmentId = contract.own.DepartmentId,
        //                     MaDV = contract.own.Department.MaDV,
        //                     CustomerName = contract.customer.Name,
        //                     CustomerAddress = contract.customer.Address,
        //                     CustomerTaxID = contract.customer.TaxID,
        //                     IsGiayDeNghi = contract.IsGiayDeNghi,         // 23-jan-2024 by Lapbt. using in index.js để hiển thị thêm ở home bên chi tiết
        //                     RatioOfGroup = contract.RatioOfGroup,
        //                     contractType = contract.contractType,         // 13-may-2024. using in index.js hiển thị loại hình hợp đồng
        //                     //TotalValueIPoC = contract.InternalPayments.Where(x => x.IPType == InternalPaymentType.ThanhToanTrungTam).Select(x => x.InternalPaymentValue).DefaultIfEmpty(0).Sum(),
        //                     //TotalValueIPoI = contract.InternalPayments.Where(x => x.IPType == InternalPaymentType.ThanhToanChuTri).Select(x => x.InternalPaymentValue).DefaultIfEmpty(0).Sum(),

        //                     //XuatHoaDon = gj1.Where(x => x.ma_hd == contract.MaHD
        //                     //            && !string.IsNullOrEmpty(x.so_ct) && !string.IsNullOrEmpty(x.ma_hd))
        //                     //                    .Sum(x => (double?)x.DT) ?? 0,                             
        //                     //ThuNo = gj2.Where(x => x.ma_hd == contract.MaHD
        //                     //        && !string.IsNullOrEmpty(x.so_ct) && !string.IsNullOrEmpty(x.ma_hd))
        //                     //               .Sum(x => (double?)(x.TM + x.CK)) ?? 0,
        //                     //DocCount = gj4.Where(x => x.Contract.Id == contract.Id).Count()
        //                     // danh sách hóa đơn chi tiết
        //                     /*
        //                     vActTurnOvers = gj1
        //                        .Where(x => x.ma_hd == contract.MaHD && !string.IsNullOrEmpty(x.so_ct))
        //                        .AsEnumerable()
        //                        .Select(x => new v_ActTurnOver
        //                        {
        //                            //ma_hd = x.ma_hd,
        //                            ngay_ct = x.ngay_ct,
        //                            so_ct = x.so_ct,
        //                            DT = x.DT,
        //                            VAT = x.VAT
        //                        }).ToList()
        //                     */

        //                 };
        //    // Giữ nguyên WhereIf như cũ
        //    result = result
        //    .Where(x => x.Id == id);
        //    return result.FirstOrDefault();
        //}

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



    }
}