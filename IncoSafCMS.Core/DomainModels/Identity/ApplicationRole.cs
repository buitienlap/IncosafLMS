using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace IncosafCMS.Core.DomainModels.Identity
{
    public class ApplicationRole: IdentityRole
    {
        public ApplicationRole()
        {
            Users = new List<ApplicationUserRole>();
            //Permissions = new List<AppPermission>();
        }

        public int Id
        {
            get; set;
        }

        public virtual ICollection<ApplicationUserRole> Users{ get; private set; }

        public string Name { get; set; }
        //public virtual List<AppPermission> Permissions { set; get; }
    }
}
