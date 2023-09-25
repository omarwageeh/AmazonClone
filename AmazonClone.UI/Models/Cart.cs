namespace AmazonClone.UI.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get => Items.Sum(item => item.Product.UnitPrice * item.ProductCount); }
    }
}
