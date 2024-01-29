using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.Dto
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserEmail is required.")]
        public string? UserEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password should have at least 8 characters.")]
        public string? UserPassword { get; set; }
        public bool RememberMe { get; set; }
    }
}
