using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class RegisterMauiModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
