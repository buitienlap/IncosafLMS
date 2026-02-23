using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Services;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncosafCMS.Web.Providers
{
    public class AppUserDataProvider
    {
        static IUnitOfWork uow = null;
        static IService<AppUser> service = null;
        const string AppUserDataContextKey = "AppUserDataContextKey";
        public static IncosafCMSContext DB
        {
            get
            {
                if (HttpContext.Current.Session[AppUserDataContextKey] == null)
                {
                    HttpContext.Current.Session[AppUserDataContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                    var repo = new EntityRepository<AppUser>((IncosafCMSContext)HttpContext.Current.Session[AppUserDataContextKey]);
                    repo.StartChangesMonitor();
                    repo.NotificationRegister(null);
                    repo.OnChanged += AppUserDataProvider_OnChanged;
                }
                return (IncosafCMSContext)HttpContext.Current.Session[AppUserDataContextKey];
            }
        }

        private static void AppUserDataProvider_OnChanged(object sender, EventArgs e)
        {
            appusers = null;
            uow = null;
            service = null;
        }

        static List<AppUser> appusers;
        public static IQueryable<AppUser> AppUsers
        {
            get
            {
                if (/*DB.ChangeTracker.HasChanges() ||*/ appusers == null)
                {
                    if (uow == null) uow = new UnitOfWork(DB);
                    if (service == null) service = new Service<AppUser>(uow);
                    //appusers = service.GetAll(e => e.SanLuongDK, e => e.Department, e => e.Position).ToList();
                    appusers = service.GetAll(e => e.Department, e => e.Position).ToList();
                    //appusers = DB.Database.SqlQuery<AppUser>("GetAllAppUsers").ToList();
                }
                return appusers.AsQueryable();
            }
        }
    }
}