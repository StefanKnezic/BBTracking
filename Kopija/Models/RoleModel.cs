using System.ComponentModel.DataAnnotations;

namespace Kopija.Models
{
    public class RoleModel
    {
        [Required(ErrorMessage = "Potrebno je uneti nivo pristupa!")]
        public string Ime { get; set; }

       
    }
}
