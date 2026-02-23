using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IncosafCMS.Core.DomainModels.Identity;

namespace IncosafCMS.DataTool
{
    public partial class ImportEmployee : Form
    {
        private string exelPath;
        internal List<AppUser> employees;

        public ImportEmployee()
        {
            InitializeComponent();
        }

        public ImportEmployee(string exelPath)
        {
            InitializeComponent();
            this.exelPath = exelPath;
            spImportEmployee.LoadDocument(exelPath);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var rows = spImportEmployee.ActiveWorksheet.Rows;
            employees = new List<AppUser>();
            for (int i = 3; i < this.spImportEmployee.ActiveWorksheet.Rows.LastUsedIndex; i++)
            {
                var dep = new AppUser()
                {
                    MaNV = rows[i][2].DisplayText,
                    DisplayName = rows[i][3].Value.TextValue,
                    Address = rows[i][4].Value.TextValue,
                    PhoneNumber = rows[i][5].Value.TextValue,
                    Email = rows[i][6].Value.TextValue,
                    Department = new Core.DomainModels.Department()
                    {
                        MaDV = rows[i][7].Value.TextValue
                    },
                    Position = new Core.DomainModels.EmployeePosition()
                    {
                        Name = rows[i][8].Value.TextValue
                    },
                };
                employees.Add(dep);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.employees = null;
            this.Close();
        }
    }
}
