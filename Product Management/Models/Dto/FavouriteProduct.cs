using Product_Management.Models.Domain;
using System.ComponentModel;

namespace Product_Management.Models.Dto
{
    public class FavouriteProduct
    {
        public int FavouriteId { get; set; }
       
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public float ProductPrice { get; set; }
        public string ProductImage { get; set; }

      
    }
}
