namespace Product_Management.Models.Domain
{
    public class Cart
    {
        public int CartId { get; set; }
        public int CartItems { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public UserModel User { get; set; }
        public Product Product { get; set; }
    }
}
