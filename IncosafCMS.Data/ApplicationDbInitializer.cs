using System;
using System.Data.Entity;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Logging;
using IncosafCMS.Data.Identity;
using IncosafCMS.Data.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using IncosafCMS.Core.DomainModels.Identity;
using IncosafCMS.Data.Extensions;

namespace IncosafCMS.Data
{
    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext> /////// DropCreateDatabaseIfModelChanges
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<IncosafCMSContext>
    {
        protected override void Seed(IncosafCMSContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public void InitializeIdentityForEF(IncosafCMSContext db)
        {
            // This is only for testing purpose
            const string name = "admin@admin.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";
            var applicationRoleManager = IdentityFactory.CreateRoleManager(db);
            var applicationUserManager = IdentityFactory.CreateUserManager(db);

            var context = new IncosafCMSContext("name=AppContext", new DebugLogger());

            //Create Role Admin if it does not exist
            AppPermission perm1 = new AppPermission() { Name = "CreateContract", Description = "Thêm mới hợp đồng" };
            AppPermission perm2 = new AppPermission() { Name = "EditContract", Description = "Chỉnh sửa hợp đồng" };

            var role = applicationRoleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationIdentityRole { Name = roleName };
                //role.Permissions.Add(perm1);
                //role.Permissions.Add(perm2);

                applicationRoleManager.Create(role);
                //perm1.Roles.Add(role.ToApplicationRole());
                //perm2.Roles.Add(role.ToApplicationRole());
            }

            var user = applicationUserManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationIdentityUser { UserName = name, Email = name };
                applicationUserManager.Create(user, password);
                applicationUserManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = applicationUserManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                applicationUserManager.AddToRole(user.Id, role.Name);
            }

            //var image = new Image { Path = "http://lorempixel.com/400/200/" };
            //context.Set<Image>().Add(image);
            //for (var i = 0; i < 100; i++)
            //{
            //    context.Set<Product>().Add(new Product { Name = "My Product", Description = "My Product", Image = image });
            //}

            //var user1 = new User() { UserName = "abc", Password = "123" };
            //var group1 = new UserGroup() { ID = "1", GroupName = "Kiểm định viên" };

            //var permission1 = new Permission() { ID = "1", Name = "createContract", Description = "Khởi tạo hợp đồng" };

            //group1.Permissions.Add(permission1);
            //user1.UserGroups.Add(group1);

            var employeePosition1 = new EmployeePosition() { Id = 1, Name = "Giám đốc" };
            var employeePosition2 = new EmployeePosition() { Id = 2, Name = "Trưởng phòng" };
            var employeePosition3 = new EmployeePosition() { Id = 3, Name = "Kiểm định viên" };

            var user1 = new AppUser() { Id = 11, UserName = "tieutuan@mail.com", Email = "tieutuan@mail.com", DisplayName = "Phạm Minh Tuấn" };
            var user2 = new AppUser() { Id = 12, UserName = "vietanh@mail.com", Email = "vietanh@mail.com", DisplayName = "Nguyễn Việt Anh" };

            role = new ApplicationIdentityRole { Name = "KDV" };
            //role.Permissions.Add(perm1);
            applicationRoleManager.Create(role);

            //perm1.Roles.Add(role.ToApplicationRole());

            var appuser = user1.ToApplicationUser();
            var appuser2 = user2.ToApplicationUser();
            applicationUserManager.Create(appuser, password);
            applicationUserManager.SetLockoutEnabled(appuser.Id, false);
            applicationUserManager.Create(appuser2, password);
            applicationUserManager.SetLockoutEnabled(appuser2.Id, false);

            applicationUserManager.AddToRole(appuser.Id, role.Name);
            applicationUserManager.AddToRole(appuser2.Id, role.Name);

            //context.Set<AppUser>().Add(user1);
            //context.Set<AppUser>().Add(user2);
            user1 = appuser.ToAppUser();
            user2 = appuser2.ToAppUser();
            var custommer1 = new Customer()
            {
                Id = 1,
                Name = "Công ty TNHH công trình xây dựng Lỗi Lợi",
                Address = "Thên Đè E, Xã Lê Lợi, Huyện Hoành Bồ, Tỉnh Quảng Ninh",
                Representative = "Trần Hải Thuật",
                RepresentativePosition = "Giám đốc",
                Phone = "04.38833584",
                Fax = "04.38835465",
                AccountNumber = "3140211000020",
                BankName = "NH NN PTNN Quảng Ninh",
                TaxID = "0101412313"
            };

            var custommer2 = new Customer()
            {
                Id = 2,
                Name = "Công ty CP Đầu tư và Phát triển nhà Hà Nội",
                Address = "Số 10 - Hoa Lư -  Hai Bà Trưng - Hà Nội",
                Representative = "Nguyễn Hải Nam",
                RepresentativePosition = "Giám đốc",
                Phone = "04.38836547",
                Fax = "04.38836248",
                AccountNumber = "120040211000020",
                BankName = "Ngân hàng BIDV - Chi nhánh Sở giao dịch I",
                TaxID = "0101416789"
            };

            //var contract1 = new Contract()
            //{
            //    Id = 1,
            //    MaHD = "3778/HĐKĐ",
            //    Name = "Hợp đồng kiểm định kỹ thuật",
            //    CreateDate = new DateTime(2016, 09, 12),
            //    customer = custommer1,
            //    Tasks = new List<AccTask>()
            //        {
            //            new AccTask()
            //            {
            //                Id=1,
            //                Name="Kiểm định máy nén khí (bình nén)",
            //                Unit = "T.bị",
            //                //Amount=3,
            //                UnitPrice=400000,
            //                Accreditations= new List<Accreditation>() { accreditaion1/*, accreditaion2*/ },
            //            },
            //            //new AccTask()
            //            //{
            //            //    Id=2,
            //            //    Name="Kiển định tời nâng các loại",
            //            //    Unit = "T.bị",
            //            //    //Amount=3,
            //            //    UnitPrice=550000,
            //            //    Accreditations= new List<Accreditation>() { accreditaion3 },
            //            //},
            //        },

            //    ResponsibilityA = "Cung cấp cho bên B các tài liệu kỹ thuật, thông tin cần thiết liên quan đến việc kiểm định.",
            //    ResponsibilityB = "Cung cấp cho bên A các quy định và quy trình kiểm tra",
            //    PaymentMethod = "Bên A thanh toán một lần sau khi hợp đồng kết thúc hoặc bên sau khi bên B thực hiện việc kiểm định thiết bị cho bên A chậm nhất 15 ngày...",
            //    Commitments = "Hai bên cam kết thực hiện đầy đủ nghĩa vụ của mình nêu trong hợp đồng.",
            //    Effective = "Hợp đồng có hiệu lực từ ngày hai bên ký hợp đồng.",
            //    Status = ApproveStatus.Waiting,
            //    own = user1
            //};

            //var contract2 = new Contract()
            //{
            //    Id = 2,
            //    MaHD = "3779/HĐKĐ",
            //    Name = "Hợp đồng kiểm định kỹ thuật 2",
            //    CreateDate = new DateTime(2016, 12, 14),
            //    customer = custommer2,
            //    Tasks = new List<AccTask>()
            //        {
            //            new AccTask()
            //            {
            //                Id=3,
            //                Name="Kiểm định máy nén khí (bình nén)",
            //                Unit = "T.bị",
            //                //Amount=2,
            //                UnitPrice=400000,
            //                Accreditations= new List<Accreditation>() { /*accreditaion1,*/ accreditaion2 },
            //            },
            //            //new AccTask()
            //            //{
            //            //    Id=4,
            //            //    Name="Kiển định tời nâng các loại",
            //            //    Unit = "T.bị",
            //            //    //Amount=5,
            //            //    UnitPrice=550000,
            //            //    Accreditations= new List<Accreditation>() { accreditaion3 },
            //            //},
            //        },

            //    ResponsibilityA = "Cung cấp cho bên B các tài liệu kỹ thuật, thông tin cần thiết liên quan đến việc kiểm định.",
            //    ResponsibilityB = "Cung cấp cho bên A các quy định và quy trình kiểm tra",
            //    PaymentMethod = "Bên A thanh toán một lần sau khi hợp đồng kết thúc hoặc bên sau khi bên B thực hiện việc kiểm định thiết bị cho bên A chậm nhất 15 ngày...",
            //    Commitments = "Hai bên cam kết thực hiện đầy đủ nghĩa vụ của mình nêu trong hợp đồng.",
            //    Effective = "Hợp đồng có hiệu lực từ ngày hai bên ký hợp đồng.",
            //    Status = ApproveStatus.ApprovedLv2,
            //    own = user1
            //};

            //var contract3 = new Contract()
            //{
            //    Id = 3,
            //    MaHD = "3780/HĐKĐ",
            //    Name = "Hợp đồng kiểm định kỹ thuật 3",
            //    CreateDate = new DateTime(2016, 12, 20),
            //    customer = custommer2,
            //    Tasks = new List<AccTask>()
            //        {
            //            //new AccTask()
            //            //{
            //            //    Id=5,
            //            //    Name="Kiểm định máy nén khí (bình nén)",
            //            //    Unit = "T.bị",
            //            //    //Amount=1,
            //            //    UnitPrice=400000,
            //            //    Accreditations= new List<Accreditation>() { accreditaion1, accreditaion2 },
            //            //},
            //            new AccTask()
            //            {
            //                Id=6,
            //                Name="Kiển định tời nâng các loại",
            //                Unit = "T.bị",
            //                //Amount=1,
            //                UnitPrice=550000,
            //                Accreditations= new List<Accreditation>() { accreditaion3 },
            //            },
            //        },

            //    ResponsibilityA = "Cung cấp cho bên B các tài liệu kỹ thuật, thông tin cần thiết liên quan đến việc kiểm định.",
            //    ResponsibilityB = "Cung cấp cho bên A các quy định và quy trình kiểm tra",
            //    PaymentMethod = "Bên A thanh toán một lần sau khi hợp đồng kết thúc hoặc bên sau khi bên B thực hiện việc kiểm định thiết bị cho bên A chậm nhất 15 ngày...",
            //    Commitments = "Hai bên cam kết thực hiện đầy đủ nghĩa vụ của mình nêu trong hợp đồng.",
            //    Effective = "Hợp đồng có hiệu lực từ ngày hai bên ký hợp đồng.",
            //    Status = ApproveStatus.ApprovedLv1,
            //    own = user2
            //};

            //equiment1.contract = contract1;
            //equiment2.contract = contract2;
            //equiment3.contract = contract3;

            context.Set<AppPermission>().Add(perm1);
            context.Set<AppPermission>().Add(perm2);

            context.Set<EmployeePosition>().Add(employeePosition1);
            context.Set<EmployeePosition>().Add(employeePosition2);
            context.Set<EmployeePosition>().Add(employeePosition3);
            context.Set<Customer>().Add(custommer1);
            context.Set<Customer>().Add(custommer2);
            //context.Set<Equipment>().Add(equiment1);
            //context.Set<Equipment>().Add(equiment2);
            //context.Set<Equipment>().Add(equiment3);
            //context.Set<Accreditation>().Add(accreditaion1);
            //context.Set<Accreditation>().Add(accreditaion2);
            //context.Set<Accreditation>().Add(accreditaion3);
            //var abc = context.Set<AccTask>();

            //contract1.Tasks.ForEach((x) => { abc.Add(x); });
            //context.Set<Contract>().Add(contract1);
            //context.Set<Contract>().Add(contract2);
            //context.Set<Contract>().Add(contract3);

            //Notification

            //Notification notifi = new Notification();
            ////notifi.owner = user.ToAppUser();
            //notifi.targetUsers.Add(user2);
            //notifi.targetUsers.Add(user1);
            //notifi.SentTime = DateTime.Now;
            //notifi.content = "abcxyz";
            //notifi.target_url = "abcxyz";

            //context.Set<Notification>().Add(notifi);


            context.SaveChanges();
        }
        class DebugLogger : ILogger
        {
            public void Log(string message)
            {

            }

            public void Log(Exception ex)
            {

            }
        }
    }
}