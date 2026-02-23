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
    public class CustomerDataProvider
    {
        static IUnitOfWork uow = null;
        static IService<Customer> service = null;
        const string CustomerDataContextKey = "CustomerDataContextKey";
        public static IncosafCMSContext DB
        {
            get
            {
                if (HttpContext.Current.Session[CustomerDataContextKey] == null)
                {
                    HttpContext.Current.Session[CustomerDataContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                    //var repo = new EntityRepository<TurnOver>((IncosafCMSContext)HttpContext.Current.Session[CustomerDataContextKey]);
                    //repo.StartChangesMonitor();
                    //repo.NotificationRegister(null);
                    //repo.OnChanged += CustomerDataProvider_OnChanged;
                }
                return (IncosafCMSContext)HttpContext.Current.Session[CustomerDataContextKey];
            }
        }

        private static void CustomerDataProvider_OnChanged(object sender, EventArgs e)
        {
            customers = null;
            uow = null;
            service = null;
        }

        static List<Customer> customers;
        public static IQueryable<Customer> Customers
        {
            get
            {
                if (/*DB.ChangeTracker.HasChanges() ||*/ customers == null)
                {
                    if (uow == null) uow = new UnitOfWork(DB);
                    if (service == null) service = new Service<Customer>(uow);
                    customers = service.GetAll(e => e.department).ToList();
                }
                return customers.AsQueryable();
            }
        }
    }
}