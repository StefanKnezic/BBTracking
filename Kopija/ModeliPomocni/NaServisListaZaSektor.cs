using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kopija.ModeliPomocni
{
    public class NaServisListaZaSektor
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
        public string? UserName  { get; set; }


        [Column("oprema.oprema_model")]
        public string? Ime { get; set; }


        [Display(Name ="Prezime Radnika")]
        public string Prezime { get; set; }


        [Display(Name = "Napomena")]
        public string? Napomena { get; set; }

        [Display(Name = "Opis Kvara:")]
        public string? OpisKvaraPre { get; set; }

        [Display(Name = "QR Kod:")]
        public string? QR { get; set; }
    }
}
