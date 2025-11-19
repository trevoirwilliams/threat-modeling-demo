using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Data;
using OrderManagementApi.Models;

namespace OrderManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        private User? GetCurrentUser() => HttpContext.Items["CurrentUser"] as User;

        public class CreateOrderDto { public decimal TotalAmount { get; set; } public string? Status { get; set; } }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderDto dto)
        {
            var user = GetCurrentUser();
            if (user == null) return Unauthorized(new { error = "Authentication required" });

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                OrderNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpperInvariant(),
                TotalAmount = dto.TotalAmount,
                CreatedAt = DateTime.UtcNow,
                Status = dto.Status ?? "New"
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            // INTENTIONALLY INSECURE: do NOT check ownership
            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public IActionResult GetMine()
        {
            var user = GetCurrentUser();
            if (user == null) return Unauthorized(new { error = "Authentication required" });

            var orders = _db.Orders.Where(o => o.UserId == user.Id).ToList();
            return Ok(orders);
        }
    }
}
