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
        public string? NoweHaslo { get; set; }

        [Compare("NoweHaslo", ErrorMessage = "Hasła nie są zgodne.")]
        [DataType(DataType.Password)]
        public string? PotwierdzenieHasla { get; set; }
    }
    
}
