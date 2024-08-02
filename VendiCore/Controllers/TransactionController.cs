using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendiCore.Data;
using VendiCore.Models;

namespace VendiCore.Controllers
{
    public class TransactionController : Controller
    {
        private readonly VendingMachineContext _context;

        public TransactionController(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Products)
                .Include(t => t.User)
                .ToListAsync();

            return View(transactions);
        }
    }
}
