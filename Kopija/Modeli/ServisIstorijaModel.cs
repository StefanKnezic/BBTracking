using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class ServisIstorijaModel
    {
        public int Id { get; set; }


        [Display(Name = "Marka opreme:")]
        public string? Marka { get; set; }


        [Display(Name = "Model opreme:")]
        public string? Model { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum predaje na servis:")]
        public DateTime Datum { get; set; }


        [Display(Name = "Predao:")]
        public string? UserName { get; set; }

        [Display(Name = "Serviser:")]
        public string? Servis { get; set; }


        [Display(Name = "Sektor opreme:")]
        public string? Sektor { get; set; }

        [Display(Name = "Kategorija opreme:")]
        public string? Kategorija { get; set; }

        [Display(Name = "Napomena:")]
        public string? Napomena { get; set; }

        [Display(Name = "Opis kvara pre:")]
        public string? OpisKvarapre { get; set; }

        [Display(Name = "Pokupio:")]
        public string? PokupioKorisnickoIme { get; set; }

        [Display(Name = "Pokupio datum:")]
        [DataType(DataType.Date)]
        public DateTime Datumpokupio { get; set; }

        [Display(Name = "Opis Kvara posle servisa:")]
        public string? OpisKvara { get; set; }

        [Display(Name = "Napomena posle servisa:")]
        public string? NapomenaPosle { get; set; }

        [Display(Name = "QR Kod:")]
        public string? QR { get; set; }

    }
}
