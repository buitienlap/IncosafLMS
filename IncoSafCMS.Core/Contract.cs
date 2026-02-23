using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    // Hợp đồng
    public class Contract
    {
        [Key]
        public string ID { get; set; }
        // Tên hợp đồng
        public string Name { get; set; }
        public DateTime Date { get; set; }
        // Khách hàng
        public virtual Custommer custommer { get; set; }
        // Nội dung công việc
        public virtual List<Task> Tasks { get; set; }
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

    }
}
