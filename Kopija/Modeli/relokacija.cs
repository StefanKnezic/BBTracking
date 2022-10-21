using System.ComponentModel.DataAnnotations;
namespace Kopija.Modeli
{
    public class relokacija
    {
        [Key]
        public int relokacija_id { get; set; }

        public int? relokacija_korisnik_od_koga_id { get; set; } = null;
        public int? relokacija_korisnik_do_koga_id { get; set; } = null;
        public int? relokacija_od_lokacija_id { get; set; } = null;
        public int? relokacija_do_lokacija_id { get; set; } = null;
        public int? relokacija_oprema_id { get; set; }

        [DataType(DataType.Date)]
        public DateTime relokacija_datum { get; set; }

        public string? relokacija_napomena { get; set; }

        public int? relokacija_do_koga { get; set; }
    }
}
