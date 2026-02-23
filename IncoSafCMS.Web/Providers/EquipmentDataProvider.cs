using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Services;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;

namespace IncosafCMS.Web.Providers
{
    public class EquipmentDataProvider
    {
        static IUnitOfWork uow = null;
        static IService<Equipment> service = null;
        const string EquipmentDataContextKey = "EquipmentDataContextKey";
        public static IncosafCMSContext DB
        {
            get
            {
                if (HttpContext.Current.Session[EquipmentDataContextKey] == null)
                {
                    HttpContext.Current.Session[EquipmentDataContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                    var repo = new EntityRepository<Equipment>((IncosafCMSContext)HttpContext.Current.Session[EquipmentDataContextKey]);
                    repo.StartChangesMonitor();
                    repo.NotificationRegister(null);
                    repo.OnChanged += EquipmentDataProvider_OnChanged;
                }
                return (IncosafCMSContext)HttpContext.Current.Session[EquipmentDataContextKey];
            }
        }

        private static void EquipmentDataProvider_OnChanged(object sender, EventArgs e)
        {
            equipments = null;
            allequipments = null;
            uow = null;
            service = null;
        }

        internal static List<Equipment> equipments;
        public static IQueryable<Equipment> Equipments
        {
            get
            {
                if (/*DB.ChangeTracker.HasChanges() || */equipments == null)
                {
                    if (uow == null) uow = new UnitOfWork(DB);
                    if (service == null) service = new Service<Equipment>(uow);
                    equipments = service.GetAll(e => e.LoadTests, e => e.Partions, e => e.specifications, e => e.TechnicalDocuments).ToList();
                }
                return equipments.AsQueryable();
            }
        }
        internal static IQueryable<EquipmentViewModel> allequipments;
        public static IQueryable<EquipmentViewModel> AllEquipments
        {
            // allequipments. 15-apr-2025 K0 sử dụng cache nữa, mỗi lần dùng luôn
            get
            {
                var clientParameter_AccrFromDate = new SqlParameter("@accrfromdate", DateTime.Today.AddYears(-5).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToUpper());
                var clientParameter_AccrToDate = new SqlParameter("@accrtodate", DateTime.Today.AddYears(1).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToUpper());
                return DB.Database.SqlQuery<EquipmentViewModel>("GetAllEquipments @accrfromdate, @accrtodate", clientParameter_AccrFromDate, clientParameter_AccrToDate).AsQueryable();
            }
        }

        /// <summary>
        /// 15-apr-2025 added by lapbt. Viết lại lọc DL ngay từ proc, sdung cho bên DS Tbi
        /// </summary>
        /// <param name="accrfromdate">DateTime</param>
        /// <param name="accrtodate">DateTime</param>
        /// <returns></returns>
        public static IQueryable<EquipmentViewModel> AllEquipmentsFilter(DateTime accrfromdate, DateTime accrtodate)
        {
            var clientParameter_AccrFromDate = new SqlParameter("@accrfromdate", accrfromdate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToUpper());
            var clientParameter_AccrToDate = new SqlParameter("@accrtodate", accrtodate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToUpper());
            return DB.Database.SqlQuery<EquipmentViewModel>("GetAllEquipments @accrfromdate, @accrtodate", clientParameter_AccrFromDate, clientParameter_AccrToDate).AsQueryable();
        }

        public static List<LoadTest> GetLoadTests(int? equipmentid)
        {
            if (equipmentid.HasValue)
            {
                var clientIdParameter = new SqlParameter("@equipmentid", equipmentid);
                var result = DB.Database
                    .SqlQuery<LoadTest>("GetLoadTestsByEquipment @equipmentid", clientIdParameter)
                    .ToList();
                return result;
            }
            else return new List<LoadTest>();
        }

        public static List<Specifications> GetSpecifications(int? equipmentid)
        {
            if (equipmentid.HasValue)
            {
                var clientIdParameter = new SqlParameter("@equipmentid", equipmentid);
                var result = DB.Database
                    .SqlQuery<Specifications>("GetSpecificationsByEquipment @equipmentid", clientIdParameter)
                    .ToList();
                return result;
            }
            else return new List<Specifications>();
        }

        public static List<EquipmentPartion> GetPartions(int? equipmentid)
        {
            if (equipmentid.HasValue)
            {
                var clientIdParameter = new SqlParameter("@equipmentid", equipmentid);
                var result = DB.Database
                    .SqlQuery<EquipmentPartion>("GetPartionsByEquipment @equipmentid", clientIdParameter)
                    .ToList();
                return result;
            }
            else return new List<EquipmentPartion>();
        }

        public static List<TechnicalDocument> GetTechnicalDocuments(int? equipmentid)
        {
            if (equipmentid.HasValue)
            {
                var clientIdParameter = new SqlParameter("@equipmentid", equipmentid);
                var result = DB.Database
                    .SqlQuery<TechnicalDocument>("GetTechnicalDocumentsByEquipment @equipmentid", clientIdParameter)
                    .ToList();
                return result;
            }
            else return new List<TechnicalDocument>();
        }

        //Thêm cái dưới này để lấy thông tin tem từ db        
        public static List<StampSerialViewModel> GetStampSerialViewByUser(int? userId, int? departmentId, bool isAdmin, bool isDeptDirector)
        {            
            var paramUser = new SqlParameter("@UserId", userId.HasValue ? (object)userId.Value : DBNull.Value);
            var paramDepartment = new SqlParameter("@DepartmentId", SqlDbType.Int);
            paramDepartment.Value = (object?)departmentId ?? DBNull.Value;
            var paramIsAdmin = new SqlParameter("@IsAdmin", isAdmin ? 1 : 0);
            var paramIsDeptDirector = new SqlParameter("@IsDeptDirector", isDeptDirector ? 1 : 0);

            return DB.Database
                     .SqlQuery<StampSerialViewModel>(
                         "EXEC GetStampSerialViewByUser @UserId, @DepartmentId, @IsAdmin, @IsDeptDirector",
                         paramUser,
                         paramDepartment,
                         paramIsAdmin,
                         paramIsDeptDirector
                     ).ToList();
        }
        //Thêm GetEquipmentsByUser để lấy danh sách TB theo tên KĐV/toàn bô
        /*
        public static IQueryable<EquipmentViewModel> GetEquipmentsByUser(
                    DateTime accrfromdate, DateTime accrtodate,
                    int? userId, bool isAdmin,
                    int? contractId)
        {
            var paramFrom = new SqlParameter("@accrfromdate", accrfromdate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToUpper());
            var paramTo = new SqlParameter("@accrtodate", accrtodate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToUpper());
            var paramUserId = new SqlParameter("@UserId", userId.HasValue ? (object)userId.Value : DBNull.Value);
            var paramIsAdmin = new SqlParameter("@IsAdmin", isAdmin ? 1 : 0);
            var paramContractId = new SqlParameter("@contractId", contractId.HasValue ? (object)contractId.Value : DBNull.Value);

            return DB.Database.SqlQuery<EquipmentViewModel>(
                "GetEquipmentsByUser @accrfromdate, @accrtodate, @UserId, @IsAdmin, @contractId",
                paramFrom, paramTo, paramUserId, paramIsAdmin, paramContractId
            ).AsQueryable();
        }
        */
        //Code sửa 13.10.2025 để tăng tốc lấy DS TB
        public static IEnumerable<EquipmentViewModel> GetEquipmentsByUser(
        DateTime accrfromdate, DateTime accrtodate,
        int? userId, bool isAdmin, int? contractId)
        {
            var paramFrom = new SqlParameter("@accrfromdate", accrfromdate);
            var paramTo = new SqlParameter("@accrtodate", accrtodate);
            var paramUserId = new SqlParameter("@UserId", (object?)userId ?? DBNull.Value);
            var paramIsAdmin = new SqlParameter("@IsAdmin", isAdmin ? 1 : 0);
            var paramContractId = new SqlParameter("@contractId", (object?)contractId ?? DBNull.Value);

            return DB.Database.SqlQuery<EquipmentViewModel>(
                "EXEC GetEquipmentsByUser @accrfromdate, @accrtodate, @UserId, @IsAdmin, @contractId",
                paramFrom, paramTo, paramUserId, paramIsAdmin, paramContractId
            ).ToList();
        }


    }
}