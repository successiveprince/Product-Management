using Product_Management.Data;
using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.Domain
{
    public class FavouriteModel
    {
        [Key]
        public int FavouriteId { get; set; }

        public int ProductId { get; set; }

        public string UserId { get; set; }
        public int Id { get; set; }

        public virtual Product Products { get; set; }

        public virtual UserModel Users { get; set; }

    }
}
