using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class UserRegisterViewModel
    {
        [Display(Name = "Username", Prompt = "UsernamePlaceHolder")]
        [Required(ErrorMessage = "UsernameRequired")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "UsernameLength")]
        public string UserName { get; set; }

        [Display(Name = "Email address", Prompt = "EmailPlaceHolder")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$", ErrorMessage = "ValidEmail")]
        [Required(ErrorMessage = "EmailAddressRequired")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "EmailAddressLength")]
        public string Email { get; set; }

        [Display(Name = "Password", Prompt = "PasswordPlaceHolder")]
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "PasswordLength")]
        public string Password { get; set; }

        [Display(Name = "Password again", Prompt = "PasswordAgainPlaceHolder")]
        [Required(ErrorMessage = "PasswordAgainRequired")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "PasswordsMatch")]
        public string PasswordConfirm { get; set; }
    }
}
