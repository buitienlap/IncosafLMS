using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    // Nội dung công việc trong hợp đồng
    public class Task
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        // Đơn vị
        public string Unit { get; set; }
        // Đơn giá
        public double UnitPrice { get; set; }
        // Khối lượng
        public double Amount { get; set; }
        // Danh sách kiểm định
        public virtual List<Accreditation> Accreditations { get; set; }
    }
}
