using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class NaServisCreateModel
    {



        [Required(ErrorMessage = "Potrebno je izabrati opremu!")]
        [Display(Name = "Izaberite opremu:")]
        public int? na_servis_oprema_id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        public DateTime na_servis_datum { get; set; }

        [Required(ErrorMessage = "Potrebno je izabrati servis na koji saljete opremu!")]
        [Display(Name = "Izaberite servis:")]
        public int? na_servis_servis_id { get; set; }

        [Display(Name = "Napomena:")]
        public string? na_servis_napomena { get; set; }

        [Display(Name = "Opis Kvara Pre:")]
        public string? na_servis_opis_kvara_pre { get; set; }

    }
}
