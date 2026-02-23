using IncosafCMS.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    public class EmployeePosition : BaseEntity
    {
        public EmployeePosition()
        {
            Users = new HashSet<AppUser>();
        }
        public string Name { get; set; }
        public virtual ICollection<AppUser> Users { set; get; }
    }
}
