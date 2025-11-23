using Microsoft.AspNetCore.Mvc;
using TEST.Models;
using System.Linq;
using TEST.Context;
using Microsoft.AspNetCore.Authorization;

namespace TEST.Controllers
{
	[Authorize(Roles = "Czytelnik")]
	public class CzytelnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CzytelnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Czytelnik/EditAccount
        [HttpGet]
        public IActionResult EditAccount(int id)
        {
            var userLogin = User.Identity.Name;

            if (string.IsNullOrEmpty(userLogin))
            {
                return RedirectToAction("Login", "Account");  
            }

            Console.WriteLine($"User login: {userLogin}");

            var czytelnik = _context.czytelnicy.FirstOrDefault(c => c.Login == userLogin);
            if (czytelnik == null)
            {
                return NotFound("Czytelnik o podanym loginie nie istnieje.");
            }

            
            var model = new EditAccountViewModel
            {
                Id = czytelnik.IdKlienta,
                Imie = czytelnik.Imie,
                Nazwisko = czytelnik.Nazwisko,
                Miasto = czytelnik.Miasto,
                Adres = czytelnik.Adres,
                NumerPocztowy = czytelnik.NumerPocztowy,
                NrTel = czytelnik.NrTel,
                Email = czytelnik.Email
            };

            return View("~/Views/Account/EditAccount.cshtml", model); 
        }

        // POST: Czytelnik/EditAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(EditAccountViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Wprowadzone dane są niepoprawne.";
                return View("~/Views/Account/EditAccount.cshtml", model);
            }

            var userLogin = User.Identity.Name;
            var czytelnik = _context.czytelnicy.FirstOrDefault(c => c.Login == userLogin);
            if (czytelnik == null)
            {
                return NotFound("Czytelnik o podanym loginie nie istnieje.");
            }

           
            czytelnik.Imie = model.Imie;
            czytelnik.Nazwisko = model.Nazwisko;
            czytelnik.Miasto = model.Miasto;
            czytelnik.Adres = model.Adres;
            czytelnik.NumerPocztowy = model.NumerPocztowy;
            czytelnik.NrTel = model.NrTel;
            czytelnik.Email = model.Email;

            
            if (!string.IsNullOrEmpty(model.NoweHaslo))
            {
                
                czytelnik.Haslo = model.NoweHaslo;
            }
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Dane zostały zaktualizowane pomyślnie!";
            return RedirectToAction("Details", new { id = model.Id });
        }

        
        public IActionResult Details(int id)
        {
            
            var czytelnik = _context.czytelnicy.FirstOrDefault(c => c.IdKlienta == id);
            if (czytelnik == null)
            {
                return NotFound("Czytelnik o podanym ID nie istnieje.");
            }
            
            var model = new EditAccountViewModel
            {
                Id = czytelnik.IdKlienta,
                Imie = czytelnik.Imie,
                Nazwisko = czytelnik.Nazwisko,
                Miasto = czytelnik.Miasto,
                Adres = czytelnik.Adres,
                NumerPocztowy = czytelnik.NumerPocztowy,
                NrTel = czytelnik.NrTel,
                Email = czytelnik.Email
            };

            
            return View("~/Views/Account/EditAccount.cshtml", model); 
        }
    }
}
