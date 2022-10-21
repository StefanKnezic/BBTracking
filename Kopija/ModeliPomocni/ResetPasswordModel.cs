using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class ResetPasswordModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Potrebno je uneti Sifru!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Sifra mora da bude izmedju 8 i 20 karaktera,Da sadrzi minimalno jedno veliko i malo slovo,jedan broj i jedan specijalni karakter")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage ="Potrebno je potvrditi Sifru!")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Sifre se ne poklapaju!")]
        public string? ConfirmPassword{ get; set; }
    }
}
