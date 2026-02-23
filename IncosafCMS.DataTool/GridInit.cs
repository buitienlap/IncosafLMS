using DevExpress.XtraSpreadsheet;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.DataTool
{
    class GridInit
    {
        private static GridInit instance;
        private string DEPART_TEMP_FILE_PATH;
        private string EMP_TEMP_FILE_PATH;
        private string CUST_TEMP_FILE_PATH;
        private string EQUIP_TEMP_FILE_PATH;
        private string CONTRACT_TEMP_FILE_PATH;
        public static GridInit Instance{
            get
            {
                if (instance == null) instance = new GridInit();
                return instance;
            }
        }
        GridInit()
        {
            DEPART_TEMP_FILE_PATH = "Temp/PhongBan.xlsx";
            EMP_TEMP_FILE_PATH = "Temp/NhanVien.xlsx";
            CUST_TEMP_FILE_PATH = "Temp/KhachHang.xlsx";
            EQUIP_TEMP_FILE_PATH = "Temp/ThietBi.xlsx";
            CONTRACT_TEMP_FILE_PATH = "Temp/HopDong.xlsx";
        }

        public void DepartmentsGridInit(SpreadsheetControl sp, bool getAllFromServ = true)
        {
            sp.LoadDocument(DEPART_TEMP_FILE_PATH);
            if (getAllFromServ)
            {
                var departments = DepartmentDataTool.Instance.GetAll<Department>();
                var row = 3;
                for (int i = 0; i < departments.Count; i++)
                {
                    var dep = departments[i];
                    row = i + 3;
                    sp.ActiveWorksheet.Cells[row, 1].Value = dep.Id;
                    sp.ActiveWorksheet.Cells[row, 2].Value = dep.MaDV;
                    sp.ActiveWorksheet.Cells[row, 3].Value = dep.Name;
                    sp.ActiveWorksheet.Cells[row, 4].Value = dep.Phone;
                    sp.ActiveWorksheet.Cells[row, 5].Value = dep.Email;
                }
            }
        }
        public void EquipmentsGridInit(SpreadsheetControl sp, bool getAllFromServ = true)
        {
            sp.LoadDocument(EQUIP_TEMP_FILE_PATH);
            if (getAllFromServ)
            {
                var row = 0;
                var equipments = EquipmentDataTool.Instance.GetAll<Equipment>();
                for (int i = 0; i < equipments.Count; i++)
                {
                    row = i + 3;
                    sp.ActiveWorksheet.Cells[row, 1].Value = equipments[i].Id;
                    sp.ActiveWorksheet.Cells[row, 2].Value = equipments[i].Code;
                    sp.ActiveWorksheet.Cells[row, 3].Value = equipments[i].Name;
                    sp.ActiveWorksheet.Cells[row, 4].Value = equipments[i].ManuFacturer;
                    sp.ActiveWorksheet.Cells[row, 5].Value = equipments[i].YearOfProduction;
                    sp.ActiveWorksheet.Cells[row, 6].Value = equipments[i].No;
                    sp.ActiveWorksheet.Cells[row, 7].Value = equipments[i].Uses;
                }
            }
        }
        public void CustomersGridInit(SpreadsheetControl sp, bool getAllFromServ = false)
        {
            sp.LoadDocument(CUST_TEMP_FILE_PATH);
            if (getAllFromServ)
            {
                var customers = CustomerDataTool.Instance.GetAll<Customer>();
                var row = 0;
                sp.BeginUpdate();
                for (int i = 0; i < customers.Count; i++)
                {
                    var cus = customers[i];
                    row = i + 3;
                    sp.ActiveWorksheet.Cells[row, 1].Value = cus.Id;
                    sp.ActiveWorksheet.Cells[row, 2].Value = cus.MaKH;
                    sp.ActiveWorksheet.Cells[row, 3].Value = cus.Name;
                    sp.ActiveWorksheet.Cells[row, 4].Value = cus.Address;
                    sp.ActiveWorksheet.Cells[row, 5].Value = cus.Phone;
                    sp.ActiveWorksheet.Cells[row, 6].Value = cus.Fax;
                    sp.ActiveWorksheet.Cells[row, 7].Value = cus.TaxID;
                    sp.ActiveWorksheet.Cells[row, 8].Value = cus.AccountNumber;
                    sp.ActiveWorksheet.Cells[row, 9].Value = cus.BankName;
                    sp.ActiveWorksheet.Cells[row, 10].Value =cus.Representative;
                    sp.ActiveWorksheet.Cells[row, 11].Value =cus.RepresentativePosition;
                    if (customers[i].department != null)
                        sp.ActiveWorksheet.Cells[i, 12].Value = customers[i].department.MaDV;
                }
                sp.EndUpdate();
            }
        }
        public void ContractsGridInit(SpreadsheetControl sp, bool getAllFromServ = false)
        {
            sp.LoadDocument(CONTRACT_TEMP_FILE_PATH);
            if (getAllFromServ)
            {
                var row = 0;
                sp.BeginUpdate();
                var contracts = ContractDataTool.Instance.GetAll<Contract>();
                row = row + 4;
                for (int i = 0; i < contracts.Count; i++)
                {
                    var contract = contracts[i];
                    sp.ActiveWorksheet.Cells[row, 1].Value = contract.Id;
                    sp.ActiveWorksheet.Cells[row, 4].Value = contract.GetSignDate;
                    sp.ActiveWorksheet.Cells[row, 5].Value = contract.customer.Name;
                    sp.ActiveWorksheet.Cells[row, 6].Value = contract.Name;
                    sp.ActiveWorksheet.Cells[row, 7].Value = contract.Value;
                    sp.ActiveWorksheet.Cells[row, 15].Value = contract.RatioOfCompany;
                    sp.ActiveWorksheet.Cells[row, 16].Value = contract.RatioOfInternal;
                    if (contract.own != null)
                        sp.ActiveWorksheet.Cells[row, 17].Value = contract.own.DisplayName;

                    var turnRow = row;
                    for (int j = 0; j < contract.TurnOvers.Count; j++)
                    {
                        var turn = contract.TurnOvers[j];
                        sp.ActiveWorksheet.Cells[turnRow, 8].Value = turn.TurnOverDate;
                        sp.ActiveWorksheet.Cells[turnRow, 9].Value = turn.HDNumber;
                        sp.ActiveWorksheet.Cells[turnRow, 10].Value = turn.HDValue;
                        sp.ActiveWorksheet.Cells[turnRow, 11].Value = turn.VAT;
                        turnRow++;
                    }

                    var payRow = row;
                    for (int j = 0; j < contract.Payments.Count; j++)
                    {
                        var pay = contract.Payments[j];
                        sp.ActiveWorksheet.Cells[payRow, 12].Value = pay.PaymentDate;
                        sp.ActiveWorksheet.Cells[payRow, 13].Value = pay.PaymentNumber;
                        sp.ActiveWorksheet.Cells[payRow, 14].Value = pay.PaymentValue;
                        payRow++;
                    }

                    row = payRow > turnRow? payRow : turnRow;
                    row++;
                }
                sp.EndUpdate();
            }
        }

        internal void EmployeesGridInit(SpreadsheetControl sp, bool getAllFromServ = true)
        {
            sp.LoadDocument(EMP_TEMP_FILE_PATH);
            if (getAllFromServ)
            {
                var employees = EmployeeDataTool.Instance.GetAll<AppUser>();
                var row = 0;
                sp.BeginUpdate();
                for (int i = 0; i < employees.Count; i++)
                {
                    var emp = employees[i];
                    row = i + 3;
                    sp.ActiveWorksheet.Cells[row, 1].Value = emp.Id;
                    sp.ActiveWorksheet.Cells[row, 2].Value = emp.MaNV;
                    sp.ActiveWorksheet.Cells[row, 3].Value = emp.DisplayName;
                    sp.ActiveWorksheet.Cells[row, 4].Value = emp.Address;
                    sp.ActiveWorksheet.Cells[row, 5].Value = emp.PhoneNumber;
                    sp.ActiveWorksheet.Cells[row, 6].Value = emp.Email;
                    sp.ActiveWorksheet.Cells[row, 7].Value = emp.Department?.MaDV;
                    sp.ActiveWorksheet.Cells[row, 8].Value = emp.Position?.Name;
                }
                sp.EndUpdate();
            }
        }
    }
}
