using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string UserId { get; set; } 

        public DateTime OrderTime { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
