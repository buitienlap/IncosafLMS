using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using IncosafCMS.Web.Helpers;
using IncosafCMS.Core.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

/// <summary>
/// Summary description for ContractReport
/// </summary>
public class ContractSuggestReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource;
    private PageFooterBand pageFooterBand;
    private ReportHeaderBand reportHeaderBand;
    private XRLabel xrLabel33;
    private XRControlStyle Title;
    private XRControlStyle FieldCaption;
    private XRControlStyle PageInfo;
    private XRControlStyle DataField;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private XRLabel xrLabel29;
    private XRLabel xrLabel32;
    private SubBand SubBand1;
    private SubBand SubBand6;
    private SubBand SubBand7;
    private XRTable xrTable2;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableRow xrTableRow22;
    private XRTableCell xrTableCell5;
    private XRTable xrTableTasks;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    private SubBand SubBand9;
    private XRLabel xrLabel8;
    private XRLabel xrLabel28;
    private XRLabel xrLabel39;
    private XRLabel xrLabel1;
    private XRLabel xrLabel2;
    private XRTableRow xrTableRow15;
    private XRTableCell xrTableCell23;
    private XRTable xrTableChungKien;
    private XRTableRow xrTableRow24;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell63;
    private XRTableRow xrTableRow27;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell70;
    private XRTableRow xrTableRow28;
    private XRTableCell xrTableCell72;
    private XRTableCell xrTableCell78;
    private XRTableRow xrTableRow29;
    private XRTableCell xrTableCell75;
    private XRTableCell xrTableCell76;
    private XRTableRow xrTableRow26;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell6;
    private XRLabel xrLabel3;
    private XRLabel xrLabel4;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCellValueToWord;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell11;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ContractSuggestReport()
    {
        InitializeComponent();
        objectDataSource.DataSource = ReportHelper.ContractReportDataSource;
        var contract = (objectDataSource.DataSource as Contract);
        contract.own = ReportHelper.ContractReportDataSource.own;
        contract.KDV1 = ReportHelper.ContractReportDataSource.KDV1;
        contract.KDV2 = ReportHelper.ContractReportDataSource.KDV2;
        contract.own.Department = ReportHelper.ContractReportDataSource.own.Department;     // lapbt xly loi k0 in dc o ReportsController.ContractSuggestReport
        contract.customer = ReportHelper.ContractReportDataSource.customer;
        contract.Proprietor = ReportHelper.ContractReportDataSource.Proprietor;

        // edited & added by Lapbt 08/11/2020
        var local = contract.own?.Department?.MaDV == "HCM" ? "TP. Hồ Chí Minh" : contract.own?.Department?.MaDV == "DN" ? "Đà Nẵng" : "Hà Nội";
        xrLabel32.Text = $"{local}, ngày {contract.SignDate?.ToString("dd")} tháng {contract.SignDate?.ToString("MM")} năm {contract.SignDate?.Year}";

        var contractno = contract.own?.Department?.MaDV == "HCM" ? "Số: ......................... /KĐXD-SG" : contract.own?.Department?.MaDV == "DN" ? "Số: ......................... /KĐXD-ĐN" : "Số: ......................... /KĐXD";
        xrLabel39.Text = $"{contractno}";

        if(contract.own?.Department?.MaDV == "HCM" || contract.own?.Department?.MaDV == "DN")
        {
            xrTableCell1.Text = "Đề nghị Giám đốc cho phép thực hiện công việc như sau:";
            xrLabel28.Text = "GIÁM ĐỐC";
        } else
        {
            xrTableCell1.Text = "Đề nghị Tổng Giám đốc cho phép thực hiện công việc như sau:";
            xrLabel28.Text = "TỔNG GIÁM ĐỐC";
        }

        string ptdv = ConfigurationManager.AppSettings["in_giay_de_nghi_phu_trach_don_vi"];
        if (ptdv != "Không hiển thị")
            xrLabel3.Text = ptdv;
        else
            xrLabel3.Visible = false;
        //var firstTask = contract.Tasks.FirstOrDefault();
        //if (firstTask != null)
        //{
        //    var firstAccreditation = firstTask.Accreditations.FirstOrDefault();
        //    if (firstAccreditation != null && !string.IsNullOrEmpty(firstAccreditation.AccrLocation))
        //    {
        //        xrLabel32.Text = $"{firstAccreditation.AccrLocation}, ngày {contract.NgayThucHien.Value.Day} tháng {contract.NgayThucHien.Value.Month} năm {contract.NgayThucHien.Value.Year}";
        //        xrTableCell78.Text = firstAccreditation.AccrLocation;
        //    }
        //    else
        //        xrLabel32.Text = $"............, ngày {contract.NgayThucHien.Value.Day} tháng {contract.NgayThucHien.Value.Month} năm {contract.NgayThucHien.Value.Year}";
        //}
        //else
        //    xrLabel32.Text = "............, ngày ......... tháng ......... năm 20......";


        var InsertRow = xrTableTasks.Rows.FirstRow;
        int stt = 1;
        double tong_noVAT = 0;
        double mucVAT = 0;
        double VAT = 0;
        foreach (var item in contract.Tasks)
        {
            InsertRow = xrTableTasks.InsertRowBelow(InsertRow);

            InsertRow.Cells[0].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            InsertRow.Cells[0].Text = stt.ToString();

            InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
            InsertRow.Cells[1].Text = item.Name;

            InsertRow.Cells[2].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            InsertRow.Cells[2].Text = item.Unit;

            InsertRow.Cells[3].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            InsertRow.Cells[3].Text = item.Amount.ToString();

            InsertRow.Cells[4].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            InsertRow.Cells[4].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
            InsertRow.Cells[4].Text = item.UnitPrice_noVAT.ToString("N0");

            InsertRow.Cells[5].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[5].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            InsertRow.Cells[5].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
            InsertRow.Cells[5].Text = (item.Amount * item.UnitPrice_noVAT).ToString("N0");

            tong_noVAT += item.Amount * item.UnitPrice_noVAT;
            VAT += item.Amount * item.UnitPrice_noVAT * item.VAT / 100;
            if (item.VAT > 0) mucVAT = item.VAT;
            stt++;
        }

        InsertRow = xrTableTasks.InsertRowBelow(InsertRow);
        InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Bold);
        InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
        InsertRow.Cells[1].Text = "Tổng tiền chưa VAT:";

        InsertRow.Cells[5].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Bold);
        InsertRow.Cells[5].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        InsertRow.Cells[5].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
        InsertRow.Cells[5].Text = tong_noVAT.ToString("N0");

        InsertRow = xrTableTasks.InsertRowBelow(InsertRow);
        InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11);
        InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
        InsertRow.Cells[1].Text = "Tiền thuế VAT " + mucVAT + " %";

        InsertRow.Cells[5].Font = new Font(new FontFamily("Times New Roman"), 11);
        InsertRow.Cells[5].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        InsertRow.Cells[5].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
        InsertRow.Cells[5].Text = VAT.ToString("N0");

        InsertRow = xrTableTasks.InsertRowBelow(InsertRow);
        InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Bold);
        InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
        InsertRow.Cells[1].Text = "Tổng cộng:";

        InsertRow.Cells[5].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Bold);
        InsertRow.Cells[5].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        InsertRow.Cells[5].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
        InsertRow.Cells[5].Text = (tong_noVAT + VAT).ToString("N0");

        xrTableCellValueToWord.Text = NumberToWord.DocTienBangChu((long)contract.Value, " đồng");
        //
        // TODO: Add constructor logic here
        //
        string com_name = ConfigurationManager.AppSettings["company_name"];
        xrLabel2.Text = com_name;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractSuggestReport));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.SubBand7 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand6 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTableTasks = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand1 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTableChungKien = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellValueToWord = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow27 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow28 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow29 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand9 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.pageFooterBand = new DevExpress.XtraReports.UI.PageFooterBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableChungKien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel33});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 124.46F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.StylePriority.UseBorders = false;
            this.Detail.SubBands.AddRange(new DevExpress.XtraReports.UI.SubBand[] {
            this.SubBand7,
            this.SubBand6,
            this.SubBand1,
            this.SubBand9});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel33
            // 
            this.xrLabel33.Dpi = 254F;
            this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(15.24003F, 25.4F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(1770F, 99.06F);
            this.xrLabel33.StyleName = "Title";
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "GIẤY ĐỀ NGHỊ THỰC HIỆN CÔNG VIỆC";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // SubBand7
            // 
            this.SubBand7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.SubBand7.Dpi = 254F;
            this.SubBand7.HeightF = 216F;
            this.SubBand7.Name = "SubBand7";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Dpi = 254F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(16.19273F, 25.4F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow22,
            this.xrTableRow15,
            this.xrTableRow1});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1768.913F, 190.5001F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell5});
            this.xrTableRow22.Dpi = 254F;
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "Tôi tên là:";
            this.xrTableCell7.Weight = 0.32883591906389609D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[own.DisplayName]")});
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.TextFormatString = "{0}";
            this.xrTableCell5.Weight = 2.6711640809361041D;
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell8,
            this.xrTableCell23});
            this.xrTableRow15.Dpi = 254F;
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.Weight = 1D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "Đơn vị:";
            this.xrTableCell8.Weight = 0.32883591906389609D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[own.department.Name]")});
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.TextFormatString = "{0}";
            this.xrTableCell23.Weight = 2.6711640809361041D;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.CanShrink = true;
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.EditOptions.Enabled = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "Đề nghị Tổng Giám đốc cho phép thực hiện công việc như sau:";
            this.xrTableCell1.Weight = 3D;
            // 
            // SubBand6
            // 
            this.SubBand6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableTasks});
            this.SubBand6.Dpi = 254F;
            this.SubBand6.HeightF = 89F;
            this.SubBand6.Name = "SubBand6";
            // 
            // xrTableTasks
            // 
            this.xrTableTasks.Dpi = 254F;
            this.xrTableTasks.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableTasks.LocationFloat = new DevExpress.Utils.PointFloat(22.11905F, 25.4F);
            this.xrTableTasks.Name = "xrTableTasks";
            this.xrTableTasks.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow13});
            this.xrTableTasks.SizeF = new System.Drawing.SizeF(1762.987F, 63.5F);
            this.xrTableTasks.StylePriority.UseFont = false;
            this.xrTableTasks.StylePriority.UseTextAlignment = false;
            this.xrTableTasks.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell42,
            this.xrTableCell43});
            this.xrTableRow13.Dpi = 254F;
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.Dpi = 254F;
            this.xrTableCell38.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.StylePriority.UseFont = false;
            this.xrTableCell38.Text = "TT";
            this.xrTableCell38.Weight = 0.96224834679905014D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Dpi = 254F;
            this.xrTableCell39.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StylePriority.UseFont = false;
            this.xrTableCell39.Text = "Nội dung công việc";
            this.xrTableCell39.Weight = 4.8870320365360795D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Dpi = 254F;
            this.xrTableCell40.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseFont = false;
            this.xrTableCell40.Text = "ĐVT";
            this.xrTableCell40.Weight = 1.3576481455554919D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Dpi = 254F;
            this.xrTableCell41.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.StylePriority.UseFont = false;
            this.xrTableCell41.Text = "KL";
            this.xrTableCell41.Weight = 1.2152032164269468D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Dpi = 254F;
            this.xrTableCell42.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell42.Multiline = true;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseFont = false;
            this.xrTableCell42.Text = "Đơn giá chưa VAT (đ)";
            this.xrTableCell42.Weight = 1.8276216752029562D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Dpi = 254F;
            this.xrTableCell43.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.Text = "Thành tiền (đ)";
            this.xrTableCell43.Weight = 2.5445406359843235D;
            // 
            // SubBand1
            // 
            this.SubBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableChungKien});
            this.SubBand1.Dpi = 254F;
            this.SubBand1.HeightF = 723.9F;
            this.SubBand1.Name = "SubBand1";
            // 
            // xrTableChungKien
            // 
            this.xrTableChungKien.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableChungKien.Dpi = 254F;
            this.xrTableChungKien.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableChungKien.LocationFloat = new DevExpress.Utils.PointFloat(22.11905F, 25.4F);
            this.xrTableChungKien.Name = "xrTableChungKien";
            this.xrTableChungKien.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3,
            this.xrTableRow24,
            this.xrTableRow10,
            this.xrTableRow27,
            this.xrTableRow28,
            this.xrTableRow29,
            this.xrTableRow26,
            this.xrTableRow4,
            this.xrTableRow6,
            this.xrTableRow2,
            this.xrTableRow5});
            this.xrTableChungKien.SizeF = new System.Drawing.SizeF(1768.913F, 698.5F);
            this.xrTableChungKien.StylePriority.UseBorders = false;
            this.xrTableChungKien.StylePriority.UseFont = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCellValueToWord});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.Text = "Số tiền bằng chữ:";
            this.xrTableCell9.Weight = 0.91376786831022361D;
            // 
            // xrTableCellValueToWord
            // 
            this.xrTableCellValueToWord.Dpi = 254F;
            this.xrTableCellValueToWord.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCellValueToWord.Name = "xrTableCellValueToWord";
            this.xrTableCellValueToWord.StylePriority.UseFont = false;
            this.xrTableCellValueToWord.Weight = 2.6311966288856024D;
            // 
            // xrTableRow24
            // 
            this.xrTableRow24.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell56,
            this.xrTableCell57});
            this.xrTableRow24.Dpi = 254F;
            this.xrTableRow24.Name = "xrTableRow24";
            this.xrTableRow24.Weight = 1D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.Dpi = 254F;
            this.xrTableCell56.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.StylePriority.UseFont = false;
            this.xrTableCell56.Text = "- Đơn vị yêu cầu (Bên A):";
            this.xrTableCell56.Weight = 0.83117381255045075D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.Dpi = 254F;
            this.xrTableCell57.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Name]")});
            this.xrTableCell57.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.StylePriority.UseFont = false;
            this.xrTableCell57.Weight = 2.7137906846453754D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell35,
            this.xrTableCell63});
            this.xrTableRow10.Dpi = 254F;
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Dpi = 254F;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Text = "- Địa chỉ:";
            this.xrTableCell35.Weight = 0.83117405785450516D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.Dpi = 254F;
            this.xrTableCell63.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Address]")});
            this.xrTableCell63.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.StylePriority.UseFont = false;
            this.xrTableCell63.Weight = 2.713790351700315D;
            // 
            // xrTableRow27
            // 
            this.xrTableRow27.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell69,
            this.xrTableCell70});
            this.xrTableRow27.Dpi = 254F;
            this.xrTableRow27.Name = "xrTableRow27";
            this.xrTableRow27.Weight = 1D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.Dpi = 254F;
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.Text = "- Mã số thuế:";
            this.xrTableCell69.Weight = 0.831174057854505D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.Dpi = 254F;
            this.xrTableCell70.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.TaxID]")});
            this.xrTableCell70.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.StylePriority.UseFont = false;
            this.xrTableCell70.Weight = 2.713790351700315D;
            // 
            // xrTableRow28
            // 
            this.xrTableRow28.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell72,
            this.xrTableCell78});
            this.xrTableRow28.Dpi = 254F;
            this.xrTableRow28.Name = "xrTableRow28";
            this.xrTableRow28.Weight = 1D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.Dpi = 254F;
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.Text = "- Địa điểm thực hiện:";
            this.xrTableCell72.Weight = 0.831174057854505D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.Dpi = 254F;
            this.xrTableCell78.EditOptions.Enabled = true;
            this.xrTableCell78.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DiaDiemThucHien]")});
            this.xrTableCell78.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.StylePriority.UseFont = false;
            this.xrTableCell78.Weight = 2.7137903517003146D;
            // 
            // xrTableRow29
            // 
            this.xrTableRow29.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell75,
            this.xrTableCell76});
            this.xrTableRow29.Dpi = 254F;
            this.xrTableRow29.Name = "xrTableRow29";
            this.xrTableRow29.Weight = 1D;
            // 
            // xrTableCell75
            // 
            this.xrTableCell75.Dpi = 254F;
            this.xrTableCell75.Name = "xrTableCell75";
            this.xrTableCell75.Text = "- Thời gian thực hiện:";
            this.xrTableCell75.Weight = 0.831174057854505D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.Dpi = 254F;
            this.xrTableCell76.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", resources.GetString("xrTableCell76.ExpressionBindings"))});
            this.xrTableCell76.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell76.Multiline = true;
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.StylePriority.UseFont = false;
            this.xrTableCell76.TextFormatString = "{0:MM/yyyy}";
            this.xrTableCell76.Weight = 2.713790351700315D;
            // 
            // xrTableRow26
            // 
            this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell66,
            this.xrTableCell67,
            this.xrTableCell14,
            this.xrTableCell15});
            this.xrTableRow26.Dpi = 254F;
            this.xrTableRow26.Name = "xrTableRow26";
            this.xrTableRow26.Weight = 1D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.Dpi = 254F;
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.Text = "- Người liên hệ:";
            this.xrTableCell66.Weight = 0.83117375206295252D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.Dpi = 254F;
            this.xrTableCell67.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NguoiLienHe]")});
            this.xrTableCell67.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.StylePriority.UseFont = false;
            this.xrTableCell67.Weight = 1.0448727534394582D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.xrTableCell14.Multiline = true;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.Text = "- Số điện thoại liên hệ:";
            this.xrTableCell14.Weight = 0.75415900769412159D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DienThoaiNguoiLienHe]")});
            this.xrTableCell15.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.Text = "xrTableCell15";
            this.xrTableCell15.Weight = 0.91475889635828778D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell6});
            this.xrTableRow4.Dpi = 254F;
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "- Người chịu trách nhiệm kỹ thuật:";
            this.xrTableCell4.Weight = 1.1576756175781915D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.EditOptions.Enabled = true;
            this.xrTableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([KDV2].[DisplayName] is null,[KDV1].[DisplayName],[KDV1].[DisplayName] + \'; \'" +
                    " + [KDV2].[DisplayName])")});
            this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.Weight = 2.3872887919766281D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell11});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "- Loại hình hợp đồng:";
            this.xrTableCell10.Weight = 0.89043746889199449D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", resources.GetString("xrTableCell11.ExpressionBindings"))});
            this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.Weight = 2.6545269406628251D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell3});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "- Đơn vị/Dự án sử dụng:";
            this.xrTableCell2.Weight = 0.89043746889199449D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([Proprietor].[PropName]<>\'\',[Proprietor].[PropName],\'\')\n")});
            this.xrTableCell3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.Weight = 2.6545269406628251D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrTableCell13});
            this.xrTableRow5.Dpi = 254F;
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "- Địa chỉ ĐV/DA sử dụng:";
            this.xrTableCell12.Weight = 0.89043746889199449D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([Proprietor].[PropAddress]<>\'\',[Proprietor].[PropAddress],\'\')\n")});
            this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Multiline = true;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.Text = "xrTableCell13";
            this.xrTableCell13.Weight = 2.6545269406628251D;
            // 
            // SubBand9
            // 
            this.SubBand9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel8,
            this.xrLabel28});
            this.SubBand9.Dpi = 254F;
            this.SubBand9.HeightF = 449.7917F;
            this.SubBand9.Name = "SubBand9";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[own.DisplayName]")});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1258.389F, 310.8092F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(454.7771F, 58.41998F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(641.375F, 40.63996F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(454.7771F, 45.72F);
            this.xrLabel3.StyleName = "FieldCaption";
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "PHỤ TRÁCH ĐƠN VỊ";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(1258.389F, 40.63996F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(454.7771F, 45.72F);
            this.xrLabel8.StyleName = "FieldCaption";
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "NGƯỜI ĐỀ NGHỊ";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Dpi = 254F;
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 40.63996F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(454.7771F, 45.72F);
            this.xrLabel28.StyleName = "FieldCaption";
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "TỔNG GIÁM ĐỐC";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 120F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 120F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // objectDataSource
            // 
            this.objectDataSource.DataSource = typeof(IncosafCMS.Core.DomainModels.Contract);
            this.objectDataSource.Name = "objectDataSource";
            // 
            // pageFooterBand
            // 
            this.pageFooterBand.Dpi = 254F;
            this.pageFooterBand.HeightF = 53F;
            this.pageFooterBand.Name = "pageFooterBand";
            // 
            // reportHeaderBand
            // 
            this.reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrLabel2,
            this.xrLabel39,
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel29});
            this.reportHeaderBand.Dpi = 254F;
            this.reportHeaderBand.HeightF = 233F;
            this.reportHeaderBand.Name = "reportHeaderBand";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(15.24003F, 99.99998F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(870.4168F, 50F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "----o0o----";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(15.24003F, 0F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(870.4168F, 100F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "CÔNG TY CỔ PHẦN KIỂM ĐỊNH KỸ THUẬT,\r\nAN TOÀN VÀ TƯ VẤN XD - INCOSAF";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel39
            // 
            this.xrLabel39.Dpi = 254F;
            this.xrLabel39.EditOptions.Enabled = true;
            this.xrLabel39.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif( IsNullOrEmpty(Trim([MaHD])) , \'Số: ................................. /KĐXD\' " +
                    ", Concat(\'Số: \', [MaHD],  \'/KĐXD\' ) )")});
            this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(15.24003F, 150F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(870.4168F, 59.3725F);
            this.xrLabel39.StyleName = "Title";
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.StylePriority.UseTextAlignment = false;
            this.xrLabel39.Text = "Số: ................................. /KĐXD";
            this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel32
            // 
            this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel32.Dpi = 254F;
            this.xrLabel32.EditOptions.Enabled = true;
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(910.3361F, 150F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(874.9038F, 58.42007F);
            this.xrLabel32.StylePriority.UseBorders = false;
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "..................., ngày ......... tháng ......... năm 20......";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel31
            // 
            this.xrLabel31.Dpi = 254F;
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(914.8234F, 99.99999F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(870.4165F, 49.99999F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "----o0o----";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel30
            // 
            this.xrLabel30.Dpi = 254F;
            this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(914.8234F, 50.00002F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(870.4168F, 50F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "Độc lập - Tự do - Hạnh phúc";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Dpi = 254F;
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(914.8234F, 0F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(870.4168F, 50F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
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
            this.DataField.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            // 
            // ContractSuggestReport
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
            this.Dpi = 254F;
            this.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(200, 100, 120, 120);
            this.PageHeight = 2970;
            this.PageWidth = 2100;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 25F;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTasks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableChungKien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
