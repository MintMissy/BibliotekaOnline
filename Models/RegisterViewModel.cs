using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using System.ComponentModel.DataAnnotations;
using TEST.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Imię jest wymagane.")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\-]{2,40}$", ErrorMessage = "Imię może zawierać tylko litery i myślnik.")]
    public string Imie { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\-]{2,50}$", ErrorMessage = "Nazwisko może zawierać tylko litery i myślnik.")]
    public string Nazwisko { get; set; } = null!;

    
    [Required(ErrorMessage = "Login jest wymagany.")]
    [MinLength(4, ErrorMessage = "Login musi mieć co najmniej 4 znaki.")]
    [MaxLength(30, ErrorMessage = "Login może mieć maksymalnie 30 znaków.")]
    public string Login { get; set; } = null!;


    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [MinLength(12, ErrorMessage = "Hasło musi mieć co najmniej 12 znaków.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{12,}$",
        ErrorMessage = "Hasło musi zawierać małą literę, dużą literę, cyfrę oraz znak specjalny.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Miasto jest wymagane.")]
    [MaxLength(50)]
    public string Miasto { get; set; } = null!;

    [Required(ErrorMessage = "Adres jest wymagany.")]
    [MaxLength(80)]
    public string Adres { get; set; } = null!;
    
    [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
    [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy musi być w formacie 00-000.")]
    public string NumerPocztowy { get; set; } = null!;

    [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
    [RegularExpression(@"^\+?\d{9,15}$", ErrorMessage = "Wprowadź poprawny numer telefonu.")]
    public string NrTel { get; set; } = null!;

    [Required(ErrorMessage = "Email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Wprowadź poprawny adres email.")]
    public string Email { get; set; } = null!;

    public List<SelectListItem> Oddzial { get; set; } = new List<SelectListItem>();


    [Required(ErrorMessage = "Wybór oddziału jest wymagany.")]
    public int SelectedOddzialId { get; set; }

}
