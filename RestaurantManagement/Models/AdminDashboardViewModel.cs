
namespace RestaurantManagement.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }

        public List<dynamic> BestSellingProducts { get; set; } = new List<dynamic>();
        public List<dynamic> LowStockProducts { get; set; } = new List<dynamic>();
        public List<dynamic> OrdersPerDay { get; set; } = new List<dynamic>();
    }
}
