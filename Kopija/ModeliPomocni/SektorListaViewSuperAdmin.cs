using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class SektorListaViewSuperAdmin
    {
        public int servis_id { get; set; }


        [Required(ErrorMessage = "Morate uneti ime servisera!")]
        [Display(Name = "Ime Servisera")]
        public string? servis_ime { get; set; }




        [Display(Name = "Pib servisa")]
        public string? servis_pib { get; set; }

        [Display(Name = "Adresa")]
        [Required(ErrorMessage = "Morate uneti adresu servisa!")]
        public string servis_adresa { get; set; }


        [Required(ErrorMessage = "Morate uneti ime  kontakt osobe!")]
        [Display(Name = "Ime kontakt osobe")]

        public string? servis_kontakt_osoba { get; set; }


        [Required(ErrorMessage = "Morate uneti kontakt broj osobe!")]

        [Display(Name = "Kontakt broj osobe")]
        public string? servis_kontakt_osoba_broj { get; set; }




        [Display(Name = "2. kontakt broj(nije obavezno)")]

        public string? servis_kontakt_osoba_broj1 { get; set; }

        [Required(ErrorMessage = "Morate izabrati sektor servisera!")]

        [Display(Name = "Sektor:")]
        public string? servis_sektor_id { get; set; }
    }
}
