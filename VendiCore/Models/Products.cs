using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VendiCore.Models
{
    public class Products
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int QuantityAvailable { get; set; }
    }
}
