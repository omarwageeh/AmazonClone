using System.ComponentModel.DataAnnotations;

namespace AmazonClone.Model
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public Category(string name) 
        {
            Name = name;
        }
    }
}
