using System.ComponentModel.DataAnnotations;

namespace POS.Core.Model.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(15)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(6)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
