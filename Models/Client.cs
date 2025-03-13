using System.ComponentModel.DataAnnotations;

namespace ProjetUa2_ServeursWeb.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        [Required]
        public string ClientNom { get; set; }
        [Required]
        public string ClientPrenom {  get; set; }
        [Required]
        public string ClientTelephone { get; set; }
        [Required]  
        public string addresseClient { get; set; }

    }
}
