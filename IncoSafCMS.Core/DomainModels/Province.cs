using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    public class Province: BaseEntity
    {
        public string Ma_TP { set; get; }   // added by lapbt
        public string ProvinceCode { set; get; }
        public string ProvinceName { set; get; }
    }
    
}
