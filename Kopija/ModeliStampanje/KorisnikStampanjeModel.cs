using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliStampanje
{
    public class KorisnikStampanjeModel
    {
        public string Ime { get; set; }

        [Display(Name = "Prezime radnika")]
        public string Prezime { get; set; }

        [Display(Name = "Broj telefona")]
        public string BrojTelefona { get; set; }

        [Display(Name = "Sektor kom pripada")]
        public string Sektor { get; set; }

        [Display(Name = "Naziv radnog mesta")]
        public string RadnoMesto { get; set; }

        [Display(Name = "Lokacija radnog mesta")]
        public string LokacijaIme { get; set; }

        [Display(Name = "adresa lokacije radnog mesta")]
        public string Adresa { get; set; }

        [Display(Name = "Korisnicko ime")]
        public string KorisnickoIme { get; set; }
    }
}
