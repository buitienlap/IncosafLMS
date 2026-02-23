using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncosafCMS.Web.Helpers
{
    public class GridViewToolbarHelper
    {
        public const string KeyFieldName = "Id";

        static MVCxGridViewColumnCollection exportedColumns;
        public static MVCxGridViewColumnCollection ExportedColumns
        {
            get
            {
                if (exportedColumns == null)
                    exportedColumns = CreateExportedColumns();
                return exportedColumns;
            }
        }

        static GridViewSettings exportGridSettings;
        public static GridViewSettings ExportGridSettings
        {
            get
            {
                if (exportGridSettings == null)
                    exportGridSettings = CreateExportGridSettings();
                return exportGridSettings;
            }
        }

        static MVCxGridViewColumnCollection CreateExportedColumns()
        {
            var columns = new MVCxGridViewColumnCollection();
            columns.Add("Name", "Tên thiết bị");
            columns.Add("Code", "Mã TB");
            columns.Add("MaHD", "Mã HĐ");
            columns.Add("mahieu", "Mã hiệu");
            columns.Add("No", "Số chế tạo");
            columns.Add("YearOfProduction", "Năm SX");
            columns.Add("ManuFacturer", "Nhà sản xuất");

            columns.Add("TypeOfAccr", "Hình thức KĐ");


            columns.Add("ContractCreateDate", "Ngày tạo HĐ", MVCxGridViewColumnType.DateEdit).PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            columns.Add("ContractSignDate", "Ngày ký", MVCxGridViewColumnType.DateEdit).PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            columns.Add("CreateDate", "Ngày tạo TB", MVCxGridViewColumnType.DateEdit).PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            columns.Add("AccreDate", "Ngày KĐ", MVCxGridViewColumnType.DateEdit).PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            columns.Add("NextAccreDate", "Hạn KĐ", MVCxGridViewColumnType.DateEdit).PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            columns.Add("GetAccreResultNumber", "Số KQKĐ");
            columns.Add("StampNumber", "Số tem");
            columns.Add("CustomerName", "Khách hàng");
            columns.Add("CustomerPhoneNumber", "Điện thoại");
            columns.Add("OwnerDisplayname", "KĐV");

            //columns.Add("XuatHoaDon", "Xuất h.đơn");
            //columns.Add("ThuNo", "Thu nợ");
            return columns;
        }

        static GridViewSettings CreateExportGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Danh sách thiết bị";
            settings.KeyFieldName = KeyFieldName;
            settings.Columns.Assign(ExportedColumns);
            return settings;
        }
    }
}