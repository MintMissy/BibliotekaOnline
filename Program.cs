using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TEST.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja pamiêci podrêcznej dla sesji
builder.Services.AddDistributedMemoryCache();

// Konfiguracja sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas trwania sesji
    options.Cookie.HttpOnly = true; // Tylko Http
    options.Cookie.IsEssential = true; // Ustawienie na "essential"
});

// Rejestracja DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information);
});

// Rejestracja autoryzacji i uwierzytelniania
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Œcie¿ka logowania
        options.LogoutPath = "/Account/Logout"; // Œcie¿ka wylogowania
        options.AccessDeniedPath = "/Account/AccessDenied"; // Brak dostêpu
        options.SlidingExpiration = true; // Odœwie¿anie sesji
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    app.UseHsts();
}
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Ksiazka}/{action=Index}/{id?}"
         

        );


});
app.Run();
