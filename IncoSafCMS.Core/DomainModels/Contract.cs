using IncosafCMS.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;




namespace IncosafCMS.Core.DomainModels
{
    // Hợp đồng
    public class Contract : BaseEntity
    {
        public Contract()
        {
            //this.Tasks = new List<AccTask>();
            //this.Payments = new List<Payment>();
            //this.TurnOvers = new List<TurnOver>();
            //this.InternalPayments = new List<InternalPayment>();
            //this.vActTurnOvers = new List<v_ActTurnOver>();
            //this.vActPayment = new List<v_ActPayment>();
        }
        // Tên hợp đồng
        public string MaHD { get; set; }
        //[Required(ErrorMessage = "Tên hợp đồng không được bỏ trống")]
        public string Name { get; set; }
        // Ngày tạo/Ngày đề nghị
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime CreateDate { get; set; } = DateTime.Today;
        //public DateTime CreateDate { get; set; }
        public string GetSignDate { get { return this.SignDate?.ToString("dd/MM/yyyy"); } }
        // Ngày ký hợp đồng
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        //public DateTime? SignDate { get; set; } = DateTime.Today;
        public DateTime? SignDate { get; set; }
        // Ngày ký thực hiện
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? NgayThucHien { get; set; }// = DateTime.Today;
        // Ngày thực hiện đến. Added by lapbt 10/05/2025
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? NgayThucHienDen { get; set; }
        //public virtual int customerId { set; get; }
        // Khách hàng
        [Required(ErrorMessage = "Khách hàng không được bỏ trống")]
        public virtual Customer customer { get; set; }
        // Trách nhiệm bên A
        public string ResponsibilityA { get; set; }
        // Trách nhiệm bên B
        public string ResponsibilityB { get; set; }
        // Phương thức thanh toán
        public string PaymentMethod { get; set; }
        // Cam kết chung
        public string Commitments { get; set; }
        // Hiệu lực hợp đồng
        public string Effective { get; set; }
        // added by lapbt, k0 co lien ket voi ma khach hang!!! them vao se bi loi tao hop dong moi, co the khi tao k0 chon KH, nen k0 co lien ket
        // public int customer_id { get; set; }
        public virtual AppUser own { get; set; }
        public virtual AppUser KDV1 { get; set; }
        public virtual AppUser KDV2 { get; set; }
        public string DiaDiemThucHien { set; get; }
        [Required(ErrorMessage = "Tình trạng hợp đồng không được bỏ trống")]
        public ApproveStatus Status { get; set; }
        // Địa điểm thực hiện
        //public string DiaDiemThucHien { get; set; }
        // Giá trị hợp đồng
        public double Value
        {
            set; get;
        }
        
        // Giá trị hóa đơn đã trừ thuế
        public double TienTruThue
        {
            set; get;
        }

        // Tổng giá trị tiền về
        public double TongTienVe
        {
            set; get;
            //get
            //{
            //    double tien = 0;
            //    if (Payments != null)
            //    {
            //        foreach (var payment in Payments)
            //        {
            //            tien += payment.PaymentValue;
            //        }
            //    }
            //    return tien;
            //}
        }
        // Tổng giá trị xuất hóa đơn
        public double TongTienXuatHoaDon
        {
            set; get;
        }
        // Tỉ lệ giao khoán công ty
        //public double RatioOfCompany { get; set; } = 78;
        public double RatioOfCompany { get; set; } = Convert.ToDouble(ConfigurationManager.AppSettings["RatioOfCompany"]);
        //
        // Giá trị giao khoán công ty
        public double ValueRoC
        {
            get
            {
                return (this.RatioOfCompany * this.Value / (1 + this.VAT / 100)) / 100;
            }
        }
        // Giá trị hóa đơn giao khoán công ty
        public double ValueHDRoC
        {
            get
            {
                return (this.RatioOfCompany * this.TienTruThue) / 100;
            }
        }
        // Tỉ lệ giao khoán nội bộ
        //public double RatioOfInternal { get; set; } = 52;
        public double RatioOfInternal { get; set; } = Convert.ToDouble(ConfigurationManager.AppSettings["RatioOfInternal"]);
        // Giá trị giao khoán nội bộ
        public double ValueRoI
        {
            get
            {
                return (this.RatioOfInternal * this.Value / (1 + this.VAT / 100)) / 100;
            }
        }
        // Tỉ lệ giao khoán nhóm
        public double RatioOfGroup { get; set; } = 0;
        public double ValueRoG
        {
            get
            {
                return (this.RatioOfGroup * this.Value / (1 + this.VAT / 100)) / 100;
            }
        }
        
        // Giá trị hóa đơn giao khoán nội bộ
        public double ValueHDRoI
        {
            get
            {
                return (this.RatioOfInternal * this.TienTruThue) / 100;
            }
        }

        // Loại thuế VAT                
        public VATType vatType { get; set; }
        // Thuế VAT, Default VAT 8%, Edit by Lapbt
        //public double VAT { get; set; } = 8;
        public double VAT { get; set; }

       
        public double CongNo
        {
            get
            {
                if (this.Finished) return 0;
                var congno = this.Value - this.TongTienVe;
                if (congno < 0) return 0;
                return this.Value - this.TongTienVe;
            }
        }
       

        public bool Finished { get; set; }
        public bool IsGiayDeNghi { get; set; }
        public int contractType { get; set; } = 0;    // 22-may-2024. de su dung cho loai hop dong
        public string NguoiLienHe { get; set; }
        public string DienThoaiNguoiLienHe { get; set; }

        // 14.11.2025 THÊM CỘT CreateYear TỪ CreateDate  
        [Column("CreateYear")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int CreateYear { get; set; }

        public Contract Copy()
        {
            var newContract = new Contract()
            {
                Id = 0,
                MaHD = "",
                Name = this.Name,
                CreateDate = DateTime.Today,
                customer = this.customer,
                Commitments = this.Commitments,
                Effective = this.Effective,
                ResponsibilityA = this.ResponsibilityA,
                ResponsibilityB = this.ResponsibilityB,
                PaymentMethod = this.PaymentMethod,
                own = this.own,
                Status = ApproveStatus.Waiting,
                //Payments = new List<Payment>(),
                RatioOfCompany = this.RatioOfCompany,
                RatioOfInternal = this.RatioOfInternal,
                SignDate = DateTime.Today,
                //Tasks = new List<AccTask>(),
                //TurnOvers = new List<TurnOver>(),
                //InternalPayments = new List<InternalPayment>(),
                VAT = this.VAT,
                vatType = this.vatType,
                NgayThucHien = this.NgayThucHien,
                NgayThucHienDen = this.NgayThucHienDen,
                TienTruThue = this.TienTruThue,
                //TongTienVe = this.TongTienVe,
                //TongTienXuatHoaDon = this.TongTienXuatHoaDon,
                // lapbt edit 30-may-2021. 
                TongTienVe = 0,
                TongTienXuatHoaDon = 0,
                IsGiayDeNghi = this.IsGiayDeNghi,
                Value = this.Value,
                NguoiLienHe = this.NguoiLienHe,
                DienThoaiNguoiLienHe = this.DienThoaiNguoiLienHe,
                DiaDiemThucHien = this.DiaDiemThucHien
            };

            //foreach (var task in this.Tasks)
            //{
            //    var newTask = task.Copy();
            //    newContract.Tasks.Add(newTask);
            //}

            return newContract;
        }
    }

}
