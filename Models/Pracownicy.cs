using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Pracownicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_pracownicy")]
        public int IdPracownicy { get; set; }

        [Column("imie")]
        public string Imie { get; set; } = null!;

        [Column("nazwisko")]
        public string Nazwisko { get; set; } = null!;

        [Column("login")]
        public string Login { get; set; } = null!;

        [Column("haslo")]
        public string Haslo { get; set; } = null!;

        [Column("stanowisko")]
        public string Stanowisko { get; set; } = null!;

        [ForeignKey("Oddzial")]
        [Column("id_oddzial")]
        public int? IdOddzial { get; set; }
        public Oddzial? Oddzial { get; set; }
        

    }
}
