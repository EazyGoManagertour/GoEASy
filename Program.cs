using System;
using Microsoft.EntityFrameworkCore;
using GoEASy.Models;
using GoEASy.Repositories;
using GoEASy.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Session configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Chỉ giữ Cookie Authentication, bỏ Google Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/login/logout";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
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


// Đăng ký LoginService
builder.Services.AddScoped<LoginService>();

builder.Services.AddScoped<IBlogTagService, BlogTagService>();

builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddScoped<IBlogDetailService, BlogDetailService>();

builder.Services.AddScoped<IDiscountService, DiscountService>();

// Đăng ký RuleService
builder.Services.AddScoped<IRuleService, RuleService>();

builder.Services.AddScoped<IActionService, ActionService>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.CheckConsentNeeded = context => false;
}); // Chỉ giữ một cấu hình CookiePolicyOptions


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
app.UseCookiePolicy();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "tour-detail",
    pattern: "tour/details/{id}",
    defaults: new { controller = "TourDetail", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



























