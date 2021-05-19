using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.UIModel
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
