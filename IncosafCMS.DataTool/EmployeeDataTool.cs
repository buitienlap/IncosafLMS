using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Core.Services;
using IncosafCMS.Data;
using IncosafCMS.Data.Extensions;
using IncosafCMS.Data.Identity;
using IncosafCMS.Data.Identity.Models;
using IncosafCMS.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.DataTool
{
    class EmployeeDataTool : IDatatool
    {
        IncosafCMSContext context;
        IUnitOfWork uow;
        IService<AppUser> service;
        UserManager<ApplicationIdentityUser, int> appUserMan;
        private List<AppUser> lastSessionData;
        public EmployeeDataTool()
        {
            context = new IncosafCMSContext("AppContext", new DebugLogger());
            uow = new UnitOfWork(context);
            service = new Service<AppUser>(uow);
            appUserMan = IdentityFactory.CreateUserManager(context);
        }
        private static EmployeeDataTool instance;
        public static EmployeeDataTool Instance
        {
            get
            {
                if (instance == null)
                    instance = new EmployeeDataTool();
                return instance;
            }
        }
        public List<T> GetAll<T>()
        {
            return service.GetAll() as List<T>;
        }
        public List<T> ImportToDatabase<T>(string exelPath, bool confirm = true)
        {
            ImportEmployee ui = new ImportEmployee(exelPath);
            ui.ShowDialog();
            lastSessionData = ui.employees;
            if (!confirm)
                Commit();
            return lastSessionData as List<T>;
        }
        public void Commit()
        {
            CommitAsync();
            //if (lastSessionData?.Count > 0)
            //{
            //    var depRepo = uow.Repository<Department>();
            //    var depPos = uow.Repository<EmployeePosition>();
            //    foreach (var cus in lastSessionData)
            //    {
            //        var existCus = service.FindBy(u => u.Email == cus.Email).FirstOrDefault();
            //        if (existCus != null) continue;
            //        var dep = depRepo.FindBy(d => d.MaDV == cus.Department.MaDV).FirstOrDefault();
            //        cus.Department = dep;
            //        var pos = depPos.FindBy(p => p.Name == cus.Position.Name).FirstOrDefault();
            //        cus.Position = pos;
            //        var iduser = cus.ToApplicationUser();
            //        appUserMan.Create(iduser, cus.Email);
            //        var newUser = appUserMan.FindByEmail(cus.Email);
            //        var newCus = newUser.ToAppUser();
            //        service.Add(newCus);
            //    }

            //}
        }

        void CommitAsync()
        {
            if (lastSessionData?.Count > 0)
            {
                var depRepo = uow.Repository<Department>();
                var depPos = uow.Repository<EmployeePosition>();
                foreach (var cus in lastSessionData)
                {
                    var existCus = service.FindBy(u => u.Email == cus.Email).FirstOrDefault();
                    if (existCus != null) continue;
                    var dep = depRepo.FindBy(d => d.MaDV == cus.Department.MaDV).FirstOrDefault();
                    cus.Department = dep;
                    var pos = depPos.FindBy(p => p.Name == cus.Position.Name).FirstOrDefault();
                    cus.Position = pos;
                    cus.UserName = cus.Email;
                    var iduser = cus.ToApplicationUser();
                    if (string.IsNullOrWhiteSpace(cus.Email)) continue;
                    var result = appUserMan.Create(iduser, cus.Email);
                    if (result.Succeeded)
                    {
                        var newUser = appUserMan.FindByEmail(cus.Email);
                        var newCus = newUser.ToAppUser();
                        newCus.DepartmentId = newUser.Department?.Id;
                        newCus.PositionId = newUser.Position?.Id;
                        service.Add(newCus);
                    }
                }

            }
        }
    }
}
