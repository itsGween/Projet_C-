using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetUa2_ServeursWeb.Data;
using ProjetUa2_ServeursWeb.Models;
using System.Linq;

namespace ProjetUa2_ServeursWeb.Controllers
{
    public class CommandeController : Controller
    {
        private readonly CommandeContext _context;

        public CommandeController(CommandeContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        //AFFICHAGE DU FORMULAIRE DE COMMANDE
        public IActionResult Commander()
        {
            ViewBag.Produits = new SelectList(_context.Produits, "ProduitId", "Nom"); 
            return View();
        }

        //TRAITEMENT DE LA SOUMISSION DU FORMULAIRE
        [HttpPost]
        public IActionResult Commander(Client client, int ProduitId, int Quantite)
        {
            if (ModelState.IsValid)
            {
                var produitExistant = _context.Produits.Find(ProduitId);
                if (produitExistant == null)
                {
                    ModelState.AddModelError("ProduitId", "Le produit sélectionné n'existe pas.");
                    ViewBag.Produits = new SelectList(_context.Produits, "ProduitId", "Nom");
                    return View(client);
                }

                
                _context.Clients.Add(client);
                _context.SaveChanges();

               
                var commande = new Commande
                {
                    ClientId = client.ClientId,
                    ProduitId = ProduitId,
                    Quantite = Quantite,
                    CommandeDate = DateTime.Now,
                    CommandeName = "Commande de " + produitExistant.Nom
                };

                _context.Commandes.Add(commande);
                _context.SaveChanges();

                
                return RedirectToAction("Confirmation", new { id = commande.CommandeId });
            }

            ViewBag.Produits = new SelectList(_context.Produits, "ProduitId", "Nom");
            return View(client);
        }

        //AFFICHAGE DE LA CONFIRMATION DE COMMANDE
        public IActionResult Confirmation(int id)
        {
            var commande = _context.Commandes
                .Include(c => c.Client) 
                .Include(c => c.Produit) 
                .FirstOrDefault(c => c.CommandeId == id);

            if (commande == null)
            {
                return NotFound(); 
            }

            return View(commande); 
        }

        //AFFICHAGE DE L'HISTORIQUE DES COMMANDES PASSEES
        public IActionResult Historique()
        {
            var commandes = _context.Commandes
                .Include(c => c.Client) 
                .Include(c => c.Produit) 
                .OrderBy(c => c.CommandeDate) 
                .ToList();

            return View(commandes); 
        }

    }
}

