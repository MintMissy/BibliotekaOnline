using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEST.Models;
using TEST.Context;

namespace TEST.Controllers
{
    public class WypozyczeniaController : Controller
    {
        private readonly ApplicationDbContext _context;


        public WypozyczeniaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var userLogin = User.Identity.Name;

            bool isLibrarianOrAdmin = User.IsInRole("Bibliotekarz") || User.IsInRole("Administrator");

            Console.WriteLine("User role: " + (isLibrarianOrAdmin ? "Bibliotekarz/Administrator" : "Inna rola"));

            var wypozyczeniaKopieViewModel = new WypozyczeniaKopieViewModel
            {
                Wypozyczenia = _context.wypozyczenia
                    .Include(w => w.Czytelnicy)
                    .Include(w => w.Kopie)
                    .Include(w => w.Oddzial)
                    .Where(w => w.Czytelnicy.Login == userLogin)
                    .ToList(),

                Kopie = _context.kopie
                    .Include(k => k.Ksiazka)
                    .ToList()
            };

            Console.WriteLine("Liczba wypożyczeń: " + wypozyczeniaKopieViewModel.Wypozyczenia.Count);

            return View("~/Views/Home/WypozyczeniaKopie.cshtml", wypozyczeniaKopieViewModel);
        }
       
        [HttpGet]
        public IActionResult EditReturnDate(int id)
        {
            var wypozyczenie = _context.wypozyczenia
                .FirstOrDefault(w => w.IdWypozyczenia == id);

            if (wypozyczenie == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Bibliotekarz") && !User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }


           
            return View("~/Views/Home/EditReturnDate.cshtml", wypozyczenie);
        }

       
        [HttpPost]
        public IActionResult EditReturnDate(int id, DateTime dataZwrotu)
        {
            var wypozyczenie = _context.wypozyczenia
                .FirstOrDefault(w => w.IdWypozyczenia == id);

            if (wypozyczenie == null)
            {
                return NotFound();
            }

            wypozyczenie.DataZwrotu = dataZwrotu.ToUniversalTime();

            _context.SaveChanges();
            var userLogin = User.Identity.Name;
            var wypozyczeniaKopieViewModel = new WypozyczeniaKopieViewModel
            {
                Wypozyczenia = _context.wypozyczenia
                    .Include(w => w.Czytelnicy)
                    .Include(w => w.Kopie)
                    .Include(w => w.Oddzial)
                    .Where(w => w.Czytelnicy.Login == userLogin)
                    .ToList(),

                Kopie = _context.kopie
                    .Include(k => k.Ksiazka)
                    .ToList()
            };


            return View("~/Views/Home/WypozyczeniaKopie.cshtml", wypozyczeniaKopieViewModel);
        }
    }


}
