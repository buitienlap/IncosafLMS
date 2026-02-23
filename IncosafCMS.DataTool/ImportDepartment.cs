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
    public partial class ImportDepartment : Form
    {
        private string exelPath;
        public List<Department> departments;
        public ImportDepartment()
        {
            InitializeComponent();
        }

        public ImportDepartment(string exelPath)
        {
            InitializeComponent();
            this.exelPath = exelPath;
            spImportToDepart.LoadDocument(exelPath);

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var rows = spImportToDepart.ActiveWorksheet.Rows;
            departments = new List<Department>();
            for (int i = 3; i < this.spImportToDepart.ActiveWorksheet.Rows.LastUsedIndex; i++)
            {
                var dep = new Department()
                {
                    MaDV = rows[i][2].Value.TextValue,
                    Name = rows[i][3].Value.TextValue,
                    Phone = rows[i][4].Value.TextValue,
                    Email = rows[i][5].Value.TextValue
                };
                departments.Add(dep);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.departments = null;
            this.Close();
        }
    }
}
