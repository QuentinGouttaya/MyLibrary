using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OumouClient
{
    class Oumou
    {
        static readonly HttpClient client = new HttpClient();
        static string apiUrl = "http://localhost:5290/livres";

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Client console Oumou pour la gestion des livres ===\n");

            bool continuer = true;

            while (continuer)
            {
                Console.WriteLine("\nMenu :");
                Console.WriteLine("1. Afficher la liste des livres");
                Console.WriteLine("2. Rechercher un livre par titre ou auteur");
                Console.WriteLine("3. Ajouter un nouveau livre");
                Console.WriteLine("4. Modifier un livre");
                Console.WriteLine("5. Supprimer un livre");
                Console.WriteLine("6. Quitter");
                Console.Write("Votre choix : ");

                string? choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        await AfficherLivres();
                        break;
                    case "2":
                        await RechercherLivre();
                        break;
                    case "3":
                        await AjouterLivre();
                        break;
                    case "4":
                        await ModifierLivre();
                        break;
                    case "5":
                        await SupprimerLivre();
                        break;
                    case "6":
                        continuer = false;
                        Console.WriteLine("Au revoir !");
                        break;
                    default:
                        Console.WriteLine("Choix invalide, veuillez réessayer.");
                        break;
                }
            }
        }

        static async Task AfficherLivres()
        {
            try
            {
                var livres = await client.GetFromJsonAsync<List<Media>>(apiUrl);
                if (livres is null || livres.Count == 0)
                {
                    Console.WriteLine("Aucun livre disponible.");
                    return;
                }

                Console.WriteLine("\nListe des livres :");
                foreach (var livre in livres)
                {
                    Console.WriteLine($"ID: {livre.Id} | Titre: {livre.Title} | Auteur: {livre.Author}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des livres : {ex.Message}");
            }
        }

        static async Task RechercherLivre()
        {
            Console.Write("Entrez un mot-clé (titre ou auteur) : ");
            string? recherche = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(recherche))
            {
                Console.WriteLine("Recherche vide, veuillez saisir un mot-clé.");
                return;
            }

            try
            {
                var livres = await client.GetFromJsonAsync<List<Media>>(apiUrl);
                var resultats = livres?.FindAll(l =>
                    (l.Title?.ToLower().Contains(recherche) ?? false) ||
                    (l.Author?.ToLower().Contains(recherche) ?? false)
                );

                if (resultats is null || resultats.Count == 0)
                {
                    Console.WriteLine("Aucun livre trouvé pour cette recherche.");
                }
                else
                {
                    Console.WriteLine($"\n{resultats.Count} livre(s) trouvé(s) :");
                    foreach (var livre in resultats)
                    {
                        Console.WriteLine($"ID: {livre.Id} | Titre: {livre.Title} | Auteur: {livre.Author}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la recherche : {ex.Message}");
            }
        }

        static async Task AjouterLivre()
        {
            Console.Write("Titre du nouveau livre : ");
            string? titre = Console.ReadLine()?.Trim();
            Console.Write("Auteur du nouveau livre : ");
            string? auteur = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(titre) || string.IsNullOrWhiteSpace(auteur))
            {
                Console.WriteLine("Titre et auteur sont obligatoires.");
                return;
            }

            var nouveauLivre = new Media { Title = titre, Author = auteur };

            try
            {
                var response = await client.PostAsJsonAsync(apiUrl, nouveauLivre);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Livre ajouté avec succès !");
                else
                    Console.WriteLine($"Erreur lors de l'ajout (code {response.StatusCode}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout : {ex.Message}");
            }
        }

        static async Task ModifierLivre()
        {
            Console.Write("ID du livre à modifier : ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalide.");
                return;
            }

            Console.Write("Nouveau titre : ");
            string? titre = Console.ReadLine()?.Trim();
            Console.Write("Nouvel auteur : ");
            string? auteur = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(titre) || string.IsNullOrWhiteSpace(auteur))
            {
                Console.WriteLine("Titre et auteur sont obligatoires.");
                return;
            }

            var livreModifie = new Media { Id = id, Title = titre, Author = auteur };

            try
            {
                var response = await client.PutAsJsonAsync($"{apiUrl}/{id}", livreModifie);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Livre modifié avec succès !");
                else
                    Console.WriteLine($"Erreur lors de la modification (code {response.StatusCode}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification : {ex.Message}");
            }
        }

        static async Task SupprimerLivre()
        {
            Console.Write("ID du livre à supprimer : ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalide.");
                return;
            }

            try
            {
                var response = await client.DeleteAsync($"{apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Livre supprimé avec succès !");
                else
                    Console.WriteLine($"Erreur lors de la suppression (code {response.StatusCode}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression : {ex.Message}");
            }
        }
    }

    public class Media
    {
        public int Id { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
    }
}
