using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public Stock? Stock { get; set; }
    }
}
