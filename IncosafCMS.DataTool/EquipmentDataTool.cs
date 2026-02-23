using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.DataTool
{
    class EquipmentDataTool:IDatatool
    {
        IEntitiesContext context;
        IUnitOfWork uow;
        IService<Equipment> service;
        private List<Equipment> lastSessionData;
        EquipmentDataTool()
        {
            context = new IncosafCMSContext("AppContext", new DebugLogger());
            uow = new UnitOfWork(context);
            service = new Service<Equipment>(uow);
        }
        private static EquipmentDataTool instance;
        public static EquipmentDataTool Instance
        {
            get
            {
                if (instance == null)
                    instance = new EquipmentDataTool();
                return instance;
            }
        }
        public List<T> ImportToDatabase<T>(string exelPath, bool confirm = true)
        {
            ImportEquipment ui = new ImportEquipment(exelPath);
            ui.ShowDialog();
            lastSessionData = ui.equipments;
            if (!confirm)
                Commit();
            return lastSessionData as List<T>;
        }

        public void Commit()
        {
            if (lastSessionData?.Count > 0)
            {
                foreach (var cus in lastSessionData)
                    service.Add(cus);
            }
        }

        public List<T> GetAll<T>()
        {
            return service.GetAll() as List<T>;
        }
    }
}
