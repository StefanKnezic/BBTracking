using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class KorisnikModel
    {

        [Required(ErrorMessage = "Potrebno je uneti  ime!")]
        [Display(Name = "Ime")]
        public string? Ime { get; set; }


        [Required(ErrorMessage = "Potrebno je uneti  prezime!")]
        [Display(Name = "Prezime")]
        public string? Prezime { get; set; }

        [Required(ErrorMessage = "Potrebno je uneti  broj telefona radnika!")]
        [Display(Name = "Broj telefona")]
        [RegularExpression(@"^(\+381)?(\s|-)?6(([0-6]|[8-9])\d{7}|(77|78)\d{6}){1}$", ErrorMessage = "broj telefona nije u zeljenom formatu,,+381-61,+381 61,+38161...")] //mozda promeniti nekad
        public string? BrojTelefona { get; set; }

        [Required(ErrorMessage = "Potrebno je izabrati sektor !")]
        [Display(Name = "Sektor")]
        public int SektorId { get; set; }

       [Required(ErrorMessage = "Potrebno je uneti  radno mesto radnika!")]
        [Display(Name = "radno Mesto")]
        public string? RadnoMesto { get; set; }

        [Required(ErrorMessage = "Potrebno je izabrati lokaciju radnika !")]
        [Display(Name = "Lokacija")]
        public int LokacijaId { get; set; }

       public int OtpisanRadnik { get; set; } = 0;


        [Required(ErrorMessage = "Potrebno je uneti  ime!")]
        [Display(Name = "Korisnicko ime")]
        public string? KorisnickoIme { get; set; }

       [Required(ErrorMessage = "Potrebno je uneti  sifru!")]                                            
        [Display(Name = "Sifra")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Sifra mora da bude izmedju 8 i 20 karaktera,Da sadrzi minimalno jedno veliko i malo slovo,jedan broj i jedan specijalni karakter")]
        public string? Sifra { get; set; }

        [Required(ErrorMessage = "Potrebno je izabrati nivo pristupa!")] 
        [Display(Name = "Nivo pristupa")]
        public string? NivoPristupa { get; set; }


    }
}
