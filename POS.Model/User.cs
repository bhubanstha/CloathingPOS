using System;

namespace POS.Model
{
    public class User
    {
        public int Id { get; set; }
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