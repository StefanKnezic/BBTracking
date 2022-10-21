using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class OpremaKategorijaListaModel
    {


        [Display(Name ="Kategorija opreme")]
        public string? oprema_kategorija_ime { get; set; }

        [Display(Name = "Ime Sektora")]
        public string? oprema_kategorija_sektor_iime { get; set; }
    }
}
