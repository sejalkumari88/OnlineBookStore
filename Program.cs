using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add MVC services
builder.Services.AddControllersWithViews();

// ✅ Register DbContext for Entity Framework Core
builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register custom services
builder.Services.AddScoped<EmailService>();
builder.Services.AddHttpClient(); // Required for Razorpay integration
builder.Services.AddHttpContextAccessor();

// ✅ Register session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Build the app
var app = builder.Build();

// ✅ Seed sample data (optional)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
    context.Database.EnsureCreated();

    if (!context.Books.Any())
    {
        context.Books.Add(new OnlineBookStore.Models.Book
        {
            Title = "Sample Book",
            Author = "Admin",
            Price = 100,
            Stock = 10
        });

        context.SaveChanges();
    }
}

// ✅ Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Use session before endpoints and authorization
app.UseSession();
app.UseAuthorization();

// ✅ Map default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
