namespace AmazonClone.UI.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get => TotalPrice; set => value = Items.Sum(item => item.Product.UnitPrice); }
    }
}
