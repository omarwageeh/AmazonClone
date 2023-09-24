using AmazonClone.Model;
using System.ComponentModel.DataAnnotations;

namespace AmazonClone.UI.Models
{
    public class ProductModel
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductModel(string nameEn, string nameAr)
        {
            NameEn = nameEn;
            NameAr = nameAr;
        }
    }
}
