namespace Product_Management.Models.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set;}
        public string UserPassword { get; set;}
        public Boolean IsAdmin { get; set;}
        public DateTime CreatedAt { get; set;}

        public ICollection<Cart> Carts { get; set;}
    }
}
