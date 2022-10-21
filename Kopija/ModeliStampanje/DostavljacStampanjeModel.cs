using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliStampanje
{
    public class DostavljacStampanjeModel
    {
        [Required(ErrorMessage = "Morate uneti ime dostavljaca!")]

        [Display(Name = "Ime Dostavljaca")]
        public string? dostavljac_ime { get; set; }

        [Display(Name = "Pib broj")]
        public string? dostavljac_pib { get; set; }


        [Required(ErrorMessage = "Morate uneti adresu dobavljaca!")]
        [Display(Name = "Adresa Dobavljaca")]
        public string dostavljac_adresa { get; set; }

        [Required(ErrorMessage = "Morate uneti kontakt osobu!")]
        [Display(Name = "Kontakt osoba")]
        public string? dostavljac_kontakt_osoba { get; set; }

        [Required(ErrorMessage = "Morate uneti broj osobe!")]
        [Display(Name = "Broj kontakt osobe")]
        public string? dostavljac_kontakt_osoba_broj { get; set; }

        [Display(Name = "Drugi Broj kontakt osobe(nije obavezno)")]
        public string? dostavljac_kontakt_osoba_broj1 { get; set; }
    }
}
