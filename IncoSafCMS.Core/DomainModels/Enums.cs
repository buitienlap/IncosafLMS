using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    public enum ApproveStatus
    {
        [Description("Chờ duyệt")]
        Waiting,
        [Description("Đã duyệt")]
        ApprovedLv1,
        [Description("Đã cấp số")]
        ApprovedLv2
    }
    public enum TypeOfAccr
    {
        [Description("")]
        ChuaChon,
        [Description("Lần đầu")]
        LanDau,
        [Description("Định kỳ")]
        DinhKy,
        [Description("Sau lắp đặt")]
        SauLapDat,
        [Description("Sau sửa chữa")]
        SauSuaChua,
        [Description("Bất thường")]
        BatThuong,
        [Description("Hàng năm")]
        HangNam
    }
    public enum PaymentMethod
    {
        [Description("Tiền mặt")]
        directly,
        [Description("Chuyển khoản")]
        transfer
    }
    public enum VATType
    {
        [Description("Có thuế")]
        cothue,
        [Description("Không thuế")]
        khongthue
    }

    public enum InternalPaymentType
    {
        [Description("Trung tâm")]
        ThanhToanTrungTam,
        [Description("Chủ trì")]
        ThanhToanChuTri,
        [Description("Nhóm")]
        ThanhToanNhom
    }
}
