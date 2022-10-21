using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class LokacijaSaOpremom
    {


        [Display(Name = "Marka opreme")]

        public string? Marka { get; set; }

        [Display(Name = "Model opreme")]
        public string? Model { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum  dobijanja opreme")]
        public DateTime Datum { get; set; }

        [Display(Name = "Lokacija:")]
        public string? LokacijaIme { get; set; }

        public string? LokacijaAdresa { get; set; }

        [Display(Name = "QR Kod:")]
        public string? Qr { get; set; }

        public string? SerijskiBroj { get; set; }

        [Display(Name = "Napomena:")]
        public string? Napomena { get; set; }

        [Display(Name = "Mesto/Grad:")]
        public string? LokacijaMesto { get; set; }


    }
}
