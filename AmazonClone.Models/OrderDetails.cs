namespace AmazonClone.Model
{
    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int ProductCount { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
