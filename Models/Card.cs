using System.ComponentModel.DataAnnotations;
using System.Net;
namespace WeddingPlanner.Models
{
    public class Card
    {
        [Key]
        public int CardId {get;set;}
        public int WedId {get;set;}
        public int UserId {get;set;}
        public User NavUser {get;set;}
        public Wedding NavWed {get;set;}
    }
}