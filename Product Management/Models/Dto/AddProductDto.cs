using System.ComponentModel;

namespace Product_Management.Models.Dto
{
    public class AddProductDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public float ProductPrice { get; set; }
        public IFormFile? ProductImage { get; set; }

        [DefaultValue(true)]
        public bool IsAvailable { get; set; }

        public bool IsTrending { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public DateTime ProductCreatedAt { get; set; }
        public int ProductCategoryId { get; set; }

    }
}
