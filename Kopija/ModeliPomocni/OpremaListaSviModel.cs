using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class OpremaListaSviModel
    {
        public int Id { get; set; }



        [Display(Name = "Marka:")]
        public string? Marka { get; set; }


        [Display(Name = "Model:")]
        public string? Model { get; set; }

        [Display(Name = "Kategorija Opreme:")]
        public string? KategorijaIme { get; set; }

        [Display(Name = "QR Kod:")]
        public string? QRKod { get; set; }

        [Display(Name = "Serijski Broj:")]
        public string? SerijskiBroj { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Datum Nabavke(mm/dd/yyyy):")]
        public DateTime DatumNabavke { get; set; }


        [Display(Name = "Cena:")]
        public string? Cena { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Garancija do(mm/dd/yyyy):")]
        public DateTime Garancija { get; set; }

        [Display(Name = "Ime Dobavljaca:")]
        public string? ImeDobavljaca { get; set; }


        [Display(Name = "Sektor:")]
        public string? Sektor { get; set; }

        [Display(Name = "Napomena:")]
        public string? Napomena { get; set; }
    }
}
