using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Data;
using OrderManagementApi.Middleware;
using OrderManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration (intentionally simple/insecure for demo)
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("OrdersDb"));
builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

// Developer exception page (return detailed errors, intentionally)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

// Simple auth middleware (intentionally naive)
app.UseMiddleware<SimpleAuthMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Seed demo data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.Seed(db, scope.ServiceProvider.GetRequiredService<TokenService>());
}

app.Run();
