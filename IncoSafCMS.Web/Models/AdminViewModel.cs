using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IncosafCMS.Web.Models
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Tên nhóm")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}