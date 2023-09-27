namespace AmazonClone.UI.Models
{
    public class ProductDetailsModel
    {
        public ProductModel Product { get; set; }
        public int Quantity { get; set; } = 1;
        public ProductDetailsModel(ProductModel productModel)
        {
            Product = productModel;
        }
    }
}
