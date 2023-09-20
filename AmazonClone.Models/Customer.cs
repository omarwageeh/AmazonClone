using System.ComponentModel.DataAnnotations;

namespace AmazonClone.Model
{
    public class Customer : User
    {
        [MaxLength(12)]
        public string? Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public Customer(string fullName, string address) : base(fullName)
        {
            Address = address;
        }
    }
}
