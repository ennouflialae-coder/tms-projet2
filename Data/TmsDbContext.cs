using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Models;

namespace TMS_Project.Data
{
    public class TmsDbContext : IdentityDbContext<IdentityUser>
    {
        public TmsDbContext(DbContextOptions<TmsDbContext> options) : base(options)
        {
        }

        public DbSet<Transporteur> Transporteurs { get; set; }
        public DbSet<Camion> Camions { get; set; }
        public DbSet<Chauffeur> Chauffeurs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Tournee> Tournees { get; set; }
        public DbSet<Livraison> Livraisons { get; set; }
        public DbSet<Cout> Couts { get; set; }
        public DbSet<DemandeLivraison> DemandesLivraison { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transporteur>().ToTable("Transporteur");
            modelBuilder.Entity<Camion>().ToTable("Camion");
            modelBuilder.Entity<Chauffeur>().ToTable("Chauffeur");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Tournee>().ToTable("Tournee");
            modelBuilder.Entity<Livraison>().ToTable("Livraison");
            modelBuilder.Entity<Cout>().ToTable("Cout");
            modelBuilder.Entity<DemandeLivraison>().ToTable("DemandeLivraison");

            modelBuilder.Entity<Transporteur>().HasKey(t => t.TransporteurId);
            modelBuilder.Entity<Transporteur>().Property(t => t.Nom).IsRequired().HasMaxLength(255);

            modelBuilder.Entity<Camion>().HasKey(c => c.CamionId);
            modelBuilder.Entity<Camion>().HasOne(c => c.Transporteur).WithMany(t => t.Camions)
                .HasForeignKey(c => c.TransporteurId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chauffeur>().HasKey(ch => ch.ChauffeurId);
            modelBuilder.Entity<Chauffeur>().HasOne(ch => ch.Transporteur).WithMany(t => t.Chauffeurs)
                .HasForeignKey(ch => ch.TransporteurId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>().HasKey(cl => cl.ClientId);

            modelBuilder.Entity<DemandeLivraison>().HasKey(d => d.DemandeLivraisonId);
            modelBuilder.Entity<DemandeLivraison>().Property(d => d.MontantEstime).HasPrecision(10, 2);

            modelBuilder.Entity<Tournee>().HasKey(to => to.TourneeId);
            modelBuilder.Entity<Tournee>().HasOne(to => to.Transporteur).WithMany(t => t.Tournees)
                .HasForeignKey(to => to.TransporteurId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Tournee>().HasOne(to => to.Camion).WithMany(c => c.Tournees)
                .HasForeignKey(to => to.CamionId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Tournee>().HasOne(to => to.Chauffeur).WithMany(ch => ch.Tournees)
                .HasForeignKey(to => to.ChauffeurId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Livraison>().HasKey(l => l.LivraisonId);
            modelBuilder.Entity<Livraison>().Property(l => l.MontantEstime).HasPrecision(10, 2);
            modelBuilder.Entity<Livraison>().HasOne(l => l.Tournee).WithMany(to => to.Livraisons)
                .HasForeignKey(l => l.TourneeId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cout>().HasKey(co => co.CoutId);
            modelBuilder.Entity<Cout>().Property(co => co.Montant).HasPrecision(10, 2);
        }
    }
}
