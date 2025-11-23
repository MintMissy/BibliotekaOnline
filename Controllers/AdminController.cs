using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEST.Context;
using System.Linq;
using TEST.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles = "Administrator, Bibliotekarz")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string entity)
    {
        if (string.IsNullOrEmpty(entity))
        {
            return NotFound("Nie podano typu encji.");
        }

        ViewData["Entity"] = entity;

        switch (entity.ToLower())
        {
            case "autor":
                var autorzy = _context.autor?.ToList();
                return View("~/Views/Home/EntityView.cshtml", autorzy);

            case "czytelnicy":
                var czytelnicy = _context.czytelnicy?.ToList();
                return View("~/Views/Home/EntityView.cshtml", czytelnicy);

            case "gatunek":
                var gatunki = _context.gatunek?.ToList();
                return View("~/Views/Home/EntityView.cshtml", gatunki);

            case "oddzialregal":
                var viewModel = new OddzialRegalViewModel
                {
                    Oddzial = _context.oddzial
                    .Include(o => o.Regal) 
                    .ToList(),
                    Regal = _context.regal
                    .Include(r => r.Oddzial)
                    .ToList()
                };

                return View("~/Views/Home/OddzialRegal.cshtml", viewModel);

            case "wypozyczeniakopie":
                var wypozyczeniaKopieViewModel = new WypozyczeniaKopieViewModel
                {
                    Wypozyczenia = _context.wypozyczenia
                    .Include(w => w.Czytelnicy)
                    .Include(w => w.Kopie)
                    .Include(w => w.Oddzial)
                    .ToList(),

                    Kopie = _context.kopie
                    .Include(k => k.Ksiazka)
                    .ToList()
                };
                return View("~/Views/Home/WypozyczeniaKopie.cshtml", wypozyczeniaKopieViewModel);

            case "wydawnictwo":
                var wydawnictwa = _context.wydawnictwo.ToList();
                return View("~/Views/Home/EntityView.cshtml", wydawnictwa);

            case "pracownicy":
                var pracownicy = _context.pracownicy
                    .Include(p => p.Oddzial)
                    .ToList();
                return View("~/Views/Home/EntityView.cshtml", pracownicy);

            case "ksiazki":
                var ksiazki = _context.ksiazka
                    .Include(k => k.Wydawnictwo)
                    .Include(k => k.Autor)
                    .ToList();
                return View("~/Views/Home/EntityView.cshtml", ksiazki);

            case "kopie":
                var kopie = _context.kopie
                    .Include(k => k.Ksiazka)
                    .Include(k => k.Oddzial)
                    .Include(k => k.Regal)
                    .ToList();
                return View("~/Views/Home/EntityView.cshtml", kopie);

            case "DodajRegalPietro":
                var dodajregalpietro = _context.regal.ToList();

                return View("~/Views/Home/AdminInterfejs/OddzialRegal/Dodawanie.cshtml", dodajregalpietro);

            default:
                return NotFound("Nieznana encja.");
        }
    }

    private IActionResult ViewEntity(string viewPath, object entityList)
    {
        return View(viewPath, entityList);
    }

    [HttpGet]
    public IActionResult AddKopie()
    {
        var ksiazki = _context.ksiazka.ToList();
        var oddzialy = _context.oddzial.ToList();

        var czyDostepneOpcje = _context.kopie
        .Select(k => k.CzyDostepna)
        .Distinct()
        .ToList();

        ViewBag.Ksiazki = ksiazki ?? new List<Ksiazka>();
        ViewBag.Regaly = _context.regal
            .Include(r => r.Oddzial) 
            .Select(r => new {
                r.IdRegal,
                OpisRegalu = r.NrRegalu + " - " + r.Pietro + " - " + r.Oddzial.Adres + " - " + r.Oddzial.Miasto,
                r.Oddzial.IdOddzial  
            })
            .ToList();
        ViewBag.Oddzialy = oddzialy ?? new List<Oddzial>();
        ViewBag.CzyDostepneOpcje = czyDostepneOpcje;


        return View("~/Views/Home/AdminInterfejs/Kopie/Dodawanie.cshtml");
    }



    [HttpGet]
    public IActionResult AddWypozyczeniaKopie()
    {
        ViewData["Klienci"] = _context.czytelnicy.ToList();
        ViewData["Kopie"] = _context.kopie.Include(k => k.Ksiazka).ToList();
        ViewData["Oddzialy"] = _context.oddzial.ToList();

        return View("~/Views/Home/AdminInterfejs/WypozyczeniaKopie/Dodawanie.cshtml");
    }


    [HttpGet]
    public IActionResult AddOddzialRegal()
    {
        return View("~/Views/Home/AdminInterfejs/OddzialRegal/Dodawanie.cshtml");
    }

    [HttpGet]
    public IActionResult AddCzytelnik()
    {
        return View("~/Views/Home/AdminInterfejs/Czytelnicy/Dodawanie.cshtml");
    }

    [HttpGet]
    public IActionResult AddGatunek()
    {
        return View("~/Views/Home/AdminInterfejs/Gatunek/Dodawanie.cshtml");
    }

    [HttpGet]
    public IActionResult AddPracownik()
    {
        ViewData["Oddzialy"] = _context.oddzial.ToList();
        return View("~/Views/Home/AdminInterfejs/Pracownicy/Dodawanie.cshtml");
    }

    [HttpGet]
    public IActionResult AddWydawnictwo()
    {
        return View("~/Views/Home/AdminInterfejs/Wydawnictwo/Dodawanie.cshtml");
    }
    [HttpGet]
    public IActionResult DodajRegalPietro(int id)
    {
        ViewBag.Gatunek = _context.gatunek
         .Select(g => new SelectListItem
         {
             Value = g.IdGatunku.ToString(),
             Text = g.NazwaGatunku
         })
         .ToList();

        ViewBag.Oddzialy = _context.oddzial
        .Select(o => new SelectListItem
        {
            Value = o.IdOddzial.ToString(),
            Text = $"{o.Miasto}, {o.Adres}"
        }).ToList();

        return View("~/Views/Home/AdminInterfejs/OddzialRegal/DodajRegalPietro.cshtml");
    }



    // Add Action (GET)
    [HttpGet]
    public IActionResult AddAutor(string entity)
    {
        if (string.IsNullOrEmpty(entity))
        {
            return NotFound("Entity nie zostało przekazane.");
        }

        var wypozyczenia = _context.wypozyczenia?.Include(w => w.Czytelnicy)
                                             .Include(w => w.Oddzial)
                                             .Include(w => w.Kopie)
                                             .ToList();

        var kopie = _context.kopie?.Include(k => k.Ksiazka).ToList();
        ViewData["Klienci"] = _context.czytelnicy?.ToList();
        ViewData["Oddzialy"] = _context.oddzial?.ToList();
        ViewData["Ksiazki"] = _context.ksiazka?.ToList();


        ViewBag.Wypozyczenia = wypozyczenia;
        ViewBag.Kopie = kopie;

        string viewPath = GetAddViewPath(entity.ToLower());
        if (string.IsNullOrEmpty(viewPath))
        {
            return NotFound("Nieznana encja.");
        }

        return View(viewPath);
    }

    private string GetAddViewPath(string entity)
    {
        return entity switch
        {
            "autor" => "~/Views/Home/AdminInterfejs/Autor/Dodawanie.cshtml",
            "czytelnicy" => "~/Views/Home/AdminInterfejs/czytelnicy/Dodawanie.cshtml",
            "gatunek" => "~/Views/Home/AdminInterfejs/gatunek/Dodawanie.cshtml",
            "wydawnictwo" => "~/Views/Home/AdminInterfejs/wydawnictwo/Dodawanie.cshtml",
            "pracownicy" => "~/Views/Home/AdminInterfejs/pracownicy/Dodawanie.cshtml",
            "wypozyczeniakopie" => "~/Views/Home/AdminInterfejs/WypozyczeniaKopie/Dodawanie.cshtml",
            "kopie" => "~/Views/Home/AdminInterfejs/Kopie/Dodawanie.cshtml",
            "oddzialregal" => "~/Views/Home/AdminInterfejs/OddzialRegal/Dodawanie.cshtml",
            "DodajRegalPietro" => "~/Views/Home/AdminInterfejs/OddzialRegal/DodajRegalPietro.cshtml",
            _ => null,
        };
    }

    [HttpPost]
    public IActionResult AddKopie(Kopie model)
    {
      
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(model);
        }

        if (model.IdOddzial == 0 && model.IdRegal > 0)
        {
            var regal = _context.regal.FirstOrDefault(r => r.IdRegal == model.IdRegal);
            if (regal != null)
            {
                model.IdOddzial = regal.Oddzial.IdOddzial;
            }
        }

        _context.kopie.Add(model);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Kopia została dodana!";
        return RedirectToAction("AddKopie");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddWypozyczeniaKopie(Wypozyczenia wypozyczenia)
    {
        if (ModelState.IsValid)
        {
            if (wypozyczenia.DataWypozyczenia.Kind == DateTimeKind.Unspecified)
            {
                wypozyczenia.DataWypozyczenia = DateTime.SpecifyKind(wypozyczenia.DataWypozyczenia, DateTimeKind.Utc);
            }
            wypozyczenia.DataZwrotu = null; 
            _context.wypozyczenia.Add(wypozyczenia);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Wypożyczenie zostało dodane pomyślnie!";
            return RedirectToAction("Index", "Wypozyczenia");
        }

        ViewData["Klienci"] = _context.czytelnicy.ToList();
        ViewData["Kopie"] = _context.kopie.Include(k => k.Ksiazka).ToList();
        ViewData["Oddzialy"] = _context.oddzial.ToList();


        return View("~/Views/Home/WypozyczeniaKopie.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DodajRegalPietro(Regal model)
    {
        if (ModelState.IsValid)
        {
            var gatunek = _context.gatunek.FirstOrDefault(g => g.IdGatunku == model.IdGatunku);
            if (gatunek == null)
            {
                TempData["ErrorMessage"] = "Wybrany gatunek nie istnieje.";
                return RedirectToAction("DodajRegalPietro");
            }
            // Sprawdzamy, czy oddział o danym IdOddzial istnieje
            var oddzial = _context.oddzial.FirstOrDefault(o => o.IdOddzial == model.IdOddzial);

            if (oddzial == null)
            {
                TempData["ErrorMessage"] = "Wybrany oddział nie istnieje.";
                return RedirectToAction("DodajRegalPietro");
            }

            // Dodaj nowy regał
            var regal = new Regal
            {
                NrRegalu = model.NrRegalu,
                Pietro = model.Pietro,
                IdOddzial = model.IdOddzial,
                IdGatunku = model.IdGatunku
            };

            _context.regal.Add(regal);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Regał i piętro zostały dodane pomyślnie.";
            return RedirectToAction("Index", new { entity = "OddzialRegal" });
        }
        return View("~/Views/Home/OddzialRegal.cshtml", new OddzialRegalViewModel
        {
            Oddzial = _context.oddzial.ToList(),
            Regal = _context.regal.ToList()
        });
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddOddzialRegal(Oddzial oddzial)
    {
        if (ModelState.IsValid)
        {
            _context.oddzial?.Add(oddzial);
            _context.SaveChanges();

            var viewModel = new OddzialRegalViewModel
            {
                Oddzial = _context.oddzial.ToList(),
                Regal = _context.regal.ToList()
            };
            TempData["SuccessMessage"] = "Oddzial został pomyślnie dodany.";

            return RedirectToAction(nameof(Index), new { entity = "oddzialregal" });
        }

        return View("~/Views/Home/OddzialRegal.cshtml", oddzial);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddCzytelnik(Czytelnicy model)
    {
        if (ModelState.IsValid)
        {
            _context.czytelnicy?.Add(model);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Czytelnik został pomyślnie dodany.";
            return RedirectToAction(nameof(Index), new { entity = "czytelnicy" });
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddGatunek(Gatunek model)
    {
        if (ModelState.IsValid)
        {
            _context.gatunek?.Add(model);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Gatunek został pomyślnie dodany.";
            return RedirectToAction(nameof(Index), new { entity = "gatunek" });
        }

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddPracownik(Pracownicy pracownik)
    {
        if (ModelState.IsValid)
        {
            _context.pracownicy?.Add(pracownik);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Pracownik został pomyślnie dodany.";
            return RedirectToAction(nameof(Index), new { entity = "pracownicy" });
        }

        ViewData["Oddzialy"] = _context.oddzial?.ToList();
        return View(pracownik);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddWydawnictwo(string entity, Wydawnictwo wydawnictwo)
    {
        if (ModelState.IsValid)
        {
            if (wydawnictwo.DataZalozenia.Kind == DateTimeKind.Unspecified)
            {
                wydawnictwo.DataZalozenia = DateTime.SpecifyKind(wydawnictwo.DataZalozenia, DateTimeKind.Utc);
            }
            switch (entity.ToLower())
            {
                case "wydawnictwo":
                    _context.wydawnictwo?.Add(wydawnictwo);
                    break;
                default:
                    return NotFound("Nieznana encja.");
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { entity });
        }

        return View(wydawnictwo);
    }


    // Add Action (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddAutor(string entity, Autor autor)
    {

        if (ModelState.IsValid)
        {
            if (autor.DataUrodzenia.Kind == DateTimeKind.Unspecified)
            {
                autor.DataUrodzenia = DateTime.SpecifyKind(autor.DataUrodzenia, DateTimeKind.Utc);
            }

            switch (entity.ToLower())
            {
                case "autor":
                    _context.autor?.Add(autor);
                    break;
                default:
                    return NotFound("Nieznana encja.");
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { entity });
        }

        return View(autor);
    }

    private object GetEntityForAdd(string entity, Autor autor, Czytelnicy czytelnicy, Gatunek gatunek, Pracownicy pracownicy,
                                   OddzialRegalViewModel oddzialregal, WypozyczeniaKopieViewModel wypozyczeniakopie, Kopie kopie, Wydawnictwo wydawnictwo)
    {
        if (string.IsNullOrEmpty(entity))
        {
            throw new ArgumentNullException(nameof(entity), "Nie podano typu encji.");
        }

        return entity switch
        {
            "autor" => autor ?? throw new ArgumentNullException(nameof(autor)),
            "czytelnicy" => czytelnicy ?? throw new ArgumentNullException(nameof(czytelnicy)),
            "gatunek" => gatunek ?? throw new ArgumentNullException(nameof(gatunek)),
            "pracownicy" => pracownicy ?? throw new ArgumentNullException(nameof(pracownicy)),
            "oddzialregal" => oddzialregal ?? throw new ArgumentNullException(nameof(oddzialregal)),
            "wypozyczeniakopie" => wypozyczeniakopie ?? throw new ArgumentNullException(nameof(wypozyczeniakopie)),
            "kopie" => kopie ?? throw new ArgumentNullException(nameof(kopie)),
            "wydawnictwo" => wydawnictwo ?? throw new ArgumentNullException(nameof(wydawnictwo)),
            _ => null,
        };
    }

    [HttpGet]
    public IActionResult EditKopie(int id)
    {
        var kopia = _context.kopie
            .Include(k => k.Ksiazka)
            .Include(k => k.Regal)
            .Include(k => k.Oddzial)
            .FirstOrDefault(k => k.IdKopii == id);

        if (kopia == null)
        {
            return NotFound();
        }
        var ksiazki = _context.ksiazka
       .Select(k => new { k.IdKsiazki, k.Tytul })
       .ToList();
        ViewBag.Ksiazki = new SelectList(ksiazki, "IdKsiazki", "Tytul", kopia.IdKsiazki);
        var regalyIOddzialy = _context.regal
            .Include(r => r.Oddzial)
            .Select(r => new
            {
                Id = r.IdRegal,
                Display = $"{r.Oddzial.Miasto}, {r.Oddzial.Adres} (Piętro: {r.Pietro}, Regał: {r.NrRegalu})"
            }).ToList();

        ViewBag.RegalyIOddzialy = new SelectList(regalyIOddzialy, "Id", "Display", kopia.IdRegal); 
        return View("~/Views/Home/AdminInterfejs/Kopie/Edit.cshtml", kopia);
    }

    [HttpGet]
    public IActionResult EditOddzialRegal(int id)
    {
        var oddzial = _context.oddzial?.Find(id);
        if (oddzial == null) return NotFound();

        return View("~/Views/Home/AdminInterfejs/OddzialRegal/Edit.cshtml", oddzial);
    }

    [HttpGet]
    public IActionResult EditCzytelnik(int id)
    {
        var czytelnik = _context.czytelnicy?.Find(id);
        if (czytelnik == null) return NotFound();
        return View("~/Views/Home/AdminInterfejs/Czytelnicy/Edit.cshtml", czytelnik);
    }

    [HttpGet]
    public IActionResult EditGatunek(int id)
    {
        var gatunek = _context.gatunek?.Find(id);
        if (gatunek == null) return NotFound();
        return View("~/Views/Home/AdminInterfejs/Gatunek/Edit.cshtml", gatunek);
    }

    [HttpGet]
    public IActionResult EditPracownik(int id)
    {
        var pracownik = _context.pracownicy?.Find(id);
        if (pracownik == null)
        {
            return NotFound();
        }

        return View("~/Views/Home/AdminInterfejs/Pracownicy/Edit.cshtml", pracownik);
    }

    [HttpGet]
    public IActionResult EditWydawnictwo(int id)
    {
        var wydawnictwo = _context.wydawnictwo?.Find(id);
        if (wydawnictwo == null) return NotFound();
        return View("~/Views/Home/AdminInterfejs/Wydawnictwo/Edit.cshtml", wydawnictwo);
    }

    // Edit Action (GET)
    [HttpGet]
    public IActionResult EditAutor(string entity, int id)
    {
        if (string.IsNullOrEmpty(entity) || id == 0)
        {
            return NotFound("Nie podano encji lub ID.");
        }

        // Sprawdź typ encji
        object entityData = GetEntityById(entity.ToLower(), id);
        if (entityData == null)
        {
            return NotFound("Nie znaleziono encji o podanym ID.");
        }

        // Przekaż dane do widoku
        string viewPath = GetEditViewPath(entity.ToLower());
        if (viewPath == null)
        {
            return NotFound("Niepoprawna encja.");
        }

        return View(viewPath, entityData);
    }

    private object GetEntityById(string entity, int id)
    {
        object result = entity switch
        {
            "autor" => _context.autor?.FirstOrDefault(a => a.IdAutora == id),
            "czytelnicy" => _context.czytelnicy?.FirstOrDefault(c => c.IdKlienta == id),
            "gatunek" => _context.gatunek?.FirstOrDefault(g => g.IdGatunku == id),
            "wydawnictwo" => _context.wydawnictwo?.FirstOrDefault(w => w.IdWydawnictwa == id),
            "pracownicy" => _context.pracownicy?.Include(p => p.Oddzial).FirstOrDefault(p => p.IdPracownicy == id),
            "kopie" => _context.kopie?.Include(k => k.Ksiazka).Include(k => k.Oddzial).Include(k => k.Regal).FirstOrDefault(k => k.IdKopii == id),
            "oddzialregal" => _context.oddzial?.FirstOrDefault(o => o.IdOddzial == id),
            "regal" => _context.regal?.FirstOrDefault(r => r.IdRegal == id),
            "wypozyczeniakopie" => _context.wypozyczenia?.FirstOrDefault(w => w.IdWypozyczenia == id),
            _ => null,
        };

        if (result == null)
        {
            Console.WriteLine($"Entity '{entity}' with ID '{id}' not found.");
        }
        return result;
    }

    private string GetEditViewPath(string entity)
    {
        return entity switch
        {
            "autor" => "~/Views/Home/AdminInterfejs/Autor/Edit.cshtml",
            "czytelnicy" => "~/Views/Home/AdminInterfejs/Czytelnicy/Edit.cshtml",
            "gatunek" => "~/Views/Home/AdminInterfejs/Gatunek/Edit.cshtml",
            "wydawnictwo" => "~/Views/Home/AdminInterfejs/Wydawnictwo/Edit.cshtml",
            "pracownicy" => "~/Views/Home/AdminInterfejs/Pracownicy/Edit.cshtml",
            "wypozyczenia" => "~/Views/Home/AdminInterfejs/WypozyczeniaKopie/Edit.cshtml",
            "kopie" => "~/Views/Home/AdminInterfejs/Kopie/Edit.cshtml",
            "oddzialregal" => "~/Views/Home/AdminInterfejs/OddzialRegal/Edit.cshtml",
            _ => null,
        };
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditOddzialRegal(Oddzial model)
    {
        if (ModelState.IsValid)
        {
            var oddzial = _context.oddzial?.Find(model.IdOddzial);

            oddzial.Adres = model.Adres;
            oddzial.Miasto = model.Miasto;
            oddzial.KodPocztowy = model.KodPocztowy;

            _context.oddzial?.Update(oddzial);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Oddział został pomyślnie zaktualizowany.";
            return RedirectToAction(nameof(Index), new { entity = "oddzialregal" });
        }

        return View("EditOddzialRegal");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditKopie(Kopie model)
    {
        if (ModelState.IsValid)
        {
            var kopia = _context.kopie.FirstOrDefault(k => k.IdKopii == model.IdKopii);

            if (kopia == null)
            {
                return NotFound();
            }

            kopia.IdRegal = model.IdRegal; 
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Zmiany zapisano pomyślnie.";
            return RedirectToAction("Index", new { entity = "Kopie" });
        }

        
        var regalyIOddzialy = _context.regal
            .Include(r => r.Oddzial)
            .Select(r => new
            {
                Id = r.IdRegal,
                Display = $"{r.Oddzial.Miasto}, {r.Oddzial.Adres} (Piętro: {r.Pietro}, Regał: {r.NrRegalu})"
            }).ToList();

        ViewBag.RegalyIOddzialy = new SelectList(regalyIOddzialy, "Id", "Display", model.IdRegal);
        return RedirectToAction("EditKopie");
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditCzytelnik(Czytelnicy model)
    {
        if (ModelState.IsValid)
        {
            var czytelnik = _context.czytelnicy?.Find(model.IdKlienta);
            if (czytelnik == null) return NotFound();

            czytelnik.Imie = model.Imie;
            czytelnik.Nazwisko = model.Nazwisko;
            czytelnik.Login = model.Login;
            czytelnik.Haslo = model.Haslo;
            czytelnik.Miasto = model.Miasto;
            czytelnik.Adres = model.Adres;
            czytelnik.NumerPocztowy = model.NumerPocztowy;
            czytelnik.NrTel = model.NrTel;
            czytelnik.Email = model.Email;

            _context.czytelnicy?.Update(czytelnik);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Dane czytelnika zostały pomyślnie zapisane.";
            return RedirectToAction(nameof(Index), new { entity = "czytelnicy" });
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditGatunek(Gatunek model)
    {
        if (ModelState.IsValid)
        {
            var gatunek = _context.gatunek?.Find(model.IdGatunku);
            if (gatunek == null) return NotFound();

            gatunek.NazwaGatunku = model.NazwaGatunku;

            _context.gatunek.Update(gatunek);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Dane gatunku zostały pomyślnie zapisane.";
            return RedirectToAction(nameof(Index), new { entity = "gatunek" });
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditPracownik(Pracownicy model)
    {
        if (ModelState.IsValid)
        {
            var pracownik = _context.pracownicy?.FirstOrDefault(p => p.IdPracownicy == model.IdPracownicy);

            if (pracownik == null)
            {
                return NotFound();
            }
            pracownik.Imie = model.Imie;
            pracownik.Nazwisko = model.Nazwisko;
            pracownik.Login = model.Login;

            if (!string.IsNullOrEmpty(model.Haslo))
            {
                pracownik.Haslo = model.Haslo;
            }

            _context.pracownicy.Update(pracownik);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Dane pracownika zostały pomyślnie zapisane.";
            return RedirectToAction(nameof(Index), new { entity = "pracownicy" });
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditWydawnictwo(Wydawnictwo model)
    {
        if (ModelState.IsValid)
        {
            var wydawnictwo = _context.wydawnictwo?.Find(model.IdWydawnictwa);
            if (wydawnictwo == null) return NotFound();

            if (model.DataZalozenia.Kind == DateTimeKind.Unspecified)
            {
                model.DataZalozenia = DateTime.SpecifyKind(model.DataZalozenia, DateTimeKind.Utc);
            }

            wydawnictwo.NazwaWydawnictwa = model.NazwaWydawnictwa;
            wydawnictwo.Siedziba = model.Siedziba;
            wydawnictwo.DataZalozenia = model.DataZalozenia;

            _context.wydawnictwo?.Update(wydawnictwo);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Dane wydawnictwa zostały pomyślnie zapisane.";
            return RedirectToAction(nameof(Index), new { entity = "wydawnictwo" });
        }

        return View(model);
    }


    // Edit Action (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditAutor(Autor model)
    {
        if (ModelState.IsValid)
        {
            var autor = _context.autor?.Find(model.IdAutora);
            if (autor == null)
            {
                return NotFound();
            }

            if (model.DataUrodzenia.Kind == DateTimeKind.Unspecified)
            {
                model.DataUrodzenia = DateTime.SpecifyKind(model.DataUrodzenia, DateTimeKind.Utc);
            }

            autor.Imie = model.Imie;
            autor.Nazwisko = model.Nazwisko;
            autor.DataUrodzenia = model.DataUrodzenia;

            _context.autor.Update(autor);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Dane autora zostały pomyślnie zapisane.";

            return View("~/Views/Home/AdminInterfejs/Autor/Edit.cshtml", model);

        }

        return View(model);
    }


    private IActionResult UpdateEntity<T>(int id, T entity, DbSet<T> dbSet) where T : class
    {
        if (entity != null)
        {
           
            var entityId = entity.GetType().GetProperty("Id")?.GetValue(entity);

            if (entityId is int actualId && actualId == id)
            {
                if (ModelState.IsValid)
                {
                    dbSet.Update(entity);  
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index), new { entity = entity.GetType().Name.ToLower() });
                }
                return View(entity);  
            }
            else
            {
                return NotFound("ID w formularzu nie pasuje do ID encji.");
            }
        }

        return NotFound("Niepoprawny typ danych.");
    }


    [HttpGet]
    public IActionResult Delete(string entity, int id)
    {
        if (string.IsNullOrEmpty(entity))
        {
            return BadRequest("Entity nie zostało przekazane.");
        }

        object data = GetEntityById(entity.ToLower(), id);
        if (data == null)
        {
            return NotFound("Nie znaleziono encji o podanym ID.");
        }

        string viewPath = GetDeleteViewPath(entity.ToLower());
        if (string.IsNullOrEmpty(viewPath))
        {
            return NotFound("Nie znaleziono ścieżki widoku dla tej encji.");
        }

        if (entity.ToLower() == "wypozyczeniakopie")
        {
            var wypozyczenie = _context.wypozyczenia
            .Include(w => w.Czytelnicy)   
            .Include(w => w.Kopie)        
            .ThenInclude(k => k.Ksiazka)  
            .FirstOrDefault(w => w.IdWypozyczenia == id);

            if (wypozyczenie != null)
            {
                data = wypozyczenie;
            }
        }

        ViewData["Entity"] = entity;
        return View(viewPath, data);
    }

    private string GetDeleteViewPath(string entity)
    {
        return entity switch
        {
            "autor" => "~/Views/Home/AdminInterfejs/Autor/Delete.cshtml",
            "czytelnicy" => "~/Views/Home/AdminInterfejs/Czytelnicy/Delete.cshtml",
            "gatunek" => "~/Views/Home/AdminInterfejs/Gatunek/Delete.cshtml",
            "wydawnictwo" => "~/Views/Home/AdminInterfejs/Wydawnictwo/Delete.cshtml",
            "pracownicy" => "~/Views/Home/AdminInterfejs/Pracownicy/Delete.cshtml",
            "wypozyczeniakopie" => "~/Views/Home/AdminInterfejs/WypozyczeniaKopie/Delete.cshtml",
            "kopie" => "~/Views/Home/AdminInterfejs/Kopie/Delete.cshtml",
            "regal" => "~/Views/Home/AdminInterfejs/WypozyczeniaKopie/Delete.cshtml",
            "oddzial" => "~/Views/Home/AdminInterfejs/WypozyczeniaKopie/Delete.cshtml",
            "oddzialregal" => "~/Views/Home/AdminInterfejs/OddzialRegal/Delete.cshtml",
            _ => null,
        };
    }

    // Delete Action
    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(string entity, int id)
    {
        if (string.IsNullOrEmpty(entity))
        {
            return BadRequest("Entity nie zostało przekazane.");
        }

        switch (entity.ToLower())
        {
            case "autor":
                var autor = _context.autor.Find(id);
                if (autor != null)
                {
                    _context.autor.Remove(autor);
                }
                break;
            case "czytelnicy":
                var czytelnicy = _context.czytelnicy.Find(id);
                if (czytelnicy != null)
                {
                    _context.czytelnicy.Remove(czytelnicy);
                }
                break;
            case "gatunek":
                var gatunek = _context.gatunek.Find(id);
                if (gatunek != null)
                {
                    _context.gatunek.Remove(gatunek);
                }
                break;
            case "pracownicy":
                var pracownik = _context.pracownicy.Find(id);
                if (pracownik != null)
                {
                    _context.pracownicy.Remove(pracownik);
                }
                break;
            case "oddzialregal":
                var oddzial = _context.oddzial.Find(id);
                if (oddzial != null)
                {
                    _context.oddzial.Remove(oddzial);
                }
                break;
            case "kopie":
                var kopia = _context.kopie.Find(id);
                if (kopia != null)
                {
                    _context.kopie.Remove(kopia);
                    _context.SaveChanges();
                }
                break;
            case "wydawnictwo":
                var wydawnictwo = _context.wydawnictwo.Find(id);
                if (wydawnictwo != null) _context.wydawnictwo.Remove(wydawnictwo);
                break;
            case "wypozyczeniakopie":
                var wypozyczenie = _context.wypozyczenia.Find(id);
                if (wypozyczenie != null) _context.wypozyczenia.Remove(wypozyczenie);
                break;
        }


        _context.SaveChanges();
        return RedirectToAction(nameof(Index), new { entity });

    }

}

