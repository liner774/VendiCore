using Microsoft.EntityFrameworkCore;
using VendiCore.Models;

namespace VendiCore.Data
{
    public class VendingMachineContext : DbContext
    {
        public VendingMachineContext(DbContextOptions<VendingMachineContext> options) : base(options) { }

        public DbSet<Products> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
