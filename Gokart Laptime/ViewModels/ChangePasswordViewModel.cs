using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "The current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "The new password is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "The new password has to be between 8 - 20 charaters")]
        public string NewPassword { get; set; }

        [Display(Name = "New Password again")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The passwords don't match")]
        public string NewPasswordConfirm { get; set; }
    }
}
