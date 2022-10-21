using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Kopija.Modeli
{
    public class KorisnikListaModel
    {
        //dodaj id ovde i samo tamo jos ubacis u contoleru liste i to je to
        public int Id {get; set;}

        [Display(Name = "Ime radnika")]
        public string? Ime { get; set; }

        [Display(Name = "Prezime radnika")]
        public string? Prezime { get; set; }

        [Display(Name = "Broj telefona")]
        public string? BrojTelefona { get; set; }

        [Display(Name = "Sektor kom pripada")]
        public string? Sektor { get; set; }

        [Display(Name = "Naziv radnog mesta")]
        public string? RadnoMesto { get; set; }

        [Display(Name = "Lokacija")]
        public string? LokacijaIme { get; set; }

        [Display(Name = "Adresa radnje")]
        public string? Adresa { get; set; }

        [Display(Name = "Mesto/Grad")]
        public string? Mesto { get; set; }

        [Display(Name = "Korisnicko ime")]
        public string? KorisnickoIme { get; set; }
    }
}
