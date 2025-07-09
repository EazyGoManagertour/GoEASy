using System;
using Microsoft.EntityFrameworkCore;
using GoEASy.Models;
using GoEASy.Repositories;
using GoEASy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Đăng ký GoEasyContext
builder.Services.AddDbContext<GoEasyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký repository generic
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Đăng ký UserService
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IFavoriteService, FavoriteService>();

builder.Services.AddScoped<TourService>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
