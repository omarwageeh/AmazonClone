using AmazonClone.Model;
using System.ComponentModel.DataAnnotations;

namespace AmazonClone.UI.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid CategoryId { get; set; }
        public ProductModel(Product product)
        {
            Id = product.Id;
            NameAr = product.NameAr;
            NameEn = product.NameEn;
            UnitPrice = product.UnitPrice;
            CategoryId = product.CategoryId;
        }
        public ProductModel()
        {
            NameEn = string.Empty; 
            NameAr = string.Empty;
        }
    }
}
