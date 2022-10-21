using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kopija.Models
{
    [Table("aspnetusers")]
    public class AppUser : IdentityUser<int>
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public int SektorId { get; set; }
        public string RadnoMesto { get; set; }
        public int LokacijaId { get; set; }
        public int OtpisanRadnik { get; set; } = 0;

       


    }
}
