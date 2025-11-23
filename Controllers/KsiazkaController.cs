using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using TEST.Context;
using TEST.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

public class KsiazkaController : Controller
{
    private readonly ApplicationDbContext _context;

    public KsiazkaController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string sortOrder, string searchString, string yearFilter, List<int> publishers, List<int> branches, List<int> genres)
    {
        // Sortowanie - ustawienie kierunku sortowania
        ViewData["TitleSort"] = sortOrder == "title_desc" ? "title" : "title_desc";
        ViewData["AuthorSort"] = sortOrder == "author_desc" ? "author" : "author_desc";
        ViewData["YearSort"] = sortOrder == "year_desc" ? "year" : "year_desc";
        ViewData["PublisherSort"] = sortOrder == "publisher_desc" ? "publisher" : "publisher_desc";
        ViewData["BranchSort"] = sortOrder == "branch_desc" ? "branch" : "branch_desc";
        ViewData["GenreSort"] = sortOrder == "genre_desc" ? "genre" : "genre_desc";

        // Przekazanie parametrów filtrów do widoku
        ViewData["YearFilter"] = yearFilter;
        ViewData["SelectedPublishers"] = publishers;
        ViewData["SelectedBranches"] = branches;
        ViewData["SelectedGenres"] = genres;

        // Zapytanie o książki z dołączonymi danymi
        var booksQuery = _context.ksiazka
            .Include(k => k.Autor)
            .Include(k => k.Wydawnictwo)
            .Include(k => k.Gatunek)
            .Include(k => k.Kopie) 
            .ThenInclude(k => k.Regal) 
            .ThenInclude(r => r.Oddzial) 
            .AsQueryable();

        // Wyszukiwanie książek
        if (!string.IsNullOrEmpty(searchString))
        {
            booksQuery = booksQuery.Where(b => b.Tytul.Contains(searchString) ||
                                                b.Autor.Imie.Contains(searchString) ||
                                                b.Autor.Nazwisko.Contains(searchString));
        }

        // Filtruj po roku wydania
        if (!string.IsNullOrEmpty(yearFilter))
        {
            if (int.TryParse(yearFilter, out var year))
            {
                booksQuery = booksQuery.Where(b => b.RokWydania == year);
            }
        }

        // Filtruj po wydawnictwie
        if (publishers != null && publishers.Any())
        {
            booksQuery = booksQuery.Where(b => publishers.Contains(b.Wydawnictwo.IdWydawnictwa));
        }


        // Filtruj po oddziale 
        if (branches != null && branches.Any())
        {
            booksQuery = booksQuery.Where(b => b.Kopie.Any(k => branches.Contains(k.Regal.Oddzial.IdOddzial)));
        }

        // Filtruj po gatunku
        if (genres != null && genres.Any())
        {
            booksQuery = booksQuery.Where(b => genres.Contains(b.Gatunek.IdGatunku));
        }

        // Sortowanie książek
        switch (sortOrder)
        {
            case "title":
                booksQuery = booksQuery.OrderBy(b => b.Tytul);
                break;
            case "title_desc":
                booksQuery = booksQuery.OrderByDescending(b => b.Tytul);
                break;
            case "author":
                booksQuery = booksQuery.OrderBy(b => b.Autor.Imie);
                break;
            case "author_desc":
                booksQuery = booksQuery.OrderByDescending(b => b.Autor.Imie);
                break;
            case "year":
                booksQuery = booksQuery.OrderBy(b => b.RokWydania);
                break;
            case "year_desc":
                booksQuery = booksQuery.OrderByDescending(b => b.RokWydania);
                break;
            case "publisher":
                booksQuery = booksQuery.OrderBy(b => b.Wydawnictwo.NazwaWydawnictwa);
                break;
            case "publisher_desc":
                booksQuery = booksQuery.OrderByDescending(b => b.Wydawnictwo.NazwaWydawnictwa);
                break;
            case "branch":
                booksQuery = booksQuery.OrderBy(b => b.Kopie.Select(k => k.Regal.Oddzial.Miasto).FirstOrDefault());
                break;
            case "branch_desc":
                booksQuery = booksQuery.OrderByDescending(b => b.Kopie.Select(k => k.Regal.Oddzial.Miasto).FirstOrDefault());
                break;
            case "genre":
                booksQuery = booksQuery.OrderBy(b => b.Gatunek.NazwaGatunku);
                break;
            case "genre_desc":
                booksQuery = booksQuery.OrderByDescending(b => b.Gatunek.NazwaGatunku);
                break;
            default:
                booksQuery = booksQuery.OrderBy(b => b.Tytul); 
                break;
        }

        var books = booksQuery.ToList();

        ViewBag.Publishers = _context.wydawnictwo?.ToList();
        ViewBag.Branches = _context.oddzial?.ToList();
        ViewBag.Genres = _context.gatunek?.ToList();

        return View(books);
    }

    // GET: Ksiazka/Create
    public IActionResult Dodawanie(int? selectedBranchId = null)
    {
        var ksiazka = new Ksiazka
        {
            Kopie = new List<Kopie> { new Kopie() }
        };

        ViewBag.Shelves = _context.regal?.ToList();
        ViewBag.Branches = _context.oddzial?.ToList();
        ViewBag.Publishers = _context.wydawnictwo?.ToList();
        ViewBag.Genres = _context.gatunek?.ToList();
        ViewBag.Authors = _context.autor?.ToList();


        return View(ksiazka);
    }

    // POST: Ksiazka/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Dodawanie(Ksiazka ksiazka, Kopie kopie)
    {
        if (ModelState.IsValid)
        {
            
            _context.Add(ksiazka);
            _context.SaveChangesAsync(); 

            kopie.IdKsiazki = ksiazka.IdKsiazki;

            _context.Add(kopie);
            _context.SaveChangesAsync(); 

            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Shelves = _context.regal?.ToList();
        ViewBag.Branches = _context.oddzial?.ToList();
        ViewBag.Publishers = _context.wydawnictwo?.ToList();
        ViewBag.Genres = _context.gatunek?.ToList();
        ViewBag.Authors = _context.autor?.ToList();

        return View(ksiazka);
    }

    // GET: Ksiazka/Edit/5
    public IActionResult Edit(int id)
    {
        var ksiazka = _context.ksiazka?
         .Include(k => k.Autor)
         .Include(k => k.Wydawnictwo)
        .Include(k => k.Gatunek)
        .Include(k => k.Kopie)
        .ThenInclude(r => r.Oddzial)
        .ThenInclude(k => k.Regal)
        .FirstOrDefault(k => k.IdKsiazki == id);

        if (ksiazka == null)
        {
            return NotFound();
        }

        if (ksiazka.Kopie == null || !ksiazka.Kopie.Any())
        {
            ksiazka.Kopie = new List<Kopie> { new Kopie() };
        }

        ViewBag.Branches = new SelectList(_context.oddzial, "IdOddzial", "MiastoAdres");

        ViewBag.SelectedBranchId = ksiazka.Kopie.FirstOrDefault()?.IdOddzial ?? 0;

        if (ksiazka.Kopie.Any())
        {
            var selectedBranchId = ksiazka.Kopie.FirstOrDefault()?.IdOddzial;
            ViewBag.Shelves = _context.regal?
                .Where(r => r.IdOddzial == selectedBranchId)
                .ToList();
        }
        else
        {
            ViewBag.Shelves = new List<Regal>();  
        }

        ViewBag.Authors = _context.autor?.ToList() ?? new List<Autor>();
        ViewBag.Publishers = _context.wydawnictwo?.ToList() ?? new List<Wydawnictwo>();
        ViewBag.Genres = _context.gatunek?.ToList() ?? new List<Gatunek>();

        return View(ksiazka);
    }

    // POST: Ksiazka/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Ksiazka ksiazka)
    {
        if (id != ksiazka.IdKsiazki)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingBook = _context.ksiazka?
                .Include(k => k.Kopie)
                .ThenInclude(k => k.Regal)
                .ThenInclude(r => r.Oddzial)
                .FirstOrDefault(k => k.IdKsiazki == id);

                if (existingBook == null)
                {
                    return NotFound();
                }

                existingBook.Tytul = ksiazka.Tytul;
                existingBook.RokWydania = ksiazka.RokWydania;
                existingBook.LiczbaStron = ksiazka.LiczbaStron;

                if (ksiazka.Autor != null)
                {
                    existingBook.Autor = ksiazka.Autor;
                }

                foreach (var kopia in ksiazka.Kopie ?? new List<Kopie>())
                {
                    var existingCopy = existingBook.Kopie?.FirstOrDefault(k => k.IdKopii == kopia.IdKopii);
                    if (existingCopy != null)
                    {
                        existingCopy.IdOddzial = kopia.IdOddzial;
                        existingCopy.IdRegal = kopia.IdRegal;
                    }
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KsiazkaExists(ksiazka.IdKsiazki))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        ViewBag.Authors = _context.autor?.ToList();
        ViewBag.Publishers = _context.wydawnictwo?.ToList();
        ViewBag.Branches = _context.oddzial?.ToList();
        ViewBag.Genres = _context.gatunek?.ToList();
        return View(ksiazka);
    }

    // GET: Ksiazka/Delete/5
    public IActionResult Delete(int id)
    {
        var ksiazka = _context.ksiazka?.Include(k => k.Wydawnictwo)
                                      .Include(k => k.Kopie) 
                                      .ThenInclude(k => k.Regal)
                                      .ThenInclude(r => r.Oddzial)
                                      .Include(k => k.Gatunek)
                                      .Include(k => k.Autor)
                                      .FirstOrDefault(k => k.IdKsiazki == id);

        if (ksiazka == null)
        {
            return NotFound();
        }

        return View(ksiazka);
    }

    // POST: Ksiazka/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var ksiazka = _context.ksiazka?.Find(id);
        if (ksiazka != null)
        {
            _context.ksiazka.Remove(ksiazka);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool KsiazkaExists(int id)
    {
        return _context.ksiazka.Any(k => k.IdKsiazki == id);
    }
}


