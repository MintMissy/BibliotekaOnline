using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using System.ComponentModel.DataAnnotations;
using TEST.Models;

public class RegisterViewModel
{
    public string Imie { get; set; } = null!;
    public string Nazwisko { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Miasto { get; set; } = null!;
    public string Adres { get; set; } = null!;
    public string NumerPocztowy { get; set; } = null!;
    public string NrTel { get; set; } = null!;
    public string Email { get; set; } = null!;

    public List<SelectListItem> Oddzial { get; set; } = new List<SelectListItem>();


    [Required(ErrorMessage = "Wybór oddziału jest wymagany.")]
    public int SelectedOddzialId { get; set; }

}
