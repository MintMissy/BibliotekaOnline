using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text; 
using TEST.Context;
using TEST.Models;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration; 

    
    public AccountController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // --- METODA POMOCNICZA DO SZYFROWANIA ---
    private string HashPassword(string password)
    {
        // Pobieramy klucz z appsettings.json
        var key = _configuration["Security:PasswordKey"];

        if (string.IsNullOrEmpty(key))
        {
            throw new Exception("Nie znaleziono klucza szyfrowania w appsettings.json");
        }

        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
    // ----------------------------------------

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        var model = new RegisterViewModel { };
        return View(model);
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        var model = new ForgotPasswordViewModel { };
        return View(model);
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (_context.czytelnicy.Any(c => c.Login == model.Login))
            {
                ModelState.AddModelError("Login", "Ten login jest już zajęty.");
                return View(model);
            }

            if (_context.czytelnicy.Any(c => c.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Ten email jest już zajęty.");
                return View(model);
            }

            var klient = new Czytelnicy
            {
                Imie = model.Imie,
                Nazwisko = model.Nazwisko,
                Login = model.Login,
                
                Haslo = HashPassword(model.Password),
                Miasto = model.Miasto,
                Adres = model.Adres,
                NumerPocztowy = model.NumerPocztowy,
                NrTel = model.NrTel,
                Email = model.Email,
            };
            _context.czytelnicy.Add(klient);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        model.Oddzial = _context.oddzial
       .Select(o => new SelectListItem
       {
           Value = o.IdOddzial.ToString(),
           Text = $"{o.Miasto}, {o.Adres}"
       })
       .ToList();

        return View(model);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            
            string hashedPassword = HashPassword(model.Password);

            var pracownik = _context.pracownicy
                .FirstOrDefault(p => p.Login == model.Login && p.Haslo == hashedPassword);

            if (pracownik != null)
            {
                TempData["Message"] = "Zalogowano jako pracownik!";
                HttpContext.Session.SetString("UserLogin", model.Login);

                var role = pracownik.Stanowisko;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Login),
                    new Claim(ClaimTypes.Role, role)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToAction("Index", "Ksiazka");
            }

            
            var klient = _context.czytelnicy
                .FirstOrDefault(c => c.Login == model.Login && c.Haslo == hashedPassword);

            if (klient != null)
            {
                TempData["Message"] = "Zalogowano jako Czytelnik!";
                HttpContext.Session.SetString("UserLogin", model.Login);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Login),
                    new Claim(ClaimTypes.Role, "Czytelnik")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToAction("Index", "Ksiazka");
            }

            TempData["ErrorMessage"] = "Niepoprawny login lub hasło. Spróbuj ponownie.";
        }

        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return SignOut(new AuthenticationProperties
        {
            RedirectUri = Url.Action("Login", "Account")
        });
    }

    public IActionResult MyAccount()
    {
        var userLogin = User.Identity.Name;
        var klient = _context.czytelnicy.FirstOrDefault(c => c.Login == userLogin);
        if (klient == null)
        {
            return NotFound();
        }

        var model = new EditAccountViewModel
        {
            Imie = klient.Imie,
            Nazwisko = klient.Nazwisko,
            Miasto = klient.Miasto,
            Adres = klient.Adres,
            NumerPocztowy = klient.NumerPocztowy,
            NrTel = klient.NrTel,
            Email = klient.Email
        };

        return View(model);
    }
}