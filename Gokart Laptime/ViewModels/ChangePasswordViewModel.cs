using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Current Password", Prompt = "CurrentPasswordPlaceHolder")]
        [Required(ErrorMessage = "CurrentPasswordRequired")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password", Prompt = "NewPasswordPlaceHolder")]
        [Required(ErrorMessage = "NewPasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "NewPasswordLength")]
        public string NewPassword { get; set; }

        [Display(Name = "New Password again", Prompt = "NewPasswordAgainPlaceHolder")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "PasswordsMatch")]
        public string NewPasswordConfirm { get; set; }
    }
}
