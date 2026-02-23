using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IncosafCMS.DataTool
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            SplashScreenManager.ShowForm(typeof(DemoWaitForm));
            SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu xuống");
            SplashScreenManager.Default.SetWaitFormDescription("Đang tải...");
            GridInit.Instance.DepartmentsGridInit(spDepartments);
            GridInit.Instance.EmployeesGridInit(spEmployees);
            GridInit.Instance.CustomersGridInit(spCustomers);
            GridInit.Instance.EquipmentsGridInit(spEquipments);
            GridInit.Instance.ContractsGridInit(spContracts);
            SplashScreenManager.CloseForm();

        }
        private void btnCusOpenXls_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != "")
            {
                var result = CustomerDataTool.Instance.ImportToDatabase<Customer>(ofd.FileName);
                if (result == null || result?.Count < 1) return;
                var rows = spCustomers.ActiveWorksheet.Rows;
                var curPos = spCustomers.ActiveWorksheet.GetUsedRange().BottomRowIndex;
                var fIndex = curPos + 1;
                spCustomers.BeginUpdate();
                foreach (var cus in result)
                {
                    curPos++;
                    rows[curPos][2].Value = cus.MaKH;
                    rows[curPos][3].Value = cus.Name;
                    rows[curPos][4].Value = cus.Address;
                    rows[curPos][5].Value = cus.Phone;
                    rows[curPos][6].Value = cus.Fax;
                    rows[curPos][7].Value = cus.TaxID;
                    rows[curPos][8].Value = cus.AccountNumber;
                    rows[curPos][9].Value = cus.BankName;
                    rows[curPos][10].Value = cus.Representative;
                    rows[curPos][11].Value = cus.RepresentativePosition;
                }
                var range = spCustomers.ActiveWorksheet.Range.FromLTRB(2, fIndex, 11, curPos);
                range.Font.Color = Color.Red;
                spCustomers.EndUpdate();
            }
        }
        private void btnCusImport_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(DemoWaitForm));
            SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu lên");
            SplashScreenManager.Default.SetWaitFormDescription("Đang tải...");
            CustomerDataTool.Instance.Commit();
            GridInit.Instance.CustomersGridInit(spCustomers);
            SplashScreenManager.CloseForm();
        }
        private void btnOpenFileDepartment_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != "")
            {
                var result = DepartmentDataTool.Instance.ImportToDatabase<Department>(ofd.FileName);
                if (result == null || result?.Count < 1) return;
                var rows = spDepartments.ActiveWorksheet.Rows;
                var curPos = spDepartments.ActiveWorksheet.GetUsedRange().BottomRowIndex;
                var fIndex = curPos + 1;
                spDepartments.BeginUpdate();
                foreach (var item in result)
                {
                    curPos++;
                    rows[curPos][2].Value = item.MaDV;
                    rows[curPos][3].Value = item.Name;
                    rows[curPos][4].Value = item.Phone;
                    rows[curPos][5].Value = item.Email;
                }
                var range = spDepartments.ActiveWorksheet.Range.FromLTRB(2, fIndex, 11, curPos);
                range.Font.Color = Color.Red;
                spDepartments.EndUpdate();
            }
        }
        private void btnImportDepartment_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(DemoWaitForm));
            SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu lên");
            SplashScreenManager.Default.SetWaitFormDescription("Đang tải...");
            DepartmentDataTool.Instance.Commit();
            GridInit.Instance.DepartmentsGridInit(spDepartments);
            SplashScreenManager.CloseForm();
        }
        private void btnOpenXlsEquip_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != "")
            {
                var result = EquipmentDataTool.Instance.ImportToDatabase<Equipment>(ofd.FileName);
                if (result == null || result?.Count < 1) return;
                var rows = spEquipments.ActiveWorksheet.Rows;
                var curPos = spEquipments.ActiveWorksheet.GetUsedRange().BottomRowIndex;
                var fIndex = curPos + 1;
                spEquipments.BeginUpdate();
                foreach (var item in result)
                {
                    curPos++;
                    rows[curPos][2].Value = item.Code;
                    rows[curPos][3].Value = item.Name;
                    rows[curPos][4].Value = item.ManuFacturer;
                    rows[curPos][5].Value = item.YearOfProduction;
                    rows[curPos][6].Value = item.No;
                    rows[curPos][7].Value = item.Uses;
                }
                var range = spEquipments.ActiveWorksheet.Range.FromLTRB(2, fIndex, 11, curPos);
                range.Font.Color = Color.Red;
                spEquipments.EndUpdate();
            }
        }
        private void btnImportEquip_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(DemoWaitForm));
            SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu lên");
            SplashScreenManager.Default.SetWaitFormDescription("Đang tải...");
            EquipmentDataTool.Instance.Commit();
            GridInit.Instance.EquipmentsGridInit(spEquipments);
            SplashScreenManager.CloseForm();
        }
        private void btnOpenXlsContracts_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != "")
            {
                var result = ContractDataTool.Instance.ImportToDatabase<Contract>(ofd.FileName);
                if (result == null || result?.Count < 1) return;
                var rows = spContracts.ActiveWorksheet.Rows;
                var curPos = spContracts.ActiveWorksheet.GetUsedRange().BottomRowIndex;
                var fIndex = curPos + 1;
                spContracts.BeginUpdate();
                foreach (var item in result)
                {
                    curPos++;
                    rows[curPos][2].Value = item.MaHD;
                    rows[curPos][4].Value = item.SignDate;
                    rows[curPos][5].Value = item.customer.Name;
                    rows[curPos][6].Value = item.Name;
                    rows[curPos][7].Value = item.Value;

                    var turnRow = curPos;
                    for (int i = 0; i < item.TurnOvers.Count; i++)
                    {
                        var turn = item.TurnOvers[i];
                        rows[turnRow][8].Value = turn.TurnOverDate;
                        rows[turnRow][9].Value = turn.HDNumber;
                        rows[turnRow][10].Value = turn.HDValue;
                        rows[turnRow][11].Value = turn.VAT;
                        turnRow++;
                    }

                    var payRow = curPos;
                    for (int i = 0; i < item.Payments.Count; i++)
                    {
                        var pay = item.Payments[i];
                        rows[payRow][12].Value = pay.PaymentDate;
                        rows[payRow][13].Value = pay.PaymentNumber;
                        rows[payRow][14].Value = pay.PaymentValue;
                        payRow++;
                    }

                    rows[curPos][15].Value = item.RatioOfCompany;
                    rows[curPos][16].Value = item.RatioOfInternal;
                    rows[curPos][18].Value = item.own?.Tags;

                    var lrow = turnRow > payRow ? turnRow : payRow;
                    if (lrow > curPos) curPos = lrow - 1;
                }
                var range = spContracts.ActiveWorksheet.Range.FromLTRB(2, fIndex, 11, curPos);
                range.Font.Color = Color.Red;
                spContracts.EndUpdate();
            }
        }
        private void btnImportContracts_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(DemoWaitForm));
            SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu lên");
            SplashScreenManager.Default.SetWaitFormDescription("Đang tải...");
            ContractDataTool.Instance.Commit();
            GridInit.Instance.ContractsGridInit(spContracts);
            SplashScreenManager.CloseForm();
        }
        private void btnOpenEmployeeXls_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != "")
            {
                var result = EmployeeDataTool.Instance.ImportToDatabase<AppUser>(ofd.FileName);
                if (result == null || result?.Count < 1) return;
                var rows = spEmployees.ActiveWorksheet.Rows;
                var curPos = spEmployees.ActiveWorksheet.GetUsedRange().BottomRowIndex;
                var fIndex = curPos + 1;
                spEmployees.BeginUpdate();
                foreach (var item in result)
                {
                    curPos++;
                    rows[curPos][2].Value = item.MaNV;
                    rows[curPos][3].Value = item.DisplayName;
                    rows[curPos][4].Value = item.Address;
                    rows[curPos][5].Value = item.PhoneNumber;
                    rows[curPos][6].Value = item.Email;
                    rows[curPos][7].Value = item.Department.MaDV;
                    rows[curPos][8].Value = item.Position.Name;
                }
                var range = spEmployees.ActiveWorksheet.Range.FromLTRB(2, fIndex, 11, curPos);
                range.Font.Color = Color.Red;
                spEmployees.EndUpdate();
            }
        }
        private void btnImportEmployees_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(DemoWaitForm));
            SplashScreenManager.Default.SetWaitFormCaption("Đang tải dữ liệu lên");
            SplashScreenManager.Default.SetWaitFormDescription("Đang tải...");
            EmployeeDataTool.Instance.Commit();
            GridInit.Instance.EmployeesGridInit(spEmployees);
            SplashScreenManager.CloseForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var contracts = ContractDataTool.Instance.GetAll<Contract>();
            foreach (var contract in contracts)
            {
                contract.CalValue();
                contract.CalTongTienXuatHoaDon();
                contract.CalTongTienVe();
                contract.CalTienTruThue();
                contract.customer = contract.customer;
                ContractDataTool.Instance.Update(contract);
            }
        }
    }
}
