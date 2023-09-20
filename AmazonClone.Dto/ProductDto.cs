namespace AmazonClone.Dto
{
    public class ProductDto
    {
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public string catName { get; set; } = string.Empty;
    }
}
