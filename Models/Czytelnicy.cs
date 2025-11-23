using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Czytelnicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_klienta")]
        public int IdKlienta { get; set; }

        [Column("imie")]
        public string Imie { get; set; } = null!;

        [Column("nazwisko")]
        public string Nazwisko { get; set; } = null!;

        public string PełneImieNazwisko => $"{Imie} {Nazwisko}";

        [Column("login")]
        public string Login { get; set; } = null!;

        [Column("haslo")]
        public string Haslo { get; set; } = null!;

        [Column("miasto")]
        public string Miasto { get; set; } = null!;

        [Column("adres")]
        public string Adres { get; set; } = null!;

        [Column("numer_pocztowy")]
        public string NumerPocztowy { get; set; } = null!;

        [Column("nr_tel")]
        public string NrTel { get; set; } = null!;

        [Column("email")]
        public string Email { get; set; } = null!;

        public ICollection<Wypozyczenia>? Wypozyczenia { get; set; }

       
    }
}
