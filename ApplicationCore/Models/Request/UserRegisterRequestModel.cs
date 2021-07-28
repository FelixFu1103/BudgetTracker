using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.Request
{
    public class UserRegisterRequestModel
    {
        [Required]
        [EmailAddress]
        [StringLength(64)]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = " The password should be minimum of 8 characters ", MinimumLength = 8)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$",
            ErrorMessage = "Password should have minimum of 8 characters and should include one upper, lower number and a special character")]
        public string Password { get; set; }
        [Required]
        [StringLength(64)]
        public string FullName { get; set; }
    }
}
