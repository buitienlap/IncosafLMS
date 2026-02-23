using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    public class Permission
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
