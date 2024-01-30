using System.ComponentModel;

namespace Product_Management.Models.Dto
{
    public class CategoryProductDto
    {

        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string ProductDescription { get; set; }

        public float ProductPrice { get; set; }

        public string ProductImage { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public DateTime ProductCreatedAt { get; set; }

        [DefaultValue(true)]
        public bool IsAvailable { get; set; }

        public bool IsTrending { get; set; }
    }
}
