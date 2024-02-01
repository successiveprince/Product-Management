using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Product_Management.Models.Domain;

namespace Product_Management.Models.Dto
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? PhoneNo { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        
        public DateTime CreatedAt { get; set; }
        public Boolean IsActive { get; set; }

    }
}
