using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Kopie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_kopii")]
        public int IdKopii { get; set; }
        [ForeignKey("Ksiazka")]
        [Column("id_ksiazki")]
        public int IdKsiazki { get; set; }

        [Column("czy_dostepna")]
        public char CzyDostepna { get; set; }

        [ForeignKey("Regal")]
        [Column("id_regal")]
        public int IdRegal { get; set; }

        [ForeignKey("Oddzial")]
        [Column("id_oddzial")]
        public int IdOddzial { get; set; }

        public Ksiazka? Ksiazka { get; set; }
        public Regal? Regal { get; set; }
        public Oddzial? Oddzial { get; set; }

        public ICollection<Wypozyczenia>? Wypozyczenia { get; set; }
    }
}
