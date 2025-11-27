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

            var wypozyczeniaQuery = _context.wypozyczenia
                .Include(w => w.Czytelnicy)
                .Include(w => w.Kopie)
                .Include(w => w.Oddzial)
                .AsQueryable();

            if (!isLibrarianOrAdmin)
            {
                wypozyczeniaQuery = wypozyczeniaQuery.Where(w => w.Czytelnicy != null && w.Czytelnicy.Login == userLogin);
            }

            var wypozyczeniaKopieViewModel = new WypozyczeniaKopieViewModel
            {
                Wypozyczenia = wypozyczeniaQuery.ToList(),
                Kopie = _context.kopie
                    .Include(k => k.Ksiazka)
                    .ToList()
            };

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

            // Redirect to Index to use the same logic for filtering
            return RedirectToAction(nameof(Index));
        }
    }


}
