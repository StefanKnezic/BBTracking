using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class oprema
    {

        [Key]
        public int oprema_id { get; set; }


        [Required(ErrorMessage = "Potrebno je uneti  marku opreme!")]
        [Display(Name = "Marka")]
        public string? oprema_marka { get; set; }

        [Required(ErrorMessage = "Potrebno je uneti  model opreme!")]
        [Display(Name = "Model")]
        public string? oprema_model { get; set; }

        [Required(ErrorMessage = "Potrebno je izabrati kategoriju opreme!")]
        [Display(Name = "Kategorija Opreme")]
        public int? oprema_kategorija_id { get; set; }

        [Required(ErrorMessage = "Potrebno je uneti  qr kod opreme!")]
        [RegularExpression("\\d{1,15}", ErrorMessage = "QR kod ne sme biti duzi od 15 karaktera,i mora sadrzati samo cifre!")]
        [Display(Name = "QR Kod")]
        public string? oprema_qr_kod { get; set; }



        [Display(Name = "Serijski Broj")]
        
        public string? oprema_serijski_broj { get; set; } 


        [DataType(DataType.Date)]
        [Display(Name = "Datum nabavke:")]
        public DateTime oprema_datum_nabavke { get; set; }



        [Display(Name = "Cena:")]
        public string? oprema_cena { get; set; } 



        [Display(Name = "razlog otpisa:")]
        public string? oprema_razlog_otpisa { get; set; } = null;


        [DataType(DataType.Date)]
        [Display(Name = "Garancija do:")]
        public DateTime oprema_garancija { get; set; } 

        [Required(ErrorMessage = "Potrebno je izabrati dobavljaca opreme!")]
        [Display(Name = "Dobavljac opreme")]
        public int? oprema_dostavljac_id { get; set; }


        public int oprema_otpisano { get; set; } = 0;

        public int oprema_stanje { get; set; } = 0;

        [Display(Name = "Napomena:")]
        public string? oprema_napomena { get; set; }



    }
}
