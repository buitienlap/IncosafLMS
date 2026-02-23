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
    public static class ContractDataProvider
    {
        static IUnitOfWork uow = null;
        static IService<Contract> service = null;
        const string ContractDataContextKey = "ContractDataContextKey";
        public static IncosafCMSContext DB1
        {
            get
            {
                if (HttpContext.Current.Session[ContractDataContextKey] == null)
                {
                    HttpContext.Current.Session[ContractDataContextKey] = new IncosafCMSContext("name=AppContext", new DebugLogger());
                    var repo = new EntityRepository<Contract>((IncosafCMSContext)HttpContext.Current.Session[ContractDataContextKey]);
                    repo.StartChangesMonitor();
                    repo.NotificationRegister(null);
                    repo.OnChanged += ContractDataProvider_OnChanged;
                }
                return (IncosafCMSContext)HttpContext.Current.Session[ContractDataContextKey];
            }
        }

        private static void ContractDataProvider_OnChanged(object sender, EventArgs e)
        {
            contracts1 = null;
            contracts = null;
            uow = null;
            service = null;
        }

        internal static List<Contract> contracts;
        public static IQueryable<Contract> Contracts1
        {
            get
            {
                if (/*DB1.ChangeTracker.HasChanges() ||*/ contracts == null)
                {
                    if (uow == null) uow = new UnitOfWork(DB1);
                    if (service == null) service = new Service<Contract>(uow);
                    //contracts = service.GetAll(e => e.customer, e => e.own, e => e.Tasks, e => e.Payments, e => e.TurnOvers, e => e.InternalPayments).ToList();
                    contracts = service.GetAll(e => e.customer, e => e.own).ToList();
                }
                return contracts.AsQueryable();
            }
        }

        public static IQueryable<Contract> GetContractsForReport(DateTime fromDate, DateTime toDate, int employeeId = -1, int departmentId = -1)
        {
            if (uow == null) uow = new UnitOfWork(DB1);
            if (service == null) service = new Service<Contract>(uow);
            if (employeeId > 0)
            {
                var contracts = service.GetAll(x => x.SignDate >= fromDate && x.SignDate <= toDate && x.own != null && x.own.Id == employeeId, e => e.customer).ToList(); // , e => e.own, e => e.Tasks, e => e.Payments, e => e.TurnOvers, e => e.InternalPayments
                return contracts.AsQueryable();
            }
            else if (departmentId > 0)
            {
                var contracts = service.GetAll(x => x.SignDate >= fromDate && x.SignDate <= toDate && x.own != null && x.own.DepartmentId == departmentId, e => e.customer).ToList();
                return contracts.AsQueryable();
            }
            else
            {
                var contracts = service.GetAll(x => x.SignDate >= fromDate && x.SignDate <= toDate, e => e.customer).ToList();
                return contracts.AsQueryable();
            }
        }

        internal static List<ContractViewModel> contracts1;
        public static IQueryable<ContractViewModel> Contracts2
        {
            get
            {
                if (contracts1 == null)
                {
                    //var clientIdParameter = new SqlParameter("@OwnerId", 4);

                    //contracts1 = DB1.Database
                    //    .SqlQuery<ContractViewModel>("GetContractsNew @OwnerId", clientIdParameter)
                    //    .AsQueryable();
                    contracts1 = DB1.Database
                        .SqlQuery<ContractViewModel>("GetAllContracts").ToList();
                }

                return contracts1.AsQueryable();

            }
        }

        //internal static List<ContractPaymentViewModel> contractsPayment;
        //public static IQueryable<ContractPaymentViewModel> ContractsPayment
        //{
        //    get
        //    {
        //        if (contractsPayment == null)
        //        {
        //            //var clientIdParameter = new SqlParameter("@OwnerId", 4);
        //            contractsPayment = DB1.Database
        //                .SqlQuery<ContractPaymentViewModel>("GetAllContractsPayment").ToList();
        //        }

        //        return contractsPayment.AsQueryable();

        //    }
        //}

        public static IQueryable<ContractViewModel> GetContractsByOwnerID(int? OwnerID)
        {
            if (OwnerID.HasValue)
            {
                var clientIdParameter = new SqlParameter("@ownerid", OwnerID);
                var contracts = DB1.Database
                    .SqlQuery<ContractViewModel>("GetAllContracts @ownerid", clientIdParameter)
                    .AsQueryable();
                return contracts;
            }
            else
            {
                var contracts = DB1.Database
                    .SqlQuery<ContractViewModel>("GetAllContracts")
                    .AsQueryable();
                return contracts;
            }
        }

        internal static IQueryable<ContractViewModel> GetContractByMaHD(string MaHD)
        {
            var clientIdParameter = new SqlParameter("@MaHD", MaHD);
            var contracts = DB1.Database
                .SqlQuery<ContractViewModel>("GetContractByMaHD @MaHD", clientIdParameter)
                .AsQueryable();
            return contracts;
        }

    }
}