using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class SektorView
    {
        [Required(ErrorMessage = "Morate uneti naziv sektora!")]

        [Display(Name = "Naziv sektora")]
        public string? sektor_ime { get; set; }

        [Display(Name = "Pregled opreme u svim sektorima")]
        public bool sektor_bitan { get; set; } 
    }
}
