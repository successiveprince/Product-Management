using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.Domain
{
    public class UserModel : IdentityUser
    {
      
      

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? PhoneNo { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set;}

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string? Role { get; set; } = "User";
        public DateTime CreatedAt { get; set;}
        public Boolean IsActive { get; set;} = true;
        public virtual Cart Carts { get; set;}
    }
}
