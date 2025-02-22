using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models { 
    public class OrderViewModel
    {
        public string UserId { get; set; }

        public List<OrderProduct> AvailableProducts { get; set; } = new List<OrderProduct>();

        public List<int> SelectedProductIds { get; set; } = new List<int>();
        public List<int> SelectedQuantities { get; set; } = new List<int>();
    }

    public class OrderProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}