using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Management.Models.Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
       
        public float ProductPrice { get; set; }

        [NotMapped]
        public IFormFile ProductUploadImage { get; set; }
        public string ProductImage { get; set; }

        [DefaultValue(true)]
        public bool IsAvailable { get; set; }

        public bool IsTrending { get; set; }
        public bool IsActive { get; set; }
        public DateTime ProductCreatedAt { get; set; }
        public int ProductCategoryId { get; set; }

        public virtual Cart Cart { get; set; }
        public Category Category { get; set; }
    }
}
