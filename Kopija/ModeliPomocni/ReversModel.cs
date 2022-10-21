using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class ReversModel
    {
        public string? Model { get; set; }
        public string? Marka { get; set; }
        public string? SerijskiBroj { get; set; }
        public string? Qr { get; set; }

        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        public string? KorisnickoIme { get; set; }

        public string? KorisnikImePrezime { get; set; }
        public string? LokacijaIme { get; set; }
        public string? LokacijaMesto { get; set; }
        public string? LokacijaAdresa { get; set; }
        public string? Napomena { get; set; }
    }
}
