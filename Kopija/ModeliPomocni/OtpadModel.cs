using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Kopija.ModeliPomocni
{
    public class OtpadModel
    {

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Morate uneti razlog otpada!")]
        [Display(Name = "Razlog Otpada")]
        public string RazlogOtpada { get; set; }
    }
}
