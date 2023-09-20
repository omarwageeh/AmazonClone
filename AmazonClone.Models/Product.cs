﻿using System.ComponentModel.DataAnnotations;

namespace AmazonClone.Model
{
    public class Product : BaseEntity
    {
        [Required]
        public string NameEn { get; set; }
        [Required]
        public string NameAr { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public Product(string nameEn, string nameAr) 
        {
            NameEn = nameEn;
            NameAr = nameAr;
        }
    }
}
