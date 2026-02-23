using IncosafCMS.Core.DomainModels;
using System;
using System.Collections.Generic;               // ← bắt buộc cho List<>
using System.ComponentModel.DataAnnotations.Schema;

namespace IncosafCMS.Web.Models
{
    public class ContractViewModel
    {
        
        public ContractViewModel() { }
        public int Id { get; set; }
        public DateTime? SignDate { get; set; }
        public string GetSignDate { get { return this.SignDate?.ToString("dd/MM/yyyy"); } }
        public DateTime? CreateDate { get; set; }
        public string MaHD { get; set; }
        public string Name { get; set; }
        public int? OwnerId { get; set; }
        public string UserName { get; set; }
        public string OwnerDisplayName { get; set; }
        public string KDV1DisplayName { get; set; }
        public string KDV2DisplayName { get; set; }
        public int? DepartmentId { get; set; }
        public string MaDV { get; set; }
        public string CustomerName { get; set; }
        //public string CustomerRepresentative { get; set; }
        public string CustomerAddress { get; set; }
       // public string CustomerPhone { get; set; }
       // public string CustomerFax { get; set; }
       // public string CustomerAccountNumber { get; set; }
        public string CustomerTaxID { get; set; }
        public bool IsGiayDeNghi { get; set; }      // added by lapbt. hien thi ben home chi tiet
        public double RatioOfGroup { get; set; } 
        public int contractType { get; set; } // để hiển thị Loại hình hợp đồng
        public double Value { get; set; }
        public double RatioOfCompany { get; set; }
        public double ValueRoC { get; set; }
        public double RatioOfInternal { get; set; }
        public double ValueRoI { get; set; }       
        public bool Finished { get; set; }
        public double XuatHoaDon { get; set; }        
        public double ThuNo { get; set; }
        public double CongNo
        {
            get
            {
                if (this.Finished) return 0;
                return this.XuatHoaDon - this.ThuNo;
            }
        }
        public double DoDang
        {
            get
            {
                if (this.Finished) return 0;
                return this.Value - this.XuatHoaDon;
            }
        }

        // Tổng số thiết bị đã thực hiện KĐ
        public double? TotalEquipment { get; set; }
        // Tổng giá trị đã thanh toán cho Trung tâm
        public double? TotalValueIPoC { get; set; }
        
        // Hoàn thành thanh toán cho Trung tâm
        public bool IsCompletedIPoC
        {
            get { return ValueRoC - TotalValueIPoC == 0; }
        }
        // Tổng giá trị đã thanh toán cho Chủ trì
        public double? TotalValueIPoI { get; set; }
        
        // Hoàn thành thanh toán cho Chủ trì
        public bool IsCompletedIPoI
        {
            get { return ValueRoI - TotalValueIPoI == 0; }
        }

        public ApproveStatus Status { get; set; }

        public double DocCount { get; set; } //số lượng hồ sơ lưu
    }
}