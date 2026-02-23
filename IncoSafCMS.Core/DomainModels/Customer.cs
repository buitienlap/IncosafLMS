using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            //Contracts = new HashSet<Contract>();
        }
        public string MaKH { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string TaxID { get; set; }
        public string Representative { get; set; }
        public string RepresentativePosition { get; set; }
        //public virtual int? departmentId { set; get; }
        public virtual Department department { get; set; }

        //public virtual ICollection<Contract> Contracts { get; set; }

    }
}
