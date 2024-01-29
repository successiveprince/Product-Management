using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.Dto
{
    public class AddUserDto
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? PhoneNo { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? Role { get; set; } = "user";

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required]
        [Compare("UserPassword", ErrorMessage = "Passwords don't match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
