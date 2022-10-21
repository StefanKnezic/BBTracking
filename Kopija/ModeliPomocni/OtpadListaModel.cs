using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class OtpadListaModel
    {
        public int Id { get; set; }



        [Display(Name = "Marka")]
        public string Marka { get; set; }


        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "Kategorija Opreme")]
        public string? KategorijaIme { get; set; }

        [Display(Name = "QR Kod")]
        public string QRKod { get; set; }

        [Display(Name = "Cena")]
        public string Cena { get; set; }


        [Display(Name = "Ime Dobavljaca")]
        public string ImeDobavljaca { get; set; }

        [Display(Name = "Sektor Opreme")]
        public string Sektor { get; set; }

        public string Razlog { get; set; }
    }
}
