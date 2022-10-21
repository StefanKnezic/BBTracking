using Kopija.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Kopija.Modeli
{
    public class aspnetroles 
    {
        [Key]
        public int Id { get; set; }


        public string? Name { get; set; }
    }
}
