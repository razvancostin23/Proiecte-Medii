using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int NumberOfPeople { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
