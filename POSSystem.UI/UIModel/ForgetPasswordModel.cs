﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.UIModel
{
    public class ForgetPasswordModel
    {

        [Required(ErrorMessage = "User name is required")]
        [MaxLength(15, ErrorMessage = "User name can not be longer than 15 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(6, ErrorMessage = "Password can not be longer than 6 charactes long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(6, ErrorMessage = "Password can not be longer than 6 charactes long.")]
        [Compare("Password", ErrorMessage = "Confirmation password and  password didn't match.")]
        public string ConfirmPassword { get; set; }
    }
}