using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class UserModel
    {

        public int UserId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "UsernameRequired")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "UsernameLength")]
        public string UserName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "EmailAddressRequired")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "EmailAddressLength")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "PasswordLength")]
        public string Password { get; set; }

        [Display(Name = "Registered at")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegisteredAt { get; set; }


    }
}
