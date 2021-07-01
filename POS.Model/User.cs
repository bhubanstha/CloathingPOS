using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class User : EntityBase
    {

        [Required(ErrorMessage = "User name is required")]
        [StringLength(15, ErrorMessage = "User name can be upto 15 characters long")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "User display name is required.")]
        [MaxLength(40, ErrorMessage = "User display name can not be longer than 40 charcters.")]
        public string DisplayName { get; set; }


        [Required(ErrorMessage = "User password is required.")]
        [MaxLength(60, ErrorMessage = "Password can be upto 10 character long")]
        [MinLength(6, ErrorMessage = "Password should be atleast 6 character long")]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public bool PromptForPasswordReset { get; set; }


        [MaxLength(30)]
        public string ProfileImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeactivationDate { get; set; }

        public DateTime? LastPasswordChangeDate { get; set; }

        [ForeignKey("Branch")]
        public Int64? BranchId { get; set; }
        public bool CanAccessAllBranch { get; set; }



        public virtual Branch Branch { get; set; }

    }
}