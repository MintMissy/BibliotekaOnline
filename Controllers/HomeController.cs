using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TEST.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using TEST.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TEST.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string userRole = null;

            if (User.Identity.IsAuthenticated)
            {
                var userLogin = User.Identity.Name;

                var czytelnik = _context.czytelnicy.FirstOrDefault(c => c.Login == userLogin);
                if (czytelnik != null)
                {
                    userRole = "Czytelnik";
                }

                var pracownik = _context.pracownicy.FirstOrDefault(p => p.Login == userLogin);
                if (pracownik != null)
                {
                    userRole = pracownik.Stanowisko;
                }

                HttpContext.Session.SetString("UserRole", userRole);
            }

            ViewData["UserRole"] = userRole;

            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public IActionResult MyAccount()
        {
            var userLogin = User.Identity.Name;

            var czytelnik = _context.czytelnicy.FirstOrDefault(c => c.Login == userLogin);
            if (czytelnik == null)
            {
                return NotFound();
            }

            var model = new EditAccountViewModel
            {
                Imie = czytelnik.Imie,
                Nazwisko = czytelnik.Nazwisko,
                Miasto = czytelnik.Miasto,
                Adres = czytelnik.Adres,
                NumerPocztowy = czytelnik.NumerPocztowy,
                NrTel = czytelnik.NrTel,
                Email = czytelnik.Email
            };

            return View(model);
        }
    }
}
