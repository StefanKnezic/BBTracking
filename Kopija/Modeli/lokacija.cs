using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class lokacija
    {
        [Key]


        
        public int lokacija_id { get; set; }

        [Required(ErrorMessage = "Morate uneti ime objekta!")]

        [Display(Name = "Lokacija ime")]
        public string? lokacija_ime { get; set; }


        [Required(ErrorMessage = "Morate uneti adresu objekta!")]

        [Display(Name = "Adresa lokacije")]
        public string? lokacija_adresa { get; set; }

        [Required(ErrorMessage = "Morate uneti mesto objekta!")]

        [Display(Name = "Mesto/Grad:")]

        public string? lokacija_mesto { get; set; } //ovo je u sql tabela za 
    }
}
