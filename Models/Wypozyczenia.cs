using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Wypozyczenia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_wypozyczenia")]
        public int IdWypozyczenia { get; set; }

   
        [Column("id_klienta")]
        public int IdKlienta { get; set; }

        [Column("id_kopii")]
        public int IdKopii { get; set; }

        [Column("data_wypozyczenia")]
        public DateTime DataWypozyczenia { get; set; }

        [Column("data_zwrotu")]
        public DateTime? DataZwrotu { get; set; }

        [ForeignKey("Oddzial")]
        [Column("id_oddzial")]
        public int IdOddzial { get; set; }
        public Oddzial? Oddzial { get; set; }

        [ForeignKey("IdKlienta")]
        public Czytelnicy? Czytelnicy { get; set; }

        [ForeignKey("IdKopii")]
        public Kopie? Kopie { get; set; }
    }
}
