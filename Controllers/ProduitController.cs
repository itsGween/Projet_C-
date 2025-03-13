using Microsoft.AspNetCore.Mvc;
using ProjetUa2_ServeursWeb.Data;
using ProjetUa2_ServeursWeb.Models;
using System.Linq;

namespace ProjetUa2_ServeursWeb.Controllers
{
    public class ProduitController : Controller
    {
        private readonly CommandeContext _context;

        public ProduitController(CommandeContext context)
        {
            _context = context;
        }

        // LISTE DES PRODUITS
        public IActionResult Listes()
        {
            var produits = _context.Produits.ToList();
            return View("Listes", produits); 
        }

        // DÉTAILS D'UN PRODUIT
        public IActionResult Details(int id)
        {
            var produit = _context.Produits.FirstOrDefault(p => p.ProduitId == id);
            if (produit == null) return NotFound();
            return View(produit);
        }

        // FORMULAIRE DE CRÉATION D'UN PRODUIT
        public IActionResult Creer()
        {
            return View();
        }

        // TRAITEMENT DE L'AJOUT D'UN PRODUIT
        [HttpPost]
        public IActionResult Creer(Produit produit, IFormFile ImageFile)
        {
            Console.WriteLine("📌 Tentative d'ajout du produit...");

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = Path.GetFileName(ImageFile.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                produit.ImageUrl = "/images/" + fileName;
            }
            else
            {
                Console.WriteLine("Aucun fichier image envoyé !");
                produit.ImageUrl = "/images/default.jpg"; 
            }

            ModelState.Clear(); 

            if (ModelState.IsValid)
            {
                _context.Produits.Add(produit);
                _context.SaveChanges();
                Console.WriteLine("Produit ajouté avec succès !");
                return RedirectToAction("Listes");
            }
            else
            {
                Console.WriteLine("Model invalide !");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("Erreur : " + error.ErrorMessage);
                    }
                }
            }

            return View(produit);
        }

        // FORMULAIRE DE MODIFICATION D'UN PRODUIT (GET)
        public IActionResult Modifier(int id)
        {
            var produit = _context.Produits.Find(id);
            if (produit == null) return NotFound();
            return View("Modifier", produit); 
        }

        // TRAITEMENT DE LA MODIFICATION D'UN PRODUIT (POST)
        [HttpPost]
        public IActionResult ModifierPost(Produit produit, IFormFile ImageFile)
        {
            Console.WriteLine("📌 Tentative de modification du produit...");

            var produitExistant = _context.Produits.Find(produit.ProduitId);
            if (produitExistant == null) return NotFound();

            produitExistant.Nom = produit.Nom;
            produitExistant.Prix = produit.Prix;
            produitExistant.Stock = produit.Stock;
            produitExistant.Description = produit.Description;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = Path.GetFileName(ImageFile.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                produitExistant.ImageUrl = "/images/" + fileName; 
            }

            
            _context.Produits.Update(produitExistant);
            _context.SaveChanges();

            Console.WriteLine("✅ Modification réussie !");
            return RedirectToAction("Listes"); 
        }


        // TRAITEMENT DE SUPPRESSION D'UN PRODUIT
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var produit = _context.Produits.Find(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
                _context.SaveChanges();
            }
            return RedirectToAction("Listes");
        }


        

    }
}
