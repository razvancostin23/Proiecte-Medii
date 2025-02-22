using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
