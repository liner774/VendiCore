using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VendiCore.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [DisplayName("Password")]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
