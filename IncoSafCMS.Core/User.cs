using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        // Số hiệu kiểm định viên
        public string TesterNumber { get; set; }
        public virtual List<UserGroup> UserGroups { get; set; }
        public User()
        {
            UserGroups = new List<UserGroup>();
        }
    }
}
