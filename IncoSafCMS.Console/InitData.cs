using IncoSafCMS.Core;
using IncoSafCMS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Console
{
    public class InitData
    {
        public InitData()
        {
            using (var db = new IncoSafCMSContext())
            {
                var user1 = new User() { UserName = "abc", Password = "123" };
                var group1 = new UserGroup() { ID = "1", GroupName = "Kiểm định viên" };

                var permission1 = new Permission() { ID = "1", Name = "createContract", Description = "Khởi tạo hợp đồng" };

                group1.Permissions.Add(permission1);
                user1.UserGroups.Add(group1);


                var custommer1 = new Custommer()
                {
                    ID = "1",
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

                List<EquimentPartion> partions1 = new List<EquimentPartion>()
                {
                    new EquimentPartion()
                    {
                        ID = "1",
                        Name="Móc (hoặc bàn nâng)",
                        Passed1 = true,
                        Passed2 = true,
                        Note = ""
                    },
                    new EquimentPartion()
                    {
                        ID = "2",
                        Name="Tang",
                        Passed1 = true,
                        Passed2 = true,
                        Note = ""
                    },
                    new EquimentPartion()
                    {
                        ID = "3",
                        Name="Cáp",
                        Passed1 = true,
                        Passed2 = true,
                        Note = ""
                    },
                    new EquimentPartion()
                    {
                        ID = "4",
                        Name="Pu li dẫn hương",
                        Passed1 = true,
                        Passed2 = true,
                        Note = ""
                    }
                };

                List<Specifications> specifications1 = new List<Specifications>()
                {
                    new Specifications()
                    {
                        ID="1",
                        Name ="Tải trọng thiết kế",
                        Value="5.0 Tấn"
                    },
                    new Specifications()
                    {
                        ID="2",
                        Name ="Tải trọng thực tế",
                        Value="1.5 Tấn"
                    },
                    new Specifications()
                    {
                        ID="3",
                        Name ="Vận tốc nâng",
                        Value="20.0 m/ph"
                    },
                    new Specifications()
                    {
                        ID="4",
                        Name ="Chiều cao nâng",
                        Value="146.5 m"
                    }
                };
                List<TechnicalDocument> technicaldocuments1 = new List<Core.TechnicalDocument>()
                {
                   new Core.TechnicalDocument()
                   {
                       ID = "1",
                       Name="Lý lịch máy trục",
                       Passed = true,
                       Note=""
                   },
                   new TechnicalDocument()
                   {
                       ID = "2",
                       Name="Hồ sơ móng",
                       Passed = true,
                       Note=""
                   }
                };

                var equiment1 = new Equiment()
                {
                    ID = "1",
                    Name = "Tời nâng",
                    Code = "JM5",
                    ManuFacturer = "Trung Quốc",
                    No = "No1",
                    YearOfProduction = "2015",
                    Partions = partions1,
                    specifications = specifications1,
                    TechnicalDocuments = technicaldocuments1
                };
                var procedure = new Procedure()
                {
                    Name = "TCVN 424:2005",
                    Description = ""
                };
                var standard = new Standard()
                {
                    Name = "23-2014/BLĐTBXH",
                    Description = ""
                };
                var accreditaion1 = new Accreditation()
                {
                    Number = "18560/KĐXD-TBN",
                    Date = new DateTime(2016, 09, 22),
                    equiment = equiment1,
                    custommer = custommer1,
                    Location = "Ống khói nhà máy nhiệt điện Thăng Long",
                    EmployedStandard = standard,
                    EmployedProcedure = procedure,
                    Tester = new List<User>() { user1 },
                    Type = "Lần đầu",
                    CorrespondingLoad = 1.5,
                    StaticLoad = 1.875,
                    DynamicLoad = 1.65,
                    StampNumber = "18559",
                    StampLocated = "Tủ điện điều khiển",
                    Requests = "Tuân thủ các quy phạm an toàn, các quy trình hướng dẫn của nhà chế tạo khi sử dụng thiết bị\nTuân thủ các biện pháp an toàn thi công trinh công trường hoặc trong nhà máy.",
                    DateOfNext = new DateTime(2017, 09, 22)
                };
                var accreditaion2 = new Accreditation()
                {
                    Number = "18566/KĐXD-TBN",
                    Date = new DateTime(2016, 09, 22),
                    equiment = equiment1,
                    custommer = custommer1,
                    Location = "Ống khói nhà máy nhiệt điện Thăng Long",
                    EmployedStandard = standard,
                    EmployedProcedure = procedure,
                    Tester = new List<User>() { user1 },
                    Type = "Lần đầu",
                    CorrespondingLoad = 1.5,
                    StaticLoad = 1.875,
                    DynamicLoad = 1.65,
                    StampNumber = "18560",
                    StampLocated = "Tủ điện điều khiển",
                    Requests = "Tuân thủ các quy phạm an toàn, các quy trình hướng dẫn của nhà chế tạo khi sử dụng thiết bị\nTuân thủ các biện pháp an toàn thi công trinh công trường hoặc trong nhà máy.",
                    DateOfNext = new DateTime(2017, 09, 22)
                };
                var accreditaion3 = new Accreditation()
                {
                    Number = "18567/KĐXD-TBN",
                    Date = new DateTime(2016, 09, 22),
                    equiment = equiment1,
                    custommer = custommer1,
                    Location = "Ống khói nhà máy nhiệt điện Thăng Long",
                    EmployedStandard = standard,
                    EmployedProcedure = procedure,
                    Tester = new List<User>() { user1 },
                    Type = "Lần đầu",
                    CorrespondingLoad = 1.5,
                    StaticLoad = 1.875,
                    DynamicLoad = 1.65,
                    StampNumber = "18561",
                    StampLocated = "Tủ điện điều khiển",
                    Requests = "Tuân thủ các quy phạm an toàn, các quy trình hướng dẫn của nhà chế tạo khi sử dụng thiết bị\nTuân thủ các biện pháp an toàn thi công trinh công trường hoặc trong nhà máy.",
                    DateOfNext = new DateTime(2017, 09, 22)
                };

                var contract1 = new Contract()
                {
                    ID = "1",
                    Date = new DateTime(2016, 09, 12),
                    custommer = custommer1,
                    Tasks = new List<Core.Task>()
                    {
                        new Core.Task()
                        {
                            ID="1",
                            Name="Kiểm định máy nén khí (bình nén)",
                            Unit = "T.bị",
                            Amount=3,
                            UnitPrice=400000,
                            Accreditations= new List<Accreditation>() { accreditaion1, accreditaion2 },
                        },
                        new Core.Task()
                        {
                            ID="2",
                            Name="Kiển định tời nâng các loại",
                            Unit = "T.bị",
                            Amount=3,
                            UnitPrice=550000,
                            Accreditations= new List<Accreditation>() { accreditaion3 },
                        },
                    },

                    ResponsibilityA = "Cung cấp cho bên B các tài liệu kỹ thuật, thông tin cần thiết liên quan đến việc kiểm định.",
                    ResponsibilityB = "Cung cấp cho bên A các quy định và quy trình kiểm tra",
                    PaymentMethod = "Bên A thanh toán một lần sau khi hợp đồng kết thúc hoặc bên sau khi bên B thực hiện việc kiểm định thiết bị cho bên A chậm nhất 15 ngày...",
                    Commitments = "Hai bên cam kết thực hiện đầy đủ nghĩa vụ của mình nêu trong hợp đồng.",
                    Effective = "Hợp đồng có hiệu lực từ ngày hai bên ký hợp đồng."
                };


                db.Users.Add(user1);
                db.Custommers.Add(custommer1);
                db.Equiments.Add(equiment1);
                db.Accreditations.AddRange(new Accreditation[] { accreditaion1, accreditaion2, accreditaion3 });

                db.Tasks.AddRange(contract1.Tasks);
                db.Contracts.Add(contract1);
                db.SaveChanges();
            }

        }
    }
}