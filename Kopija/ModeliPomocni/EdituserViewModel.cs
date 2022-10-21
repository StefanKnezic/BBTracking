using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Kopija.ModeliPomocni
{
    public class EdituserViewModel
    {
        public int Id { get; set; }


        [Display(Name = "Ime:")]
        [Required]
        public string? Ime { get; set; }

        [Display(Name = "Prezime:")]
        [Required]
        public string? Prezime { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Potrebno je uneti  broj telefona radnika!")]
        [Display(Name = "Broj telefona")]
        [RegularExpression(@"^(\+381)?(\s|-)?6(([0-6]|[8-9])\d{7}|(77|78)\d{6}){1}$", ErrorMessage = "broj telefona nije u zeljenom formatu,,+381-61,+381 61,+38161...")]
        public string? BrojTelefona  { get; set; }


        [Display(Name = "Radno mesto:")]
        [Required]
        public string? RadnoMesto { get; set; }


        [Display(Name ="Korisnicko ime:")]
        [Required]
        public string? UserName { get; set; }
    }
}
