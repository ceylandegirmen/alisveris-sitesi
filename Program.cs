using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VegHouse.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VegHouse.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.CookiePolicy;



//builder.Services.AddDbContext<VegHouseDbContext>(options => options.UseSqlServer("ConnectionStrings"));



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("VegHouseDbContextConnection") ?? throw new InvalidOperationException("Connection string 'VegHouseDbContextConnection' not found.");

builder.Services.AddDbContext<VegHouseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("VegHouseDbContextConnection")));


//builder.Services.AddDefaultIdentity<VegHouseUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//})

//    .AddEntityFrameworkStores<VegHouseDbContext>();


builder.Services
    .AddIdentity<VegHouseUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<VegHouseDbContext>();



//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddUserStore<AddDbContext>(); // burdan kaynaklý sorun olabilr



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();




builder.Services
.AddAuthentication()
.AddCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Manage";
});



builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.Name = "veghouseadm";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.LoginPath = "/Identity/Account/Login";
    //options.LoginPath = "/Admin/Pages/Account/Login";

    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;

});


builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".VegHouse.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build(); // çalýþtýðýnda sorun var bunda 


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();

app.UseSession();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");



app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    //pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();
});

app.Run();
