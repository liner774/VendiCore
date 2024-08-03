using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendiCore.Data;
using VendiCore.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace VendiCore.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class TransactionController : Controller
    {
        private readonly VendingMachineContext _context;

        public TransactionController(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, int page = 1)
        {
            int pageSize = 10;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["QuantitySortParam"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";

            var transactions = _context.Transactions
                .Include(t => t.Products)
                .Include(t => t.User)
                .AsQueryable();

            switch (sortOrder)
            {
                case "date_desc":
                    transactions = transactions.OrderByDescending(t => t.PurchaseDate);
                    break;
                case "Quantity":
                    transactions = transactions.OrderBy(t => t.QuantityPurchased);
                    break;
                case "quantity_desc":
                    transactions = transactions.OrderByDescending(t => t.QuantityPurchased);
                    break;
                default:
                    transactions = transactions.OrderBy(t => t.PurchaseDate);
                    break;
            }

            var count = await transactions.CountAsync();
            var items = await transactions.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)count / pageSize);

            return View(items);
        }
    }
}
