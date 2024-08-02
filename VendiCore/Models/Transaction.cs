using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendiCore.Models
{
    public class Transaction
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Products Products { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity purchased must be at least 1.")]
        public int QuantityPurchased { get; set; }

    }
}
