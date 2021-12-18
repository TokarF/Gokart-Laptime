using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class UserModel
    {

        public int UserId { get; set; }
        [Display(Name = "Username")]
        [Required(ErrorMessage = "The username is required")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "The username has to be between 8 - 50 charaters")]
        public string UserName { get; set; }
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

        [Display(Name = "Password again")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Registered at")]
        public DateTime RegisteredAt { get; set; }


    }
}
