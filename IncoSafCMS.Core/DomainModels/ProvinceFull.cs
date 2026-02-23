using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    // added by lapbt
    public class ProvinceFull : BaseEntity
    {
        public string Ten_TP { set; get; }
        public string Ma_TP { set; get; }
        public string Ten_QH { set; get; }
        public string Ma_QH { set; get; }
        public string Ten_PX { set; get; }
        public string Ma_PX { set; get; }
    }
}
