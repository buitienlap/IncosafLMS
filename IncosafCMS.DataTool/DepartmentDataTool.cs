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
    class DepartmentDataTool:IDatatool
    {
        IEntitiesContext context;
        IUnitOfWork uow;
        IService<Department> service;
        private List<Department> lastSessionData;
        DepartmentDataTool()
        {
            context = new IncosafCMSContext("AppContext", new DebugLogger());
            uow = new UnitOfWork(context);
            service = new Service<Department>(uow);
        }
        private static DepartmentDataTool instance;
        public static DepartmentDataTool Instance
        {
            get
            {
                if (instance == null)
                    instance = new DepartmentDataTool();
                return instance;
            }
        }
        public List<T> GetAll<T>()
        {
            return service.GetAll() as List<T>;
        }

        public List<T> ImportToDatabase<T>(string exelPath, bool confirm = true)
        {
            ImportDepartment ui = new ImportDepartment(exelPath);
            ui.ShowDialog();
            lastSessionData = ui.departments;
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
    }
}
