using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Services;
using System.Collections.Generic;
using System;

namespace IncosafCMS.DataTool
{
    public class CustomerDataTool: IDatatool
    {
        IEntitiesContext context;
        IUnitOfWork uow;
        IService<Customer> service;
        private List<Customer> lastSessionData;
        CustomerDataTool()
        {
            context = new IncosafCMSContext("AppContext", new DebugLogger());
            uow = new UnitOfWork(context);
            service = new Service<Customer>(uow);
        }
        private static CustomerDataTool instance;
        public static CustomerDataTool Instance
        {
            get
            {
                if (instance == null)
                    instance = new CustomerDataTool();
                return instance;
            }
        }
        public void Commit()
        {
            if (lastSessionData?.Count > 0)
            {
                foreach (var cus in lastSessionData)
                    service.Add(cus);
            }
        }
        public List<T> ImportToDatabase<T>(string exelPath, bool confirm = true)
        {
            ImportCustomer ui = new ImportCustomer(exelPath);
            ui.ShowDialog();
            lastSessionData = ui.customers;
            if (!confirm)
                Commit();
            return lastSessionData as List<T>;
        }

        public List<T> GetAll<T>()
        {
            var result = service.GetAll();
            return result as List<T>;
        }
    }
}
