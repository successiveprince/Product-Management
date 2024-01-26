﻿namespace Product_Management.Models.Domain
{
    public class Cart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
    }
}
