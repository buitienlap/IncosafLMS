using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    public class UserGroup
    {
        public string ID { get; set; }
        public string GroupName { get; set; }
        public virtual List<Permission> Permissions { get; set; }
        public UserGroup()
        {
            Permissions = new List<Permission>();
        }
    }
}
