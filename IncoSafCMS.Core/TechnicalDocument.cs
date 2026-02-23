using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    // Hồ sơ kỹ thuật
    public class TechnicalDocument
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public bool Passed { get; set; }
        public string Note { get; set; }
    }
}
