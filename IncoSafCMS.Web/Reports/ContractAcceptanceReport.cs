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
public class ContractAcceptanceReport : DevExpress.XtraReports.UI.XtraReport
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
    private XRLabel xrLabel28;
    private XRTableRow xrTableRow15;
    private XRTable xrTableChungKien;
    private XRTableCell xrTableCell8;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCellValueToWord;
    private XRLabel xrLabel39;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell18;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell21;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell20;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell29;
    private XRTableRow xrTableRow16;
    private XRTableCell xrTableCell27;
    private XRTableRow xrTableRow17;
    private XRTableCell xrTableCell28;
    private XRTableRow xrTableRow18;
    private XRTableCell xrTableCell30;
    private XRTableRow xrTableRow19;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell31;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRLabel xrLabel4;
    private XRLabel xrLabel5;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell22;
    private XRLabel xrLabel1;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ContractAcceptanceReport()
    {
        InitializeComponent();
        objectDataSource.DataSource = ReportHelper.ContractReportDataSource;
        var contract = (objectDataSource.DataSource as Contract);
        contract.own = ReportHelper.ContractReportDataSource.own;
        contract.KDV1 = ReportHelper.ContractReportDataSource.KDV1;
        contract.KDV2 = ReportHelper.ContractReportDataSource.KDV2;
        contract.own.Department = ReportHelper.ContractReportDataSource.own.Department;     // lapbt xly loi k0 in dc o ReportsController.ContractAcceptanceReport
        contract.customer = ReportHelper.ContractReportDataSource.customer;
        contract.Proprietor = ReportHelper.ContractReportDataSource.Proprietor;

        // edited & added by Lapbt 08/11/2020
        var local = contract.own?.Department?.MaDV == "HCM" ? "TP.HCM" : contract.own?.Department?.MaDV == "DN" ? "Đà Nẵng" : "Hà Nội";
        xrLabel32.Text = $"{local}, ngày {contract.SignDate?.Day} tháng {contract.SignDate?.Month} năm {contract.SignDate?.Year}";


        var tgd = contract.own?.Department?.MaDV == "HCM" ? "Ông:       Vũ Thu Trang" : contract.own?.Department?.MaDV == "DN" ? "Ông:       Dương Kim Ái" : "Ông:       Lâm Văn Khánh";
        xrTableCell23.Text = $"{tgd}";

        var chucvutgd = contract.own?.Department?.MaDV == "HCM" ? "Chức vụ:  Phó Tổng Giám đốc" : contract.own?.Department?.MaDV == "DN" ? "Chức vụ:  Phó Tổng Giám đốc" : "Chức vụ:  Tổng Giám đốc";
        xrTableCell24.Text = $"{chucvutgd}";

        if (contract.own?.Department?.MaDV == "HCM" || contract.own?.Department?.MaDV == "DN")
        {
            xrTableCell1.Text = "";//"Đề nghị Giám đốc cho phép thực hiện công việc như sau:"; Hung sua
            xrLabel28.Text = "ĐẠI DIỆN BÊN A"; //"GIÁM ĐỐC"; Hung sua
        } else
        {
            xrTableCell1.Text = "";//"Đề nghị Tổng Giám đốc cho phép thực hiện công việc như sau:"; Hung sua
            xrLabel28.Text = "ĐẠI DIỆN BÊN A";//"TỔNG GIÁM ĐỐC"; Hung sua
        }
        
        //Thông tin: tên hợp đồng; đơn vị SD; địa chỉ ĐVSD
        xrTableCell27.Multiline = true;
        xrTableCell27.AllowMarkupText = true;
        // xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 6, 6);

        if (contract.Proprietor == null)
            xrTableCell27.Text = "Công việc bên B đã thực hiện theo yêu cầu bên A: " + $"<b><i><font size='12'>{contract.Name}</font></i></b>";        
        if (contract.Proprietor != null && !string.IsNullOrEmpty(contract.Proprietor.PropName))
        {            
            xrTableCell27.Text = "Công việc bên B đã thực hiện theo yêu cầu bên A: " + $"<b><i><font size='12'>{contract.Name}</font></i></b>" +
            Environment.NewLine + "Đơn vị/DA sử dụng: " + $"<b><i><font size='12'>{contract.Proprietor.PropName}</font></i></b>";
                        
            if (contract.Proprietor != null && !string.IsNullOrEmpty(contract.Proprietor.PropAddress))
            {                
                xrTableCell27.Text = "Công việc bên B đã thực hiện theo yêu cầu bên A: " + $"<b><i><font size='12'>{contract.Name}</font></i></b>" +
                Environment.NewLine + "Đơn vị/DA sử dụng: " + $"<b><i><font size='12'>{contract.Proprietor.PropName}</font></i></b>" +
                Environment.NewLine + "Địa chỉ: " + $"<b><i><font size='12'>{contract.Proprietor.PropAddress}</font></i></b>";
                
            }
        }


        //string ptdv = ConfigurationManager.AppSettings["in_giay_de_nghi_phu_trach_don_vi"]; Hung sua
        //if (ptdv != "Không hiển thị") Hung sua
        //    xrLabel3.Text = ptdv; Hung sua
        //else Hung sua
        //    xrLabel3.Visible = false; Hung sua
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
        string com_name = ConfigurationManager.AppSettings["company_name"];//Hung sua
        xrLabel1.Text = com_name; //Hung sua
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
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow16 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
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
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand9 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.pageFooterBand = new DevExpress.XtraReports.UI.PageFooterBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.objectDataSource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
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
            this.Detail.HeightF = 96.1268F;
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
            this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(15.10575F, 0F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(1770F, 88.47667F);
            this.xrLabel33.StyleName = "Title";
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "BIÊN BẢN NGHIỆM THU";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // SubBand7
            // 
            this.SubBand7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.SubBand7.Dpi = 254F;
            this.SubBand7.HeightF = 785.4055F;
            this.SubBand7.Name = "SubBand7";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Dpi = 254F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(16.19273F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow22,
            this.xrTableRow2,
            this.xrTableRow15,
            this.xrTableRow1,
            this.xrTableRow19,
            this.xrTableRow7,
            this.xrTableRow8,
            this.xrTableRow11,
            this.xrTableRow12,
            this.xrTableRow9,
            this.xrTableRow14,
            this.xrTableRow16});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1768.913F, 762.0005F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5});
            this.xrTableRow22.Dpi = 254F;
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Số :\'+Iif( IsNullOrEmpty(Trim([MaHD])) , \' ................................. /KĐ" +
                    "XD-NT\' , Concat(\'  \', [MaHD],  \'/KĐXD-NT\' ) )")});
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell5.TextFormatString = "{0}";
            this.xrTableCell5.Weight = 2.9999999999999996D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.Weight = 3D;
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell8});
            this.xrTableRow15.Dpi = 254F;
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.Weight = 1D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.Text = "I. CÁC BÊN THAM GIA NGHIỆM THU";
            this.xrTableCell8.Weight = 3D;
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
            this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.Weight = 3D;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell31});
            this.xrTableRow19.Dpi = 254F;
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell7.Multiline = true;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.Text = "1. Bên A:";
            this.xrTableCell7.Weight = 0.28396366418039976D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Dpi = 254F;
            this.xrTableCell31.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Name]")});
            this.xrTableCell31.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell31.Multiline = true;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.StylePriority.UseFont = false;
            this.xrTableCell31.Text = "xrTableCell31";
            this.xrTableCell31.Weight = 2.7160363358196005D;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell16});
            this.xrTableRow7.Dpi = 254F;
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Text = "Ông/Bà: ...................................................";
            this.xrTableCell15.Weight = 1.3110430417360923D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Multiline = true;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "Chức vụ: .......................................";
            this.xrTableCell16.Weight = 1.6889569582639077D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell25,
            this.xrTableCell18});
            this.xrTableRow8.Dpi = 254F;
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Multiline = true;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Text = "Ông/Bà:";
            this.xrTableCell17.Weight = 0.28396366418039976D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Dpi = 254F;
            this.xrTableCell25.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NguoiLienHe]")});
            this.xrTableCell25.Multiline = true;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Text = "xrTableCell25";
            this.xrTableCell25.Weight = 1.0270793775556926D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Multiline = true;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Text = "Chức vụ: .......................................";
            this.xrTableCell18.Weight = 1.6889569582639075D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell21,
            this.xrTableCell22});
            this.xrTableRow11.Dpi = 254F;
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Dpi = 254F;
            this.xrTableCell21.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell21.Multiline = true;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.Text = "2. Bên B:";
            this.xrTableCell21.Weight = 0.2839636641803997D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.xrTableCell22.Dpi = 254F;
            this.xrTableCell22.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell22.Multiline = true;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseFont = false;
            this.xrTableCell22.Weight = 2.7160363358196D;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.EditOptions.Enabled = true;
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.103516E-05F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(1601.477F, 63.5F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell23,
            this.xrTableCell24});
            this.xrTableRow12.Dpi = 254F;
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell23.Multiline = true;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseFont = false;
            this.xrTableCell23.Text = "Ông:        Lâm Văn Khánh ";
            this.xrTableCell23.Weight = 1.3110434557880497D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Dpi = 254F;
            this.xrTableCell24.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell24.Multiline = true;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.Text = "Chức vụ:  Tổng Giám đốc";
            this.xrTableCell24.Weight = 1.6889565442119503D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell26,
            this.xrTableCell20});
            this.xrTableRow9.Dpi = 254F;
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Multiline = true;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.Text = "Ông/Bà:";
            this.xrTableCell19.Weight = 0.28396366418039976D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Dpi = 254F;
            this.xrTableCell26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[own.DisplayName]")});
            this.xrTableCell26.Multiline = true;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Text = "xrTableCell26";
            this.xrTableCell26.Weight = 1.0270794810686821D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Multiline = true;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Text = "Chức vụ: .......................................";
            this.xrTableCell20.Weight = 1.6889568547509182D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell29});
            this.xrTableRow14.Dpi = 254F;
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Dpi = 254F;
            this.xrTableCell29.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell29.Multiline = true;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.Text = "II. NỘI DUNG NGHIỆM THU";
            this.xrTableCell29.Weight = 3D;
            // 
            // xrTableRow16
            // 
            this.xrTableRow16.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell27});
            this.xrTableRow16.Dpi = 254F;
            this.xrTableRow16.Name = "xrTableRow16";
            this.xrTableRow16.Weight = 1D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Dpi = 254F;
            this.xrTableCell27.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell27.Multiline = true;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.Weight = 3D;
            // 
            // SubBand6
            // 
            this.SubBand6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableTasks});
            this.SubBand6.Dpi = 254F;
            this.SubBand6.HeightF = 88.9F;
            this.SubBand6.Name = "SubBand6";
            // 
            // xrTableTasks
            // 
            this.xrTableTasks.Dpi = 254F;
            this.xrTableTasks.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableTasks.LocationFloat = new DevExpress.Utils.PointFloat(22.11836F, 10.17077F);
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
            // SubBand1
            // 
            this.SubBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableChungKien});
            this.SubBand1.Dpi = 254F;
            this.SubBand1.HeightF = 220.3708F;
            this.SubBand1.Name = "SubBand1";
            // 
            // xrTableChungKien
            // 
            this.xrTableChungKien.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableChungKien.Dpi = 254F;
            this.xrTableChungKien.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableChungKien.LocationFloat = new DevExpress.Utils.PointFloat(22.11905F, 0F);
            this.xrTableChungKien.Name = "xrTableChungKien";
            this.xrTableChungKien.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3,
            this.xrTableRow17,
            this.xrTableRow18});
            this.xrTableChungKien.SizeF = new System.Drawing.SizeF(1768.913F, 190.5F);
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
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Số tiền bằng chữ:";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell9.Weight = 0.58502243166134327D;
            // 
            // xrTableCellValueToWord
            // 
            this.xrTableCellValueToWord.Dpi = 254F;
            this.xrTableCellValueToWord.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCellValueToWord.Name = "xrTableCellValueToWord";
            this.xrTableCellValueToWord.StylePriority.UseFont = false;
            this.xrTableCellValueToWord.StylePriority.UseTextAlignment = false;
            this.xrTableCellValueToWord.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCellValueToWord.Weight = 2.9599420655344826D;
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell28});
            this.xrTableRow17.Dpi = 254F;
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Weight = 1D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic);
            this.xrTableCell28.Multiline = true;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.Text = "(Đã bao gồm thuế VAT)";
            this.xrTableCell28.Weight = 3.5449644971958261D;
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell30});
            this.xrTableRow18.Dpi = 254F;
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 1D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Dpi = 254F;
            this.xrTableCell30.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.xrTableCell30.Multiline = true;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.StylePriority.UseFont = false;
            this.xrTableCell30.Text = "Biên bản được lập thành 02 bản, mỗi bên giữ 01 bản có giá trị như nhau.";
            this.xrTableCell30.Weight = 3.5449644971958261D;
            // 
            // SubBand9
            // 
            this.SubBand9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel28});
            this.SubBand9.Dpi = 254F;
            this.SubBand9.HeightF = 449.7917F;
            this.SubBand9.Name = "SubBand9";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1099.104F, 24.99993F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(454.7771F, 45.72F);
            this.xrLabel4.StyleName = "FieldCaption";
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "ĐẠI DIỆN BÊN B";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Dpi = 254F;
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(194.2117F, 24.99993F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(454.7771F, 45.72F);
            this.xrLabel28.StyleName = "FieldCaption";
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "ĐẠI DIỆN BÊN A";
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
            // pageFooterBand
            // 
            this.pageFooterBand.Dpi = 254F;
            this.pageFooterBand.HeightF = 53F;
            this.pageFooterBand.Name = "pageFooterBand";
            // 
            // reportHeaderBand
            // 
            this.reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel39,
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel29});
            this.reportHeaderBand.Dpi = 254F;
            this.reportHeaderBand.HeightF = 249.479F;
            this.reportHeaderBand.Name = "reportHeaderBand";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(929.5831F, 181.3748F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(870.4168F, 57.27089F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Ngày .......... tháng ........ năm ...........";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel39
            // 
            this.xrLabel39.Dpi = 254F;
            this.xrLabel39.EditOptions.Enabled = true;
            this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(22.11905F, 151.5025F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(21.10433F, 59.37251F);
            this.xrLabel39.StyleName = "Title";
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.StylePriority.UseTextAlignment = false;
            this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel32
            // 
            this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel32.Dpi = 254F;
            this.xrLabel32.EditOptions.Enabled = true;
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Italic);
            this.xrLabel32.ForeColor = System.Drawing.Color.White;
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(925.096F, 139.4167F);
            this.xrLabel32.Multiline = true;
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(874.904F, 35.89578F);
            this.xrLabel32.StylePriority.UseBorders = false;
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseForeColor = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "..................., ngày ......... tháng ......... năm 20......";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel31
            // 
            this.xrLabel31.Dpi = 254F;
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 100F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(1775F, 39.41669F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "----o0o----";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel30
            // 
            this.xrLabel30.Dpi = 254F;
            this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 50.00002F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(1775F, 50F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "Độc lập - Tự do - Hạnh phúc";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Dpi = 254F;
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 0F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(1775F, 50F);
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
            // objectDataSource
            // 
            this.objectDataSource.DataSource = typeof(IncosafCMS.Core.DomainModels.Contract);
            this.objectDataSource.Name = "objectDataSource";
            // 
            // ContractAcceptanceReport
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
