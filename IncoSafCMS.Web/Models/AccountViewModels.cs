using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.DomainModels.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IncosafCMS.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
        public IEnumerable<ApplicationAuthenticationDescription> LoginProviders { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Mã xác nhận")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Ghi nhớ trình duyệt này?")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Địa chỉ Email không được bỏ trống")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập?", Description = "Ghi nhớ đăng nhập?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        [Display(Name = "Tên hiển thị")]
        public string DisplayName { get; set; }

        [Display(Name = "Số ĐT")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không đúng định dạng")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} phải chứa ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và Xác nhận mật khẩu không khớp nhau.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Phòng ban")]
        public Department department { get; set; }

        [Display(Name = "Chức vụ")]
        public EmployeePosition Position { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Địa chỉ Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không đúng định dạng")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} phải chứa ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và Xác nhận mật khẩu không khớp nhau.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Địa chỉ Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không đúng định dạng")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}