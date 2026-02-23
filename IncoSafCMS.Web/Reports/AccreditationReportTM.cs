using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using IncosafCMS.Web.Helpers;
using IncosafCMS.Core.DomainModels;
using System.Collections.Generic;
using System.Linq;
using IncosafCMS.Web.Dto;

/// <summary>
/// Summary description for AccreditationReportTM
/// </summary>
public class AccreditationReportTM : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource;
    private PageFooterBand pageFooterBand;
    private ReportHeaderBand reportHeaderBand;
    private XRControlStyle Title;
    private XRControlStyle FieldCaption;
    private XRControlStyle PageInfo;
    private XRControlStyle DataField;
    private XRPictureBox xrPictureBoxLeft;
    private XRLabel xrLabel34;
    private XRLabel xrLabel36;
    private XRLabel xrLabel35;
    private XRLabel xrLabel38;
    private XRLabel xrLabel37;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private XRLabel xrLabel29;
    private XRLabel xrLabel32;
    private SubBand SubBand1;
    private XRTable xrTableTechnicalDocument1;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell46;
    private SubBand SubBand2;
    private XRTable xrTablePartion1;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell47;
    private XRTable xrTablePartion2;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell48;
    private SubBand SubBand3;
    private XRTable xrTableLoadTest;
    private SubBand SubBand5;
    private SubBand SubBand6;
    private XRTable xrTableSpecification;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTable xrTableDacTinhKyThuat;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
    private SubBand SubBand7;
    private XRTable xrTableTester;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell5;
    private SubBand SubBand8;
    private XRTable xrTableChungKien;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell34;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private XRTableRow xrTableRow19;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell56;
    private XRTableRow xrTableRow18;
    private XRTableCell xrTableCell58;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell55;
    private XRTableRow xrTableRow25;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell71;
    private XRTableRow xrTableRow24;
    private XRTableCell xrTableCell68;
    private XRTableCell xrTableCell69;
    private XRTableRow xrTableRow23;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private XRTableRow xrTableRow22;
    private XRTableCell xrTableCell64;
    private XRTableCell xrTableCell65;
    private XRTableRow xrTableRow21;
    private XRTableCell xrTableCell62;
    private XRTableCell xrTableCell63;
    private XRTableRow xrTableRow20;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell72;
    private XRTableCell xrTableCell73;
    private XRTableRow xrTableRow26;
    private XRTableCell xrTableCell76;
    private XRTableCell xrTableCell74;
    private SubBand SubBand9;
    private XRTable xrTable2;
    private XRTableRow xrTableRow27;
    private XRTableCell xrTableCell81;
    private XRTableCell xrTableCellTypeAcc;
    private XRTableRow xrTableRow28;
    private XRTableCell xrTableCell77;
    private XRTable xrTable3;
    private XRTableRow xrTableRow33;
    private XRTableCell xrTableCell87;
    private XRTableRow xrTableRow29;
    private XRTableCell xrTableCell78;
    private XRTableCell xrTableCell79;
    private XRTableRow xrTableRow30;
    private XRTableCell xrTableCell80;
    private XRTableRow xrTableRow35;
    private XRTableCell xrTableCell95;
    private XRTableRow xrTableRow37;
    private XRTableCell xrTableCell91;
    private SubBand SubBand10;
    private XRLabel xrLabel28;
    private XRLabel xrLabel7;
    private XRLabel xrLabel8;
    private XRLabel xrLabel9;
    private XRLabel xrLabel10;
    private XRTable xrTable1;
    private XRTableRow xrTableRow15;
    private XRTableCell xrTableCell51;
    private XRTableRow xrTableRow17;
    private XRTableCell xrTableCell52;
    private XRTableRow xrTableRow16;
    private XRTableCell xrTableCell53;
    private XRTable xrTable4;
    private XRTableRow xrTableRow39;
    private XRTableCell xrTableCell92;
    private XRTableCell xrTableCell93;
    private XRTableCell xrtAccreditationResult;
    private XRTableRow xrTableRow40;
    private XRTableCell xrTableCell116;
    private XRTableCell xrTableCell96;
    private XRTableCell xrTableCell97;
    private XRTableRow xrTableRow41;
    private XRTableCell xrTableCell117;
    private XRTableCell xrTableCell98;
    private XRTableCell xrTableCell118;
    private XRTableCell xrTableCell119;
    private XRTableCell xrTableCell99;
    private XRTableRow xrTableRow42;
    private XRTableCell xrTableCell120;
    private XRTableCell xrTableCell100;
    private XRTableRow xrTableRow44;
    private XRTableCell xrTableCell103;
    private XRTableCell xrTableCell104;
    private XRTableRow xrTableRow45;
    private XRTableCell xrTableCell107;
    private XRTableRow xrTableRow48;
    private XRTableCell xrTableCell105;
    private XRTableCell xrTableCell106;
    private XRTableRow xrTableRow49;
    private XRTableCell xrTableCell121;
    private XRTableCell xrTableCell122;
    private XRTableCell xrTableCell123;
    private XRTableRow xrTableRow38;
    private XRTableCell xrTableCell90;
    private XRTableRow xrTableRow50;
    private XRTableCell xrTableCell125;
    private XRTableRow xrTableRow51;
    private XRTableCell xrTableCell124;
    private XRTableCell xrTableCell101;
    private XRTableCell xrTableCell102;
    private XRTableCell xrTableCell108;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell109;
    private XRTableCell xrTableCell75;
    private XRTableCell xrTableCell110;
    private XRTableCell xrTableCell111;
    private XRTableCell xrTableCell112;
    private XRCheckBox cbKDLanDau;
    private XRCheckBox cbKDDinhKy;
    private XRCheckBox cbKDSauSuaChua;
    private XRCheckBox cbKDBatThuong;
    private XRCheckBox cbKDDat;
    private XRCheckBox cbKDKhongDat;
    private XRCheckBox cbKDSauLapDat;
    private SubBand SubBand11;
    private XRTable xrTable5;
    private XRTableRow xrTableRow52;
    private XRTableCell xrTableCell128;
    private XRTableRow xrTableRow53;
    private XRTableCell xrTableCell129;
    private XRTableCell xrTableCell113;
    private XRTableRow xrTableRow54;
    private XRTableCell xrTableCell131;
    private XRTableCell xrTableCell114;
    private XRTableRow xrTableRow31;
    private XRTableCell xrTableCell82;
    private XRTableCell xrTableCell115;
    private XRTableRow xrTableRow32;
    private XRTableCell xrTableCell83;
    private XRTableRow xrTableRow34;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell127;
    private XRTableRow xrTableRow43;
    private XRTableCell xrTableCell85;
    private XRTableRow xrTableRow46;
    private XRTableCell xrTableCell88;
    private XRCheckBox xrCheckBox2;
    private XRCheckBox xrCheckBox1;
    private XRTableRow xrTableRow36;
    private XRTableCell xrTableCell89;
    private XRTableCell xrTableCell126;
    private XRTableCell xrTableCell86;
    private XRTableRow xrTableRow47;
    private XRTableCell xrTableCell130;
    private XRTableCell xrTableCell132;
    private XRTableCell xrTableCell133;
    private XRTableRow xrTableRow56;
    private XRTableCell xrTableCell137;
    private XRTableCell xrTableCell138;
    private XRTableCell xrTableCell139;
    private XRTableRow xrTableRow55;
    private XRTableCell xrTableCell134;
    private XRTableCell xrTableCell135;
    private XRTableCell xrTableCell136;
    private XRTableRow xrTableRow57;
    private XRTableCell xrTableCell140;
    private XRTableRow xrTableRow58;
    private XRTableCell xrTableCell143;
    private XRTableCell xrTableCell145;
    private XRTableRow xrTableRow59;
    private XRTableCell xrTableCell141;
    private XRTableRow xrTableRow60;
    private XRTableCell xrTableCell144;
    private XRTableCell xrTableCell147;
    private XRTableCell xrTableCell148;
    private XRTableRow xrTableRow61;
    private XRTableCell xrTableCell142;
    private XRTableCell xrTableCell146;
    private XRTableCell xrTableCell149;
    private XRTableRow xrTableRow62;
    private XRTableCell xrTableCell150;
    private XRTableCell xrTableCell151;
    private XRTableCell xrTableCell152;
    private XRTableRow xrTableRow63;
    private XRTableCell xrTableCell153;
    private XRTableRow xrTableRow66;
    private XRTableCell xrTableCell162;
    private XRTableCell xrTableCell164;
    private XRTableRow xrTableRow65;
    private XRTableCell xrTableCell159;
    private XRTableRow xrTableRow64;
    private XRTableCell xrTableCell156;
    private XRTableCell xrTableCell157;
    private XRTableCell xrTableCell158;
    private XRTableRow xrTableRow67;
    private XRTableCell xrTableCell154;
    private XRTableCell xrTableCell155;
    private XRTableCell xrTableCell160;
    private XRCheckBox xrCheckBox3;
    private XRCheckBox xrCheckBox4;
    private XRCheckBox xrCheckBox5;
    private XRCheckBox xrCheckBox6;
    private XRCheckBox xrCheckBox7;
    private XRCheckBox xrCheckBox8;
    private XRCheckBox xrCheckBox9;
    private XRCheckBox xrCheckBox10;
    private XRCheckBox xrCheckBox11;
    private XRCheckBox xrCheckBox12;
    private XRCheckBox xrCheckBox13;
    private XRCheckBox xrCheckBox14;
    private XRCheckBox xrCheckBox15;
    private XRCheckBox xrCheckBox16;
    private XRCheckBox xrCheckBox17;
    private XRCheckBox xrCheckBox18;
    private XRCheckBox xrCheckBox19;
    private XRCheckBox xrCheckBox20;
    private XRTable xrTableTechnicalDocument2;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTable xrTable6;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell23;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public AccreditationReportTM()
    {
        InitializeComponent();
        objectDataSource.DataSource = ReportHelper.AccreditationReportDataSource;
        var Accreditation = (objectDataSource.DataSource as AccreditationDto);
        Accreditation.equiment = ReportHelper.AccreditationReportDataSource.equiment;

        AccTask accTask = null;

        if (Accreditation.equiment != null)
        {
            Accreditation.EmployedProcedure = ReportHelper.AccreditationReportDataSource.EmployedProcedure;
            Accreditation.EmployedStandard = ReportHelper.AccreditationReportDataSource.EmployedStandard;
            Accreditation.equiment.contract = ReportHelper.AccreditationReportDataSource.equiment?.contract;
            Accreditation.equiment.contract.customer = ReportHelper.AccreditationReportDataSource.equiment?.contract?.customer;
            Accreditation.equiment.contract.own = ReportHelper.AccreditationReportDataSource.equiment?.contract.own;
            Accreditation.equiment.specifications = ReportHelper.AccreditationReportDataSource.equiment?.specifications;
            Accreditation.equiment.Partions = ReportHelper.AccreditationReportDataSource.equiment?.Partions;
            Accreditation.equiment.TechnicalDocuments = ReportHelper.AccreditationReportDataSource.equiment?.TechnicalDocuments;

            accTask = Accreditation.equiment.contract.Tasks.FirstOrDefault(x => x.Accreditations.Select(e => e.Id == Accreditation.Id).Count() > 0);
        }

        if (accTask != null && !string.IsNullOrEmpty(accTask.AccTaskNote)) xrTableCell28.Text = "/" + accTask.AccTaskNote;

        // Danh sách kiểm định viên
        int stt = 1;
        var InsertRow = xrTableTester.Rows[1];
        if (Accreditation.Tester1 != null)
        {
            var item = Accreditation.Tester1;
            InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Italic);
            InsertRow.Cells[1].Text = stt.ToString() + ". " + item.DisplayName;

            InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[2].Text = "Số hiệu kiểm định viên số:";

            InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[3].Text = item.MaNV;

            InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[4].Text = "ĐT:";

            InsertRow.Cells[5].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[5].Text = item.PhoneNumber;

            if (Accreditation.Tester2 != null)
            {
                InsertRow = xrTableTester.InsertRowBelow(InsertRow);
                var tester2 = Accreditation.Tester2;
                InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Italic);
                InsertRow.Cells[1].Text = stt.ToString() + ". " + tester2.DisplayName;

                InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[2].Text = "Số hiệu kiểm định viên số:";

                InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[3].Text = tester2.MaNV;

                InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[4].Text = "ĐT:";

                InsertRow.Cells[5].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[5].Text = tester2.PhoneNumber;
            }

        }

        this.cbKDLanDau.Checked = Accreditation.TypeAcc == TypeOfAccr.LanDau;
        this.cbKDDinhKy.Checked = Accreditation.TypeAcc == TypeOfAccr.DinhKy;
        this.cbKDSauSuaChua.Checked = Accreditation.TypeAcc == TypeOfAccr.SauSuaChua;
        this.cbKDSauLapDat.Checked = Accreditation.TypeAcc == TypeOfAccr.SauLapDat;
        this.cbKDBatThuong.Checked = Accreditation.TypeAcc == TypeOfAccr.BatThuong;

        //this.xrTableCellTypeAcc.Text = Accreditation.TypeAcc == TypeOfAccr.LanDau ? "Lần đầu" : Accreditation.TypeAcc == TypeOfAccr.DinhKy ? "Định kỳ" : Accreditation.TypeAcc == TypeOfAccr.SauLapDat ? "Sau lắp đặt" : Accreditation.TypeAcc == TypeOfAccr.SauSuaChua ? "Sau sửa chữa" : "Bất thường";

        // Thông số kỹ thuật
        xrTableSpecification.Rows.Clear();
        if (Accreditation?.equiment?.specifications != null)
            foreach (var item in Accreditation?.equiment?.specifications)
            {
                XRTableRow row = new XRTableRow();
                var index = xrTableSpecification.Rows.Add(row);
                XRTableCell cell = new XRTableCell();
                cell.Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                var indexcell = xrTableSpecification.Rows[index].Cells.Add(cell);
                xrTableSpecification.Rows[index].Cells[indexcell].Text = "- " + item.Name + ":";

                cell = new XRTableCell();
                cell.Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Bold);
                indexcell = xrTableSpecification.Rows[index].Cells.Add(cell);
                xrTableSpecification.Rows[index].Cells[indexcell].Text = item.Value;

            }


        // Hồ sơ kỹ thuật
        stt = 1;
        InsertRow = xrTableTechnicalDocument1.Rows.FirstRow;
        if (Accreditation?.equiment?.TechnicalDocuments != null)
            foreach (var item in Accreditation?.equiment?.TechnicalDocuments)
            {
                InsertRow = xrTableTechnicalDocument1.InsertRowBelow(InsertRow);

                InsertRow.Cells[0].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                InsertRow.Cells[0].Text = stt.ToString();

                InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 0);
                InsertRow.Cells[1].Text = item.Name;

                InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                InsertRow.Cells[2].Text = (bool)item.Passed ? "√" : "";

                InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                InsertRow.Cells[3].Text = !(bool)item.Passed ? "√" : "";

                InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                InsertRow.Cells[3].Text = item.Note;

                stt++;
            }

        // Kiểm tra bên ngoài; thử không tải
        stt = 1;
        InsertRow = xrTablePartion1.Rows.FirstRow;

        var partions = Accreditation?.equiment?.Partions.Where(x => x.Passed2 == false).ToList();

        if (partions != null)
        {
            int count = (int)(partions.Count / 2);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var item = partions[i];
                    InsertRow = xrTablePartion1.InsertRowBelow(InsertRow);

                    InsertRow.Cells[0].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[0].Text = stt.ToString();

                    InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 0);
                    InsertRow.Cells[1].Text = item.Name;

                    InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[2].Text = (bool)item.Passed1 ? "√" : "";

                    InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[3].Text = !(bool)item.Passed1 ? "√" : "";

                    InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[4].Text = item.Note;

                    stt++;
                }

                InsertRow = xrTablePartion2.Rows.FirstRow;
                for (int i = count; i < partions.Count; i++)
                {
                    var item = partions[i];
                    InsertRow = xrTablePartion2.InsertRowBelow(InsertRow);

                    InsertRow.Cells[0].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[0].Text = stt.ToString();

                    InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 0);
                    InsertRow.Cells[1].Text = item.Name;

                    InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[2].Text = (bool)item.Passed1 ? "√" : "";

                    InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[3].Text = !(bool)item.Passed1 ? "√" : "";

                    InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[4].Text = item.Note;

                    stt++;
                }
            }
            else
            {
                xrTablePartion2.Rows.Clear();
                xrTablePartion1.WidthF = xrTablePartion1.WidthF * 2;
                foreach (var item in partions)
                {
                    InsertRow = xrTablePartion1.InsertRowBelow(InsertRow);

                    InsertRow.Cells[0].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[0].Text = stt.ToString();

                    InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 0);
                    InsertRow.Cells[1].Text = item.Name;

                    InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[2].Text = (bool)item.Passed1 ? "√" : "";

                    InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[3].Text = !(bool)item.Passed1 ? "√" : "";

                    InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[4].Text = item.Note;

                    stt++;
                }

            }
        }


        // Kết quả thử có tải
        stt = 1;
        InsertRow = xrTableTechnicalDocument1.Rows.FirstRow;

        var technicalDocuments = Accreditation?.equiment?.TechnicalDocuments;

        if (technicalDocuments != null)
        {
            int count = (int)(technicalDocuments.Count / 2);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var item = technicalDocuments[i];
                    InsertRow = xrTableTechnicalDocument1.InsertRowBelow(InsertRow);

                    InsertRow.Cells[0].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[0].Text = stt.ToString();

                    InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 0);
                    InsertRow.Cells[1].Text = item.Name;

                    InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[2].Text = (bool)item.Passed ? "√" : "";

                    InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[3].Text = !(bool)item.Passed ? "√" : "";

                    InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
                    InsertRow.Cells[4].Text = item.Note;

                    stt++;
                }

                InsertRow = xrTableTechnicalDocument2.Rows.FirstRow;
                for (int i = count; i < technicalDocuments.Count; i++)
                {
                    var item = technicalDocuments[i];
                    InsertRow = xrTableTechnicalDocument2.InsertRowBelow(InsertRow);

                    InsertRow.Cells[0].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[0].Text = stt.ToString();

                    InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 0);
                    InsertRow.Cells[1].Text = item.Name;

                    InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[2].Text = (bool)item.Passed ? "√" : "";

                    InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[3].Text = !(bool)item.Passed ? "√" : "";

                    InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
                    InsertRow.Cells[4].Text = item.Note;

                    stt++;
                }

            }
            else
            {
                xrTableTechnicalDocument2.Rows.Clear();
                xrTableTechnicalDocument1.WidthF = xrTableTechnicalDocument1.WidthF * 2;

                foreach (var item in technicalDocuments)
                {
                    InsertRow = xrTableTechnicalDocument1.InsertRowBelow(InsertRow);

                    InsertRow.Cells[0].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[0].Text = stt.ToString();

                    InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 0);
                    InsertRow.Cells[1].Text = item.Name;

                    InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[2].Text = (bool)item.Passed ? "√" : "";

                    InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[3].Text = !(bool)item.Passed ? "√" : "";

                    InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
                    InsertRow.Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    InsertRow.Cells[4].Text = item.Note;

                    stt++;
                }
            }
        }

        //this.cbKDDat.Checked = Accreditation.AccreditationResult;
        //this.cbKDKhongDat.Checked = !Accreditation.AccreditationResult;
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccreditationReportTM));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand7 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTableTester = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell102 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell108 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand8 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTableChungKien = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow25 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow21 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell109 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell110 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand9 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow27 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellTypeAcc = new DevExpress.XtraReports.UI.XRTableCell();
            this.cbKDLanDau = new DevExpress.XtraReports.UI.XRCheckBox();
            this.cbKDDinhKy = new DevExpress.XtraReports.UI.XRCheckBox();
            this.cbKDSauSuaChua = new DevExpress.XtraReports.UI.XRCheckBox();
            this.cbKDBatThuong = new DevExpress.XtraReports.UI.XRCheckBox();
            this.cbKDSauLapDat = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow28 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand6 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTableSpecification = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableDacTinhKyThuat = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand1 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTableTechnicalDocument2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow46 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow33 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow29 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow30 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableTechnicalDocument1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand11 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow52 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell128 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow53 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell129 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell113 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow54 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell131 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell114 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow31 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell115 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox2 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox1 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow32 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow34 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell127 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow43 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand2 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTablePartion1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTablePartion2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand3 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTableLoadTest = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow35 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow37 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow36 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell126 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox3 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox4 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow47 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell130 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell132 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell133 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox5 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox6 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow56 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell137 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell138 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell139 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox7 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox8 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow55 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell134 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell135 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell136 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox9 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox10 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow57 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell140 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow58 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell143 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell145 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow59 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell141 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow60 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell144 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell147 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell148 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox11 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox12 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow61 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell142 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell146 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell149 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox13 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox14 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow62 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell150 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell151 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell152 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox15 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox16 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow63 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell153 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow66 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell162 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell164 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow65 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell159 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow64 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell156 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell157 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell158 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox17 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox18 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow67 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell154 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell155 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell160 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox19 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox20 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.SubBand5 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow38 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow39 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtAccreditationResult = new DevExpress.XtraReports.UI.XRTableCell();
            this.cbKDDat = new DevExpress.XtraReports.UI.XRCheckBox();
            this.cbKDKhongDat = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrTableRow40 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell116 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell97 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow41 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell117 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell98 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell118 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell119 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell99 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow42 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell120 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell101 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell100 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow44 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell103 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell104 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow48 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell105 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell106 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow49 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell121 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell122 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow45 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell123 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell107 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow50 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell125 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow51 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell124 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand10 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell111 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow16 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell112 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.pageFooterBand = new DevExpress.XtraReports.UI.PageFooterBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBoxLeft = new DevExpress.XtraReports.UI.XRPictureBox();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.objectDataSource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTester)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableChungKien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableSpecification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableDacTinhKyThuat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTechnicalDocument2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTechnicalDocument1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTablePartion1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTablePartion2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableLoadTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
            this.Detail.HeightF = 62.37502F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseBorders = false;
            this.Detail.SubBands.AddRange(new DevExpress.XtraReports.UI.SubBand[] {
            this.SubBand7,
            this.SubBand8,
            this.SubBand9,
            this.SubBand6,
            this.SubBand1,
            this.SubBand11,
            this.SubBand2,
            this.SubBand3,
            this.SubBand5,
            this.SubBand10});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable6
            // 
            this.xrTable6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(5.249756F, 12.37505F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow8});
            this.xrTable6.SizeF = new System.Drawing.SizeF(776.5005F, 49.99997F);
            this.xrTable6.StylePriority.UseBorders = false;
            this.xrTable6.StylePriority.UseTextAlignment = false;
            this.xrTable6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell23});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 11.5D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseFont = false;
            this.xrTableCell23.Text = "BIÊN BẢN KIỂM ĐỊNH AN TOÀN";
            this.xrTableCell23.Weight = 0.30463317857270172D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell24,
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell28,
            this.xrTableCell29});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 11.5D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.Weight = 0.10444791563444125D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell25.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseBorders = false;
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "Số";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell25.Weight = 0.013209927359943268D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell26.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseBorders = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = ".......";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell26.Weight = 0.020461059529396232D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Id]")});
            this.xrTableCell27.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.StylePriority.UseBorders = false;
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell27.Weight = 0.020742713038177554D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell28.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseBorders = false;
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "/KĐXD-TBN";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell28.Weight = 0.041141490860136791D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.Weight = 0.10463007215060668D;
            // 
            // SubBand7
            // 
            this.SubBand7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableTester});
            this.SubBand7.HeightF = 60.00001F;
            this.SubBand7.Name = "SubBand7";
            // 
            // xrTableTester
            // 
            this.xrTableTester.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableTester.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableTester.LocationFloat = new DevExpress.Utils.PointFloat(5.62501F, 10.00001F);
            this.xrTableTester.Name = "xrTableTester";
            this.xrTableTester.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow26,
            this.xrTableRow1});
            this.xrTableTester.SizeF = new System.Drawing.SizeF(776.5004F, 50F);
            this.xrTableTester.StylePriority.UseBorders = false;
            this.xrTableTester.StylePriority.UseFont = false;
            // 
            // xrTableRow26
            // 
            this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell76});
            this.xrTableRow26.Name = "xrTableRow26";
            this.xrTableRow26.Weight = 11.5D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.Text = "Chúng tôi gồm:";
            this.xrTableCell76.Weight = 0.39521126193715989D;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell74,
            this.xrTableCell1,
            this.xrTableCell102,
            this.xrTableCell3,
            this.xrTableCell108,
            this.xrTableCell5});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 11.5D;
            // 
            // xrTableCell74
            // 
            this.xrTableCell74.Name = "xrTableCell74";
            this.xrTableCell74.Weight = 0.024848153581101137D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Weight = 0.10372647582703216D;
            // 
            // xrTableCell102
            // 
            this.xrTableCell102.Name = "xrTableCell102";
            this.xrTableCell102.Text = "Số hiệu kiểm định viên số:";
            this.xrTableCell102.Weight = 0.0954529725569535D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Weight = 0.072298563373505181D;
            // 
            // xrTableCell108
            // 
            this.xrTableCell108.Name = "xrTableCell108";
            this.xrTableCell108.Text = "ĐT:";
            this.xrTableCell108.Weight = 0.015993254521415819D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Weight = 0.082891842077152084D;
            // 
            // SubBand8
            // 
            this.SubBand8.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableChungKien});
            this.SubBand8.HeightF = 260.0001F;
            this.SubBand8.Name = "SubBand8";
            // 
            // xrTableChungKien
            // 
            this.xrTableChungKien.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableChungKien.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableChungKien.LocationFloat = new DevExpress.Utils.PointFloat(6.375082F, 10F);
            this.xrTableChungKien.Name = "xrTableChungKien";
            this.xrTableChungKien.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow25,
            this.xrTableRow24,
            this.xrTableRow23,
            this.xrTableRow22,
            this.xrTableRow21,
            this.xrTableRow20,
            this.xrTableRow19,
            this.xrTableRow18,
            this.xrTableRow9,
            this.xrTableRow10});
            this.xrTableChungKien.SizeF = new System.Drawing.SizeF(775.7501F, 250.0001F);
            this.xrTableChungKien.StylePriority.UseBorders = false;
            this.xrTableChungKien.StylePriority.UseFont = false;
            // 
            // xrTableRow25
            // 
            this.xrTableRow25.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell70,
            this.xrTableCell71});
            this.xrTableRow25.Name = "xrTableRow25";
            this.xrTableRow25.Weight = 1D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.Text = "Thuộc tổ chức kiểm định:";
            this.xrTableCell70.Weight = 0.62536769597279718D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.StylePriority.UseFont = false;
            this.xrTableCell71.Text = "Công ty cổ phần kiểm định kỹ thuật, an toàn và tư vấn xây dựng - Incosaf";
            this.xrTableCell71.Weight = 1.3746323040272028D;
            // 
            // xrTableRow24
            // 
            this.xrTableRow24.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell68,
            this.xrTableCell69});
            this.xrTableRow24.Name = "xrTableRow24";
            this.xrTableRow24.Weight = 1D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.Text = "Số đăng ký của Tổ chức:";
            this.xrTableCell68.Weight = 0.62536769597279718D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.StylePriority.UseFont = false;
            this.xrTableCell69.Text = "03/GCN - KĐ";
            this.xrTableCell69.Weight = 1.3746323040272028D;
            // 
            // xrTableRow23
            // 
            this.xrTableRow23.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell66,
            this.xrTableCell67});
            this.xrTableRow23.Name = "xrTableRow23";
            this.xrTableRow23.Weight = 1D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.Text = "Đã tiến hành kiểm định (Tên thiết bị):";
            this.xrTableCell66.Weight = 0.62536769597279718D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.Name]")});
            this.xrTableCell67.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.StylePriority.UseFont = false;
            this.xrTableCell67.Weight = 1.3746323040272028D;
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell64,
            this.xrTableCell65});
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Weight = 1D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.Text = "Tên tổ chức, cá nhân đề nghị:";
            this.xrTableCell64.Weight = 0.62536769597279718D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.contract.customer.Name]")});
            this.xrTableCell65.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.StylePriority.UseFont = false;
            this.xrTableCell65.Weight = 1.3746323040272028D;
            // 
            // xrTableRow21
            // 
            this.xrTableRow21.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell62,
            this.xrTableCell63});
            this.xrTableRow21.Name = "xrTableRow21";
            this.xrTableRow21.Weight = 1D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.Text = "Địa chỉ:";
            this.xrTableCell62.Weight = 0.62536769597279718D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.contract.customer.Address]")});
            this.xrTableCell63.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.StylePriority.UseFont = false;
            this.xrTableCell63.Weight = 1.3746323040272028D;
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell61,
            this.xrTableCell60});
            this.xrTableRow20.Name = "xrTableRow20";
            this.xrTableRow20.Weight = 1D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.Text = "Địa chỉ (Vị trí) lắp đặt thiết bị:";
            this.xrTableCell61.Weight = 0.62536769597279718D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]")});
            this.xrTableCell60.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.StylePriority.UseFont = false;
            this.xrTableCell60.Weight = 1.3746323040272028D;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell59,
            this.xrTableCell57,
            this.xrTableCell56});
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Weight = 1D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.Text = "Quy trình kiểm định, tiêu chuẩn áp dụng:";
            this.xrTableCell59.Weight = 0.69921539047272563D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmployedProcedure.Name]")});
            this.xrTableCell57.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.StylePriority.UseFont = false;
            this.xrTableCell57.Weight = 0.50480146628818723D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmployedStandard.Name]")});
            this.xrTableCell56.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.StylePriority.UseFont = false;
            this.xrTableCell56.Weight = 0.79598314323908714D;
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell58});
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 1D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.Text = "Chứng kiến việc kiểm định và thông qua biên bản có:";
            this.xrTableCell58.Weight = 2D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell54,
            this.xrTableCell22,
            this.xrTableCell33,
            this.xrTableCell72,
            this.xrTableCell109,
            this.xrTableCell34});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.Weight = 0.12203264888580007D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Text = "1.";
            this.xrTableCell22.Weight = 0.05217004517549384D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Viewer1]")});
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Weight = 0.73152879651008451D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.Weight = 0.025781339870807052D;
            // 
            // xrTableCell109
            // 
            this.xrTableCell109.Name = "xrTableCell109";
            this.xrTableCell109.Text = "Chức vụ:";
            this.xrTableCell109.Weight = 0.17850911343415127D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PositionViewer1]")});
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Weight = 0.88997805612366321D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell55,
            this.xrTableCell75,
            this.xrTableCell35,
            this.xrTableCell73,
            this.xrTableCell110,
            this.xrTableCell36});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.Weight = 0.12203260954635342D;
            // 
            // xrTableCell75
            // 
            this.xrTableCell75.Name = "xrTableCell75";
            this.xrTableCell75.Text = "2.";
            this.xrTableCell75.Weight = 0.052170163193826058D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Viewer2]")});
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Weight = 0.73152875717064259D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.Weight = 0.025781300531363516D;
            // 
            // xrTableCell110
            // 
            this.xrTableCell110.Name = "xrTableCell110";
            this.xrTableCell110.Text = "Chức vụ:";
            this.xrTableCell110.Weight = 0.17850911343415127D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PositionViewer2]")});
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Weight = 0.88997805612366321D;
            // 
            // SubBand9
            // 
            this.SubBand9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.SubBand9.HeightF = 60F;
            this.SubBand9.Name = "SubBand9";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(6.37509F, 9.999974F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow27,
            this.xrTableRow28});
            this.xrTable2.SizeF = new System.Drawing.SizeF(775.7502F, 50F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow27
            // 
            this.xrTableRow27.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell81,
            this.xrTableCellTypeAcc});
            this.xrTableRow27.Name = "xrTableRow27";
            this.xrTableRow27.Weight = 11.5D;
            // 
            // xrTableCell81
            // 
            this.xrTableCell81.Name = "xrTableCell81";
            this.xrTableCell81.StylePriority.UseTextAlignment = false;
            this.xrTableCell81.Text = "I - HÌNH THỨC KIỂM ĐỊNH:";
            this.xrTableCell81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell81.Weight = 0.11904867327301612D;
            // 
            // xrTableCellTypeAcc
            // 
            this.xrTableCellTypeAcc.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.cbKDLanDau,
            this.cbKDDinhKy,
            this.cbKDSauSuaChua,
            this.cbKDBatThuong,
            this.cbKDSauLapDat});
            this.xrTableCellTypeAcc.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCellTypeAcc.Name = "xrTableCellTypeAcc";
            this.xrTableCellTypeAcc.StylePriority.UseFont = false;
            this.xrTableCellTypeAcc.Text = "Lần đầu";
            this.xrTableCellTypeAcc.Weight = 0.27616258866414378D;
            // 
            // cbKDLanDau
            // 
            this.cbKDLanDau.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbKDLanDau.LocationFloat = new DevExpress.Utils.PointFloat(1.430511E-06F, 3.178914E-05F);
            this.cbKDLanDau.Name = "cbKDLanDau";
            this.cbKDLanDau.SizeF = new System.Drawing.SizeF(78.11385F, 23F);
            this.cbKDLanDau.Text = "Lần đầu";
            // 
            // cbKDDinhKy
            // 
            this.cbKDDinhKy.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbKDDinhKy.LocationFloat = new DevExpress.Utils.PointFloat(93.38058F, 0F);
            this.cbKDDinhKy.Name = "cbKDDinhKy";
            this.cbKDDinhKy.SizeF = new System.Drawing.SizeF(70.50555F, 23F);
            this.cbKDDinhKy.Text = "Định kỳ";
            // 
            // cbKDSauSuaChua
            // 
            this.cbKDSauSuaChua.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbKDSauSuaChua.LocationFloat = new DevExpress.Utils.PointFloat(182.2888F, 0F);
            this.cbKDSauSuaChua.Name = "cbKDSauSuaChua";
            this.cbKDSauSuaChua.SizeF = new System.Drawing.SizeF(110F, 23F);
            this.cbKDSauSuaChua.Text = "Sau sửa chữa";
            // 
            // cbKDBatThuong
            // 
            this.cbKDBatThuong.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbKDBatThuong.LocationFloat = new DevExpress.Utils.PointFloat(432.0726F, 0F);
            this.cbKDBatThuong.Name = "cbKDBatThuong";
            this.cbKDBatThuong.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.cbKDBatThuong.Text = "Bất thường";
            // 
            // cbKDSauLapDat
            // 
            this.cbKDSauLapDat.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbKDSauLapDat.LocationFloat = new DevExpress.Utils.PointFloat(310.6733F, 0F);
            this.cbKDSauLapDat.Name = "cbKDSauLapDat";
            this.cbKDSauLapDat.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.cbKDSauLapDat.Text = "Sau lắp đặt";
            // 
            // xrTableRow28
            // 
            this.xrTableRow28.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell77});
            this.xrTableRow28.Name = "xrTableRow28";
            this.xrTableRow28.Weight = 11.5D;
            // 
            // xrTableCell77
            // 
            this.xrTableCell77.Name = "xrTableCell77";
            this.xrTableCell77.StylePriority.UseTextAlignment = false;
            this.xrTableCell77.Text = "II - ĐẶC TÍNH KỸ THUẬT CỦA THIẾT BỊ:";
            this.xrTableCell77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell77.Weight = 0.39521126193715994D;
            // 
            // SubBand6
            // 
            this.SubBand6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableSpecification,
            this.xrTableDacTinhKyThuat});
            this.SubBand6.HeightF = 110F;
            this.SubBand6.Name = "SubBand6";
            // 
            // xrTableSpecification
            // 
            this.xrTableSpecification.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableSpecification.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableSpecification.LocationFloat = new DevExpress.Utils.PointFloat(437.5916F, 10F);
            this.xrTableSpecification.Name = "xrTableSpecification";
            this.xrTableSpecification.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTableSpecification.SizeF = new System.Drawing.SizeF(344.5335F, 25F);
            this.xrTableSpecification.StylePriority.UseBorders = false;
            this.xrTableSpecification.StylePriority.UseFont = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell8});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 11.5D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.specifications.Name]")});
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "xrTableCell7";
            this.xrTableCell7.Weight = 0.18845136191007311D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.specifications.Value]")});
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "xrTableCell8";
            this.xrTableCell8.Weight = 0.11618181666262863D;
            // 
            // xrTableDacTinhKyThuat
            // 
            this.xrTableDacTinhKyThuat.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableDacTinhKyThuat.LocationFloat = new DevExpress.Utils.PointFloat(33.04163F, 10F);
            this.xrTableDacTinhKyThuat.Name = "xrTableDacTinhKyThuat";
            this.xrTableDacTinhKyThuat.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow11,
            this.xrTableRow13,
            this.xrTableRow12,
            this.xrTableRow14});
            this.xrTableDacTinhKyThuat.SizeF = new System.Drawing.SizeF(377.5167F, 99.99997F);
            this.xrTableDacTinhKyThuat.StylePriority.UseBorders = false;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell37,
            this.xrTableCell38});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 11.5D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.StylePriority.UseFont = false;
            this.xrTableCell37.Text = "- Mã hiệu: ";
            this.xrTableCell37.Weight = 0.10634004654628156D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.Code]")});
            this.xrTableCell38.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.StylePriority.UseFont = false;
            this.xrTableCell38.Weight = 0.19829313202642018D;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell41,
            this.xrTableCell42});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 11.5D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.StylePriority.UseFont = false;
            this.xrTableCell41.Text = "- Số quản lý (TT):";
            this.xrTableCell41.Weight = 0.10634004654628156D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.No]")});
            this.xrTableCell42.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseFont = false;
            this.xrTableCell42.Weight = 0.19829313202642018D;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell39,
            this.xrTableCell40});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 11.5D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StylePriority.UseFont = false;
            this.xrTableCell39.Text = "- Năm sản xuất:";
            this.xrTableCell39.Weight = 0.10634004654628156D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.YearOfProduction]")});
            this.xrTableCell40.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseFont = false;
            this.xrTableCell40.Weight = 0.19829313202642018D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell43,
            this.xrTableCell44});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 11.5D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.Text = "- Nhà chế tạo:";
            this.xrTableCell43.Weight = 0.10634004654628156D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.ManuFacturer]")});
            this.xrTableCell44.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StylePriority.UseFont = false;
            this.xrTableCell44.Weight = 0.19829313202642018D;
            // 
            // SubBand1
            // 
            this.SubBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableTechnicalDocument2,
            this.xrTable3,
            this.xrTableTechnicalDocument1});
            this.SubBand1.HeightF = 135.3009F;
            this.SubBand1.Name = "SubBand1";
            // 
            // xrTableTechnicalDocument2
            // 
            this.xrTableTechnicalDocument2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableTechnicalDocument2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableTechnicalDocument2.LocationFloat = new DevExpress.Utils.PointFloat(398.3583F, 110.3009F);
            this.xrTableTechnicalDocument2.Name = "xrTableTechnicalDocument2";
            this.xrTableTechnicalDocument2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTableTechnicalDocument2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTableTechnicalDocument2.SizeF = new System.Drawing.SizeF(384.5168F, 25F);
            this.xrTableTechnicalDocument2.StylePriority.UseBorders = false;
            this.xrTableTechnicalDocument2.StylePriority.UseFont = false;
            this.xrTableTechnicalDocument2.StylePriority.UsePadding = false;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 11.5D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.StylePriority.UsePadding = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "TT";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell17.Weight = 0.13500174282035007D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "Danh mục";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell18.Weight = 0.40757100344771835D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Text = "Đạt";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell19.Weight = 0.11936548012489452D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "Không đạt";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell20.Weight = 0.22769740610906702D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "Ghi chú";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell21.Weight = 0.20225399048985204D;
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(5.62501F, 10.30097F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow46,
            this.xrTableRow33,
            this.xrTableRow29,
            this.xrTableRow30});
            this.xrTable3.SizeF = new System.Drawing.SizeF(776.1252F, 99.99992F);
            this.xrTable3.StylePriority.UseBorders = false;
            // 
            // xrTableRow46
            // 
            this.xrTableRow46.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell88});
            this.xrTableRow46.Name = "xrTableRow46";
            this.xrTableRow46.Weight = 11.5D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.StylePriority.UseFont = false;
            this.xrTableCell88.Text = "III - NỘI DUNG KIỂM ĐỊNH:";
            this.xrTableCell88.Weight = 0.30463317857270172D;
            // 
            // xrTableRow33
            // 
            this.xrTableRow33.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell87});
            this.xrTableRow33.Name = "xrTableRow33";
            this.xrTableRow33.Weight = 11.5D;
            // 
            // xrTableCell87
            // 
            this.xrTableCell87.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell87.Name = "xrTableCell87";
            this.xrTableCell87.StylePriority.UseFont = false;
            this.xrTableCell87.Text = "1. Kiểm tra hồ sơ kỹ thuật:";
            this.xrTableCell87.Weight = 0.30463317857270172D;
            // 
            // xrTableRow29
            // 
            this.xrTableRow29.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell78,
            this.xrTableCell79});
            this.xrTableRow29.Name = "xrTableRow29";
            this.xrTableRow29.Weight = 11.5D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.StylePriority.UseFont = false;
            this.xrTableCell78.Text = "Nhận xét:";
            this.xrTableCell78.Weight = 0.026815548135186815D;
            // 
            // xrTableCell79
            // 
            this.xrTableCell79.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DocumentTechnicalNotice]")});
            this.xrTableCell79.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell79.Name = "xrTableCell79";
            this.xrTableCell79.StylePriority.UseFont = false;
            this.xrTableCell79.Weight = 0.27781763043751495D;
            // 
            // xrTableRow30
            // 
            this.xrTableRow30.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell80});
            this.xrTableRow30.Name = "xrTableRow30";
            this.xrTableRow30.Weight = 11.5D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.StylePriority.UseFont = false;
            this.xrTableCell80.Text = "Đánh giá kết quả:";
            this.xrTableCell80.Weight = 0.30463317857270178D;
            // 
            // xrTableTechnicalDocument1
            // 
            this.xrTableTechnicalDocument1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableTechnicalDocument1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableTechnicalDocument1.LocationFloat = new DevExpress.Utils.PointFloat(5.625001F, 110.3009F);
            this.xrTableTechnicalDocument1.Name = "xrTableTechnicalDocument1";
            this.xrTableTechnicalDocument1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTableTechnicalDocument1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTableTechnicalDocument1.SizeF = new System.Drawing.SizeF(384.5168F, 25F);
            this.xrTableTechnicalDocument1.StylePriority.UseBorders = false;
            this.xrTableTechnicalDocument1.StylePriority.UseFont = false;
            this.xrTableTechnicalDocument1.StylePriority.UsePadding = false;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell45,
            this.xrTableCell46});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 11.5D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UsePadding = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "TT";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.13500174282035007D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Danh mục";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.40757100344771835D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "Đạt";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 0.11936548012489452D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StylePriority.UseFont = false;
            this.xrTableCell45.StylePriority.UseTextAlignment = false;
            this.xrTableCell45.Text = "Không đạt";
            this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell45.Weight = 0.22769740610906702D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.StylePriority.UseFont = false;
            this.xrTableCell46.StylePriority.UseTextAlignment = false;
            this.xrTableCell46.Text = "Ghi chú";
            this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell46.Weight = 0.20225399048985204D;
            // 
            // SubBand11
            // 
            this.SubBand11.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5});
            this.SubBand11.HeightF = 184.9999F;
            this.SubBand11.Name = "SubBand11";
            // 
            // xrTable5
            // 
            this.xrTable5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(7.749096F, 9.999974F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow52,
            this.xrTableRow53,
            this.xrTableRow54,
            this.xrTableRow31,
            this.xrTableRow32,
            this.xrTableRow34,
            this.xrTableRow43});
            this.xrTable5.SizeF = new System.Drawing.SizeF(774.3763F, 174.9999F);
            this.xrTable5.StylePriority.UseBorders = false;
            // 
            // xrTableRow52
            // 
            this.xrTableRow52.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell128});
            this.xrTableRow52.Name = "xrTableRow52";
            this.xrTableRow52.Weight = 11.5D;
            // 
            // xrTableCell128
            // 
            this.xrTableCell128.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell128.Name = "xrTableCell128";
            this.xrTableCell128.StylePriority.UseFont = false;
            this.xrTableCell128.Text = "2. Kiểm tra bên ngoài:";
            this.xrTableCell128.Weight = 0.60926635714540345D;
            // 
            // xrTableRow53
            // 
            this.xrTableRow53.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell129,
            this.xrTableCell113});
            this.xrTableRow53.Name = "xrTableRow53";
            this.xrTableRow53.Weight = 11.5D;
            // 
            // xrTableCell129
            // 
            this.xrTableCell129.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell129.Name = "xrTableCell129";
            this.xrTableCell129.StylePriority.UseFont = false;
            this.xrTableCell129.Text = "- Tính đầy đủ - đồng bộ của thang máy:";
            this.xrTableCell129.Weight = 0.21230105480086833D;
            // 
            // xrTableCell113
            // 
            this.xrTableCell113.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell113.Name = "xrTableCell113";
            this.xrTableCell113.StylePriority.UseFont = false;
            this.xrTableCell113.Text = "................................................................";
            this.xrTableCell113.Weight = 0.39696530234453514D;
            // 
            // xrTableRow54
            // 
            this.xrTableRow54.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell131,
            this.xrTableCell114});
            this.xrTableRow54.Name = "xrTableRow54";
            this.xrTableRow54.Weight = 11.5D;
            // 
            // xrTableCell131
            // 
            this.xrTableCell131.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell131.Name = "xrTableCell131";
            this.xrTableCell131.StylePriority.UseFont = false;
            this.xrTableCell131.Text = "- Các khuyết tật - biến dạng:";
            this.xrTableCell131.Weight = 0.21230105480086833D;
            // 
            // xrTableCell114
            // 
            this.xrTableCell114.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell114.Multiline = true;
            this.xrTableCell114.Name = "xrTableCell114";
            this.xrTableCell114.StylePriority.UseFont = false;
            this.xrTableCell114.Text = "................................................................";
            this.xrTableCell114.Weight = 0.39696530234453514D;
            // 
            // xrTableRow31
            // 
            this.xrTableRow31.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell82,
            this.xrTableCell115});
            this.xrTableRow31.Name = "xrTableRow31";
            this.xrTableRow31.Weight = 11.5D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.StylePriority.UseFont = false;
            this.xrTableCell82.StylePriority.UseTextAlignment = false;
            this.xrTableCell82.Text = "- Đánh giá kết quả:";
            this.xrTableCell82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell82.Weight = 0.10657702784838131D;
            // 
            // xrTableCell115
            // 
            this.xrTableCell115.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox2,
            this.xrCheckBox1});
            this.xrTableCell115.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell115.Multiline = true;
            this.xrTableCell115.Name = "xrTableCell115";
            this.xrTableCell115.StylePriority.UseFont = false;
            this.xrTableCell115.Text = "\r\n";
            this.xrTableCell115.Weight = 0.50268932929702215D;
            // 
            // xrCheckBox2
            // 
            this.xrCheckBox2.EditOptions.Enabled = true;
            this.xrCheckBox2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox2.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox2.LocationFloat = new DevExpress.Utils.PointFloat(67.85202F, 0F);
            this.xrCheckBox2.Name = "xrCheckBox2";
            this.xrCheckBox2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox2.StylePriority.UseFont = false;
            this.xrCheckBox2.StylePriority.UseTextAlignment = false;
            this.xrCheckBox2.Text = "Không đạt";
            // 
            // xrCheckBox1
            // 
            this.xrCheckBox1.EditOptions.Enabled = true;
            this.xrCheckBox1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox1.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrCheckBox1.Name = "xrCheckBox1";
            this.xrCheckBox1.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox1.StylePriority.UseFont = false;
            this.xrCheckBox1.StylePriority.UseTextAlignment = false;
            this.xrCheckBox1.Text = "Đạt";
            // 
            // xrTableRow32
            // 
            this.xrTableRow32.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell83});
            this.xrTableRow32.Name = "xrTableRow32";
            this.xrTableRow32.Weight = 11.5D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.StylePriority.UseFont = false;
            this.xrTableCell83.Text = "3. Kiểm tra kỹ thuật - thử không tải:";
            this.xrTableCell83.Weight = 0.60926635714540345D;
            // 
            // xrTableRow34
            // 
            this.xrTableRow34.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell84,
            this.xrTableCell127});
            this.xrTableRow34.Name = "xrTableRow34";
            this.xrTableRow34.Weight = 11.5D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.StylePriority.UseFont = false;
            this.xrTableCell84.Text = "- Nhận xét:";
            this.xrTableCell84.Weight = 0.069663847099707132D;
            // 
            // xrTableCell127
            // 
            this.xrTableCell127.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell127.Name = "xrTableCell127";
            this.xrTableCell127.StylePriority.UseFont = false;
            this.xrTableCell127.Text = "................................................................................." +
    "...............................................";
            this.xrTableCell127.Weight = 0.53960251004569637D;
            // 
            // xrTableRow43
            // 
            this.xrTableRow43.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell85});
            this.xrTableRow43.Name = "xrTableRow43";
            this.xrTableRow43.Weight = 11.5D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.StylePriority.UseFont = false;
            this.xrTableCell85.Text = "- Đánh giá kết quả:";
            this.xrTableCell85.Weight = 0.60926635714540345D;
            // 
            // SubBand2
            // 
            this.SubBand2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTablePartion1,
            this.xrTablePartion2});
            this.SubBand2.HeightF = 35F;
            this.SubBand2.Name = "SubBand2";
            // 
            // xrTablePartion1
            // 
            this.xrTablePartion1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTablePartion1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTablePartion1.LocationFloat = new DevExpress.Utils.PointFloat(6.375082F, 10F);
            this.xrTablePartion1.Name = "xrTablePartion1";
            this.xrTablePartion1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTablePartion1.SizeF = new System.Drawing.SizeF(383.7667F, 25F);
            this.xrTablePartion1.StylePriority.UseBorders = false;
            this.xrTablePartion1.StylePriority.UseFont = false;
            this.xrTablePartion1.StylePriority.UseTextAlignment = false;
            this.xrTablePartion1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell6,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell47});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.Text = "STT";
            this.xrTableCell4.Weight = 0.17132910919478375D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.Text = "Cơ cấu; bộ phận";
            this.xrTableCell6.Weight = 0.52553500230347416D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.Text = "Đạt";
            this.xrTableCell12.Weight = 0.15391363427307969D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.Text = "Không đạt";
            this.xrTableCell13.Weight = 0.29360025604091422D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StylePriority.UseFont = false;
            this.xrTableCell47.Text = "Ghi chú";
            this.xrTableCell47.Weight = 0.26079245727968225D;
            // 
            // xrTablePartion2
            // 
            this.xrTablePartion2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTablePartion2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTablePartion2.LocationFloat = new DevExpress.Utils.PointFloat(398.3584F, 10F);
            this.xrTablePartion2.Name = "xrTablePartion2";
            this.xrTablePartion2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
            this.xrTablePartion2.SizeF = new System.Drawing.SizeF(383.7667F, 25F);
            this.xrTablePartion2.StylePriority.UseBorders = false;
            this.xrTablePartion2.StylePriority.UseFont = false;
            this.xrTablePartion2.StylePriority.UseTextAlignment = false;
            this.xrTablePartion2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell48});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 11.5D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.Text = "STT";
            this.xrTableCell2.Weight = 0.17132910919478375D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.Text = "Cơ cấu; bộ phận";
            this.xrTableCell14.Weight = 0.52553500230347416D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.Text = "Đạt";
            this.xrTableCell15.Weight = 0.15391363427307969D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.Text = "Không đạt";
            this.xrTableCell16.Weight = 0.29360025604091422D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.StylePriority.UseFont = false;
            this.xrTableCell48.Text = "Ghi chú";
            this.xrTableCell48.Weight = 0.26079245727968225D;
            // 
            // SubBand3
            // 
            this.SubBand3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableLoadTest});
            this.SubBand3.HeightF = 435F;
            this.SubBand3.Name = "SubBand3";
            // 
            // xrTableLoadTest
            // 
            this.xrTableLoadTest.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableLoadTest.LocationFloat = new DevExpress.Utils.PointFloat(6.37509F, 9.999974F);
            this.xrTableLoadTest.Name = "xrTableLoadTest";
            this.xrTableLoadTest.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow35,
            this.xrTableRow37,
            this.xrTableRow36,
            this.xrTableRow47,
            this.xrTableRow56,
            this.xrTableRow55,
            this.xrTableRow57,
            this.xrTableRow58,
            this.xrTableRow59,
            this.xrTableRow60,
            this.xrTableRow61,
            this.xrTableRow62,
            this.xrTableRow63,
            this.xrTableRow66,
            this.xrTableRow65,
            this.xrTableRow64,
            this.xrTableRow67});
            this.xrTableLoadTest.SizeF = new System.Drawing.SizeF(775.7501F, 425F);
            this.xrTableLoadTest.StylePriority.UseFont = false;
            this.xrTableLoadTest.StylePriority.UseTextAlignment = false;
            this.xrTableLoadTest.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow35
            // 
            this.xrTableRow35.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell95});
            this.xrTableRow35.Name = "xrTableRow35";
            this.xrTableRow35.Weight = 1D;
            // 
            // xrTableCell95
            // 
            this.xrTableCell95.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell95.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell95.Name = "xrTableCell95";
            this.xrTableCell95.StylePriority.UseBorders = false;
            this.xrTableCell95.StylePriority.UseFont = false;
            this.xrTableCell95.StylePriority.UseTextAlignment = false;
            this.xrTableCell95.Text = "4. Thử tải động: 100% tải định mức:";
            this.xrTableCell95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell95.Weight = 16.9559163662537D;
            // 
            // xrTableRow37
            // 
            this.xrTableRow37.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell91});
            this.xrTableRow37.Name = "xrTableRow37";
            this.xrTableRow37.Weight = 1D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.StylePriority.UseBorders = false;
            this.xrTableCell91.StylePriority.UseTextAlignment = false;
            this.xrTableCell91.Text = "- Đánh giá kết quả:";
            this.xrTableCell91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell91.Weight = 16.9559163662537D;
            // 
            // xrTableRow36
            // 
            this.xrTableRow36.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell89,
            this.xrTableCell126,
            this.xrTableCell86});
            this.xrTableRow36.Name = "xrTableRow36";
            this.xrTableRow36.Weight = 1D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.StylePriority.UseBorders = false;
            this.xrTableCell89.StylePriority.UseTextAlignment = false;
            this.xrTableCell89.Text = "+ Vận tốc cabin:";
            this.xrTableCell89.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell89.Weight = 9.4107197531061768D;
            // 
            // xrTableCell126
            // 
            this.xrTableCell126.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell126.Name = "xrTableCell126";
            this.xrTableCell126.StylePriority.UseBorders = false;
            this.xrTableCell126.StylePriority.UseTextAlignment = false;
            this.xrTableCell126.Text = "Kết quả";
            this.xrTableCell126.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell126.Weight = 1.6057245021198829D;
            // 
            // xrTableCell86
            // 
            this.xrTableCell86.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell86.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox3,
            this.xrCheckBox4});
            this.xrTableCell86.Name = "xrTableCell86";
            this.xrTableCell86.StylePriority.UseBorders = false;
            this.xrTableCell86.StylePriority.UseTextAlignment = false;
            this.xrTableCell86.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell86.Weight = 5.93947211102764D;
            // 
            // xrCheckBox3
            // 
            this.xrCheckBox3.EditOptions.Enabled = true;
            this.xrCheckBox3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox3.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrCheckBox3.Name = "xrCheckBox3";
            this.xrCheckBox3.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox3.StylePriority.UseFont = false;
            this.xrCheckBox3.StylePriority.UseTextAlignment = false;
            this.xrCheckBox3.Text = "Đạt";
            // 
            // xrCheckBox4
            // 
            this.xrCheckBox4.EditOptions.Enabled = true;
            this.xrCheckBox4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox4.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox4.LocationFloat = new DevExpress.Utils.PointFloat(67.85196F, 0F);
            this.xrCheckBox4.Name = "xrCheckBox4";
            this.xrCheckBox4.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox4.StylePriority.UseFont = false;
            this.xrCheckBox4.StylePriority.UseTextAlignment = false;
            this.xrCheckBox4.Text = "Không đạt";
            // 
            // xrTableRow47
            // 
            this.xrTableRow47.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell130,
            this.xrTableCell132,
            this.xrTableCell133});
            this.xrTableRow47.Name = "xrTableRow47";
            this.xrTableRow47.Weight = 1D;
            // 
            // xrTableCell130
            // 
            this.xrTableCell130.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell130.Name = "xrTableCell130";
            this.xrTableCell130.StylePriority.UseBorders = false;
            this.xrTableCell130.StylePriority.UseTextAlignment = false;
            this.xrTableCell130.Text = "+ Dòng điện động cơ:";
            this.xrTableCell130.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell130.Weight = 9.4107197531061768D;
            // 
            // xrTableCell132
            // 
            this.xrTableCell132.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell132.Name = "xrTableCell132";
            this.xrTableCell132.StylePriority.UseBorders = false;
            this.xrTableCell132.StylePriority.UseTextAlignment = false;
            this.xrTableCell132.Text = "Kết quả";
            this.xrTableCell132.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell132.Weight = 1.6057245021198829D;
            // 
            // xrTableCell133
            // 
            this.xrTableCell133.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell133.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox5,
            this.xrCheckBox6});
            this.xrTableCell133.Name = "xrTableCell133";
            this.xrTableCell133.StylePriority.UseBorders = false;
            this.xrTableCell133.StylePriority.UseTextAlignment = false;
            this.xrTableCell133.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell133.Weight = 5.93947211102764D;
            // 
            // xrCheckBox5
            // 
            this.xrCheckBox5.EditOptions.Enabled = true;
            this.xrCheckBox5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox5.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.000014F);
            this.xrCheckBox5.Name = "xrCheckBox5";
            this.xrCheckBox5.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox5.StylePriority.UseFont = false;
            this.xrCheckBox5.StylePriority.UseTextAlignment = false;
            this.xrCheckBox5.Text = "Đạt";
            // 
            // xrCheckBox6
            // 
            this.xrCheckBox6.EditOptions.Enabled = true;
            this.xrCheckBox6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox6.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox6.LocationFloat = new DevExpress.Utils.PointFloat(67.85183F, 2.000014F);
            this.xrCheckBox6.Name = "xrCheckBox6";
            this.xrCheckBox6.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox6.StylePriority.UseFont = false;
            this.xrCheckBox6.StylePriority.UseTextAlignment = false;
            this.xrCheckBox6.Text = "Không đạt";
            // 
            // xrTableRow56
            // 
            this.xrTableRow56.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell137,
            this.xrTableCell138,
            this.xrTableCell139});
            this.xrTableRow56.Name = "xrTableRow56";
            this.xrTableRow56.Weight = 1D;
            // 
            // xrTableCell137
            // 
            this.xrTableCell137.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell137.Name = "xrTableCell137";
            this.xrTableCell137.StylePriority.UseBorders = false;
            this.xrTableCell137.StylePriority.UseTextAlignment = false;
            this.xrTableCell137.Text = "+ Độ sai lệch dừng tầng lớn nhất:";
            this.xrTableCell137.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell137.Weight = 9.4107197531061768D;
            // 
            // xrTableCell138
            // 
            this.xrTableCell138.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell138.Name = "xrTableCell138";
            this.xrTableCell138.StylePriority.UseBorders = false;
            this.xrTableCell138.StylePriority.UseTextAlignment = false;
            this.xrTableCell138.Text = "Kết quả";
            this.xrTableCell138.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell138.Weight = 1.6057245021198829D;
            // 
            // xrTableCell139
            // 
            this.xrTableCell139.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell139.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox7,
            this.xrCheckBox8});
            this.xrTableCell139.Name = "xrTableCell139";
            this.xrTableCell139.StylePriority.UseBorders = false;
            this.xrTableCell139.StylePriority.UseTextAlignment = false;
            this.xrTableCell139.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell139.Weight = 5.93947211102764D;
            // 
            // xrCheckBox7
            // 
            this.xrCheckBox7.EditOptions.Enabled = true;
            this.xrCheckBox7.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox7.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox7.LocationFloat = new DevExpress.Utils.PointFloat(67.85177F, 2.000014F);
            this.xrCheckBox7.Name = "xrCheckBox7";
            this.xrCheckBox7.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox7.StylePriority.UseFont = false;
            this.xrCheckBox7.StylePriority.UseTextAlignment = false;
            this.xrCheckBox7.Text = "Không đạt";
            // 
            // xrCheckBox8
            // 
            this.xrCheckBox8.EditOptions.Enabled = true;
            this.xrCheckBox8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox8.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.000014F);
            this.xrCheckBox8.Name = "xrCheckBox8";
            this.xrCheckBox8.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox8.StylePriority.UseFont = false;
            this.xrCheckBox8.StylePriority.UseTextAlignment = false;
            this.xrCheckBox8.Text = "Đạt";
            // 
            // xrTableRow55
            // 
            this.xrTableRow55.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell134,
            this.xrTableCell135,
            this.xrTableCell136});
            this.xrTableRow55.Name = "xrTableRow55";
            this.xrTableRow55.Weight = 1D;
            // 
            // xrTableCell134
            // 
            this.xrTableCell134.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell134.Name = "xrTableCell134";
            this.xrTableCell134.StylePriority.UseBorders = false;
            this.xrTableCell134.StylePriority.UseTextAlignment = false;
            this.xrTableCell134.Text = "+ Bộ hãm bảo hiểm tức thời hoặc tức thời có giảm chấn:";
            this.xrTableCell134.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell134.Weight = 9.4107197531061768D;
            // 
            // xrTableCell135
            // 
            this.xrTableCell135.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell135.Name = "xrTableCell135";
            this.xrTableCell135.StylePriority.UseBorders = false;
            this.xrTableCell135.StylePriority.UseTextAlignment = false;
            this.xrTableCell135.Text = "Kết quả";
            this.xrTableCell135.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell135.Weight = 1.6057245021198829D;
            // 
            // xrTableCell136
            // 
            this.xrTableCell136.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell136.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox9,
            this.xrCheckBox10});
            this.xrTableCell136.Name = "xrTableCell136";
            this.xrTableCell136.StylePriority.UseBorders = false;
            this.xrTableCell136.StylePriority.UseTextAlignment = false;
            this.xrTableCell136.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell136.Weight = 5.93947211102764D;
            // 
            // xrCheckBox9
            // 
            this.xrCheckBox9.EditOptions.Enabled = true;
            this.xrCheckBox9.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox9.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox9.LocationFloat = new DevExpress.Utils.PointFloat(67.85177F, 1.999982F);
            this.xrCheckBox9.Name = "xrCheckBox9";
            this.xrCheckBox9.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox9.StylePriority.UseFont = false;
            this.xrCheckBox9.StylePriority.UseTextAlignment = false;
            this.xrCheckBox9.Text = "Không đạt";
            // 
            // xrCheckBox10
            // 
            this.xrCheckBox10.EditOptions.Enabled = true;
            this.xrCheckBox10.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox10.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1.999982F);
            this.xrCheckBox10.Name = "xrCheckBox10";
            this.xrCheckBox10.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox10.StylePriority.UseFont = false;
            this.xrCheckBox10.StylePriority.UseTextAlignment = false;
            this.xrCheckBox10.Text = "Đạt";
            // 
            // xrTableRow57
            // 
            this.xrTableRow57.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell140});
            this.xrTableRow57.Name = "xrTableRow57";
            this.xrTableRow57.Weight = 1D;
            // 
            // xrTableCell140
            // 
            this.xrTableCell140.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell140.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell140.Name = "xrTableCell140";
            this.xrTableCell140.StylePriority.UseBorders = false;
            this.xrTableCell140.StylePriority.UseFont = false;
            this.xrTableCell140.StylePriority.UseTextAlignment = false;
            this.xrTableCell140.Text = "5. Thử tải động : 125% tải định mức:";
            this.xrTableCell140.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell140.Weight = 16.9559163662537D;
            // 
            // xrTableRow58
            // 
            this.xrTableRow58.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell143,
            this.xrTableCell145});
            this.xrTableRow58.Name = "xrTableRow58";
            this.xrTableRow58.Weight = 1D;
            // 
            // xrTableCell143
            // 
            this.xrTableCell143.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell143.Name = "xrTableCell143";
            this.xrTableCell143.StylePriority.UseBorders = false;
            this.xrTableCell143.StylePriority.UseTextAlignment = false;
            this.xrTableCell143.Text = "- Nhận xét:";
            this.xrTableCell143.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell143.Weight = 1.965347792566793D;
            // 
            // xrTableCell145
            // 
            this.xrTableCell145.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell145.Name = "xrTableCell145";
            this.xrTableCell145.StylePriority.UseBorders = false;
            this.xrTableCell145.StylePriority.UseTextAlignment = false;
            this.xrTableCell145.Text = "................................................................................." +
    "...............................................";
            this.xrTableCell145.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell145.Weight = 14.990568573686907D;
            // 
            // xrTableRow59
            // 
            this.xrTableRow59.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell141});
            this.xrTableRow59.Name = "xrTableRow59";
            this.xrTableRow59.Weight = 1D;
            // 
            // xrTableCell141
            // 
            this.xrTableCell141.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell141.Name = "xrTableCell141";
            this.xrTableCell141.StylePriority.UseBorders = false;
            this.xrTableCell141.StylePriority.UseTextAlignment = false;
            this.xrTableCell141.Text = "- Đánh giá kết quả:";
            this.xrTableCell141.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell141.Weight = 16.9559163662537D;
            // 
            // xrTableRow60
            // 
            this.xrTableRow60.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell144,
            this.xrTableCell147,
            this.xrTableCell148});
            this.xrTableRow60.Name = "xrTableRow60";
            this.xrTableRow60.Weight = 1D;
            // 
            // xrTableCell144
            // 
            this.xrTableCell144.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell144.Name = "xrTableCell144";
            this.xrTableCell144.StylePriority.UseBorders = false;
            this.xrTableCell144.StylePriority.UseTextAlignment = false;
            this.xrTableCell144.Text = "+ Phanh điện từ:";
            this.xrTableCell144.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell144.Weight = 9.4107190860698626D;
            // 
            // xrTableCell147
            // 
            this.xrTableCell147.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell147.Name = "xrTableCell147";
            this.xrTableCell147.StylePriority.UseBorders = false;
            this.xrTableCell147.StylePriority.UseTextAlignment = false;
            this.xrTableCell147.Text = "Kết quả";
            this.xrTableCell147.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell147.Weight = 1.6057251691561976D;
            // 
            // xrTableCell148
            // 
            this.xrTableCell148.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell148.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox11,
            this.xrCheckBox12});
            this.xrTableCell148.Name = "xrTableCell148";
            this.xrTableCell148.StylePriority.UseBorders = false;
            this.xrTableCell148.StylePriority.UseTextAlignment = false;
            this.xrTableCell148.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell148.Weight = 5.93947211102764D;
            // 
            // xrCheckBox11
            // 
            this.xrCheckBox11.EditOptions.Enabled = true;
            this.xrCheckBox11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox11.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox11.LocationFloat = new DevExpress.Utils.PointFloat(67.85189F, 1.999982F);
            this.xrCheckBox11.Name = "xrCheckBox11";
            this.xrCheckBox11.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox11.StylePriority.UseFont = false;
            this.xrCheckBox11.StylePriority.UseTextAlignment = false;
            this.xrCheckBox11.Text = "Không đạt";
            // 
            // xrCheckBox12
            // 
            this.xrCheckBox12.EditOptions.Enabled = true;
            this.xrCheckBox12.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox12.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox12.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 1.999982F);
            this.xrCheckBox12.Name = "xrCheckBox12";
            this.xrCheckBox12.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox12.StylePriority.UseFont = false;
            this.xrCheckBox12.StylePriority.UseTextAlignment = false;
            this.xrCheckBox12.Text = "Đạt";
            // 
            // xrTableRow61
            // 
            this.xrTableRow61.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell142,
            this.xrTableCell146,
            this.xrTableCell149});
            this.xrTableRow61.Name = "xrTableRow61";
            this.xrTableRow61.Weight = 1D;
            // 
            // xrTableCell142
            // 
            this.xrTableCell142.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell142.Name = "xrTableCell142";
            this.xrTableCell142.StylePriority.UseBorders = false;
            this.xrTableCell142.StylePriority.UseTextAlignment = false;
            this.xrTableCell142.Text = "+ Bộ hãm bảo hiểm êm:";
            this.xrTableCell142.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell142.Weight = 9.4107190860698626D;
            // 
            // xrTableCell146
            // 
            this.xrTableCell146.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell146.Name = "xrTableCell146";
            this.xrTableCell146.StylePriority.UseBorders = false;
            this.xrTableCell146.StylePriority.UseTextAlignment = false;
            this.xrTableCell146.Text = "Kết quả";
            this.xrTableCell146.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell146.Weight = 1.6057251691561976D;
            // 
            // xrTableCell149
            // 
            this.xrTableCell149.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell149.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox13,
            this.xrCheckBox14});
            this.xrTableCell149.Name = "xrTableCell149";
            this.xrTableCell149.StylePriority.UseBorders = false;
            this.xrTableCell149.StylePriority.UseTextAlignment = false;
            this.xrTableCell149.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell149.Weight = 5.93947211102764D;
            // 
            // xrCheckBox13
            // 
            this.xrCheckBox13.EditOptions.Enabled = true;
            this.xrCheckBox13.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox13.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox13.LocationFloat = new DevExpress.Utils.PointFloat(67.85183F, 1.999982F);
            this.xrCheckBox13.Name = "xrCheckBox13";
            this.xrCheckBox13.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox13.StylePriority.UseFont = false;
            this.xrCheckBox13.StylePriority.UseTextAlignment = false;
            this.xrCheckBox13.Text = "Không đạt";
            // 
            // xrCheckBox14
            // 
            this.xrCheckBox14.EditOptions.Enabled = true;
            this.xrCheckBox14.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox14.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox14.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 1.999982F);
            this.xrCheckBox14.Name = "xrCheckBox14";
            this.xrCheckBox14.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox14.StylePriority.UseFont = false;
            this.xrCheckBox14.StylePriority.UseTextAlignment = false;
            this.xrCheckBox14.Text = "Đạt";
            // 
            // xrTableRow62
            // 
            this.xrTableRow62.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell150,
            this.xrTableCell151,
            this.xrTableCell152});
            this.xrTableRow62.Name = "xrTableRow62";
            this.xrTableRow62.Weight = 1D;
            // 
            // xrTableCell150
            // 
            this.xrTableCell150.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell150.Name = "xrTableCell150";
            this.xrTableCell150.StylePriority.UseBorders = false;
            this.xrTableCell150.StylePriority.UseTextAlignment = false;
            this.xrTableCell150.Text = "+ Thử kéo:";
            this.xrTableCell150.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell150.Weight = 9.4107190860698626D;
            // 
            // xrTableCell151
            // 
            this.xrTableCell151.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell151.Name = "xrTableCell151";
            this.xrTableCell151.StylePriority.UseBorders = false;
            this.xrTableCell151.StylePriority.UseTextAlignment = false;
            this.xrTableCell151.Text = "Kết quả";
            this.xrTableCell151.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell151.Weight = 1.6057251691561976D;
            // 
            // xrTableCell152
            // 
            this.xrTableCell152.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell152.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox15,
            this.xrCheckBox16});
            this.xrTableCell152.Name = "xrTableCell152";
            this.xrTableCell152.StylePriority.UseBorders = false;
            this.xrTableCell152.StylePriority.UseTextAlignment = false;
            this.xrTableCell152.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell152.Weight = 5.93947211102764D;
            // 
            // xrCheckBox15
            // 
            this.xrCheckBox15.EditOptions.Enabled = true;
            this.xrCheckBox15.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox15.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox15.LocationFloat = new DevExpress.Utils.PointFloat(67.8517F, 1.999982F);
            this.xrCheckBox15.Name = "xrCheckBox15";
            this.xrCheckBox15.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox15.StylePriority.UseFont = false;
            this.xrCheckBox15.StylePriority.UseTextAlignment = false;
            this.xrCheckBox15.Text = "Không đạt";
            // 
            // xrCheckBox16
            // 
            this.xrCheckBox16.EditOptions.Enabled = true;
            this.xrCheckBox16.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox16.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox16.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-05F, 1.999982F);
            this.xrCheckBox16.Name = "xrCheckBox16";
            this.xrCheckBox16.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox16.StylePriority.UseFont = false;
            this.xrCheckBox16.StylePriority.UseTextAlignment = false;
            this.xrCheckBox16.Text = "Đạt";
            // 
            // xrTableRow63
            // 
            this.xrTableRow63.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell153});
            this.xrTableRow63.Name = "xrTableRow63";
            this.xrTableRow63.Weight = 1D;
            // 
            // xrTableCell153
            // 
            this.xrTableCell153.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell153.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell153.Name = "xrTableCell153";
            this.xrTableCell153.StylePriority.UseBorders = false;
            this.xrTableCell153.StylePriority.UseFont = false;
            this.xrTableCell153.StylePriority.UseTextAlignment = false;
            this.xrTableCell153.Text = "6. Hệ thống cứu hộ (ngắt điện nguồn cung cấp cho thang máy và kiểm tra):";
            this.xrTableCell153.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell153.Weight = 16.9559163662537D;
            // 
            // xrTableRow66
            // 
            this.xrTableRow66.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell162,
            this.xrTableCell164});
            this.xrTableRow66.Name = "xrTableRow66";
            this.xrTableRow66.Weight = 1D;
            // 
            // xrTableCell162
            // 
            this.xrTableCell162.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell162.Name = "xrTableCell162";
            this.xrTableCell162.StylePriority.UseBorders = false;
            this.xrTableCell162.StylePriority.UseTextAlignment = false;
            this.xrTableCell162.Text = "- Nhận xét:";
            this.xrTableCell162.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell162.Weight = 1.965347792566793D;
            // 
            // xrTableCell164
            // 
            this.xrTableCell164.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell164.Name = "xrTableCell164";
            this.xrTableCell164.StylePriority.UseBorders = false;
            this.xrTableCell164.StylePriority.UseTextAlignment = false;
            this.xrTableCell164.Text = "................................................................................." +
    "...............................................";
            this.xrTableCell164.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell164.Weight = 14.990568573686907D;
            // 
            // xrTableRow65
            // 
            this.xrTableRow65.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell159});
            this.xrTableRow65.Name = "xrTableRow65";
            this.xrTableRow65.Weight = 1D;
            // 
            // xrTableCell159
            // 
            this.xrTableCell159.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell159.Name = "xrTableCell159";
            this.xrTableCell159.StylePriority.UseBorders = false;
            this.xrTableCell159.StylePriority.UseTextAlignment = false;
            this.xrTableCell159.Text = "- Đánh giá kết quả:";
            this.xrTableCell159.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell159.Weight = 16.9559163662537D;
            // 
            // xrTableRow64
            // 
            this.xrTableRow64.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell156,
            this.xrTableCell157,
            this.xrTableCell158});
            this.xrTableRow64.Name = "xrTableRow64";
            this.xrTableRow64.Weight = 1D;
            // 
            // xrTableCell156
            // 
            this.xrTableCell156.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell156.Name = "xrTableCell156";
            this.xrTableCell156.StylePriority.UseBorders = false;
            this.xrTableCell156.StylePriority.UseTextAlignment = false;
            this.xrTableCell156.Text = "+ Bộ cứu hộ tự động (nếu có)";
            this.xrTableCell156.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell156.Weight = 9.4252918284324458D;
            // 
            // xrTableCell157
            // 
            this.xrTableCell157.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell157.Name = "xrTableCell157";
            this.xrTableCell157.StylePriority.UseBorders = false;
            this.xrTableCell157.StylePriority.UseTextAlignment = false;
            this.xrTableCell157.Text = "Kết quả";
            this.xrTableCell157.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell157.Weight = 1.5911524267936144D;
            // 
            // xrTableCell158
            // 
            this.xrTableCell158.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell158.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox17,
            this.xrCheckBox18});
            this.xrTableCell158.Name = "xrTableCell158";
            this.xrTableCell158.StylePriority.UseBorders = false;
            this.xrTableCell158.StylePriority.UseTextAlignment = false;
            this.xrTableCell158.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell158.Weight = 5.93947211102764D;
            // 
            // xrCheckBox17
            // 
            this.xrCheckBox17.EditOptions.Enabled = true;
            this.xrCheckBox17.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox17.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox17.LocationFloat = new DevExpress.Utils.PointFloat(67.85164F, 2.000046F);
            this.xrCheckBox17.Name = "xrCheckBox17";
            this.xrCheckBox17.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox17.StylePriority.UseFont = false;
            this.xrCheckBox17.StylePriority.UseTextAlignment = false;
            this.xrCheckBox17.Text = "Không đạt";
            // 
            // xrCheckBox18
            // 
            this.xrCheckBox18.EditOptions.Enabled = true;
            this.xrCheckBox18.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox18.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox18.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.000046F);
            this.xrCheckBox18.Name = "xrCheckBox18";
            this.xrCheckBox18.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox18.StylePriority.UseFont = false;
            this.xrCheckBox18.StylePriority.UseTextAlignment = false;
            this.xrCheckBox18.Text = "Đạt";
            // 
            // xrTableRow67
            // 
            this.xrTableRow67.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell154,
            this.xrTableCell155,
            this.xrTableCell160});
            this.xrTableRow67.Name = "xrTableRow67";
            this.xrTableRow67.Weight = 1D;
            // 
            // xrTableCell154
            // 
            this.xrTableCell154.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell154.Name = "xrTableCell154";
            this.xrTableCell154.StylePriority.UseBorders = false;
            this.xrTableCell154.StylePriority.UseTextAlignment = false;
            this.xrTableCell154.Text = "+ Hệ thống thông tin liên lạc (chuông, điện thoại liên lạc nội Bộ)";
            this.xrTableCell154.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell154.Weight = 9.410720420142491D;
            // 
            // xrTableCell155
            // 
            this.xrTableCell155.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell155.Name = "xrTableCell155";
            this.xrTableCell155.StylePriority.UseBorders = false;
            this.xrTableCell155.StylePriority.UseTextAlignment = false;
            this.xrTableCell155.Text = "Kết quả";
            this.xrTableCell155.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell155.Weight = 1.6057238350835683D;
            // 
            // xrTableCell160
            // 
            this.xrTableCell160.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell160.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox19,
            this.xrCheckBox20});
            this.xrTableCell160.Name = "xrTableCell160";
            this.xrTableCell160.StylePriority.UseBorders = false;
            this.xrTableCell160.StylePriority.UseTextAlignment = false;
            this.xrTableCell160.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell160.Weight = 5.93947211102764D;
            // 
            // xrCheckBox19
            // 
            this.xrCheckBox19.EditOptions.Enabled = true;
            this.xrCheckBox19.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox19.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox19.LocationFloat = new DevExpress.Utils.PointFloat(67.85158F, 1.999982F);
            this.xrCheckBox19.Name = "xrCheckBox19";
            this.xrCheckBox19.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrCheckBox19.StylePriority.UseFont = false;
            this.xrCheckBox19.StylePriority.UseTextAlignment = false;
            this.xrCheckBox19.Text = "Không đạt";
            // 
            // xrCheckBox20
            // 
            this.xrCheckBox20.EditOptions.Enabled = true;
            this.xrCheckBox20.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox20.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.xrCheckBox20.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1.999982F);
            this.xrCheckBox20.Name = "xrCheckBox20";
            this.xrCheckBox20.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.xrCheckBox20.StylePriority.UseFont = false;
            this.xrCheckBox20.StylePriority.UseTextAlignment = false;
            this.xrCheckBox20.Text = "Đạt";
            // 
            // SubBand5
            // 
            this.SubBand5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.SubBand5.HeightF = 285.0001F;
            this.SubBand5.Name = "SubBand5";
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(6.000066F, 10F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow38,
            this.xrTableRow39,
            this.xrTableRow40,
            this.xrTableRow41,
            this.xrTableRow42,
            this.xrTableRow44,
            this.xrTableRow48,
            this.xrTableRow49,
            this.xrTableRow45,
            this.xrTableRow50,
            this.xrTableRow51});
            this.xrTable4.SizeF = new System.Drawing.SizeF(775.7501F, 275.0001F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseFont = false;
            // 
            // xrTableRow38
            // 
            this.xrTableRow38.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell90});
            this.xrTableRow38.Name = "xrTableRow38";
            this.xrTableRow38.Weight = 1D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.Text = "IV - Kiến nghị và kết luận:";
            this.xrTableCell90.Weight = 2D;
            // 
            // xrTableRow39
            // 
            this.xrTableRow39.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell92,
            this.xrTableCell93,
            this.xrtAccreditationResult});
            this.xrTableRow39.Name = "xrTableRow39";
            this.xrTableRow39.Weight = 1D;
            // 
            // xrTableCell92
            // 
            this.xrTableCell92.Name = "xrTableCell92";
            this.xrTableCell92.Weight = 0.12203257636632386D;
            // 
            // xrTableCell93
            // 
            this.xrTableCell93.Name = "xrTableCell93";
            this.xrTableCell93.StylePriority.UseTextAlignment = false;
            this.xrTableCell93.Text = "1. Thiết bị được kiểm định có kết quả:";
            this.xrTableCell93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell93.Weight = 0.65020355409870212D;
            // 
            // xrtAccreditationResult
            // 
            this.xrtAccreditationResult.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.cbKDDat,
            this.cbKDKhongDat});
            this.xrtAccreditationResult.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrtAccreditationResult.Name = "xrtAccreditationResult";
            this.xrtAccreditationResult.StylePriority.UseFont = false;
            this.xrtAccreditationResult.Text = "Đạt yêu cầu";
            this.xrtAccreditationResult.Weight = 1.227763869534974D;
            // 
            // cbKDDat
            // 
            this.cbKDDat.EditOptions.Enabled = true;
            this.cbKDDat.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbKDDat.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.178914E-05F);
            this.cbKDDat.Name = "cbKDDat";
            this.cbKDDat.SizeF = new System.Drawing.SizeF(52.15457F, 23F);
            this.cbKDDat.StylePriority.UseTextAlignment = false;
            this.cbKDDat.Text = "Đạt";
            // 
            // cbKDKhongDat
            // 
            this.cbKDKhongDat.EditOptions.Enabled = true;
            this.cbKDKhongDat.GlyphOptions.Alignment = DevExpress.Utils.HorzAlignment.Far;
            this.cbKDKhongDat.LocationFloat = new DevExpress.Utils.PointFloat(67.85202F, 3.178914E-05F);
            this.cbKDKhongDat.Name = "cbKDKhongDat";
            this.cbKDKhongDat.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.cbKDKhongDat.StylePriority.UseTextAlignment = false;
            this.cbKDKhongDat.Text = "Không đạt";
            // 
            // xrTableRow40
            // 
            this.xrTableRow40.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell116,
            this.xrTableCell96,
            this.xrTableCell97});
            this.xrTableRow40.Name = "xrTableRow40";
            this.xrTableRow40.Weight = 1D;
            // 
            // xrTableCell116
            // 
            this.xrTableCell116.Name = "xrTableCell116";
            this.xrTableCell116.Weight = 0.12471839840615442D;
            // 
            // xrTableCell96
            // 
            this.xrTableCell96.Name = "xrTableCell96";
            this.xrTableCell96.Text = "Đủ điều kiện hoạt động với trọng tải lớn nhất là:";
            this.xrTableCell96.Weight = 0.80776142104781556D;
            // 
            // xrTableCell97
            // 
            this.xrTableCell97.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CorrespondingLoad]")});
            this.xrTableCell97.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell97.Name = "xrTableCell97";
            this.xrTableCell97.StylePriority.UseFont = false;
            this.xrTableCell97.Weight = 1.06752018054603D;
            // 
            // xrTableRow41
            // 
            this.xrTableRow41.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell117,
            this.xrTableCell98,
            this.xrTableCell118,
            this.xrTableCell119,
            this.xrTableCell99});
            this.xrTableRow41.Name = "xrTableRow41";
            this.xrTableRow41.Weight = 1D;
            // 
            // xrTableCell117
            // 
            this.xrTableCell117.Name = "xrTableCell117";
            this.xrTableCell117.Weight = 0.12471839840615442D;
            // 
            // xrTableCell98
            // 
            this.xrTableCell98.Name = "xrTableCell98";
            this.xrTableCell98.Text = "2. Đã dán tem kiểm định số:";
            this.xrTableCell98.Weight = 0.50161610380704347D;
            // 
            // xrTableCell118
            // 
            this.xrTableCell118.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StampNumber]")});
            this.xrTableCell118.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell118.Name = "xrTableCell118";
            this.xrTableCell118.StylePriority.UseFont = false;
            this.xrTableCell118.Weight = 0.36404035471906959D;
            // 
            // xrTableCell119
            // 
            this.xrTableCell119.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell119.Name = "xrTableCell119";
            this.xrTableCell119.StylePriority.UseFont = false;
            this.xrTableCell119.Text = "Tại vị trí:";
            this.xrTableCell119.Weight = 0.18938257214194987D;
            // 
            // xrTableCell99
            // 
            this.xrTableCell99.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StampLocated]")});
            this.xrTableCell99.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell99.Name = "xrTableCell99";
            this.xrTableCell99.StylePriority.UseFont = false;
            this.xrTableCell99.Weight = 0.82024257092578279D;
            // 
            // xrTableRow42
            // 
            this.xrTableRow42.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell120,
            this.xrTableCell101,
            this.xrTableCell100});
            this.xrTableRow42.Name = "xrTableRow42";
            this.xrTableRow42.Weight = 1D;
            // 
            // xrTableCell120
            // 
            this.xrTableCell120.Name = "xrTableCell120";
            this.xrTableCell120.Weight = 0.12471839840615442D;
            // 
            // xrTableCell101
            // 
            this.xrTableCell101.Name = "xrTableCell101";
            this.xrTableCell101.Text = "Kiến nghị:";
            this.xrTableCell101.Weight = 0.18568039830720806D;
            // 
            // xrTableCell100
            // 
            this.xrTableCell100.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Requests]")});
            this.xrTableCell100.Multiline = true;
            this.xrTableCell100.Name = "xrTableCell100";
            this.xrTableCell100.Weight = 1.6896012032866374D;
            // 
            // xrTableRow44
            // 
            this.xrTableRow44.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell103,
            this.xrTableCell104});
            this.xrTableRow44.Name = "xrTableRow44";
            this.xrTableRow44.Weight = 1D;
            // 
            // xrTableCell103
            // 
            this.xrTableCell103.Name = "xrTableCell103";
            this.xrTableCell103.Text = "Thời gian thực hiện kiến nghị:";
            this.xrTableCell103.Weight = 0.51928238712391583D;
            // 
            // xrTableCell104
            // 
            this.xrTableCell104.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RequestsTime]")});
            this.xrTableCell104.Name = "xrTableCell104";
            this.xrTableCell104.Weight = 1.4807176128760842D;
            // 
            // xrTableRow48
            // 
            this.xrTableRow48.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell105,
            this.xrTableCell106});
            this.xrTableRow48.Name = "xrTableRow48";
            this.xrTableRow48.Weight = 1D;
            // 
            // xrTableCell105
            // 
            this.xrTableCell105.Name = "xrTableCell105";
            this.xrTableCell105.Text = "V. Thời gian kiểm định lần sau:";
            this.xrTableCell105.Weight = 0.55150926180393856D;
            // 
            // xrTableCell106
            // 
            this.xrTableCell106.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateOfNext]")});
            this.xrTableCell106.Name = "xrTableCell106";
            this.xrTableCell106.TextFormatString = "{0:dd/MM/yyy}";
            this.xrTableCell106.Weight = 1.4484907381960614D;
            // 
            // xrTableRow49
            // 
            this.xrTableRow49.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell121,
            this.xrTableCell122});
            this.xrTableRow49.Name = "xrTableRow49";
            this.xrTableRow49.Weight = 1D;
            // 
            // xrTableCell121
            // 
            this.xrTableCell121.Name = "xrTableCell121";
            this.xrTableCell121.Text = "Bên bản được thông qua ngày:";
            this.xrTableCell121.Weight = 0.55150926180393856D;
            // 
            // xrTableCell122
            // 
            this.xrTableCell122.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CreateDate]")});
            this.xrTableCell122.Name = "xrTableCell122";
            this.xrTableCell122.TextFormatString = "{0:dd/MM/yyy}";
            this.xrTableCell122.Weight = 1.4484907381960614D;
            // 
            // xrTableRow45
            // 
            this.xrTableRow45.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell123,
            this.xrTableCell107});
            this.xrTableRow45.Name = "xrTableRow45";
            this.xrTableRow45.Weight = 1D;
            // 
            // xrTableCell123
            // 
            this.xrTableCell123.Name = "xrTableCell123";
            this.xrTableCell123.Text = "Tại:";
            this.xrTableCell123.Weight = 0.096251115961752287D;
            // 
            // xrTableCell107
            // 
            this.xrTableCell107.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.contract.customer.Name]")});
            this.xrTableCell107.Name = "xrTableCell107";
            this.xrTableCell107.Weight = 1.9037488840382477D;
            // 
            // xrTableRow50
            // 
            this.xrTableRow50.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell125});
            this.xrTableRow50.Name = "xrTableRow50";
            this.xrTableRow50.Weight = 1D;
            // 
            // xrTableCell125
            // 
            this.xrTableCell125.Name = "xrTableCell125";
            this.xrTableCell125.Text = "Bên bản được lập thành 02 bản, mỗi bên giữ 01 bản";
            this.xrTableCell125.Weight = 2D;
            // 
            // xrTableRow51
            // 
            this.xrTableRow51.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell124});
            this.xrTableRow51.Name = "xrTableRow51";
            this.xrTableRow51.Weight = 1D;
            // 
            // xrTableCell124
            // 
            this.xrTableCell124.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell124.Name = "xrTableCell124";
            this.xrTableCell124.StylePriority.UseFont = false;
            this.xrTableCell124.Text = "Chúng tôi, những kiểm định viên thực hiện việc kiểm định thiết bị này hoàn toàn c" +
    "hịu trách nhiệm về tính chính xác các nhận xét và đánh giá kết quả kiểm định ghi" +
    " trong biên bản ./.";
            this.xrTableCell124.Weight = 2D;
            // 
            // SubBand10
            // 
            this.SubBand10.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel28,
            this.xrLabel7,
            this.xrLabel8,
            this.xrLabel9,
            this.xrLabel10,
            this.xrTable1});
            this.SubBand10.HeightF = 182.8019F;
            this.SubBand10.Name = "SubBand10";
            // 
            // xrLabel28
            // 
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(7.749096F, 44.37497F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(179.0461F, 18F);
            this.xrLabel28.StyleName = "FieldCaption";
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "ĐƠN VỊ SỬ DỤNG";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(257.8788F, 44.37497F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(179.0461F, 18F);
            this.xrLabel7.StyleName = "FieldCaption";
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "NGƯỜI CHỨNG KIẾN";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(572.7536F, 44.37497F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(179.0461F, 18F);
            this.xrLabel8.StyleName = "FieldCaption";
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "KIỂM ĐỊNH VIÊN";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(7.170248F, 72.84393F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(179.0461F, 43F);
            this.xrLabel9.StyleName = "FieldCaption";
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "(Ký tên và đóng dấu)\r\n(Cam kết thực hiện dầy đủ, đúng hạn các kiến nghị)";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel10
            // 
            this.xrLabel10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.contract.own.DisplayName]")});
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(572.7537F, 164.8019F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(179.046F, 18F);
            this.xrLabel10.StyleName = "FieldCaption";
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(245.7765F, 107.8019F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow15,
            this.xrTableRow17,
            this.xrTableRow16});
            this.xrTable1.SizeF = new System.Drawing.SizeF(264.6119F, 74.99997F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell111,
            this.xrTableCell51});
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.Weight = 1D;
            // 
            // xrTableCell111
            // 
            this.xrTableCell111.Name = "xrTableCell111";
            this.xrTableCell111.Text = "1.";
            this.xrTableCell111.Weight = 0.24040345513558548D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Viewer1]")});
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.Weight = 1.7595965448644146D;
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell52});
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Weight = 1D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.Weight = 2D;
            // 
            // xrTableRow16
            // 
            this.xrTableRow16.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell112,
            this.xrTableCell53});
            this.xrTableRow16.Name = "xrTableRow16";
            this.xrTableRow16.Weight = 1D;
            // 
            // xrTableCell112
            // 
            this.xrTableCell112.Name = "xrTableCell112";
            this.xrTableCell112.Text = "2.";
            this.xrTableCell112.Weight = 0.24040345513558548D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Viewer2]")});
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.Weight = 1.7595965448644146D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 15F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 15F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // pageFooterBand
            // 
            this.pageFooterBand.HeightF = 20.83333F;
            this.pageFooterBand.Name = "pageFooterBand";
            // 
            // reportHeaderBand
            // 
            this.reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel29,
            this.xrLabel38,
            this.xrLabel37,
            this.xrLabel36,
            this.xrLabel35,
            this.xrLabel34,
            this.xrPictureBoxLeft});
            this.reportHeaderBand.HeightF = 151F;
            this.reportHeaderBand.Name = "reportHeaderBand";
            // 
            // xrLabel32
            // 
            this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel32.EditOptions.Enabled = true;
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(436.9249F, 128F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(350.0751F, 23.00003F);
            this.xrLabel32.StylePriority.UseBorders = false;
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "............, ngày ......... tháng ......... năm 20......";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel31
            // 
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(495.5833F, 36F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(291.4166F, 15F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "----o0o----";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel30
            // 
            this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(495.5834F, 18F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(291.4166F, 15F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "Độc lập - Tự do - Hạnh phúc";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(495.5833F, 0F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(291.4166F, 15F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel38
            // 
            this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(6.000042F, 68.99999F);
            this.xrLabel38.Multiline = true;
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(430.9249F, 82.00004F);
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.StylePriority.UseTextAlignment = false;
            this.xrLabel38.Text = resources.GetString("xrLabel38.Text");
            this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel37
            // 
            this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(96.29172F, 33F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(377.0915F, 18F);
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.StylePriority.UseTextAlignment = false;
            this.xrLabel37.Text = "CÔNG TY CP KIỂM ĐỊNH KỸ THUẬT, AN TOÀN VÀ TƯ VẤN XÂY DỰNG-INCOSAF";
            this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel36
            // 
            this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(96.29172F, 15F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(377.0915F, 18F);
            this.xrLabel36.StylePriority.UseFont = false;
            this.xrLabel36.StylePriority.UseTextAlignment = false;
            this.xrLabel36.Text = "TỔNG CÔNG TY TƯ VẤN XÂY DỰNG VIỆT NAM";
            this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel35
            // 
            this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(96.29172F, 0F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(377.0915F, 15F);
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.StylePriority.UseTextAlignment = false;
            this.xrLabel35.Text = "BỘ XÂY DỰNG";
            this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel34
            // 
            this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 5.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(6.000042F, 50.99999F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(90.29167F, 13F);
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "ISO 9001 : 2008";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPictureBoxLeft
            // 
            this.xrPictureBoxLeft.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleLeft;
            this.xrPictureBoxLeft.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBoxLeft.ImageSource"));
            this.xrPictureBoxLeft.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 1.999998F);
            this.xrPictureBoxLeft.Name = "xrPictureBoxLeft";
            this.xrPictureBoxLeft.SizeF = new System.Drawing.SizeF(90.29171F, 48.99999F);
            this.xrPictureBoxLeft.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Times New Roman", 24F);
            this.Title.ForeColor = System.Drawing.Color.Black;
            this.Title.Name = "Title";
            // 
            // FieldCaption
            // 
            this.FieldCaption.BackColor = System.Drawing.Color.Transparent;
            this.FieldCaption.BorderColor = System.Drawing.Color.Black;
            this.FieldCaption.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.FieldCaption.BorderWidth = 1F;
            this.FieldCaption.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.FieldCaption.ForeColor = System.Drawing.Color.Black;
            this.FieldCaption.Name = "FieldCaption";
            // 
            // PageInfo
            // 
            this.PageInfo.BackColor = System.Drawing.Color.Transparent;
            this.PageInfo.BorderColor = System.Drawing.Color.Black;
            this.PageInfo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PageInfo.BorderWidth = 1F;
            this.PageInfo.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.PageInfo.ForeColor = System.Drawing.Color.Black;
            this.PageInfo.Name = "PageInfo";
            // 
            // DataField
            // 
            this.DataField.BackColor = System.Drawing.Color.Transparent;
            this.DataField.BorderColor = System.Drawing.Color.Black;
            this.DataField.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DataField.BorderWidth = 1F;
            this.DataField.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.DataField.ForeColor = System.Drawing.Color.Black;
            this.DataField.Name = "DataField";
            this.DataField.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // objectDataSource
            // 
            this.objectDataSource.DataSource = typeof(IncosafCMS.Core.DomainModels.Accreditation);
            this.objectDataSource.Name = "objectDataSource";
            // 
            // AccreditationReportTM
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.pageFooterBand,
            this.reportHeaderBand});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource});
            this.DataSource = this.objectDataSource;
            this.Margins = new System.Drawing.Printing.Margins(20, 20, 15, 15);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "19.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTester)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableChungKien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableSpecification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableDacTinhKyThuat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTechnicalDocument2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTechnicalDocument1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTablePartion1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTablePartion2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableLoadTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
