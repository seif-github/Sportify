using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.BLL.Attributes;

namespace sportify.BLL.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and contain at least one uppercase, one lowercase, one number and one special character")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        [UniqueEmail]
        public string Email { get; set; } = null!;
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
    }
}
