using System;
using Microsoft.EntityFrameworkCore;
using GoEASy.Models;
using GoEASy.Repositories;
using GoEASy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Đăng ký Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax; // hoặc thử None nếu vẫn bị mất session
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Bắt buộc dùng HTTPS
});

// Đăng ký GoEasyContext
builder.Services.AddDbContext<GoEasyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<GoEASy.Services.DestinationService>();
// Đăng ký repository generic
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<TourService>();
// Đăng ký UserService
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<TourService>();
builder.Services.AddScoped<DestinationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddHttpClient();


// Đăng ký LoginService
builder.Services.AddScoped<LoginService>();

builder.Services.AddScoped<IBlogTagService, BlogTagService>();

builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddScoped<IBlogDetailService, BlogDetailService>();

builder.Services.AddScoped<IDiscountService, DiscountService>();

builder.Services.Configure<MomoApiOptions>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sử dụng Session
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "tour-detail",
    pattern: "tour/details/{id}",
    defaults: new { controller = "TourDetail", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
