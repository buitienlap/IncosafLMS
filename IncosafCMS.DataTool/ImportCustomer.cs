using IncosafCMS.Core.DomainModels;
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
    public partial class ImportCustomer : Form
    {
        private string exelPath;
        public List<Customer> customers;

        public ImportCustomer()
        {
            InitializeComponent();
        }

        public ImportCustomer(string exelPath)
        {
            InitializeComponent();
            this.exelPath = exelPath;
            spImportCus.LoadDocument(exelPath);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var rows = spImportCus.ActiveWorksheet.Rows;
            customers = new List<Customer>();
            for (int i = 3; i < this.spImportCus.ActiveWorksheet.Rows.LastUsedIndex; i++)
            {
                var cus = new Customer()
                {
                    MaKH = rows[i][2].Value.TextValue,
                    Name = rows[i][3].Value.TextValue,
                    Address = rows[i][4].Value.TextValue,
                    Phone = rows[i][5].Value.TextValue,
                    Fax = rows[i][6].Value.TextValue,
                    TaxID = rows[i][7].Value.TextValue,
                    AccountNumber = rows[i][8].Value.TextValue,
                    BankName = rows[i][9].Value.TextValue,
                    Representative = rows[i][10].Value.TextValue,
                    RepresentativePosition = rows[i][11].Value.TextValue
                };
                var maDV = rows[i][12].Value.TextValue;
                if (!string.IsNullOrEmpty(maDV))
                    cus.department = new Department() { MaDV = rows[i][12].Value.TextValue };
                customers.Add(cus);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            customers = null;
            this.Close();
        }
    }
}
