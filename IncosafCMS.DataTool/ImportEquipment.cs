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
    public partial class ImportEquipment : Form
    {
        private string exelPath;
        public List<Equipment> equipments;

        public ImportEquipment()
        {
            InitializeComponent();
        }

        public ImportEquipment(string exelPath)
        {
            InitializeComponent();
            this.exelPath = exelPath;
            spImportToEquipment.LoadDocument(exelPath);

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var rows = spImportToEquipment.ActiveWorksheet.Rows;
            equipments = new List<Equipment>();
            for (int i = 3; i < this.spImportToEquipment.ActiveWorksheet.Rows.LastUsedIndex; i++)
            {
                var cus = new Equipment()
                {
                    Code = rows[i][2].Value.TextValue,
                    Name = rows[i][3].Value.TextValue,
                    ManuFacturer = rows[i][4].Value.TextValue,
                    YearOfProduction = rows[i][5].Value.TextValue,
                    No = rows[i][6].Value.TextValue,
                    Uses = rows[i][7].Value.TextValue
                };
                equipments.Add(cus);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            equipments = null;
            this.Close();
        }
    }
}
