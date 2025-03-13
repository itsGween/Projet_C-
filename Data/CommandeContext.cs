using Microsoft.EntityFrameworkCore;
using ProjetUa2_ServeursWeb.Models;

namespace ProjetUa2_ServeursWeb.Data
{
    public class CommandeContext : DbContext
    {
        public CommandeContext(DbContextOptions<CommandeContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Produit> Produits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Relation entre Commande et Client
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Client)
                .WithMany()
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade); // Si un client est supprimé, toutes ses commandes sont supprimées

            // 🔹 Relation entre Commande et Produit
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Produit)
                .WithMany()
                .HasForeignKey(c => c.ProduitId)
                .OnDelete(DeleteBehavior.Restrict); // Empêche la suppression d'un produit s'il est lié à une commande
        }
    }
}
