using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    // Thống số kỹ thuật
    public class Specifications
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
