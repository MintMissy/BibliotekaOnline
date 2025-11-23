using System.ComponentModel.DataAnnotations;

namespace TEST.Models
{
    public class EditAccountViewModel
    {
        public int Id { get; set; }
        public string Imie { get; set; } = null!;
        public string Nazwisko { get; set; } = null!;
        public string Miasto { get; set; } = null!;
        public string Adres { get; set; } = null!;
        public string NumerPocztowy { get; set; } = null!;
        public string NrTel { get; set; } = null!;
        public string Email { get; set; } = null!;


         [DataType(DataType.Password)]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
            ErrorMessage = "Hasło musi zawierać małą literę, dużą literę, cyfrę i znak specjalny.")]
        public string? NoweHaslo { get; set; }

        [Compare("NoweHaslo", ErrorMessage = "Hasła nie są zgodne.")]
        [DataType(DataType.Password)]
        public string? PotwierdzenieHasla { get; set; }
    }
    
}
