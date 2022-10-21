using System.ComponentModel.DataAnnotations;

namespace Kopija.Models
{
    public class LoginModel
    {
        
        [Required(ErrorMessage = "Potrebno je uneti korisnicko ime!")]
        [Display(Name ="Korisnicko ime")]
        public string? UserName { get; set; }

        [Required(ErrorMessage ="Potrebno je uneti sifru!")]
        [Display(Name ="Sifra")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }    
    }
}
