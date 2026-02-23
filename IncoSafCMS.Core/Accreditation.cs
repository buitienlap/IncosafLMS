using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    // Thông tin Kiểm định chất lượng
    public class Accreditation
    {
        // Số kiểm định
        [Key]
        public string Number { get; set; }
        // Ngày tháng lập biên bản kiểm định
        public DateTime Date { get; set; }
        // Thiết bị
        public virtual Equiment equiment { get; set; }
        // Khách hàng
        public virtual Custommer custommer { get; set; }
        // Vị trí lắp đặt
        public string Location { get; set; }
        // Tiêu chuẩn áp dụng
        public virtual Standard EmployedStandard { get; set; }
        // Quy trình áp dụng
        public virtual Procedure EmployedProcedure { get; set; }
        // Danh sách kiểm định viên
        public virtual List<User> Tester { get; set; }
        // Hình thức kiểm định
        public string Type { get; set; }
        // Tải trọng tương ứng
        public double CorrespondingLoad { get; set; }
        // Tải trọng tĩnh
        public double StaticLoad { get; set; }
        // Tải trọng động
        public double DynamicLoad { get; set; }
        // Số tem kiểm định
        public string StampNumber { get; set; }
        // Vị trí dán tem
        public string StampLocated { get; set; }
        // Kiến nghị
        public string Requests { get; set; }
        // Thời gian kiểm định lần sau
        public DateTime DateOfNext { get; set; }


    }
}
