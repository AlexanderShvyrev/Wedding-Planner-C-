using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class LoginUser
    {
        [EmailAddress]
        [Required(ErrorMessage="Email is required")]
        public string LoginEmail {get; set;}

        [DataType(DataType.Password)]
        [Required(ErrorMessage="Enter your password")]
        public string LoginPassword { get; set;}
    }
}