using Microsoft.AspNetCore.Mvc;
using VendiCore.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VendiCore.Data;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class ProductsController : Controller
{
    private readonly VendingMachineContext _context;
    private const int PageSize = 10;

    public ProductsController(VendingMachineContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder, int page = 1)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["PriceSortParam"] = sortOrder == "Price" ? "price_desc" : "Price";
        ViewData["QuantitySortParam"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";

        var products = from p in _context.Products
                       select p;

        switch (sortOrder)
        {
            case "name_desc":
                products = products.OrderByDescending(p => p.Name);
                break;
            case "Price":
                products = products.OrderBy(p => p.Price);
                break;
            case "price_desc":
                products = products.OrderByDescending(p => p.Price);
                break;
            case "Quantity":
                products = products.OrderBy(p => p.QuantityAvailable);
                break;
            case "quantity_desc":
                products = products.OrderByDescending(p => p.QuantityAvailable);
                break;
            default:
                products = products.OrderBy(p => p.Name);
                break;
        }

        var totalItems = await products.CountAsync();
        var productsPaged = await products
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling(totalItems / (double)PageSize);

        return View(productsPaged);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Price,QuantityAvailable")] Products product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,QuantityAvailable")] Products product)
    {
        if (id != product.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ID))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ID == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.ID == id);
    }
}
