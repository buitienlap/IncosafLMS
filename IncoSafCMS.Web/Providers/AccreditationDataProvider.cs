using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
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
    public class AccreditationDataProvider
    {
        static IUnitOfWork uow = null;
        static IService<Accreditation> service = null;
        const string AccreditationDataContextKey = "AccreditationDataContextKey";
        public static IncosafCMSContext DB
        {
            get
            {
                if (HttpContext.Current.Session[AccreditationDataContextKey] == null)
                {
                    HttpContext.Current.Session[AccreditationDataContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                    var repo = new EntityRepository<Accreditation>((IncosafCMSContext)HttpContext.Current.Session[AccreditationDataContextKey]);
                    repo.StartChangesMonitor();
                    repo.NotificationRegister(null);
                    repo.OnChanged += AccreditationDataProvider_OnChanged;
                }
                return (IncosafCMSContext)HttpContext.Current.Session[AccreditationDataContextKey];
            }
        }

        private static void AccreditationDataProvider_OnChanged(object sender, EventArgs e)
        {
            accreditations = null;
            uow = null;
            service = null;
        }

        static List<Accreditation> accreditations;
        public static IQueryable<Accreditation> Accreditations
        {
            get
            {
                if (/*DB.ChangeTracker.HasChanges() ||*/ accreditations == null)
                {
                    if (uow == null) uow = new UnitOfWork(DB);
                    if (service == null) service = new Service<Accreditation>(uow);
                    accreditations = service.GetAll(e => e.equiment, e => e.equiment.contract, e => e.equiment.contract.own, e => e.equiment.contract.own.Department, e => e.equiment.contract.customer).ToList();
                }
                return accreditations.AsQueryable();
            }
        }
    }
}