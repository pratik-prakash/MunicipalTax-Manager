using Microsoft.EntityFrameworkCore;
using MunicipalTax.Entity;

namespace MunicipalTax.DAL
{
    public class MunicipalTaxContext : DbContext
    {
        public DbSet<MunicipalityTax> MunicipalityTaxes { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
               .UseSqlite(@"Data Source=TaxManagerDB.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<MunicipalityTax>().ToTable("MunicipalityTaxes");
        }
    }
}
