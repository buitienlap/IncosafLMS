using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace IncosafCMS.Data.Identity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationIdentityUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationIdentityUser :
        IdentityUser<int, ApplicationIdentityUserLogin, ApplicationIdentityUserRole, ApplicationIdentityUserClaim>
    {
        //public string Foor { set; get; }
        public virtual string MaNV { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual EmployeePosition Position { get; set; }
        public virtual string Address { get; set; }
        public string Tags { get; set; }
        public virtual Department Department { get; set; }
        //public virtual List<Accreditation> Accreditations { get; set; }
        //public virtual List<Notification> Notifications { set; get; }
        //public virtual List<SanLuongDK> SanLuongDK { get; set; }
    }


    public class ApplicationIdentityRole : IdentityRole<int, ApplicationIdentityUserRole>
    {
        public ApplicationIdentityRole()
        {
            //Permissions = new List<AppPermission>();
        }

        public ApplicationIdentityRole(string name)
        {
            Name = name;
            //Permissions = new List<AppPermission>();
        }
        //public List<AppPermission> Permissions { set; get; }
        //public string Foor { set; get; }
    }

    public class ApplicationIdentityUserRole : IdentityUserRole<int>
    {
    }

    public class ApplicationIdentityUserClaim : IdentityUserClaim<int>
    {
    }

    public class ApplicationIdentityUserLogin : IdentityUserLogin<int>
    {
    }

}