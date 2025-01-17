﻿using Microsoft.AspNetCore.Mvc;
using VendiCore.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VendiCore.Data;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class PurchaseController : Controller
{
    private readonly VendingMachineContext _context;

    public PurchaseController(VendingMachineContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Purchase()
    {
        ViewBag.SuccessMessage = TempData["SuccessMessage"];
        var products = await _context.Products.ToListAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> PurchaseProduct(int id)
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
    public async Task<IActionResult> PurchaseProduct(int id, int quantity)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || quantity <= 0 || quantity > product.QuantityAvailable)
        {
            return BadRequest();
        }

        product.QuantityAvailable -= quantity;
        _context.Products.Update(product);

        var transaction = new Transaction
        {
            ProductID = id,
            UserID = 1, // For demo purposes
            PurchaseDate = DateTime.Now,
            QuantityPurchased = quantity
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Purchase successful!";
        return RedirectToAction("Purchase");
    }
}
