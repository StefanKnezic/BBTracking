using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class SektorZaDobavljaceSuperadmin
    {
        [Display(Name = "id Dostavljaca")]
        public int dostavljac_id { get; set; }

        [Required(ErrorMessage = "Morate uneti ime dobavljaca!")]
        [Display(Name = "Ime Dobavljaca")]
        public string? dostavljac_ime { get; set; }

        [Display(Name = "Pib broj")]
        public string? dostavljac_pib { get; set; }


        [Required(ErrorMessage = "Morate uneti adresu dobavljaca!")]
        [Display(Name = "Adresa Dobavljaca")]
        public string dostavljac_adresa { get; set; }

        [Required(ErrorMessage = "Morate uneti kontakt osobu!")]
        [Display(Name = "Kontakt osoba")]
        public string? dostavljac_kontakt_osoba { get; set; }

        [Required(ErrorMessage = "Morate uneti broj osobe !")]
        [Display(Name = "Broj kontakt osobe ")]
        public string? dostavljac_kontakt_osoba_broj { get; set; }


        [Display(Name = "Drugi Broj kontakt osobe")]
        public string? dostavljac_kontakt_osoba_broj1 { get; set; }
        [Required(ErrorMessage = "izabrati sektor dobavljaca !")]
        [Display(Name = "Sektor:")]
        public string? dostavljac_sektor_id { get; set; }
    }
}
