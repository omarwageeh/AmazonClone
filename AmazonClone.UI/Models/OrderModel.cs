using AmazonClone.Model;

namespace AmazonClone.UI.Models
{
    public class OrderModel
    {
        public Order Order { get; set; } = null!;
        public List<OrderDetails?> OrderDetails { get; set; } = new();
    }
}
