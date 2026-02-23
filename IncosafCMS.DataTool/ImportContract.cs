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
    public partial class ImportContract : Form
    {
        private string exelPath;
        public List<Contract> contracts;
        public ImportContract()
        {
            InitializeComponent();
        }

        public ImportContract(string exelPath)
        {
            InitializeComponent();
            this.exelPath = exelPath;
            spImportContract.LoadDocument(exelPath);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var rows = spImportContract.ActiveWorksheet.Rows;
            contracts = new List<Contract>();
            for (int i = 4; i < this.spImportContract.ActiveWorksheet.Rows.LastUsedIndex + 1; i++)
            {
                var contract = new Contract()
                {
                    MaHD = rows[i][2].DisplayText,
                    IsGiayDeNghi = rows[i][2].DisplayText.Trim() == "N",
                    SignDate = rows[i][4].Value.DateTimeValue,
                    //customerId = 0,
                    customer = new Customer() { Name = rows[i][5].Value.TextValue },
                    Name = rows[i][6].Value.TextValue,
                    Value = rows[i][7].Value.NumericValue,
                    //Tasks = new List<AccTask>() { new AccTask() {
                    //    Name = rows[i][6].Value.TextValue,
                    //    UnitPrice = rows[i][9].Value.NumericValue,
                    //    SoLuong = 1
                    //} },
                    TurnOvers = new List<TurnOver>() { new TurnOver() {
                        TurnOverDate = rows[i][8].Value.DateTimeValue,
                        HDNumber = rows[i][9].Value.TextValue,
                        HDValue = rows[i][10].Value.NumericValue,
                        VAT = rows[i][11].Value.NumericValue * 100,
                    } },
                    Payments = new List<Payment>() { new Payment()
                    {
                        PaymentDate = rows[i][12].Value.DateTimeValue,
                        PaymentNumber = rows[i][13].Value.TextValue,
                        PaymentValue = rows[i][14].Value.NumericValue > 0? rows[i][14].Value.NumericValue: 0 /*rows[i][15].Value.NumericValue*/,
                        PaymentMethod =
                        rows[i][14].Value.NumericValue > 0 ? PaymentMethod.directly : PaymentMethod.transfer,
                    } },

                    RatioOfCompany = rows[i][15].Value.NumericValue * 100,
                    RatioOfInternal = rows[i][16].Value.NumericValue * 100,
                    own = new Core.DomainModels.Identity.AppUser()
                    {
                        Tags = rows[i][18].DisplayText
                    },
                };

                contract.TurnOvers.First().Payments.AddRange(contract.Payments);
                foreach (var payment in contract.Payments)
                {
                    var turnover = contract.TurnOvers.First();
                    if (!string.IsNullOrEmpty(turnover.HDNumber) || turnover.HDValue > 0)
                        payment.turnOver = turnover;
                    else payment.turnOver = null;
                }

                if (!string.IsNullOrWhiteSpace(contract.MaHD)) contract.Status = ApproveStatus.ApprovedLv2;
                var sameContract = contracts.FirstOrDefault(
                    xe => !string.IsNullOrWhiteSpace(xe.MaHD) && xe.MaHD == contract.MaHD &&
                    xe.SignDate == contract.SignDate);

                if (sameContract != null)
                {
                    foreach (var turn in contract.TurnOvers)
                    {
                        var sameTurnOver = sameContract.TurnOvers.FirstOrDefault(x => x.HDNumber == turn.HDNumber);
                        if (sameTurnOver == null && (!string.IsNullOrWhiteSpace(turn.HDNumber) || turn.HDValue > 0))
                            sameContract.TurnOvers.Add(turn);
                        else if (!string.IsNullOrWhiteSpace(turn.HDNumber) && turn.HDValue > 0)
                            sameContract.TurnOvers.Add(turn);
                    }
                    foreach (var pay in contract.Payments)
                    {
                        if (pay.PaymentValue > 0 || !string.IsNullOrWhiteSpace(pay.PaymentNumber))
                            sameContract.Payments.Add(pay);
                    }
                }
                else
                {
                    contract.TurnOvers.ForEach(xe => xe.Payments.RemoveAll(ee => string.IsNullOrWhiteSpace(ee.PaymentNumber) && ee.PaymentValue == 0));
                    contract.TurnOvers.RemoveAll(xe => string.IsNullOrWhiteSpace(xe.HDNumber) && xe.HDValue == 0);
                    contract.Payments.RemoveAll(xe => string.IsNullOrWhiteSpace(xe.PaymentNumber) && xe.PaymentValue == 0);
                    contracts.Add(contract);
                }

                contract.Finished = !string.IsNullOrWhiteSpace(rows[i][19].DisplayText) && rows[i][19].DisplayText.ToLower() == "hđ kết thúc";
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            contracts = null;
            this.Close();
        }
    }
}
