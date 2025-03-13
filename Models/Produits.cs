using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetUa2_ServeursWeb.Models
{
    public class Produit
    {
        [Key]
        public int ProduitId { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0.01, 100000, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public decimal Prix { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Le stock est obligatoire.")]
        public int Stock { get; set; }

        public string ImageUrl { get; set; } = "/images/default.jpg"; 

        [NotMapped] 
        public IFormFile ImageFile { get; set; }


    }
}
