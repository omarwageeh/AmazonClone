
namespace AmazonClone.UI.Models
{
    public class CartItem
    {
        public ProductModel Product{ get; set; }
        public int ProductCount { get; set; }
        public CartItem(ProductModel product, int productCount)
        {
            Product = product;
            ProductCount = productCount;
        }

    }
}
