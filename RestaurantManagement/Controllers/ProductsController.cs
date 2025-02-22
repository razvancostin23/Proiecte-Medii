using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class ProductsController : Controller
{
    private readonly RestaurantManagementContext _context;

    public ProductsController(RestaurantManagementContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category).Include(p => p.Stock)
            .ToListAsync();

        return View(products);
    }

    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
        var viewModel = new ProductViewModel
        {
            AvailableCategories = _context.Categories
                .Select(c => new CategoryCheckbox
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsSelected = false
                }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel viewModel, string SelectedCategoryIds)
    {
        Console.WriteLine("-------------- Create Submitted ----------------");

        // Convert comma-separated category IDs to List<int>
        viewModel.SelectedCategoryIds = !string.IsNullOrEmpty(SelectedCategoryIds)
            ? SelectedCategoryIds.Split(',')
                .Where(s => int.TryParse(s, out _))
                .Select(int.Parse)
                .ToList()
            : new List<int>();

        Console.WriteLine($"Selected Categories Count: {viewModel.SelectedCategoryIds.Count}");

        
        var product = new Product
        {
            Name = viewModel.Name,
            Price = viewModel.Price,
            Stock = new Stock { AvailableQuantity = viewModel.StockQuantity }
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        
        if (viewModel.SelectedCategoryIds.Any())
        {
            var productCategories = viewModel.SelectedCategoryIds
                .Select(categoryId => new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId,
                }).ToList();

            _context.ProductCategories.AddRange(productCategories);
            await _context.SaveChangesAsync();
        }

        Console.WriteLine("Product created successfully with categories!");

        return RedirectToAction(nameof(Index));
    }




    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category).Include(p => p.Stock)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();

      
        var selectedCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList();

        var viewModel = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.Stock?.AvailableQuantity > 0 ? product.Stock.AvailableQuantity : 0,
            AvailableCategories = _context.Categories
                .ToList() 
                .Select(c => new CategoryCheckbox
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsSelected = selectedCategoryIds.Contains(c.Id)
                }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel, string SelectedCategoryIds)
    {
        if (id != viewModel.Id) return NotFound();

        Console.WriteLine("-------------- Edit Submitted ----------------");
        Console.WriteLine($"Product ID: {id}");

        
        if (!string.IsNullOrEmpty(SelectedCategoryIds))
        {
            viewModel.SelectedCategoryIds = SelectedCategoryIds.Split(',')
                .Where(s => int.TryParse(s, out _))
                .Select(int.Parse)
                .ToList();
        }
        else
        {
            viewModel.SelectedCategoryIds = new List<int>();
        }

        Console.WriteLine($"Selected Categories Count: {viewModel.SelectedCategoryIds.Count}");


        var product = await _context.Products
            .Include(p => p.ProductCategories).Include(p => p.Stock)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();

        product.Name = viewModel.Name;
        product.Price = viewModel.Price;
        product.Stock.AvailableQuantity = viewModel.StockQuantity;

        _context.ProductCategories.RemoveRange(product.ProductCategories);
        Console.WriteLine("Old categories removed");

        if (viewModel.SelectedCategoryIds != null && viewModel.SelectedCategoryIds.Any())
        {
            product.ProductCategories = viewModel.SelectedCategoryIds
                .Select(categoryId => new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId
                }).ToList();

            Console.WriteLine("New categories added to product");
        }
        else
        {
            Console.WriteLine("No categories selected - product will have no categories");
        }

        _context.Update(product);
        await _context.SaveChangesAsync();
        Console.WriteLine("Product updated successfully!");

        return RedirectToAction(nameof(Index));
    }


    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products
            .Include(p => p.ProductCategories)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product != null)
        {
            _context.ProductCategories.RemoveRange(product.ProductCategories);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category).Include(p => p.Stock)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();

        var viewModel = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.Stock?.AvailableQuantity > 0 ? product.Stock.AvailableQuantity : 0,
            AvailableCategories = product.ProductCategories
                .Select(pc => new CategoryCheckbox
                {
                    Id = pc.Category.Id,
                    Name = pc.Category.Name,
                    IsSelected = true
                }).ToList()
        };

        return View(viewModel);
    }
}
