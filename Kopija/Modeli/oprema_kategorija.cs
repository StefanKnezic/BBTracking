using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class oprema_kategorija
    {
        [Key]
        public int oprema_kategorija_id { get; set; }


        [Required(ErrorMessage = "Morate uneti naziv kategorije opreme!")]

        [Display(Name = "Naziv kategorije")]
        public string? oprema_kategorija_ime { get; set; }


        [Required(ErrorMessage = "Morate izabrati sektor kom pripada kategorija!")]

        [Display(Name = "Naziv sektora")]
        public int oprema_kategorija_sektor_id { get; set; }

        

       
    }
}
