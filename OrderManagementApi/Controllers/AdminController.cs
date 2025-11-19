using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Data;
using OrderManagementApi.Models;

namespace OrderManagementApi.Controllers
{
    [ApiController]
    [Route("api/admin/orders")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminOrdersController(AppDbContext db)
        {
            _db = db;
        }

        private User? GetCurrentUser() => HttpContext.Items["CurrentUser"] as User;

        [HttpGet]
        public IActionResult GetAll()
        {
            var user = GetCurrentUser();
            if (user == null) return Unauthorized(new { error = "Authentication required" });

            // INTENTIONALLY INSECURE: only check authenticated, NOT verifying Admin role
            var orders = _db.Orders.ToList();
            return Ok(orders);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var user = GetCurrentUser();
            if (user == null) return Unauthorized(new { error = "Authentication required" });

            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();

            _db.Orders.Remove(order);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
