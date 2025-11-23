using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Wydawnictwo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_wydawnictwa")]
        public int IdWydawnictwa { get; set; }

        [Column("nazwa_wydawnictwa")]
        public string NazwaWydawnictwa { get; set; } = null!;

        [Column("siedziba")]
        public string Siedziba { get; set; } = null!;

        [Column("data_zalozenia")]
        public DateTime DataZalozenia { get; set; }

        public ICollection<Ksiazka>? Ksiazka { get; set; }
    }
}
