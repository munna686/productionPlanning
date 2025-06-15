using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductionPlanning.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Infrastructure.DbContext
{
    public class InventoryDataContext : IdentityDbContext<ApplicationUser>
    {
        public InventoryDataContext() { }
        public InventoryDataContext(DbContextOptions<InventoryDataContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<BomDetail> BomDetails { get; set; }
        public DbSet<BomMaster> BomMasters { get; set; }
        public DbSet<BomLog> BomLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=PPDB;User ID=sa;Password=68662;TrustServerCertificate=true;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BomDetail>()
                .HasOne(d => d.BomMaster)
                .WithMany(m => m.BomDetails)
                .HasForeignKey(d => d.BomId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
