using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Gatunek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_gatunku")]
        public int IdGatunku { get; set; }

        [Column("nazwa_gatunku")]
        public string NazwaGatunku { get; set; } = null!;

        public ICollection<Ksiazka>? Ksiazka { get; set; }
    }
}
