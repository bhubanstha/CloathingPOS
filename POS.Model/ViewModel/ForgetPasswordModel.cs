using System.ComponentModel.DataAnnotations;

namespace POS.Model.ViewModel
{
    public class ForgetPasswordModel
    {

        [Required(ErrorMessage = "User name is required")]
        [MaxLength(15, ErrorMessage = "User name can not be longer than 15 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(10, ErrorMessage = "Password can not be longer than 10 charactes long.")]
        [Compare("ConfirmPassword", ErrorMessage = "Password and confirmation password didn't match.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
        [MaxLength(10, ErrorMessage = "Password can not be longer than 10 charactes long.")]
        [Compare("Password", ErrorMessage = "Confirmation password and  password didn't match.")]
        public string ConfirmPassword { get; set; }
    }
}
