using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;

namespace IncosafCMS.Web.Helpers
{
    /// <summary>
    /// based on
    /// </summary>
    public class QRCodeHelper
    {
        IUnitOfWork uow;

        /* using
         * 
        
         */

        public static string GetEquipment_QRCode_GCN(int vEquip_id, bool isAutoLock = false)
        {
            //var equip = uow.Repository<Accreditation>().FindBy(x => x.AccTask_Id == GridViewHelper.SelectedTaskID);

            //string tbl = "MyList";
            //string sql = string.Format("select status, isPrintGcn, qr_gcn from Equipment where Id = {0}", vEquip_id);

            //DataSet ds = oConn.selectSQLds(sql, tbl);
            //if (ds.Tables["MyList"].Rows.Count == 1)
            //{
            //    foreach (DataRow row in ds.Tables["MyList"].Rows)
            //    {
            //        if ((bool)row["isPrintGcn"] && row["qr_gcn"].ToString() != "")
            //        {
            //            // đã in thì lấy lại mã k0 xly gì nữa
            //            return row["qr_gcn"].ToString();
            //        }
            //        else
            //        {
            //            // gen & update QR code here
            //            return GenUpdate_Equipment_QRCode_GCN(vEquip_id, isAutoLock);
            //        }
            //    }
            //}

            return ""; // lỗi k0 tạo hay k0 xác định đc mã QR
        }
    }
}