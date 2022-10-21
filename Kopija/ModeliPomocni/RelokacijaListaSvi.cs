using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class RelokacijaListaSvi
    {
        public int Id { get; set; }


        [Display(Name = "Marka opreme")]

        public string? Marka { get; set; }

        [Display(Name = "Model opreme")]
        public string? Model { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum  dobijanja opreme")]
        public DateTime Datum { get; set; }

        [Display(Name = "Korisnik:")]
        public string? UserName { get; set; }

        [Display(Name = "Sektor opreme:")]
        public string? Sektor { get; set; }

        [Display(Name = "Ime i prezime radnika:")]
        public string? DoKoga { get; set; }

        [Display(Name = "Lokacija:")]
        public string? LokacijaIme { get; set; }

        public string?   Adresa { get; set; }

        [Display(Name = "QR Kod:")]
        public string? Qr { get; set; }

        public string? SerijskiBroj { get; set; }

        [Display(Name = "Napomena:")]
        public string? Napomena { get; set; }

        [Display(Name = "Mesto/Grad:")]
        public string? Mesto { get; set; }

        [Display(Name = "Relocirao opremu:")]
        public string? OdKoga { get; set; }





    }
}
