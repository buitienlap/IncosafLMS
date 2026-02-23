using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels.Identity
{
    public class AppPermission : BaseEntity
    {
        public AppPermission()
        {
            Roles = new List<ApplicationPermissionRole>();
        }
        public string Name { set; get; }
        public string Description { set; get; }
        public virtual ICollection<ApplicationPermissionRole> Roles { private set; get; }
    }
}
