using ManagerTruck.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManagerTruck.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CaminhaoEntity> Caminhoes { get; set; }
        public DbSet<MontadoraEntity> Montadoras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.Property(r => r.NormalizedName).HasMaxLength(128);
            });

            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.Property(u => u.NormalizedUserName).HasMaxLength(128);
                entity.Property(u => u.NormalizedEmail).HasMaxLength(128);
            });


            modelBuilder.Entity<CaminhaoEntity>()
                .Property(e => e.DataCriacao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<MontadoraEntity>().HasData(
                new MontadoraEntity { Id = 1, Nome = "Volvo" },
                new MontadoraEntity { Id = 2, Nome = "Scania" },
                new MontadoraEntity { Id = 3, Nome = "Mercedes-Benz" },
                new MontadoraEntity { Id = 4, Nome = "DAF" },
                new MontadoraEntity { Id = 5, Nome = "Iveco" },
                new MontadoraEntity { Id = 6, Nome = "Volkswagen" },
                new MontadoraEntity { Id = 7, Nome = "Ford" },
                new MontadoraEntity { Id = 8, Nome = "MAN" },
                new MontadoraEntity { Id = 9, Nome = "Hyundai" },
                new MontadoraEntity { Id = 10, Nome = "Kia" },
                new MontadoraEntity { Id = 11, Nome = "Isuzu" },
                new MontadoraEntity { Id = 12, Nome = "Foton" },
                new MontadoraEntity { Id = 13, Nome = "JAC Motors" },
                new MontadoraEntity { Id = 14, Nome = "Agrale" },
                new MontadoraEntity { Id = 15, Nome = "International" },
                new MontadoraEntity { Id = 16, Nome = "Peterbilt" },
                new MontadoraEntity { Id = 17, Nome = "Kenworth" },
                new MontadoraEntity { Id = 18, Nome = "Freightliner" },
                new MontadoraEntity { Id = 19, Nome = "Mack" },
                new MontadoraEntity { Id = 20, Nome = "Renault Trucks" },
                new MontadoraEntity { Id = 21, Nome = "Hino" },
                new MontadoraEntity { Id = 22, Nome = "Tatra" },
                new MontadoraEntity { Id = 23, Nome = "Sinotruk" },
                new MontadoraEntity { Id = 24, Nome = "Shacman" },
                new MontadoraEntity { Id = 25, Nome = "Dongfeng" }
            );
        }
    }
}
