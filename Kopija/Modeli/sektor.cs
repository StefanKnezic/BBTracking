using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class sektor
    {
        [Key]
        
        public int sektor_id { get; set; }


        [Required(ErrorMessage = "Morate uneti naziv sektora!")]

        [Display(Name = "Naziv sektora")]
        public string? sektor_ime { get; set; }

        [Display(Name = "Otkaciti ako je potrebno videti opremu svih sektora:")]
        public int? sektor_bitan { get; set; } = 0;
    }
}
