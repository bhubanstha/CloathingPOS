using System;
using System.ComponentModel.DataAnnotations;

namespace POS.Model
{
    public class User : EntityBase
    {

        [Required(ErrorMessage = "User name is required")]
        [StringLength(15, ErrorMessage = "User name can be upto 15 characters long")]
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public bool PromptForPasswordReset { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
    }
}