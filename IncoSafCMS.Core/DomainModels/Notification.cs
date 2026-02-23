using IncosafCMS.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncosafCMS.Core.DomainModels
{
    public class Notification : BaseEntity
    {
        public Notification()
        {
            targetUsers = new List<AppUser>();
        }
        //public AppUser owner { set; get; }
        public virtual List<AppUser> targetUsers { set; get; }
        public string content { set; get; }
        public DateTime SentTime { set; get; }
        public bool read { set; get; }
        public string target_url { set; get; }
    }
}
