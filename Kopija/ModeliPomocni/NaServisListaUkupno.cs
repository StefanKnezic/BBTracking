using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class NaServisListaUkupno
    {
        public int Id { get; set; }


        [Display(Name = "Marka opreme")]
        public string? Marka { get; set; }

        [Display(Name = "Model opreme")]
        public string? Model { get; set; }

        [DataType(DataType.Date)]

        [Display(Name = "Datum predaje na servis")]
        public DateTime Datum { get; set; }

        [Display(Name = "Korisnicko ime Radnika")]
        public string? UserName { get; set; }

        [Display(Name = "Sektor opreme:")]
        public string? Sektor { get; set; }

         [Display(Name = "Ime")]

        public string? Ime { get; set; }


        [Display(Name = "Prezime Radnika:")]
        public string? Prezime { get; set; }

        [Display(Name = "Kategorija opreme:")]
        public string? Kategorija { get; set; }

        [Display(Name = "Napomena:")]
        public string? Napomena { get; set; }

        [Display(Name = "Napomena:")]
        public string? QR { get; set; }

        public string? OpisKvaraPre { get; set; }
    }
}
