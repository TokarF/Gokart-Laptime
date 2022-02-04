using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class UserLoginViewModel
    {
        [Display(Name = "Email address", Prompt = "EmailPlaceHolder")]
        [Required(ErrorMessage = "EmailAddressRequired")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "EmailAddressLength")]
        public string Email { get; set; }

        [Display(Name = "Password", Prompt = "PasswordPlaceHolder")]
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "PasswordLength")]
        public string Password { get; set; }
    }
}
