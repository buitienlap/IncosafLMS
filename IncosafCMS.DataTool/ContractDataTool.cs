using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
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
    class ContractDataTool : IDatatool
    {
        IEntitiesContext context;
        IUnitOfWork uow;
        IService<Contract> service;
        private List<Contract> lastSessionData;
        ContractDataTool()
        {
            context = new IncosafCMSContext("AppContext", new DebugLogger());
            uow = new UnitOfWork(context);
            service = new Service<Contract>(uow);
        }
        private static ContractDataTool instance;
        public static ContractDataTool Instance
        {
            get
            {
                if (instance == null)
                    instance = new ContractDataTool();
                return instance;
            }
        }
        public List<T> ImportToDatabase<T>(string exelPath, bool confirm = true)
        {
            ImportContract ui = new ImportContract(exelPath);
            ui.ShowDialog();
            lastSessionData = ui.contracts;
            if (!confirm)
                Commit();
            return lastSessionData as List<T>;
        }
        public void Commit()
        {
            var ownernull = lastSessionData?.Where(x => x.own == null);
            if (ownernull.Count() > 0)
            {
                System.Windows.Forms.MessageBox.Show("Vui lòng kiểm tra lại danh sách nhân viên. Nhân viên không tồn tại trong cơ sở dữ liệu.\nVui lòng thêm nhân viên vào cơ sở dữ liệu trước để tiếp tục.", "Lỗi", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            if (lastSessionData?.Count > 0)
            {
                using (var context = new IncosafCMSContext("AppContext", new DebugLogger()))
                {
                    foreach (var item in lastSessionData)
                    {
                        if (item.customer != null)
                        {
                            var cus = context.Set<Customer>().Where(e => e.Name == item.customer.Name).FirstOrDefault();
                            //var cus = uow.Repository<Customer>().FindBy(e => e.Name == item.customer.Name).FirstOrDefault();
                            if (cus != null)
                            {
                                //item.customerId = cus.Id;
                                item.customer = cus;
                            }
                            else
                            {
                                //item.customerId = 0;
                                //item.customer.Id = 0;
                                context.Set<Customer>().Add(item.customer);
                                context.SaveChanges();
                            }
                            var owner = context.Set<AppUser>().Where(e => e.DisplayName == item.own.Tags || e.Tags == item.own.Tags || e.DisplayName == item.own.DisplayName).FirstOrDefault();
                            if (owner != null)
                                item.own = owner;
                            else item.own = null;

                            item.TienTruThue = item.TurnOvers.Sum(x => x.TienTruThue);
                            item.TongTienVe = item.Payments.Sum(x => x.PaymentValue);
                            item.TongTienXuatHoaDon = item.TurnOvers.Sum(x => x.TotalValue);


                            context.Set<Contract>().Add(item);
                            context.SaveChanges();
                            //service.Add(item);
                        }
                    }
                }
            }
        }
        public List<T> GetAll<T>()
        {
            return service.GetAll() as List<T>;
        }
        public void Update(Contract contract)
        {
            service.Update(contract);
        }
    }
}
