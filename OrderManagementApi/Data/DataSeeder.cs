using OrderManagementApi.Models;
using OrderManagementApi.Services;

namespace OrderManagementApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext db, TokenService tokenService)
        {
            if (db.Users.Any()) return;

            // Intentionally naive passwords (plain text)
            var user = new User { Id = Guid.NewGuid(), Email = "user@example.com", PasswordHash = "password", Role = "User" };
            var admin = new User { Id = Guid.NewGuid(), Email = "admin@example.com", PasswordHash = "password", Role = "Admin" };

            db.Users.AddRange(user, admin);

            var o1 = new Order { Id = Guid.NewGuid(), UserId = user.Id, OrderNumber = "ORD-1001", TotalAmount = 49.99m, CreatedAt = DateTime.UtcNow, Status = "New" };
            var o2 = new Order { Id = Guid.NewGuid(), UserId = admin.Id, OrderNumber = "ORD-2001", TotalAmount = 199.99m, CreatedAt = DateTime.UtcNow, Status = "Shipped" };

            db.Orders.AddRange(o1, o2);

            db.SaveChanges();

            // Pre-generate tokens so demo can use them if desired (optional)
            var t1 = tokenService.GenerateToken(user);
            var t2 = tokenService.GenerateToken(admin);

            Console.WriteLine($"Seeded users. User token: {t1}");
            Console.WriteLine($"Seeded admin token: {t2}");
        }
    }
}
