using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using IncosafCMS.Web.Helpers;
using IncosafCMS.Core.DomainModels;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Summary description for TurnOverSuggestReport
/// </summary>
public class TurnOverSuggestReport : DevExpress.XtraReports.UI.XtraReport
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
    private XRLabel xrLabel39;
    private XRLabel xrLabel1;
    private XRLabel xrLabel2;
    private XRTableRow xrTableRow15;
    private XRTableCell xrTableCell23;
    private XRLabel xrLabel4;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRLabel xrLabel3;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private GroupFooterBand GroupFooter1;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public TurnOverSuggestReport()
    {
        InitializeComponent();
        objectDataSource.DataSource = ReportHelper.TurnOverReportDataSource;
        var contract = (objectDataSource.DataSource as Contract);
        contract.own = ReportHelper.TurnOverReportDataSource.own;
        contract.own.Department = ReportHelper.TurnOverReportDataSource.own.Department;
        contract.customer = ReportHelper.TurnOverReportDataSource.customer;

       // xrLabel3.Text = $"Ngày {contract.NgayThucHien.Value.Day} tháng {contract.NgayThucHien.Value.Month} năm {contract.NgayThucHien.Value.Year}";

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

        xrLabel6.Text = NumberToWord.DocTienBangChu((long)contract.Value, " đồng");  //Hưng sửa
        //
        // TODO: Add constructor logic here
        //
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.SubBand7 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
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
            this.SubBand9 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.pageFooterBand = new DevExpress.XtraReports.UI.PageFooterBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTasks)).BeginInit();
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
            this.xrLabel33.Text = "GIẤY ĐỀ NGHỊ XUẤT CHỨNG TỪ";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // SubBand7
            // 
            this.SubBand7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.SubBand7.Dpi = 254F;
            this.SubBand7.HeightF = 263.1885F;
            this.SubBand7.Name = "SubBand7";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Dpi = 254F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(16.19273F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5,
            this.xrTableRow22,
            this.xrTableRow15,
            this.xrTableRow1});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1768.913F, 254.0001F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell11});
            this.xrTableRow5.Dpi = 254F;
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "Kính gửi:";
            this.xrTableCell10.Weight = 0.78334592825239913D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "Phòng tài chính - kế toán";
            this.xrTableCell11.Weight = 2.2166540717476013D;
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
            this.xrTableCell7.Text = "Đề nghị xuất chứng từ cho:";
            this.xrTableCell7.Weight = 0.7833463423043564D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Name]")});
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.TextFormatString = "{0}";
            this.xrTableCell5.Weight = 2.2166536576956437D;
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
            this.xrTableCell8.Text = "- Địa chỉ:";
            this.xrTableCell8.Weight = 0.78334675635631368D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Address]")});
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Weight = 2.2166532436436865D;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrTableCell1});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "- Mã số thuế:";
            this.xrTableCell12.Weight = 0.78621919496568027D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.CanShrink = true;
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.EditOptions.Enabled = true;
            this.xrTableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.TaxID]")});
            this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.Weight = 2.21378080503432D;
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
            this.xrTableCell39.Weight = 4.3186751344403529D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Dpi = 254F;
            this.xrTableCell40.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseFont = false;
            this.xrTableCell40.Text = "ĐVT";
            this.xrTableCell40.Weight = 1.7723948346835035D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Dpi = 254F;
            this.xrTableCell41.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.StylePriority.UseFont = false;
            this.xrTableCell41.Text = "KL";
            this.xrTableCell41.Weight = 1.1076755136715339D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Dpi = 254F;
            this.xrTableCell42.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseFont = false;
            this.xrTableCell42.Text = "Đơn giá chưa VAT (đ)";
            this.xrTableCell42.Weight = 2.1502031224351583D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Dpi = 254F;
            this.xrTableCell43.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.Text = "Thành tiền (đ)";
            this.xrTableCell43.Weight = 2.4830971044752497D;
            // 
            // SubBand9
            // 
            this.SubBand9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel5});
            this.SubBand9.Dpi = 254F;
            this.SubBand9.HeightF = 80.71084F;
            this.SubBand9.Name = "SubBand9";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(200.1421F, 25.00009F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(1584.964F, 45.71999F);
            this.xrLabel6.StyleName = "FieldCaption";
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(15.24003F, 25.00009F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(184.9021F, 45.71999F);
            this.xrLabel5.StyleName = "FieldCaption";
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Bằng chữ:";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.EditOptions.Enabled = true;
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Ngày ......... tháng ......... năm \' + GetYear([NgayThucHien])")});
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(1043.354F, 14.41676F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(756.6456F, 59.3725F);
            this.xrLabel3.StyleName = "Title";
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Ngày ......... tháng ......... năm ..............";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[own.DisplayName]")});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1208.755F, 345.6417F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(454.7771F, 58.41998F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(1208.755F, 91.78413F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(454.7771F, 45.72F);
            this.xrLabel8.StyleName = "FieldCaption";
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "NGƯỜI ĐỀ NGHỊ";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
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
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel29});
            this.reportHeaderBand.Dpi = 254F;
            this.reportHeaderBand.HeightF = 209.3725F;
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
            this.xrLabel2.EditOptions.Enabled = true;
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.xrLabel39.Text = "Số: .......................... /KĐXD";
            this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel8,
            this.xrLabel4});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 429.0617F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // TurnOverSuggestReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.pageFooterBand,
            this.reportHeaderBand,
            this.GroupFooter1});
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
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
