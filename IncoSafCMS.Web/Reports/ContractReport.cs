using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using IncosafCMS.Web.Helpers;
using IncosafCMS.Core.DomainModels;
using System.Collections.Generic;
using System.Configuration;

/// <summary>
/// Summary description for ContractReport
/// </summary>
public class ContractReport : DevExpress.XtraReports.UI.XtraReport
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
    private XRLabel xrLabel39;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private XRLabel xrLabel29;
    private XRLabel xrLabel32;
    private SubBand SubBand1;
    private XRTable xrTableTechnicalDocument;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCellValueToWord;
    private SubBand SubBand2;
    private SubBand SubBand3;
    private SubBand SubBand4;
    private SubBand SubBand5;
    private SubBand SubBand6;
    private SubBand SubBand7;
    private SubBand SubBand8;
    private XRTable xrTableChungKien;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell34;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell35;
    private XRTable xrTable2;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableRow xrTableRow18;
    private XRTableCell xrTableCell54;
    private XRTableRow xrTableRow19;
    private XRTableCell xrTableCell3;
    private XRTableRow xrTableRow21;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell62;
    private XRTableRow xrTableRow20;
    private XRTableCell xrTableCell58;
    private XRTableRow xrTableRow22;
    private XRTableCell xrTableCell5;
    private XRTableRow xrTableRow23;
    private XRTableCell xrTableCell55;
    private XRTableRow xrTableRow24;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell63;
    private XRTableRow xrTableRow27;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell71;
    private XRTableRow xrTableRow28;
    private XRTableCell xrTableCell72;
    private XRTableCell xrTableCell78;
    private XRTableRow xrTableRow29;
    private XRTableCell xrTableCell75;
    private XRTableCell xrTableCell76;
    private XRTableRow xrTableRow26;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private XRTableRow xrTableRow25;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell64;
    private XRTableRow xrTableRow30;
    private XRTableCell xrTableCell65;
    private XRTableCell xrTableCell73;
    private XRTableCell xrTableCell68;
    private XRTableRow xrTableRow32;
    private XRTableCell xrTableCell80;
    private XRTableCell xrTableCell82;
    private XRTableRow xrTableRow33;
    private XRTableCell xrTableCell83;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell85;
    private XRTableRow xrTableRow35;
    private XRTableCell xrTableCell89;
    private XRTableCell xrTableCell90;
    private XRTableRow xrTableRow34;
    private XRTableCell xrTableCell86;
    private XRTableCell xrTableCell88;
    private XRTableRow xrTableRow31;
    private XRTableCell xrTableCell74;
    private XRTableCell xrTableCell79;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell7;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell8;
    private XRTable xrTableTasks;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell10;
    private XRTableRow xrTableRow36;
    private XRTableCell xrTableCell45;
    private XRTableRow xrTableRow37;
    private XRTableCell xrTableCell77;
    private XRTable xrTable4;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell4;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell6;
    private XRTableRow xrTableRow38;
    private XRTableCell xrTableCell11;
    private XRTableRow xrTableRow39;
    private XRTableCell xrTableCell12;
    private XRTableRow xrTableRow40;
    private XRTableCell xrTableCell13;
    private XRTable xrTable5;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableRow xrTableRow42;
    private XRTableCell xrTableCell17;
    private XRTable xrTable6;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell18;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell19;
    private XRTable xrTable7;
    private XRTableRow xrTableRow41;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTableRow xrTableRow43;
    private XRTableCell xrTableCell22;
    private SubBand SubBand9;
    private XRLabel xrLabel8;
    private XRLabel xrLabel28;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell25;
    private XRTableRow xrTableRow15;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell30;
    private CalculatedField CustomerName;
    private CalculatedField TaskContentInReport;
    private CalculatedField MyPhone;
    private CalculatedField MyFax;
    private CalculatedField MyAccountNumber;
    private CalculatedField MyTaxID;
    private CalculatedField MyRepresentative;
    private CalculatedField MyRepresentativePosition;
    private CalculatedField MyAddress;
    private CalculatedField MyBankName;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ContractReport()
    {
        InitializeComponent();
        objectDataSource.DataSource = ReportHelper.ContractReportDataSource;
        (objectDataSource.DataSource as Contract).customer = ReportHelper.ContractReportDataSource.customer;

        var InsertRow = xrTableTasks.Rows.FirstRow;
        int stt = 1;
        double tong = 0;
        foreach (var item in (objectDataSource.DataSource as Contract).Tasks)
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
            InsertRow.Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            InsertRow.Cells[4].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
            InsertRow.Cells[4].Text = item.UnitPrice.ToString("N0");

            InsertRow.Cells[5].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Regular);
            InsertRow.Cells[5].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            InsertRow.Cells[5].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
            InsertRow.Cells[5].Text = (item.Amount * item.UnitPrice).ToString("N0");

            tong += item.Amount * item.UnitPrice;
            stt++;
        }

        InsertRow = xrTableTasks.InsertRowBelow(InsertRow);
        InsertRow.Cells[1].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Bold);
        InsertRow.Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        InsertRow.Cells[1].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
        InsertRow.Cells[1].Text = "Tổng cộng:";

        InsertRow.Cells[5].Font = new Font(new FontFamily("Times New Roman"), 11, FontStyle.Bold);
        InsertRow.Cells[5].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        InsertRow.Cells[5].Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0);
        InsertRow.Cells[5].Text = tong.ToString("N0");

        xrTableCellValueToWord.Text = NumberToWord.DocTienBangChu((long)(objectDataSource.DataSource as Contract).Value, " đồng");
        //
        // TODO: Add constructor logic here
        //
        string com_name = ConfigurationManager.AppSettings["company_name"];
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
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.SubBand7 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow21 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand8 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTableChungKien = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow27 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow28 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow29 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow25 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow30 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow32 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow33 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow35 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow34 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow31 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
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
            this.xrTableTechnicalDocument = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellValueToWord = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow36 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow37 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand2 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow38 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow39 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow40 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand3 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow42 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand4 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand5 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow41 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow43 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBand9 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.pageFooterBand = new DevExpress.XtraReports.UI.PageFooterBand();
            this.reportHeaderBand = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.CustomerName = new DevExpress.XtraReports.UI.CalculatedField();
            this.TaskContentInReport = new DevExpress.XtraReports.UI.CalculatedField();
            this.MyPhone = new DevExpress.XtraReports.UI.CalculatedField();
            this.MyFax = new DevExpress.XtraReports.UI.CalculatedField();
            this.MyAccountNumber = new DevExpress.XtraReports.UI.CalculatedField();
            this.MyTaxID = new DevExpress.XtraReports.UI.CalculatedField();
            this.MyRepresentative = new DevExpress.XtraReports.UI.CalculatedField();
            this.MyRepresentativePosition = new DevExpress.XtraReports.UI.CalculatedField();
            this.MyAddress = new DevExpress.XtraReports.UI.CalculatedField();
            this.MyBankName = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableChungKien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTechnicalDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel39,
            this.xrLabel33});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 184F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.StylePriority.UseBorders = false;
            this.Detail.SubBands.AddRange(new DevExpress.XtraReports.UI.SubBand[] {
            this.SubBand7,
            this.SubBand8,
            this.SubBand6,
            this.SubBand1,
            this.SubBand2,
            this.SubBand3,
            this.SubBand4,
            this.SubBand5,
            this.SubBand9});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel39
            // 
            this.xrLabel39.Dpi = 254F;
            this.xrLabel39.EditOptions.Enabled = true;
            this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(15.24003F, 124.46F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(1770F, 59.37251F);
            this.xrLabel39.StyleName = "Title";
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.StylePriority.UseTextAlignment = false;
            this.xrLabel39.Text = "Số: .............. /KĐXD-TBTC";
            this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            this.xrLabel33.Text = "HỢP ĐỒNG KINH TẾ";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // SubBand7
            // 
            this.SubBand7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.SubBand7.Dpi = 254F;
            this.SubBand7.HeightF = 470F;
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
            this.xrTableRow1,
            this.xrTableRow18,
            this.xrTableRow19,
            this.xrTableRow21,
            this.xrTableRow20,
            this.xrTableRow23});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1768.913F, 444.5003F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell23,
            this.xrTableCell5});
            this.xrTableRow22.Dpi = 254F;
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Weight = 1D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Text = "Tên hợp đồng:";
            this.xrTableCell23.Weight = 0.45447786740724472D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.TextFormatString = "{0}";
            this.xrTableCell5.Weight = 2.5455221325927555D;
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
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "- Căn cứ bộ luật đân sự năm 2005 của nước Cộng hòa xã hội chủ nghĩa Việt Nam.";
            this.xrTableCell1.Weight = 3D;
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell54});
            this.xrTableRow18.Dpi = 254F;
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 1D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.Dpi = 254F;
            this.xrTableCell54.Multiline = true;
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.Text = "- Căn cứ luật thương mại năm 2005 của nước Cộng hòa xã hội chủ nghĩa Việt Nam.";
            this.xrTableCell54.Weight = 3D;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3});
            this.xrTableRow19.Dpi = 254F;
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.CanShrink = true;
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "- Căn cứ thông tư 73/2014/TT-BTC ngày 30/05/2014 của bộ tài chính về phí kiểm địn" +
    "h các loại máy, thiết bị , vật tư có yêu cầu nghiêm ngặt về an toàn lao động.";
            this.xrTableCell3.Weight = 3D;
            // 
            // xrTableRow21
            // 
            this.xrTableRow21.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell61,
            this.xrTableCell62});
            this.xrTableRow21.Dpi = 254F;
            this.xrTableRow21.Name = "xrTableRow21";
            this.xrTableRow21.Weight = 1D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.Dpi = 254F;
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.Text = "- Căn cứ công việc của: ";
            this.xrTableCell61.Weight = 0.7059867823364141D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.Dpi = 254F;
            this.xrTableCell62.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Name]")});
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.Weight = 2.2940132176635859D;
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell58});
            this.xrTableRow20.Dpi = 254F;
            this.xrTableRow20.Name = "xrTableRow20";
            this.xrTableRow20.Weight = 1D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.Dpi = 254F;
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.Text = "- Căn cứ vào khả năng thực hiện công việc của Công ty CP kiểm định kỹ thuật, an t" +
    "oàn và tư vấn Xây dựng - INCOSAF";
            this.xrTableCell58.Weight = 3D;
            // 
            // xrTableRow23
            // 
            this.xrTableRow23.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell55});
            this.xrTableRow23.Dpi = 254F;
            this.xrTableRow23.Name = "xrTableRow23";
            this.xrTableRow23.Weight = 1D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.Dpi = 254F;
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.Text = "Chúng tôi gồm:";
            this.xrTableCell55.Weight = 3D;
            // 
            // SubBand8
            // 
            this.SubBand8.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableChungKien});
            this.SubBand8.Dpi = 254F;
            this.SubBand8.HeightF = 1105F;
            this.SubBand8.Name = "SubBand8";
            // 
            // xrTableChungKien
            // 
            this.xrTableChungKien.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableChungKien.Dpi = 254F;
            this.xrTableChungKien.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableChungKien.LocationFloat = new DevExpress.Utils.PointFloat(16.19273F, 25.40002F);
            this.xrTableChungKien.Name = "xrTableChungKien";
            this.xrTableChungKien.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow24,
            this.xrTableRow9,
            this.xrTableRow10,
            this.xrTableRow27,
            this.xrTableRow28,
            this.xrTableRow29,
            this.xrTableRow26,
            this.xrTableRow25,
            this.xrTableRow30,
            this.xrTableRow32,
            this.xrTableRow33,
            this.xrTableRow35,
            this.xrTableRow34,
            this.xrTableRow31,
            this.xrTableRow3,
            this.xrTableRow11,
            this.xrTableRow15});
            this.xrTableChungKien.SizeF = new System.Drawing.SizeF(1768.913F, 1079.5F);
            this.xrTableChungKien.StylePriority.UseBorders = false;
            this.xrTableChungKien.StylePriority.UseFont = false;
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
            this.xrTableCell56.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.StylePriority.UseFont = false;
            this.xrTableCell56.Text = "I - ĐẠI DIỆN BÊN A:";
            this.xrTableCell56.Weight = 0.83423279105197323D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.Dpi = 254F;
            this.xrTableCell57.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerName]")});
            this.xrTableCell57.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.StylePriority.UseFont = false;
            this.xrTableCell57.Weight = 2.7107317061438527D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell33,
            this.xrTableCell60,
            this.xrTableCell24,
            this.xrTableCell34});
            this.xrTableRow9.Dpi = 254F;
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Dpi = 254F;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Text = "         Ông:";
            this.xrTableCell33.Weight = 0.83423286931820739D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.Dpi = 254F;
            this.xrTableCell60.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Representative]")});
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.Weight = 1.3066048555773702D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Dpi = 254F;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Text = "Chức vụ:";
            this.xrTableCell24.Weight = 0.34680653288444563D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.Dpi = 254F;
            this.xrTableCell34.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.RepresentativePosition]")});
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.TextFormatString = "{0}";
            this.xrTableCell34.Weight = 1.0573201517747972D;
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
            this.xrTableCell35.Text = "         Địa chỉ:";
            this.xrTableCell35.Weight = 0.83423286931820717D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.Dpi = 254F;
            this.xrTableCell63.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Address]")});
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.Weight = 2.7107315402366128D;
            // 
            // xrTableRow27
            // 
            this.xrTableRow27.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell69,
            this.xrTableCell70,
            this.xrTableCell25,
            this.xrTableCell71});
            this.xrTableRow27.Dpi = 254F;
            this.xrTableRow27.Name = "xrTableRow27";
            this.xrTableRow27.Weight = 1D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.Dpi = 254F;
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.Text = "         Điện thoại:";
            this.xrTableCell69.Weight = 0.83423286931820717D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.Dpi = 254F;
            this.xrTableCell70.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Phone]")});
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.Weight = 1.306605076857863D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Dpi = 254F;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Text = "Fax:";
            this.xrTableCell25.Weight = 0.34680549019162532D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.Dpi = 254F;
            this.xrTableCell71.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Fax]")});
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.Weight = 1.0573209731871243D;
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
            this.xrTableCell72.Text = "         Tài khoản số:";
            this.xrTableCell72.Weight = 0.83423286931820717D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.Dpi = 254F;
            this.xrTableCell78.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.AccountNumber]")});
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.Weight = 2.7107315402366123D;
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
            this.xrTableCell75.Text = "         Tại:";
            this.xrTableCell75.Weight = 0.83423286931820717D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.Dpi = 254F;
            this.xrTableCell76.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.BankName]")});
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.Weight = 2.7107315402366128D;
            // 
            // xrTableRow26
            // 
            this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell66,
            this.xrTableCell67});
            this.xrTableRow26.Dpi = 254F;
            this.xrTableRow26.Name = "xrTableRow26";
            this.xrTableRow26.Weight = 1D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.Dpi = 254F;
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.Text = "         Mã số thuế:";
            this.xrTableCell66.Weight = 0.83423286931820717D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.Dpi = 254F;
            this.xrTableCell67.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.TaxID]")});
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.Weight = 2.7107315402366128D;
            // 
            // xrTableRow25
            // 
            this.xrTableRow25.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell64,
            this.xrTableCell59});
            this.xrTableRow25.Dpi = 254F;
            this.xrTableRow25.Name = "xrTableRow25";
            this.xrTableRow25.Weight = 1D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.Dpi = 254F;
            this.xrTableCell64.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.StylePriority.UseFont = false;
            this.xrTableCell64.Text = "II - ĐẠI DIỆN BÊN B:";
            this.xrTableCell64.Weight = 0.8342326233739138D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.Dpi = 254F;
            this.xrTableCell59.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.StylePriority.UseFont = false;
            this.xrTableCell59.Text = "CÔNG TY CP KIỂM ĐỊNH KỸ THUẬT, AN TOÀN VÀ TƯ VẤN XÂY DỰNG - INCOSAF";
            this.xrTableCell59.Weight = 2.7107317861809062D;
            // 
            // xrTableRow30
            // 
            this.xrTableRow30.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell65,
            this.xrTableCell73,
            this.xrTableCell29,
            this.xrTableCell68});
            this.xrTableRow30.Dpi = 254F;
            this.xrTableRow30.Name = "xrTableRow30";
            this.xrTableRow30.Weight = 1D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.Dpi = 254F;
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.Text = "         Ông:";
            this.xrTableCell65.Weight = 0.8342326233739138D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.Dpi = 254F;
            this.xrTableCell73.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MyRepresentative]")});
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.Weight = 1.3066054147193107D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Dpi = 254F;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.Text = "Chức vụ:";
            this.xrTableCell29.Weight = 0.34680606560143074D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.Dpi = 254F;
            this.xrTableCell68.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MyRepresentativePosition]")});
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.Weight = 1.0573203058601648D;
            // 
            // xrTableRow32
            // 
            this.xrTableRow32.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell80,
            this.xrTableCell82});
            this.xrTableRow32.Dpi = 254F;
            this.xrTableRow32.Name = "xrTableRow32";
            this.xrTableRow32.Weight = 1D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.Dpi = 254F;
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.Text = "         Địa chỉ:";
            this.xrTableCell80.Weight = 0.8342326233739138D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.Dpi = 254F;
            this.xrTableCell82.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MyAddress]")});
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.Weight = 2.7107317861809062D;
            // 
            // xrTableRow33
            // 
            this.xrTableRow33.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell83,
            this.xrTableCell84,
            this.xrTableCell30,
            this.xrTableCell85});
            this.xrTableRow33.Dpi = 254F;
            this.xrTableRow33.Name = "xrTableRow33";
            this.xrTableRow33.Weight = 1D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.Dpi = 254F;
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.Text = "         Điện thoại:";
            this.xrTableCell83.Weight = 0.8342326233739138D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.Dpi = 254F;
            this.xrTableCell84.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MyPhone]")});
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.Weight = 1.3066054147193107D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Dpi = 254F;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Text = "Fax:";
            this.xrTableCell30.Weight = 0.34680606560143074D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.Dpi = 254F;
            this.xrTableCell85.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MyFax]")});
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.Weight = 1.0573203058601648D;
            // 
            // xrTableRow35
            // 
            this.xrTableRow35.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell89,
            this.xrTableCell90});
            this.xrTableRow35.Dpi = 254F;
            this.xrTableRow35.Name = "xrTableRow35";
            this.xrTableRow35.Weight = 1D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.Dpi = 254F;
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.Text = "         Tài khoản số:";
            this.xrTableCell89.Weight = 0.8342326233739138D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.Dpi = 254F;
            this.xrTableCell90.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MyAccountNumber]")});
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.Weight = 2.7107317861809062D;
            // 
            // xrTableRow34
            // 
            this.xrTableRow34.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell86,
            this.xrTableCell88});
            this.xrTableRow34.Dpi = 254F;
            this.xrTableRow34.Name = "xrTableRow34";
            this.xrTableRow34.Weight = 1D;
            // 
            // xrTableCell86
            // 
            this.xrTableCell86.Dpi = 254F;
            this.xrTableCell86.Name = "xrTableCell86";
            this.xrTableCell86.Text = "         Tại:";
            this.xrTableCell86.Weight = 0.8342326233739138D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.Dpi = 254F;
            this.xrTableCell88.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MyBankName]")});
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.Weight = 2.7107317861809062D;
            // 
            // xrTableRow31
            // 
            this.xrTableRow31.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell74,
            this.xrTableCell79});
            this.xrTableRow31.Dpi = 254F;
            this.xrTableRow31.Name = "xrTableRow31";
            this.xrTableRow31.Weight = 1D;
            // 
            // xrTableCell74
            // 
            this.xrTableCell74.Dpi = 254F;
            this.xrTableCell74.Name = "xrTableCell74";
            this.xrTableCell74.Text = "         Mã số thuế:";
            this.xrTableCell74.Weight = 0.8342326233739138D;
            // 
            // xrTableCell79
            // 
            this.xrTableCell79.Dpi = 254F;
            this.xrTableCell79.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MyTaxID]")});
            this.xrTableCell79.Name = "xrTableCell79";
            this.xrTableCell79.Weight = 2.7107317861809062D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "Hai bên cùng nhau thỏa thuận ký kết hợp đồng với các điều khoản sau:";
            this.xrTableCell7.Weight = 3.54496440955482D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell36,
            this.xrTableCell8});
            this.xrTableRow11.Dpi = 254F;
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Dpi = 254F;
            this.xrTableCell36.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.StylePriority.UseFont = false;
            this.xrTableCell36.Text = "Điều 1:";
            this.xrTableCell36.Weight = 0.33842736400280038D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.Text = "Nội dung công việc và giá trị hợp đồng:";
            this.xrTableCell8.Weight = 3.20653704555202D;
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell27});
            this.xrTableRow15.Dpi = 254F;
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.Weight = 1D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Dpi = 254F;
            this.xrTableCell27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TaskContentInReport]")});
            this.xrTableCell27.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.Weight = 3.5449644095548205D;
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
            this.xrTableCell39.Text = "Nộ dung công việc";
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
            this.xrTableCell42.Text = "Đơn giá";
            this.xrTableCell42.Weight = 2.1502031224351583D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Dpi = 254F;
            this.xrTableCell43.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.Text = "Thành tiền";
            this.xrTableCell43.Weight = 2.4830971044752497D;
            // 
            // SubBand1
            // 
            this.SubBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableTechnicalDocument});
            this.SubBand1.Dpi = 254F;
            this.SubBand1.HeightF = 279F;
            this.SubBand1.Name = "SubBand1";
            // 
            // xrTableTechnicalDocument
            // 
            this.xrTableTechnicalDocument.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableTechnicalDocument.Dpi = 254F;
            this.xrTableTechnicalDocument.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableTechnicalDocument.LocationFloat = new DevExpress.Utils.PointFloat(22.11905F, 25.4F);
            this.xrTableTechnicalDocument.Name = "xrTableTechnicalDocument";
            this.xrTableTechnicalDocument.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTableTechnicalDocument.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4,
            this.xrTableRow14,
            this.xrTableRow36,
            this.xrTableRow37});
            this.xrTableTechnicalDocument.SizeF = new System.Drawing.SizeF(1762.987F, 254F);
            this.xrTableTechnicalDocument.StylePriority.UseBorders = false;
            this.xrTableTechnicalDocument.StylePriority.UseFont = false;
            this.xrTableTechnicalDocument.StylePriority.UsePadding = false;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCellValueToWord});
            this.xrTableRow4.Dpi = 254F;
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 11.5D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UsePadding = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Bằng chữ:";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell9.Weight = 0.16718060446032318D;
            // 
            // xrTableCellValueToWord
            // 
            this.xrTableCellValueToWord.Dpi = 254F;
            this.xrTableCellValueToWord.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.xrTableCellValueToWord.Name = "xrTableCellValueToWord";
            this.xrTableCellValueToWord.StylePriority.UseFont = false;
            this.xrTableCellValueToWord.StylePriority.UseTextAlignment = false;
            this.xrTableCellValueToWord.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCellValueToWord.Weight = 1.1080147736038519D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10});
            this.xrTableRow14.Dpi = 254F;
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 11.5D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "- Tiến độ theo thỏa thuận giữa hai bên";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell10.Weight = 1.2751953780641752D;
            // 
            // xrTableRow36
            // 
            this.xrTableRow36.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell45});
            this.xrTableRow36.Dpi = 254F;
            this.xrTableRow36.Name = "xrTableRow36";
            this.xrTableRow36.Weight = 11.5D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Dpi = 254F;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StylePriority.UseTextAlignment = false;
            this.xrTableCell45.Text = "- Thời gian: bắt đầu tư khi ký hợp đồng đến khi kết thúc quá trình kiểm định";
            this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell45.Weight = 1.2751953780641752D;
            // 
            // xrTableRow37
            // 
            this.xrTableRow37.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell28,
            this.xrTableCell77});
            this.xrTableRow37.Dpi = 254F;
            this.xrTableRow37.Name = "xrTableRow37";
            this.xrTableRow37.Weight = 11.5D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "Địa điểm tại:";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell28.Weight = 0.16718057400523906D;
            // 
            // xrTableCell77
            // 
            this.xrTableCell77.Dpi = 254F;
            this.xrTableCell77.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customer.Name]")});
            this.xrTableCell77.Name = "xrTableCell77";
            this.xrTableCell77.StylePriority.UseTextAlignment = false;
            this.xrTableCell77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell77.TextFormatString = "{0}";
            this.xrTableCell77.Weight = 1.1080148040589362D;
            // 
            // SubBand2
            // 
            this.SubBand2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.SubBand2.Dpi = 254F;
            this.SubBand2.HeightF = 343F;
            this.SubBand2.Name = "SubBand2";
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable4.Dpi = 254F;
            this.xrTable4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(22.11896F, 25.4F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2,
            this.xrTableRow5,
            this.xrTableRow38,
            this.xrTableRow39,
            this.xrTableRow40});
            this.xrTable4.SizeF = new System.Drawing.SizeF(1762.986F, 317.5F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UsePadding = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell4});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Điều 2:";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell2.Weight = 0.11786185980782057D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Trách nhiệm của các bên:";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell4.Weight = 1.1573335182563544D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6});
            this.xrTableRow5.Dpi = 254F;
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 11.5D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "a/ Trách nhiệm của bên A:";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell6.Weight = 1.2751953780641752D;
            // 
            // xrTableRow38
            // 
            this.xrTableRow38.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11});
            this.xrTableRow38.Dpi = 254F;
            this.xrTableRow38.Name = "xrTableRow38";
            this.xrTableRow38.Weight = 11.5D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ResponsibilityA]")});
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell11.Weight = 1.2751953780641752D;
            // 
            // xrTableRow39
            // 
            this.xrTableRow39.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12});
            this.xrTableRow39.Dpi = 254F;
            this.xrTableRow39.Name = "xrTableRow39";
            this.xrTableRow39.Weight = 11.5D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "b/ Trách nhiệm của bên B:";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell12.Weight = 1.2751953780641752D;
            // 
            // xrTableRow40
            // 
            this.xrTableRow40.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13});
            this.xrTableRow40.Dpi = 254F;
            this.xrTableRow40.Name = "xrTableRow40";
            this.xrTableRow40.Weight = 11.5D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ResponsibilityB]")});
            this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell13.Weight = 1.2751953780641752D;
            // 
            // SubBand3
            // 
            this.SubBand3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5});
            this.SubBand3.Dpi = 254F;
            this.SubBand3.HeightF = 152F;
            this.SubBand3.Name = "SubBand3";
            // 
            // xrTable5
            // 
            this.xrTable5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable5.Dpi = 254F;
            this.xrTable5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(22.11896F, 25.4F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6,
            this.xrTableRow42});
            this.xrTable5.SizeF = new System.Drawing.SizeF(1762.986F, 127F);
            this.xrTable5.StylePriority.UseBorders = false;
            this.xrTable5.StylePriority.UseFont = false;
            this.xrTable5.StylePriority.UsePadding = false;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell14,
            this.xrTableCell15});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 11.5D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UsePadding = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "Điều 3:";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell14.Weight = 0.11786185980782057D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "Phương thức thanh toán: Chuyển khoản, séc, tiền mặt.";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell15.Weight = 1.1573335182563544D;
            // 
            // xrTableRow42
            // 
            this.xrTableRow42.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17});
            this.xrTableRow42.Dpi = 254F;
            this.xrTableRow42.Name = "xrTableRow42";
            this.xrTableRow42.Weight = 11.5D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PaymentMethod]")});
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell17.Weight = 1.2751953780641752D;
            // 
            // SubBand4
            // 
            this.SubBand4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
            this.SubBand4.Dpi = 254F;
            this.SubBand4.HeightF = 152F;
            this.SubBand4.Name = "SubBand4";
            // 
            // xrTable6
            // 
            this.xrTable6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable6.Dpi = 254F;
            this.xrTable6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(22.11927F, 25.4F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow8});
            this.xrTable6.SizeF = new System.Drawing.SizeF(1762.986F, 127F);
            this.xrTable6.StylePriority.UseBorders = false;
            this.xrTable6.StylePriority.UseFont = false;
            this.xrTable6.StylePriority.UsePadding = false;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell16,
            this.xrTableCell18});
            this.xrTableRow7.Dpi = 254F;
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 11.5D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UsePadding = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "Điều 4:";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell16.Weight = 0.11786185980782057D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "Cam kết chung:";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell18.Weight = 1.1573335182563544D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19});
            this.xrTableRow8.Dpi = 254F;
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 11.5D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Commitments]")});
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell19.Weight = 1.2751953780641752D;
            // 
            // SubBand5
            // 
            this.SubBand5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7});
            this.SubBand5.Dpi = 254F;
            this.SubBand5.HeightF = 152F;
            this.SubBand5.Name = "SubBand5";
            // 
            // xrTable7
            // 
            this.xrTable7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable7.Dpi = 254F;
            this.xrTable7.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(22.11927F, 25.4F);
            this.xrTable7.Name = "xrTable7";
            this.xrTable7.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow41,
            this.xrTableRow43});
            this.xrTable7.SizeF = new System.Drawing.SizeF(1762.986F, 127F);
            this.xrTable7.StylePriority.UseBorders = false;
            this.xrTable7.StylePriority.UseFont = false;
            this.xrTable7.StylePriority.UsePadding = false;
            // 
            // xrTableRow41
            // 
            this.xrTableRow41.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell20,
            this.xrTableCell21});
            this.xrTableRow41.Dpi = 254F;
            this.xrTableRow41.Name = "xrTableRow41";
            this.xrTableRow41.Weight = 11.5D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UsePadding = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "Điều 5:";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell20.Weight = 0.11786185980782057D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Dpi = 254F;
            this.xrTableCell21.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "Hiệu lực hợp đồng:";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell21.Weight = 1.1573335182563544D;
            // 
            // xrTableRow43
            // 
            this.xrTableRow43.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell22});
            this.xrTableRow43.Dpi = 254F;
            this.xrTableRow43.Name = "xrTableRow43";
            this.xrTableRow43.Weight = 11.5D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Dpi = 254F;
            this.xrTableCell22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Effective]")});
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell22.Weight = 1.2751953780641752D;
            // 
            // SubBand9
            // 
            this.SubBand9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel28});
            this.SubBand9.Dpi = 254F;
            this.SubBand9.HeightF = 254F;
            this.SubBand9.Name = "SubBand9";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(1149.91F, 104.14F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(454.7771F, 45.72F);
            this.xrLabel8.StyleName = "FieldCaption";
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "ĐẠI DIỆN BÊN B";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Dpi = 254F;
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(72.8605F, 104.14F);
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
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel29});
            this.reportHeaderBand.Dpi = 254F;
            this.reportHeaderBand.HeightF = 233F;
            this.reportHeaderBand.Name = "reportHeaderBand";
            // 
            // xrLabel32
            // 
            this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel32.Dpi = 254F;
            this.xrLabel32.EditOptions.Enabled = true;
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(729.7837F, 174.5799F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(874.9038F, 58.42007F);
            this.xrLabel32.StylePriority.UseBorders = false;
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "............, ngày ......... tháng ......... năm 20......";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel31
            // 
            this.xrLabel31.Dpi = 254F;
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(15.24003F, 100F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(1770F, 50F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "----o0o----";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel30
            // 
            this.xrLabel30.Dpi = 254F;
            this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(15.24011F, 50.00002F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(1770F, 50F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "Độc lập - Tự do - Hạnh phúc";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Dpi = 254F;
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(15.24011F, 0F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(1770F, 50F);
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
            // CustomerName
            // 
            this.CustomerName.Expression = "Upper([customer.Name])";
            this.CustomerName.Name = "CustomerName";
            // 
            // TaskContentInReport
            // 
            this.TaskContentInReport.Expression = "\'Bên A đồng ý giao và bên B đồng ý nhận thực hiện việc kiểm định, kiểm tra các th" +
    "iết bị có yêu cầu nghiêm ngặt AT của: [customer.Name]\'";
            this.TaskContentInReport.Name = "TaskContentInReport";
            // 
            // MyPhone
            // 
            this.MyPhone.Expression = "\'04.38527102\'";
            this.MyPhone.Name = "MyPhone";
            // 
            // MyFax
            // 
            this.MyFax.Expression = "\'04.38835465\'";
            this.MyFax.Name = "MyFax";
            // 
            // MyAccountNumber
            // 
            this.MyAccountNumber.Expression = "\'121.10.00.0039689\'";
            this.MyAccountNumber.Name = "MyAccountNumber";
            // 
            // MyTaxID
            // 
            this.MyTaxID.Expression = "\'0100763132\'";
            this.MyTaxID.Name = "MyTaxID";
            // 
            // MyRepresentative
            // 
            this.MyRepresentative.Expression = "\'Lê Phú Hải\'";
            this.MyRepresentative.Name = "MyRepresentative";
            // 
            // MyRepresentativePosition
            // 
            this.MyRepresentativePosition.Expression = "\'Tổng giám đốc\'";
            this.MyRepresentativePosition.Name = "MyRepresentativePosition";
            // 
            // MyAddress
            // 
            this.MyAddress.Expression = "\'243A Đê La Thành - Phường Láng Thượng - Đống Đa - Hà Nội\'";
            this.MyAddress.Name = "MyAddress";
            // 
            // MyBankName
            // 
            this.MyBankName.Expression = "\'Ngân hàng TMCP đầu tư và phát triển Việt Nam - Chi nhánh Hai Bà Trưng\'";
            this.MyBankName.Name = "MyBankName";
            // 
            // ContractReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.pageFooterBand,
            this.reportHeaderBand});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.CustomerName,
            this.TaskContentInReport,
            this.MyPhone,
            this.MyFax,
            this.MyAccountNumber,
            this.MyTaxID,
            this.MyRepresentative,
            this.MyRepresentativePosition,
            this.MyAddress,
            this.MyBankName});
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
            this.Version = "19.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableChungKien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTasks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTechnicalDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
