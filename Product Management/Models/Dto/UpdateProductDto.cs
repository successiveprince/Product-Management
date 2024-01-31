using Product_Management.Models.Domain;

namespace Product_Management.Models.Dto
{
    public class UpdateProductDto
    {
        public int ProductId { get; set;}
        public string ProductName { get; set; }

        public string ProductDescription { get; set; } 

        public float ProductPrice { get; set; }

        public IFormFile ProductImage { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsTrending { get; set; }

        public bool IsActive { get; set; }

        public DateTime ProductCreatedAt { get; set; }

        public Category Category { get; set; }

        public int ProductCategoryId { get; set; }

    }
}
