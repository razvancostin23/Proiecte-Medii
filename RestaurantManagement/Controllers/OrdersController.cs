using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class OrdersController : Controller
{
    private readonly RestaurantManagementContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public OrdersController(RestaurantManagementContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [Authorize(Roles="Chelner,Administrator")]
    public IActionResult Create()
    {
        var viewModel = new OrderViewModel
        {
            UserId = _userManager.GetUserId(User),
            AvailableProducts = _context.Products
                .Where(p => p.Stock.AvailableQuantity > 0)
                .Select(p => new OrderProduct
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.Stock.AvailableQuantity
                }).ToList()
        };

        return View(viewModel);
    }
    [Authorize(Roles = "Chelner,Administrator")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderViewModel viewModel)
    {
        if (viewModel.SelectedProductIds == null || !viewModel.SelectedProductIds.Any())
        {
            ModelState.AddModelError("", "You must select at least one product.");
            return View(viewModel);
        }

        var userId = _userManager.GetUserId(User);
        var order = new Order
        {
            UserId = userId,
            OrderTime = DateTime.UtcNow,
            Status = "Pending"
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        for (int i = 0; i < viewModel.SelectedProductIds.Count; i++)
        {
            var productId = viewModel.SelectedProductIds[i];
            var quantity = viewModel.SelectedQuantities[i];
            var product = await _context.Products.Include(p => p.Stock).FirstOrDefaultAsync(p => p.Id == productId);

            if (product != null && product.Stock.AvailableQuantity >= quantity)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = productId,
                    Quantity = quantity
                };

                product.Stock.AvailableQuantity -= quantity;

                _context.OrderDetails.Add(orderDetail);
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

        IQueryable<Order> ordersQuery = _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product);

        if (userRoles.Contains("Chelner"))
        {
            ordersQuery = ordersQuery.Where(o => o.UserId == userId);
        }

        var orders = await ordersQuery.ToListAsync();
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        return View(order);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        return View(order);
    }

    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Stock)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        
        foreach (var orderDetail in order.OrderDetails)
        {
            var product = orderDetail.Product;
            if (product != null && product.Stock != null)
            {
                product.Stock.AvailableQuantity += orderDetail.Quantity;
            }
        }

        _context.OrderDetails.RemoveRange(order.OrderDetails);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "Bucatar,Administrator")]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        order.Status = status;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
