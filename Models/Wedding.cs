using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models {
    public class Wedding {
        [Key]
        public int WedId { get; set; }

        [Required]
        public string WedderOne { get; set; }

        [Required]
        public string WedderTwo { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Address{get; set;}

        [Required]
        [FutureDate]
        [DataType (DataType.DateTime)]
        public DateTime WeddingDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public User Creator{get; set;}
        public List<Card> Guests {get; set;}

    }
    public class FutureDateAttribute : ValidationAttribute {
        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {
            DateTime WeddingDate = (DateTime) value;
            if (WeddingDate < DateTime.Now) 
            {
                return new ValidationResult ("Please Enter a Valid Future Date and Time!");
            } 
            else 
            {
                return ValidationResult.Success;
            }
        }

    }
}