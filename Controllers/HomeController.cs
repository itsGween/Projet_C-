using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetUa2_ServeursWeb.Models;
using ProjetUa2_ServeursWeb.Data;
using System;
using System.Diagnostics;
using System.Linq;

namespace ProjetUa2_ServeursWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly CommandeContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(CommandeContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //AFFICHAGE DE LA PAGE D'ACCUEIL AVEC LES PRODUITS DE LA BASE DE DONNEE
        public IActionResult Index()
        {
            var produits = _context.Produits.ToList(); 
            return View(produits); 
        }


        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult Commander()
        {
            return View("~/Views/Commande/Commander.cshtml");
        }


        [HttpPost]
        public IActionResult Commander(Client client)
        {
            if (ModelState.IsValid)
            {
                
                _context.Clients.Add(client);
                _context.SaveChanges();

                
                var savedClient = _context.Clients.FirstOrDefault(c => c.ClientNom == client.ClientNom && c.ClientPrenom == client.ClientPrenom);

                if (savedClient == null)
                {
                    return View(client); 
                }

                
                Commande commande = new Commande
                {
                    ClientId = savedClient.ClientId,
                    CommandeName = "Produit commandé",
                    CommandeDate = DateTime.Now
                };

                _context.Commandes.Add(commande);
                _context.SaveChanges();

                
                return RedirectToAction("Confirmation", new { id = commande.CommandeId });
            }

            return View(client);
        }

        public IActionResult Confirmation(int id)
        {
            var commande = _context.Commandes
                .Include(c => c.Client)
                .FirstOrDefault(c => c.CommandeId == id);

            if (commande == null)
            {
                return NotFound();
            }

            ViewData["CommandeID"] = commande.CommandeId;
            ViewData["Description"] = commande.CommandeName;
            ViewData["CommandeDate"] = commande.CommandeDate;
            ViewData["Prenom"] = commande.Client.ClientPrenom;
            ViewData["Nom"] = commande.Client.ClientNom;

            return View("~/Views/Commande/Confirmation.cshtml");
        }

        //RECUPERE ET AFFICHE LA LISTE DES PRODUITS DEPUIS LA BASE DE DONNEE
        public IActionResult Listes()
        {
            var produits = _context.Produits.ToList(); 
            return View(produits);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
