using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Oddzial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_oddzial")]
        public int IdOddzial { get; set; }

        [Column("adres")]
        public string Adres { get; set; } = null!;

        [Column("kod_pocztowy")]
        public string KodPocztowy { get; set; } = null!;

        [Column("miasto")]
        public string Miasto { get; set; } = null!;

        [NotMapped] 
        public string MiastoAdres
        {
            get
            {
                return $"{Miasto} - {Adres}";
            }
        }

        public ICollection<Regal>? Regal { get; set; }
    }
}
