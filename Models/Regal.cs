using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Models
{
    public class Regal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_regal")]
        public int IdRegal { get; set; }

        [Column("pietro")]
        public int Pietro { get; set; }

        [Column("nr_regalu")]
        public int NrRegalu { get; set; }

        [ForeignKey("Gatunek")]
        [Column("id_gatunku")]
        public int IdGatunku { get; set; }
        public Gatunek? Gatunek { get; set; }

        [ForeignKey("Oddzial")]
        [Column("id_oddzial")]
        public int IdOddzial { get; set; }
        public Oddzial? Oddzial { get; set; }

        public ICollection<Kopie>? Kopie { get; set; }


        [NotMapped]
        public string Miejsce
        {
            get
            {
                return $"{Oddzial?.MiastoAdres}, Piętro {Pietro}, Nr Regalu {NrRegalu}";
            }
        }
    }
}
