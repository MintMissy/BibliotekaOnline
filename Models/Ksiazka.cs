using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Ksiazka
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_ksiazki")]
        public int IdKsiazki { get; set; }

        [Column("tytul")]
        public string Tytul { get; set; } = null!;

        [Column("rok_wydania")]
        public int RokWydania { get; set; }

        [Column("liczba_stron")]
        public int LiczbaStron { get; set; }

        [ForeignKey("Autor")]
        [Column("id_autora")]
        public int IdAutora { get; set; }

        [Column("id_wydawnictwa")]
        public int IdWydawnictwa { get; set; }

        [Column("id_gatunku")]
        public int IdGatunku { get; set; }

        [ForeignKey("IdAutora")]
        public Autor? Autor { get; set; }

        [ForeignKey("IdWydawnictwa")]
        public Wydawnictwo? Wydawnictwo { get; set; }

        [ForeignKey("IdGatunku")]
        public Gatunek? Gatunek { get; set; } 

        public ICollection<Kopie>? Kopie { get; set; }
    }
}
