using System;
using System.ComponentModel.DataAnnotations;

namespace POS.Model.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(15)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(10)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Branch name is required")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Branch name is required")]
        public Int64 BranchId { get; set; }

        public bool RememberMe { get; set; }
    }
}
