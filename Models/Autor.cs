
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Autor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_autora")]
        public int IdAutora { get; set; }

        [Column("imie")]
        public string Imie { get; set; } = null!;

        [Column("nazwisko")]
        public string Nazwisko { get; set; } = null!;

        [Column("data_urodzenia")]
        public DateTime DataUrodzenia { get; set; }

        public ICollection<Ksiazka>? Ksiazka { get; set; }
    }
}
