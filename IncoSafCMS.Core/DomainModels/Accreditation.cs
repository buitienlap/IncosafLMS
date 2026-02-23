using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    // Thông tin Kiểm định chất lượng
    //[Table("Accreditations")]
    public class Accreditation : BaseEntity
    {
        public Accreditation()
        {
        }

        // Số kiểm định
        public string NumberAcc { get; set; }
        // Ngày tạo
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        // Ngày kiểm định
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? AccrDate { get; set; } = DateTime.Now;
        // Ngày cấp kết quả
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? AccrResultDate { get; set; } = DateTime.Now;
        // Vị trí lắp đặt
        public string Location { get; set; }
        // Danh sách kiểm định viên
        public virtual List<AppUser> Tester { get; set; }
        public int? Tester1Id { set; get; }
        //[ForeignKey("Tester1Id")]
        //public AppUser Tester1 { get; set; }
        public int? Tester2Id { set; get; }
        //[ForeignKey("Tester2Id")]
        //public AppUser Tester2 { get; set; }
        // Người chứng kiến 1
        public string Viewer1 { get; set; }
        // Chức vụ người chứng kiến 1
        public string PositionViewer1 { get; set; }
        // Người chứng kiến 2
        public string Viewer2 { get; set; }
        // Chức vụ người chứng kiến 2
        public string PositionViewer2 { get; set; }
        // Hình thức kiểm định
        public TypeOfAccr TypeAcc { get; set; }
        // Địa điểm kiểm định
        public string AccrLocation { get; set; }
        // 01-apr-2024 Added by Lapbt. Luu chi tiet TP/QH/PX
        public string Ma_TP { get; set; }
        public string Ma_QH { get; set; }
        public string Ma_PX { get; set; }

        // Tải trọng tương ứng
        public double CorrespondingLoad { get; set; }
        // Tải trọng tĩnh
        public double StaticLoad { get; set; }
        // Tải trọng động
        public double DynamicLoad { get; set; }


        // Nhận xét về việc kiểm tra hồ sơ kỹ thuật
        public string DocumentTechnicalNotice { get; set; }
        // Đánh giá kết quả về việc kiểm tra hồ sơ kỹ thuật
        public string DocumentTechicalResult { get; set; }
        public string MissingDocs { set; get; }
        public DateTime? MissingDocsCompletionDate { set; get; }
        // Nhận xét về việc kiểm tra bên ngoài; thử không tải --> Tên Đơn vị sử dụng
        public string PartionsNotice { get; set; }
        // Nhận xét về việc thử tải     --> Địa chỉ Đơn vị sử dụng
        public string LoadTestNotice { get; set; }

        // Số tem kiểm định
        public string StampNumber { get; set; }
        // Vị trí dán tem
        public string StampLocated { get; set; }
        // Số serial tem để làm id đối chiếu với bảng quản lý cấp phát tem. Added 28/12
        public int? SerialNumber { get; set; } = 0;
        // Kiến nghị
        public string Requests { get; set; }
        // Thời hạn thực hiện kiến nghị
        public string RequestsTime { get; set; }
        // Thời gian kiểm định lần sau
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? DateOfNext { get; set; } = DateTime.Now.AddYears(1);
        // Hoàn thành việc kiểm định?
        public bool IsCompleted { get; set; }
        // Kết quả kiểm định
        public bool AccreditationResult { get; set; }
        public int? AccTask_Id { set; get; }
        public Accreditation Copy()
        {
            var newAccreditation = new Accreditation()
            {
                Id = 0,
                CorrespondingLoad = this.CorrespondingLoad,
                CreateDate = this.CreateDate,
                AccrDate = this.AccrDate,
                AccrResultDate = this.AccrResultDate,
                DateOfNext = this.DateOfNext,
                DynamicLoad = this.DynamicLoad,
                IsCompleted = this.IsCompleted,
                Location = this.Location,
                //NumberSuggest = this.NumberSuggest,
                NumberAcc = this.NumberAcc,
                Requests = this.Requests,
                RequestsTime = this.RequestsTime,
                DocumentTechnicalNotice = this.DocumentTechnicalNotice,
                DocumentTechicalResult = this.DocumentTechicalResult,
                PartionsNotice = this.PartionsNotice,
                LoadTestNotice = this.LoadTestNotice,
                StampLocated = this.StampLocated,
                StampNumber = this.StampNumber,
                SerialNumber = this.SerialNumber,
                StaticLoad = this.StaticLoad,
                //Tester = this.Tester,
                Viewer1 = this.Viewer1,
                PositionViewer1 = this.PositionViewer1,
                Viewer2 = this.Viewer2,
                PositionViewer2 = this.PositionViewer2,
                TypeAcc = this.TypeAcc,
                AccrLocation = this.AccrLocation,
                Ma_TP = this.Ma_TP,
                Ma_QH = this.Ma_QH,
                Ma_PX = this.Ma_PX
            };
            return newAccreditation;
        }
    }
}
