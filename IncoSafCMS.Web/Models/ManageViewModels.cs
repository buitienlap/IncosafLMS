using IncosafCMS.Core.DomainModels.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IncosafCMS.Web.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<ApplicationUserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<ApplicationUserLoginInfo> CurrentLogins { get; set; }
        public IList<ApplicationAuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu mới không được bỏ trống.")]
        [StringLength(100, ErrorMessage = "{0} phải chứa ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp nhau.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại không được bỏ trống.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được bỏ trống.")]
        [StringLength(100, ErrorMessage = "{0} phải chưa ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp nhau.")]
        public string ConfirmPassword { get; set; }
    }


    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Điện thoại")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Mã xác nhận")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Điện thoại")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

}