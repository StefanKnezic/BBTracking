using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class OpremaListaIstorijaModel
    {
        
        public int Id { get; set; }


        [Display(Name ="Marka opreme:")]
        public string? Marka { get; set; }
        [Display(Name = "Model opreme:")]
        public string? Model { get; set; }

        [Display(Name = "Napomena:")]
        public string? Napomena { get; set; }

        [Display(Name = "Kod:")]
        public string? DoKoga { get; set; }

        [Display(Name = "Kod Korisnika:")]
        public string UserName { get; set; }

        [Display(Name = "Na Lokaciji:")]
        public string? LokacijaIme { get; set; }

        [Display(Name = "Adresa Lokacije:")]
        public string? LokacijaAdresa { get; set; }

        [Display(Name = "Mesto/Grad:")]
        public string? LokacijaMesto { get; set; }

        //[Display(Name = "Od:")]
        //public string? OdKoga { get; set; }

        [Display(Name = "Datum primanja opreme:")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
    }
}
