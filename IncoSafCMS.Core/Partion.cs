using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    // Cơ cấu, bộ phận
    public class EquimentPartion
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        // Thử không tải
        public bool? Passed1 { get; set; }
        // Thử có tải
        public bool? Passed2 { get; set; }
        // Ghi chú
        public string Note { get; set; }
    }
}
