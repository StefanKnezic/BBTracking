using Kopija.Modeli;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class NaServisUpdate
    {

        [DataType(DataType.Date)]
        [Display(Name = "Izaberite datum kada ste pokupili sa servisa:")]
        public DateTime? PokupljenoDatum { get; set; } = null;

        [Display(Name ="opisite opis kvara koji je bio(opciono)")]
        public string? OpisKvara { get; set; } = null;

        [Display(Name = "Napomena(opciono)")]
        public string? Napomena { get; set; } = null;


    }
}
