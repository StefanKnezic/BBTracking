using Kopija.Modeli;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace Kopija.Models
{
    public class IdentityAppContext : IdentityDbContext<AppUser,AppRole,int> 
    {
        public IdentityAppContext(DbContextOptions<IdentityAppContext> options): base(options)
        {
            
        }

        
        public DbSet<servis> servis { get; set; }
        public DbSet<sektor> sektor { get; set; }

        public DbSet<oprema> oprema { get; set; }

        public DbSet<relokacija> relokacija { get; set; }

        public DbSet<na_servis> na_servis { get; set; }
        public DbSet<oprema_kategorija> oprema_kategorija { get; set; }
        
        public DbSet<lokacija> lokacija { get; set; }

        public DbSet<dostavljac> dostavljac { get; set; }  
        public DbSet<aspnetroles> aspnetroles { get; set; }

        
        public DbSet<AppUser> aspnetusers { get; set; }

        
      

    }
}
