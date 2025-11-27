using System.ComponentModel.DataAnnotations;

namespace TEST.Models
{
    public class EditAccountViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\-]{2,40}$", ErrorMessage = "Imię może zawierać tylko litery i myślnik.")]
        public string Imie { get; set; } = null!;
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\-]{2,50}$", ErrorMessage = "Nazwisko może zawierać tylko litery i myślnik.")]
        public string Nazwisko { get; set; } = null!;
        
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


        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
        ErrorMessage = "Hasło musi zawierać małą literę, dużą literę, cyfrę i znak specjalny.")]
        public string? NoweHaslo { get; set; }

        [Compare("NoweHaslo", ErrorMessage = "Hasła nie są zgodne.")]
        [DataType(DataType.Password)]
        public string? PotwierdzenieHasla { get; set; }
    }
    
}
