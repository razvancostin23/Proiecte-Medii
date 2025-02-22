using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace RestaurantManagement.Models 
{ 
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [BindNever]
        public List<CategoryCheckbox> AvailableCategories { get; set; }

        [Required]
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
    }

    public class CategoryCheckbox
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}