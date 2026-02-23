using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    // Tiêu chuẩn áp dụng
    public class Standard
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
