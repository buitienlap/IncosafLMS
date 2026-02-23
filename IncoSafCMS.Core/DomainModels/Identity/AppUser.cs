using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncosafCMS.Core.DomainModels.Identity
{
    public class AppUser : BaseEntity
    {
        public AppUser()
        {
            Claims = new List<ApplicationUserClaim>();
            Roles = new List<ApplicationUserRole>();
            Logins = new List<ApplicationUserLogin>();
            Accreditations = new List<Accreditation>();
            Notifications = new List<Notification>();
            //SanLuongDK = new List<SanLuongDK>();
        }
        public virtual int AccessFailedCount { get; set; }
        public virtual ICollection<ApplicationUserClaim> Claims { get; private set; }

        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        //public virtual int Id { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        //public virtual DateTime? LockoutEndDateUtc { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; private set; }
        public virtual string PasswordHash { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual ICollection<ApplicationUserRole> Roles { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual string UserName { get; set; }
        public virtual string MaNV { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual int? PositionId { set; get; }
        public virtual EmployeePosition Position { get; set; }
        public virtual string Address { get; set; }
        public virtual int? DepartmentId { set; get; }
        public virtual Department Department { get; set; }
        public virtual List<Accreditation> Accreditations { get; set; }
        public virtual List<Notification> Notifications { set; get; }
        //public virtual List<SanLuongDK> SanLuongDK { get; set; }
        public string Tags { get; set; }
    }
}
