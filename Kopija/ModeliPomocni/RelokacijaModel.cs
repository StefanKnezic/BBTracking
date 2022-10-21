using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class RelokacijaModel
    {
        [Required(ErrorMessage = "Morate izabrati opremu!")]
        [Display(Name = "Izaberite opremu:")]
        public int OpremaId { get; set; }
        [Display(Name = "Izaberite kome zelite relocirati opremu:")]
        public int DokleId { get; set; }

        [Display(Name = "Izaberite kom objektu zelite relocirati opremu:")]
        public int DokleLokacijaId { get; set; }

        [Display(Name = "Unesite ime i prezime kome zelite relocirati opremu:")]
        public string? DoKoga { get; set; }

        [Display(Name = "Napomena(nije obavezno):")]
        public string? Napomena { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum relokacije:")]
        public DateTime Datum { get; set; }

    }
}
