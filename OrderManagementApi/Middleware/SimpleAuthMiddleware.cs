using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Data;
using OrderManagementApi.Services;
using OrderManagementApi.Models;

namespace OrderManagementApi.Middleware
{
    // Very naive middleware: reads Authorization: Bearer <token>, looks up user and attaches to HttpContext.Items["CurrentUser"].
    public class SimpleAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SimpleAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TokenService tokenService, AppDbContext db)
        {
            try
            {
                if (context.Request.Headers.TryGetValue("Authorization", out var header))
                {
                    var val = header.ToString();
                    if (val.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var token = val.Substring("Bearer ".Length).Trim();
                        if (!string.IsNullOrEmpty(token) && tokenService.TryGetUserId(token, out var userId))
                        {
                            // Lookup user in DB and attach (intentionally simple)
                            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                            if (user != null)
                            {
                                context.Items["CurrentUser"] = user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log full exception (intentionally) and continue without auth
                Console.WriteLine($"Auth middleware error: {ex}");
            }

            await _next(context);
        }
    }
}
