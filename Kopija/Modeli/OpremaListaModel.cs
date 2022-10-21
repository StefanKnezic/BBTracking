using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class OpremaListaModel
    {
   
        public int Id { get; set; }


  
        [Display(Name = "Marka:")]
        public string? Marka { get; set; }


        [Display(Name = "Model:")]
        public string? Model { get; set; }

        [Display(Name = "Kategorija:")]
        public string? KategorijaIme { get; set; }

        [Display(Name = "QR Kod:")]
        public string? QRKod { get; set; }

        [Display(Name = "Serijski Broj:")]
        public string? SerijskiBroj { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Datum Nabavke:")]
        public DateTime DatumNabavke { get; set; }


        [Display(Name = "Cena:")]
        public string? Cena { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Garancija do:")]
        public DateTime Garancija { get; set; }

        [Display(Name = "Dobavljac:")]
        public string? ImeDobavljaca { get; set; }

        [Display(Name = "Sektor:")]
        public string? Sektor { get; set; }

        [Display(Name = "Napomena:")]
        public string? Napomena { get; set; }

    }
}
