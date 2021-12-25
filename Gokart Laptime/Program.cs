using Gokart_Laptime.Controllers;
using Gokart_Laptime.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie("Cookies", options =>
     {
         options.LoginPath = "/User/Login";
         options.LogoutPath = "/User/Logout";
         options.AccessDeniedPath = "/User/AccessDenied";
         options.ReturnUrlParameter = "";
     });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);//We set Time here 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IUserDAO, UserDAO>();
builder.Services.AddSingleton<IRaceTrackDAO, RaceTrackDAO>();
builder.Services.AddSingleton<IRaceDAO, RaceDAO>();

builder.Services.AddHttpContextAccessor();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
