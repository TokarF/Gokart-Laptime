using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class UserLoginViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "The email adress has to be between 8 - 50 charaters")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "The password is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "The password has to be between 8 - 20 charaters")]
        public string Password { get; set; }
    }
}
