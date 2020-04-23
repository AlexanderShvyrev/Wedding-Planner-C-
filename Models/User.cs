using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
namespace WeddingPlanner.Models 
{
    public class User {
        [Key]
        public int UserId { get; set; }

        [Required (ErrorMessage = "First name is required")]
        [Display (Name = "First Name")]
        [MinLength (2, ErrorMessage = "First Name must be at least 2 characters long")]
        public string FirstName { get; set; }

        [Required (ErrorMessage = "Last name is required")]
        [Display (Name = "Last Name")]
        [MinLength (2, ErrorMessage = "Last Name must be at least 2 characters long")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required (ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataType (DataType.Password)]
        [Required]
        [MinLength (8, ErrorMessage = "Password must be 8 characters or longer!")]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Will not be mapped to your users table!
        [NotMapped]
        [Compare ("Password")]
        [DataType (DataType.Password)]
        [Display (Name = "Confirm Password")]
        public string Confirm { get; set; }

        public List<Wedding> MyWeds{get; set;}
        public List<Card> MyCards{get; set;}
    }
}