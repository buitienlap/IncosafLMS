using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Services;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IncosafCMS.Web.Providers
{
    public static class ActivityDataProvider
    {
        static IUnitOfWork uow = null;
        static IService<ActivityLog> service = null;
        const string ActivityDataContextKey = "ActivityDataContextKey";
        public static IncosafCMSContext DB1
        {
            get
            {
                if (HttpContext.Current.Session[ActivityDataContextKey] == null)
                {
                    HttpContext.Current.Session[ActivityDataContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                    var repo = new EntityRepository<ActivityLog>((IncosafCMSContext)HttpContext.Current.Session[ActivityDataContextKey]);
                    repo.StartChangesMonitor();
                    repo.NotificationRegister(null);
                    repo.OnChanged += ActivityDataProvider_OnChanged;
                }
                return (IncosafCMSContext)HttpContext.Current.Session[ActivityDataContextKey];
            }
        }

        private static void ActivityDataProvider_OnChanged(object sender, EventArgs e)
        {
            activity1 = null;
            activity = null;
            uow = null;
            service = null;
        }

        internal static List<ActivityLog> activity;
        public static IQueryable<ActivityLog> activity0
        {
            get
            {
                if (/*DB1.ChangeTracker.HasChanges() ||*/ activity == null)
                {
                    if (uow == null) uow = new UnitOfWork(DB1);
                    if (service == null) service = new Service<ActivityLog>(uow);
                    //Activitys = service.GetAll(e => e.customer, e => e.own, e => e.Tasks, e => e.Payments, e => e.TurnOvers, e => e.InternalPayments).ToList();
                    activity = service.GetAll(e => e.UserId).ToList();
                }
                return activity.AsQueryable();
            }
        }

        internal static List<ActivityLog> activity1;
        public static IQueryable<ActivityLog> activity11
        {
            get
            {
                if (activity1 == null)
                {
                    if (uow == null) uow = new UnitOfWork(DB1);
                    if (service == null) service = new Service<ActivityLog>(uow);
                    activity1 = service.GetAll(e => e.UserId).ToList();
                }

                return activity1.AsQueryable();

            }
        }

        public static IQueryable<ActivityLog> GetActivityForReport(DateTime fromDate, DateTime toDate, int employeeId = -1, int departmentId = -1)
        {
            if (uow == null) uow = new UnitOfWork(DB1);
            if (service == null) service = new Service<ActivityLog>(uow);
            if (employeeId > 0)
            {
                var activity = service.GetAll(x => x.UserId == employeeId).ToList();
                return activity.AsQueryable();
            }
            else if (departmentId > 0)
            {
                var activity = service.GetAll(x => x.UserId == employeeId && x.User.DepartmentId == departmentId).ToList();
                return activity.AsQueryable();
            }
            else
            {
                var activity = service.GetAll(x => x.UserId == employeeId).ToList(); // x.SignDate >= fromDate && x.SignDate <= toDate, e => e.customer
                return activity.AsQueryable();
            }
        }

        public static IQueryable<ActivityLog> GetActivityByOwnerID(int? OwnerID)
        {
            if (uow == null) uow = new UnitOfWork(DB1);
            if (service == null) service = new Service<ActivityLog>(uow);

            if (OwnerID.HasValue)
            {
                //var clientIdParameter = new SqlParameter("@ownerid", OwnerID);
                //var contracts = DB1.Database
                //    .SqlQuery<ContractViewModel>("GetAllContracts @ownerid", clientIdParameter)
                //    .AsQueryable();
                //return contracts;
                var activity = service.GetAll(x => x.UserId == OwnerID).ToList();
                return activity.AsQueryable();
            }
            else
            {
                //var contracts = DB1.Database
                //    .SqlQuery<ContractViewModel>("GetAllContracts")
                //    .AsQueryable();
                //return contracts;
                var activity = service.GetAll().ToList();
                return activity.AsQueryable();
            }
        }


    }
}