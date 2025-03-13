using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetUa2_ServeursWeb.Models
{
    public class Commande
    {
        [Key]
        public int CommandeId { get; set; }
        [Required]
        public string CommandeName { get; set; }
        [Required]
        public DateTime CommandeDate { get; set;}

        [Required]
        public int Quantite { get; set; }

        // Cle etrangere
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public int ProduitId { get; set; }

        [ForeignKey("ProduitId")]
        public Produit Produit { get; set; }



    }
}
