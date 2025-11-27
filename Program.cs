using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TEST.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using TEST.Services;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja pami�ci podr�cznej dla sesji
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
        options.LoginPath = "/Account/Login"; // �cie�ka logowania
        options.LogoutPath = "/Account/Logout"; // �cie�ka wylogowania
        options.AccessDeniedPath = "/Account/AccessDenied"; // Brak dost�pu
        options.SlidingExpiration = true; // Od�wie�anie sesji
    });

builder.Services.AddControllersWithViews();

// Rejestracja strategii raportów i serwisu raportów
builder.Services.AddScoped<KsiazkiWedlugOddzialuIRokuStrategy>();
builder.Services.AddScoped<KsiazkiWedlugOddzialuIGatunkuStrategy>();
builder.Services.AddScoped<SumaStronWedlugOddzialuStrategy>();
builder.Services.AddScoped<IReportStrategyFactory, ReportStrategyFactory>();
builder.Services.AddScoped<RaportyKsiazekService>();

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
