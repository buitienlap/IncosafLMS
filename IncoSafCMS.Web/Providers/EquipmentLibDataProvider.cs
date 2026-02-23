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
    public class EquipmentLibDataProvider
    {
        static IUnitOfWork uow = null;
        static IService<OriginalEquipment> service = null;
        const string EquipmentLibDataContextKey = "EquipmentLibDataContextKey";
        public static IncosafCMSContext DB
        {
            get
            {
                if (HttpContext.Current.Session[EquipmentLibDataContextKey] == null)
                {
                    HttpContext.Current.Session[EquipmentLibDataContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                    var repo = new EntityRepository<OriginalEquipment>((IncosafCMSContext)HttpContext.Current.Session[EquipmentLibDataContextKey]);
                    repo.StartChangesMonitor();
                    repo.NotificationRegister(null);
                    repo.OnChanged += EquipmentLibDataProvider_OnChanged;
                }
                return (IncosafCMSContext)HttpContext.Current.Session[EquipmentLibDataContextKey];
            }
        }

        private static void EquipmentLibDataProvider_OnChanged(object sender, EventArgs e)
        {
            equipmentlibs = null;
            uow = null;
            service = null;
        }

        static List<OriginalEquipment> equipmentlibs;
        public static IQueryable<OriginalEquipment> EquipmentLibs
        {
            get
            {
                if (/*DB.ChangeTracker.HasChanges() ||*/ equipmentlibs == null)
                {
                    if (uow == null) uow = new UnitOfWork(DB);
                    if (service == null) service = new Service<OriginalEquipment>(uow);
                    equipmentlibs = service.GetAll(e => e.LoadTests, e => e.Partions, e => e.specifications, e => e.TechnicalDocuments).ToList();
                }
                return equipmentlibs.AsQueryable();
            }
        }
    }
}