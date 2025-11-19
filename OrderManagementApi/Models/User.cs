using System.ComponentModel.DataAnnotations;

namespace OrderManagementApi.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; // intentionally naive plain-text or simple hash
        public string Role { get; set; } = "User";
    }
}
