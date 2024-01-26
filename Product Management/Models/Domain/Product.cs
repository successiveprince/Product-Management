namespace Product_Management.Models.Domain
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductCategoryId { get; set;}
        public float ProductPrice { get; set; }
        public Boolean IsAvailable { get; set; }
        public ICollection<Cart> Cart { get; set; }
        public Category Category { get; set; }
    }
}
