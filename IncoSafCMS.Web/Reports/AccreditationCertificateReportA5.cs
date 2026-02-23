
using DevExpress.XtraReports.UI;
using IncosafCMS.Web.Helpers;
using IncosafCMS.Core.DomainModels;
using System.Collections.Generic;
using DevExpress.XtraPrinting.BarCode;
using System.Data;
using System.Linq;
using IncosafCMS.Data;




/// <summary>
/// Summary description for AccreditationReport
/// </summary>
public class AccreditationCertificateReportA5 : DevExpress.XtraReports.UI.XtraReport
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
    private XRLine xrLine2;
    private XRPictureBox xrPictureBoxLeft;
    private XRLabel xrLabel34;
    private XRLabel xrLabel36;
    private XRLabel xrLabel38;
    private XRLabel xrLabel37;
    private XRPictureBox xrPictureBox1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell5;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell10;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell14;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private SubBand SubBand1;
    private XRTable xrTableDacTinhKyThuat1;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell35;
    private SubBand SubBand2;
    private XRTable xrTable3;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell8;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell9;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    private XRTable xrTable4;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell29;
    private SubBand SubBand3;
    private XRLabel xrLabel13;
    private XRLabel xrLabel12;
    private XRLabel xrLabel_qrcode;
    private XRLabel xrLabel2;
    private XRLabel xrLabel4;
    private XRBarCode xrBarCode1;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell12;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell17;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell18;


    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    
    public AccreditationCertificateReportA5()
    {
        
        InitializeComponent();
        objectDataSource.DataSource = ReportHelper.AccreditationReportDataSource;
        var Accreditation = (objectDataSource.DataSource as Accreditation);
        Accreditation.equiment = ReportHelper.AccreditationReportDataSource.equiment;       
        if (Accreditation.equiment != null)
        {
            Accreditation.equiment.contract = ReportHelper.AccreditationReportDataSource.equiment?.contract;
            Accreditation.equiment.contract.customer = ReportHelper.AccreditationReportDataSource.equiment?.contract?.customer;
            Accreditation.equiment.contract.own = ReportHelper.AccreditationReportDataSource.equiment?.contract.own;
            Accreditation.equiment.specifications = ReportHelper.AccreditationReportDataSource.equiment?.specifications;
        }       

        if (Accreditation?.equiment?.specifications != null)
        {
            using (var db = new IncosafCMSContext())
            {
                var specEngDict = db.OriginalSpecs
                    .Where(x => x.f_key != null && x.f_key != "")
                    .GroupBy(x => x.f_key)
                    .Select(g => new
                    {
                        Key = g.Key,
                        NameEng = g.Where(s => !string.IsNullOrEmpty(s.NameEng))
                                   .Select(s => s.NameEng)
                                   .FirstOrDefault()
                                   ?? g.Select(s => s.NameEng).FirstOrDefault()
                    })
                    .ToDictionary(x => x.Key, x => (x.NameEng ?? "").Trim());

                int count = Accreditation.equiment.specifications.Count;
                var InsertRow = xrTableDacTinhKyThuat1.Rows.FirstRow;
                bool checkName = false;
                bool checkValue = false;
                int withCell = 335;
                for (int i = 0; i < count; i++)
                {
                    var item = Accreditation.equiment.specifications[i];
                    specEngDict.TryGetValue(item.f_key ?? "", out string nameEng);
                    string congText = item.Name + nameEng;
                    if (congText.Length > 40 && i < 7 && congText.Length < 44) checkName = true;
                    if (item.Value.Length > 20 && i < 7) checkValue = true;
                    if ((checkName == true && checkValue == false)
                        || (checkName == false && checkValue == true)) withCell = 405;
                    if (checkName && checkValue) withCell = 435;
                    if ((congText.Length + item.Value.Length) > 46) withCell = 448;

                }

                for (int i = 0; i < count; i++)
                {
                    var item = Accreditation.equiment.specifications[i];
                    specEngDict.TryGetValue(item.f_key ?? "", out string nameEng);

                    // Tạo text đầy đủ
                    string textViet = item.Name;
                    string textEng = nameEng;

                    // Nếu độ dài tổng > giới hạn (ví dụ 48 ký tự) thì xuống dòng
                    string displayText = $"- {textViet} <i><size=10>({textEng})</size></i>:";
                    if ((textViet.Length + textEng.Length) > 48 && !string.IsNullOrEmpty(textEng))
                    {
                        displayText = $"- {textViet}:\n(<i><font size='10'>{textEng}</font></i>)";
                    }
                    if (string.IsNullOrEmpty(textEng)) displayText = textViet + ":";
                    // Gán vào ô                    
                    InsertRow.Cells[0].Text = displayText;
                    // Cho phép markup và tự xuống dòng
                    InsertRow.Cells[0].AllowMarkupText = true;
                    InsertRow.Cells[0].Multiline = true;
                    InsertRow.Cells[0].WordWrap = true;
                    InsertRow.Cells[1].Text = item.Value + " ";
                    InsertRow.Cells[2].Text = " " + item.f_unit;
                    InsertRow.Cells[2].WidthF = 65;

                    InsertRow.Cells[0].WidthF = (textViet.Length + textEng.Length + 9) * 6;
                    InsertRow.Cells[1].WidthF = withCell - (textViet.Length + textEng.Length + 9) * 6;

                    // Chỉ thêm dòng mới nếu chưa quá giới hạn
                    if (i < count - 1 && i < 7)
                        InsertRow = xrTableDacTinhKyThuat1.InsertRowBelow(InsertRow);
                }
            }
        }

        var Location = Accreditation.Location;
        xrTableCell3.AllowMarkupText = true; // Đảm bảo cell cho phép Rich Text
        //xrTableCell3.Text = $"Vị trí lắp đặt <i><font size=\'10\'>(Install possition)</font></i>: <i><font size=\'11\'>{Location}</font></i>";
        xrTableCell3.Text = "<b>Vị trí lắp đặt</b> " +
    "<font size='10' style='font-weight:normal'><i>(Install position)</i></font>: " +
    $"<b><i><font size='11'>{Location}</font></i></b>";
        xrTableCell9.Text = Accreditation.TypeAcc == TypeOfAccr.LanDau ? "Lần đầu" : Accreditation.TypeAcc == TypeOfAccr.BatThuong ? "Bất thường" : Accreditation.TypeAcc == TypeOfAccr.DinhKy ? "Định kỳ" : Accreditation.TypeAcc == TypeOfAccr.SauLapDat ? "Sau lắp đặt" : Accreditation.TypeAcc == TypeOfAccr.SauSuaChua ? "Sau sửa chữa" : "Lần đầu";

        // edited & added by Hung 31.05.2025        
        var local = Accreditation.equiment.contract.own?.Department?.MaDV == "HCM" ? "TP. Hồ Chí Minh" : Accreditation.equiment.contract.own?.Department?.MaDV == "DN" ? "Đà Nẵng" : "Hà Nội";
        xrLabel2.Text = $"{local}, ngày {Accreditation.AccrResultDate?.ToString("dd")} tháng {Accreditation.AccrResultDate?.ToString("MM")} năm {Accreditation.AccrResultDate?.Year}";

        // Thiết lập thuộc tính cho XRBarCode
        if (!string.IsNullOrEmpty(ReportHelper.AccreditationReportDataSource.QRCodeLink))
        {
            xrBarCode1.Text = ReportHelper.AccreditationReportDataSource.QRCodeLink; // Nội dung mã QR.
            xrBarCode1.ShowText = false;
            xrBarCode1.Width = 80; // Chiều rộng mã QR (pixel)
            xrBarCode1.Height = 80; // Chiều cao mã QR (pixel)
            //xrBarCode1.AutoModule = true; // Tự động điều chỉnh kích thước module

            // Cấu hình thêm thuộc tính QR Code nếu cần
            QRCodeGenerator qrGenerator = (QRCodeGenerator)xrBarCode1.Symbology;
            qrGenerator.CompactionMode = QRCodeCompactionMode.Byte; // Chế độ nén dữ liệu
            qrGenerator.ErrorCorrectionLevel = QRCodeErrorCorrectionLevel.H; // Mức sửa lỗi cao
            qrGenerator.Version = QRCodeVersion.AutoVersion; // Tự động chọn phiên bản QR

            // label
            xrLabel_qrcode.Text = "Mã tra cứu: " + ReportHelper.AccreditationReportDataSource.QRCode_GCN;
        }
        else
        {
            xrBarCode1.Visible = false;
            xrLabel_qrcode.Visible = false;
            xrLabel4.Visible = false;
        }

    }
   

    /*
     public AccreditationCertificateReportA5()
    {
        InitializeComponent();

        // Lấy dữ liệu từ session
        var des = ReportHelper.AccreditationReportDataSource;
        objectDataSource.DataSource = des;

        if (des == null)
            return;

        var Accreditation = des;

        if (Accreditation.equiment != null)
        {
            // Gán các quan hệ liên quan nếu cần (nhiều chỗ đã có trong DTO)
            Accreditation.equiment.contract = Accreditation.equiment.contract;
            Accreditation.equiment.contract.customer = Accreditation.equiment.contract?.customer;
            Accreditation.equiment.contract.own = Accreditation.equiment.contract?.own;
            Accreditation.equiment.specifications = Accreditation.equiment.specifications;
        }

        // Gán thông số kỹ thuật (TSKT)
        if (Accreditation?.equiment?.specifications != null)
        {
            int count = Accreditation.equiment.specifications.Count;
            var InsertRow = xrTableDacTinhKyThuat1.Rows.FirstRow;

            for (int i = 0; i < count; i++)
            {
                var item = Accreditation.equiment.specifications[i];
                InsertRow.Cells[0].Text = "- " + item.Name + ":";
                InsertRow.Cells[1].Text = item.Value + "  ";
                InsertRow.Cells[2].Text = "   " + item.f_unit;

                // Chỉ lấy tối đa 8 TSKT
                if (i < count - 1 && i < 6)
                    InsertRow = xrTableDacTinhKyThuat1.InsertRowBelow(InsertRow);
            }
        }

        // Gán loại kiểm định
        xrTableCell9.Text = Accreditation.TypeAcc == TypeOfAccr.LanDau ? "Lần đầu" :
                            Accreditation.TypeAcc == TypeOfAccr.BatThuong ? "Bất thường" :
                            Accreditation.TypeAcc == TypeOfAccr.DinhKy ? "Định kỳ" :
                            Accreditation.TypeAcc == TypeOfAccr.SauLapDat ? "Sau lắp đặt" :
                            Accreditation.TypeAcc == TypeOfAccr.SauSuaChua ? "Sau sửa chữa" : "Lần đầu";

        // Gán địa phương và ngày tháng
        var local = Accreditation.equiment.contract.own?.Department?.MaDV == "HCM" ? "TP. Hồ Chí Minh" :
                    Accreditation.equiment.contract.own?.Department?.MaDV == "DN" ? "Đà Nẵng" : "Hà Nội";
        xrLabel2.Text = $"{local}, ngày {Accreditation.AccrResultDate?.ToString("dd")} tháng {Accreditation.AccrResultDate?.ToString("MM")} năm {Accreditation.AccrResultDate?.Year}";

        // Mã QR
        if (!string.IsNullOrEmpty(Accreditation.QRCodeLink))
        {
            xrBarCode1.Text = Accreditation.QRCodeLink;
            xrBarCode1.ShowText = false;
            xrBarCode1.Width = 80;
            xrBarCode1.Height = 80;

            QRCodeGenerator qrGenerator = (QRCodeGenerator)xrBarCode1.Symbology;
            qrGenerator.CompactionMode = QRCodeCompactionMode.Byte;
            qrGenerator.ErrorCorrectionLevel = QRCodeErrorCorrectionLevel.H;
            qrGenerator.Version = QRCodeVersion.AutoVersion;

            xrLabel_qrcode.Text = "Mã tra cứu: " + Accreditation.QRCode_GCN;
        }
        else
        {
            xrBarCode1.Visible = false;
            xrLabel_qrcode.Visible = false;
            xrLabel4.Visible = false;
        }
    }
     
     */



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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccreditationCertificateReportA5));
            DevExpress.XtraPrinting.BarCode.QRCodeGenerator qrCodeGenerator1 = new DevExpress.XtraPrinting.BarCode.QRCodeGenerator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand1 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableDacTinhKyThuat1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand2 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand3 = new DevExpress.XtraReports.UI.SubBand();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel_qrcode = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.pageFooterBand = new DevExpress.XtraReports.UI.PageFooterBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBoxLeft = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.objectDataSource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableDacTinhKyThuat1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Detail.BorderWidth = 0F;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 165.9858F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
            this.Detail.StylePriority.UseBorders = false;
            this.Detail.StylePriority.UseBorderWidth = false;
            this.Detail.SubBands.AddRange(new DevExpress.XtraReports.UI.SubBand[] {
            this.SubBand1,
            this.SubBand2,
            this.SubBand3});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow3,
            this.xrTableRow4,
            this.xrTableRow5,
            this.xrTableRow6,
            this.xrTableRow9,
            this.xrTableRow10});
            this.xrTable1.SizeF = new System.Drawing.SizeF(535.9999F, 165.9858F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell5});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "1. Cơ sở sử dụng:";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell1.Weight = 1.1578611584530172D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(IsNullOrEmpty([PartionsNotice]),\' \'+ [equiment].[contract].[customer].[Name]," +
                    "\' \'+[PartionsNotice])")});
            this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell5.Weight = 4.0721385363712015D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell10});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "   Trụ sở chính:";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell7.Weight = 0.945232244971238D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(IsNullOrEmpty([PartionsNotice]),[equiment.contract.customer.Address],Iif(IsNu" +
                    "llOrEmpty([LoadTestNotice]),\'\',[LoadTestNotice]))")});
            this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell10.Weight = 4.2847674498529811D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell14});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.AllowMarkupText = true;
            this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.Text = "<b>2. Đối tượng </b><font size =\'10\' style=\'font-weight:normal\'><i>(Object)</i></" +
    "font>: ";
            this.xrTableCell11.Weight = 1.5218790904763231D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(CharIndex(\'_\',[equiment.Name])>0,Substring([equiment.Name],0,CharIndex(\'_\',[e" +
                    "quiment.Name])),[equiment.Name])")});
            this.xrTableCell14.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.Weight = 3.7081206043478954D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell16});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.AllowMarkupText = true;
            this.xrTableCell15.BorderWidth = 0F;
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.RowSpan = 0;
            this.xrTableCell15.StylePriority.UseBorderWidth = false;
            this.xrTableCell15.Text = "   + Mã hiệu <i><font size=\'10\'>(Model)</font></i>:";
            this.xrTableCell15.Weight = 1.5218787901882047D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.BorderWidth = 0F;
            this.xrTableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment].[mahieu]")});
            this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.RowSpan = 0;
            this.xrTableCell16.StylePriority.UseBorderWidth = false;
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.Weight = 3.7081209046360146D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.AllowMarkupText = true;
            this.xrTableCell19.BorderWidth = 0F;
            this.xrTableCell19.Multiline = true;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.RowSpan = 0;
            this.xrTableCell19.StylePriority.UseBorderWidth = false;
            this.xrTableCell19.Text = "   + Số chế tạo <i><font size=\'10\'>(Serial number)</font></i>:";
            this.xrTableCell19.Weight = 1.9779202421979105D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.BorderWidth = 0F;
            this.xrTableCell20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.No]")});
            this.xrTableCell20.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.RowSpan = 0;
            this.xrTableCell20.StylePriority.UseBorderWidth = false;
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.Weight = 3.2520794526263086D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6,
            this.xrTableCell12});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.AllowMarkupText = true;
            this.xrTableCell6.BorderWidth = 0F;
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.RowSpan = 0;
            this.xrTableCell6.StylePriority.UseBorderWidth = false;
            this.xrTableCell6.Text = "   + Nhà chế tạo <i><font size=\'10\'>(Manufacturer)</font></i>:";
            this.xrTableCell6.Weight = 1.9779200933108208D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.BorderWidth = 0F;
            this.xrTableCell12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.ManuFacturer]")});
            this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.RowSpan = 0;
            this.xrTableCell12.StylePriority.UseBorderWidth = false;
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.Weight = 3.2520796015133984D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell17});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.AllowMarkupText = true;
            this.xrTableCell13.BorderWidth = 0F;
            this.xrTableCell13.Multiline = true;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.RowSpan = 0;
            this.xrTableCell13.StylePriority.UseBorderWidth = false;
            this.xrTableCell13.Text = "   + Năm sản xuất <i><font size=\'10\'>(Year of manufacture)</font></i>:";
            this.xrTableCell13.Weight = 2.5280806711023311D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.BorderWidth = 0F;
            this.xrTableCell17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[equiment.YearOfProduction]")});
            this.xrTableCell17.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell17.Multiline = true;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.RowSpan = 0;
            this.xrTableCell17.StylePriority.UseBorderWidth = false;
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.Weight = 2.7019190237218877D;
            // 
            // SubBand1
            // 
            this.SubBand1.BorderWidth = 0F;
            this.SubBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrTableDacTinhKyThuat1});
            this.SubBand1.HeightF = 50.00003F;
            this.SubBand1.Name = "SubBand1";
            this.SubBand1.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
            this.SubBand1.StylePriority.UseBorderWidth = false;
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable4.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(6.000008F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow13});
            this.xrTable4.SizeF = new System.Drawing.SizeF(499.2581F, 25F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseFont = false;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell29});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.AllowMarkupText = true;
            this.xrTableCell29.BorderWidth = 0F;
            this.xrTableCell29.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell29.Multiline = true;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.RowSpan = 0;
            this.xrTableCell29.StylePriority.UseBorderWidth = false;
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.Text = "<b>   + Đặc tính kỹ thuật </b><i><font size=\'10\' style=\'font-weight:normal\'>(Tech" +
    "nical specifications)</font></i>:";
            this.xrTableCell29.Weight = 2.0270842009156063D;
            // 
            // xrTableDacTinhKyThuat1
            // 
            this.xrTableDacTinhKyThuat1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableDacTinhKyThuat1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableDacTinhKyThuat1.LocationFloat = new DevExpress.Utils.PointFloat(30.14201F, 25F);
            this.xrTableDacTinhKyThuat1.Name = "xrTableDacTinhKyThuat1";
            this.xrTableDacTinhKyThuat1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTableDacTinhKyThuat1.SizeF = new System.Drawing.SizeF(513.858F, 25.00003F);
            this.xrTableDacTinhKyThuat1.StylePriority.UseBorders = false;
            this.xrTableDacTinhKyThuat1.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell4,
            this.xrTableCell35});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell2.BorderWidth = 0F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.RowSpan = 0;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseBorderWidth = false;
            this.xrTableCell2.Weight = 0.24574946369794698D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell4.BorderWidth = 0F;
            this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.RowSpan = 0;
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseBorderWidth = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell4.Weight = 0.11923691181151509D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell35.BorderWidth = 0F;
            this.xrTableCell35.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell35.Multiline = true;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.RowSpan = 0;
            this.xrTableCell35.StylePriority.UseBorders = false;
            this.xrTableCell35.StylePriority.UseBorderWidth = false;
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.Weight = 0.14148541512046969D;
            // 
            // SubBand2
            // 
            this.SubBand2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.SubBand2.HeightF = 95.00008F;
            this.SubBand2.Name = "SubBand2";
            this.SubBand2.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable3.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow8,
            this.xrTableRow14,
            this.xrTableRow11,
            this.xrTableRow12});
            this.xrTable3.SizeF = new System.Drawing.SizeF(535.9999F, 95.00008F);
            this.xrTable3.StylePriority.UseBorders = false;
            this.xrTable3.StylePriority.UseFont = false;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell8});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.AllowMarkupText = true;
            this.xrTableCell3.BorderWidth = 0F;
            this.xrTableCell3.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.RowSpan = 0;
            this.xrTableCell3.StylePriority.UseBorderWidth = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.Weight = 0.664068488679533D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Weight = 0.002487148466228592D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", resources.GetString("xrTableCell9.ExpressionBindings"))});
            this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.Weight = 2.9999999500666905D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.BorderWidth = 0F;
            this.xrTableCell18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", resources.GetString("xrTableCell18.ExpressionBindings"))});
            this.xrTableCell18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell18.Multiline = true;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.RowSpan = 0;
            this.xrTableCell18.StylePriority.UseBorderWidth = false;
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.Weight = 2.9999999500666905D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25,
            this.xrTableCell26});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.AllowMarkupText = true;
            this.xrTableCell25.BorderWidth = 0F;
            this.xrTableCell25.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell25.Multiline = true;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.RowSpan = 0;
            this.xrTableCell25.StylePriority.UseBorderWidth = false;
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.Text = "Tem kiểm định số <i><font size=\'10\'>(Inspection stamp)</font></i>:";
            this.xrTableCell25.Weight = 1.3907208344971662D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.BorderWidth = 0F;
            this.xrTableCell26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StampNumber]")});
            this.xrTableCell26.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.RowSpan = 0;
            this.xrTableCell26.StylePriority.UseBorderWidth = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.Weight = 1.6092791155695243D;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell27,
            this.xrTableCell28});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.AllowMarkupText = true;
            this.xrTableCell27.BorderWidth = 0F;
            this.xrTableCell27.Multiline = true;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.RowSpan = 0;
            this.xrTableCell27.StylePriority.UseBorderWidth = false;
            this.xrTableCell27.Text = "Giấy CN kết quả kiểm định có hiệu lực đến ngày <i><font size=\'10\'>(Expire date)</" +
    "font></i>:";
            this.xrTableCell27.Weight = 2.2077497163127493D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.BorderWidth = 0F;
            this.xrTableCell28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateOfNext]")});
            this.xrTableCell28.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.RowSpan = 0;
            this.xrTableCell28.StylePriority.UseBorderWidth = false;
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrTableCell28.Weight = 0.7922502337539411D;
            // 
            // SubBand3
            // 
            this.SubBand3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode1,
            this.xrLabel4,
            this.xrLabel2,
            this.xrLabel_qrcode,
            this.xrLabel13,
            this.xrLabel12});
            this.SubBand3.HeightF = 144.7213F;
            this.SubBand3.Name = "SubBand3";
            this.SubBand3.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.Alignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            this.xrBarCode1.AutoModule = true;
            this.xrBarCode1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode1.BorderWidth = 0F;
            this.xrBarCode1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 28.00002F);
            this.xrBarCode1.Module = 4F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(78.12273F, 79.20545F);
            this.xrBarCode1.StylePriority.UseBorders = false;
            this.xrBarCode1.StylePriority.UseBorderWidth = false;
            this.xrBarCode1.StylePriority.UseFont = false;
            this.xrBarCode1.StylePriority.UsePadding = false;
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            this.xrBarCode1.Symbology = qrCodeGenerator1;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(2.000008F, 125.2213F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(187.875F, 13.00003F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Tại website: tracuu.incosaf.com";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.BorderWidth = 0F;
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(202.7084F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(330.2916F, 23F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseBorderWidth = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UsePadding = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel_qrcode
            // 
            this.xrLabel_qrcode.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel_qrcode.BorderWidth = 0F;
            this.xrLabel_qrcode.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel_qrcode.LocationFloat = new DevExpress.Utils.PointFloat(2F, 107.2055F);
            this.xrLabel_qrcode.Name = "xrLabel_qrcode";
            this.xrLabel_qrcode.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrLabel_qrcode.SizeF = new System.Drawing.SizeF(187.875F, 18.01579F);
            this.xrLabel_qrcode.StylePriority.UseBorders = false;
            this.xrLabel_qrcode.StylePriority.UseBorderWidth = false;
            this.xrLabel_qrcode.StylePriority.UseFont = false;
            this.xrLabel_qrcode.StylePriority.UsePadding = false;
            this.xrLabel_qrcode.StylePriority.UseTextAlignment = false;
            this.xrLabel_qrcode.Text = "\'Mã tra cứu :\' ";
            this.xrLabel_qrcode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel13.BorderWidth = 0F;
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(202.7084F, 22.99998F);
            this.xrLabel13.Multiline = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(330.2916F, 34.6476F);
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseBorderWidth = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UsePadding = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "CÔNG TY CỔ PHẦN KIỂM ĐỊNH KỸ THUẬT,\r\nAN TOÀN VÀ TƯ VẤN XÂY DỰNG - INCOSAF";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", resources.GetString("xrLabel12.ExpressionBindings"))});
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 0F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(196.7084F, 23F);
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 9F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 6F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // pageFooterBand
            // 
            this.pageFooterBand.Expanded = false;
            this.pageFooterBand.HeightF = 0.1279195F;
            this.pageFooterBand.Name = "pageFooterBand";
            // 
            // reportHeaderBand
            // 
            this.reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrLabel38,
            this.xrLabel37,
            this.xrLabel36,
            this.xrLabel34,
            this.xrPictureBoxLeft,
            this.xrLine2});
            this.reportHeaderBand.HeightF = 119.2787F;
            this.reportHeaderBand.Name = "reportHeaderBand";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(451.6667F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(90.33319F, 84.19534F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            this.xrPictureBox1.Visible = false;
            // 
            // xrLabel38
            // 
            this.xrLabel38.BorderWidth = 0F;
            this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 4.5F, System.Drawing.FontStyle.Bold);
            this.xrLabel38.ForeColor = System.Drawing.Color.White;
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(118F, 42.07038F);
            this.xrLabel38.Multiline = true;
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(333.6667F, 47.87498F);
            this.xrLabel38.StylePriority.UseBorderWidth = false;
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.StylePriority.UseForeColor = false;
            this.xrLabel38.StylePriority.UsePadding = false;
            this.xrLabel38.StylePriority.UseTextAlignment = false;
            this.xrLabel38.Text = resources.GetString("xrLabel38.Text");
            this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel37
            // 
            this.xrLabel37.BorderWidth = 0F;
            this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 6F);
            this.xrLabel37.ForeColor = System.Drawing.Color.White;
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(118F, 28.00001F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(333.6667F, 14.07036F);
            this.xrLabel37.StylePriority.UseBorderWidth = false;
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.StylePriority.UseForeColor = false;
            this.xrLabel37.StylePriority.UsePadding = false;
            this.xrLabel37.StylePriority.UseTextAlignment = false;
            this.xrLabel37.Text = "CONTRUCTION CONSULTANT AND SAFETY TECHNIQUE INSPECTION JSC";
            this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel36
            // 
            this.xrLabel36.BorderWidth = 0F;
            this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 6F, System.Drawing.FontStyle.Bold);
            this.xrLabel36.ForeColor = System.Drawing.Color.White;
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(118F, 10.00001F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(333.6667F, 18F);
            this.xrLabel36.StylePriority.UseBorderWidth = false;
            this.xrLabel36.StylePriority.UseFont = false;
            this.xrLabel36.StylePriority.UseForeColor = false;
            this.xrLabel36.StylePriority.UsePadding = false;
            this.xrLabel36.StylePriority.UseTextAlignment = false;
            this.xrLabel36.Text = "CÔNG TY CP KIỂM ĐỊNH KỸ THUẬT, AN TOÀN VÀ TƯ VẤN XÂY DỰNG-INCOSAF";
            this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify;
            // 
            // xrLabel34
            // 
            this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 6F, System.Drawing.FontStyle.Bold);
            this.xrLabel34.ForeColor = System.Drawing.Color.White;
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 59F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(108F, 21.87501F);
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.StylePriority.UseForeColor = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "SINCE 1990";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPictureBoxLeft
            // 
            this.xrPictureBoxLeft.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBoxLeft.ImageSource"));
            this.xrPictureBoxLeft.LocationFloat = new DevExpress.Utils.PointFloat(9.99999F, 10.00001F);
            this.xrPictureBoxLeft.Name = "xrPictureBoxLeft";
            this.xrPictureBoxLeft.SizeF = new System.Drawing.SizeF(108F, 48.99999F);
            this.xrPictureBoxLeft.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            this.xrPictureBoxLeft.Visible = false;
            // 
            // xrLine2
            // 
            this.xrLine2.BorderColor = System.Drawing.Color.White;
            this.xrLine2.ForeColor = System.Drawing.Color.White;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 89.94536F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(531.9999F, 5.749992F);
            this.xrLine2.StylePriority.UseBorderColor = false;
            this.xrLine2.StylePriority.UseForeColor = false;
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
            // AccreditationCertificateReportA5
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
            this.Margins = new System.Drawing.Printing.Margins(23, 16, 9, 6);
            this.PageHeight = 827;
            this.PageWidth = 583;
            this.PaperKind = System.Drawing.Printing.PaperKind.A5;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableDacTinhKyThuat1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
