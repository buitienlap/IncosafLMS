using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels.Identity
{
    public class ApplicationPermissionRole : BaseEntity
    {
        //public virtual AppPermission Permission { set; get; }
        public virtual int RoleId { get; set; }
        public virtual int PermissionId { get; set; }
    }
}
