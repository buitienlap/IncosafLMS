using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    // added by lapbt
    public class v_ProvinceDistrict : BaseEntity
    {
        public string Ten_TP { set; get; }
        public string Ma_TP { set; get; }
        public string Ten_QH { set; get; }
        public string Ma_QH { set; get; }
    }
}
