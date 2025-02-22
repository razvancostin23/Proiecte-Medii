using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RestaurantManagementContext _context;

        public HomeController(ILogger<HomeController> logger, RestaurantManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending");
            var completedOrders = await _context.Orders.CountAsync(o => o.Status == "Completed");

            var bestSellingProducts = await _context.OrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductName = _context.Products.Where(p => p.Id == g.Key).Select(p => p.Name).FirstOrDefault(),
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalSold)
                .Take(5)
                .ToListAsync();

            var lowStockProducts = await _context.Products
                .Include(p => p.Stock)
                .Where(p => p.Stock.AvailableQuantity < 5)
                .Select(p => new
                {
                    p.Name,
                    Stock = p.Stock.AvailableQuantity
                })
                .ToListAsync();

            var ordersPerDay = await _context.Orders
                .GroupBy(o => o.OrderTime.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    OrderCount = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                CompletedOrders = completedOrders,
                BestSellingProducts = bestSellingProducts.Cast<dynamic>().ToList(),
                LowStockProducts = lowStockProducts.Cast<dynamic>().ToList(),
                OrdersPerDay = ordersPerDay.Cast<dynamic>().ToList()
            };

            return View(viewModel);
        }
    }
}
