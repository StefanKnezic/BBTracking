using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class na_servis
    {
#nullable enable
        [Key]
        public int na_servis_id { get; set; }

        [Required(ErrorMessage = "Potrebno je izabrati opremu!")]
        [Display(Name = "Izaberite opremu:")]
        public int? na_servis_oprema_id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        public DateTime na_servis_datum { get; set; }

        [Required(ErrorMessage = "Potrebno je izabrati servis na koji saljete opremu!")]
        [Display(Name = "Izaberite servis:")]
        public int? na_servis_servis_id { get; set; }

        
        public int? na_servis_korisnik_id { get; set; } 


        public int na_servis_pokupljeno { get; set; } = 0;

        [Display(Name = "Napomena:")]
        public string? na_servis_napomena { get; set; } = null;


        [Display(Name = "Opis Kvara:")]
        public string? na_servis_opis_kvara_pre { get; set; } = null;

        public int? na_servis_pokupio_korisnik_id { get; set; } = null;

        [DataType(DataType.Date)]
        [Required]
        public DateTime? na_servis_datum_pokupljeno { get; set; } = null;

        [Required(ErrorMessage ="morate uneti opis kvara!")]
        [Display(Name ="Opis kvara")]
        public string? na_servis_opis_kvara { get; set; } = null;

        [Display(Name = "Napomena posle servisa:")]
        public string? na_servis_napomena_posle { get; set; } = null;

    }
}
