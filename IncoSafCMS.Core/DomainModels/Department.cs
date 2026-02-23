using IncosafCMS.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    public class Department : BaseEntity
    {
        public Department()
        {
            Employees = new List<AppUser>();
            Customers = new List<Customer>();
        }
        public string MaDV { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        //public virtual AppUser Representative { set; get; }
        public virtual List<AppUser> Employees { get; set; }
        public string Email { get; set; }
        public virtual List<Customer> Customers { get; set; }
    }
}
