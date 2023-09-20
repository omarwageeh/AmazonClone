using AmazonClone.Model.Enum;

namespace AmazonClone.Model
{
    public class Order : BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public Customer Customer { get; set; } = null!;
        public Status Status { get; set; }
    }
}
