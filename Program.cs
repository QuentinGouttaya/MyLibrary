using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

// Modèle correspondant à l'API
record MediaDto(int Id, string Title, string Author);
record CreateMediaDto(string Title, string Author);
record UpdateMediaDto(string Title, string Author);

public class Media
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
}

class Program
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5290") };

    static async Task Main(string[] args)
    {
        bool exit = false;
        string url = "http://localhost:5290/livres";

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== Gestion des Livres ===");
            Console.WriteLine("1. Lister tous les livres");
            Console.WriteLine("2. Rechercher des livres");
            Console.WriteLine("3. Ajouter un livre");
            Console.WriteLine("4. Modifier un livre");
            Console.WriteLine("5. Supprimer un livre");
            Console.WriteLine("6. Quitter");
            Console.Write("Choisissez une option : ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await GetAllLivresAsync();
                        break;
                    case "2":
                        await SearchLivresAsync();
                        break;
                    case "3":
                        await AddLivreAsync();
                        break;
                    case "4":
                        Console.Write("Entrez l'ID du livre à mettre à jour : ");
                        string updateId = Console.ReadLine();
                        await UpdateLivreAsync(url, updateId);
                        break;

                    case "5":
                        await DeleteLivreAsync();
                        break;
                    case "6":
                        exit = true;
                        Console.WriteLine("Au revoir !");
                        break;
                    default:
                        Console.WriteLine("Option invalide.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur : {ex.Message}");
            }

            if (!exit)
            {
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
    }

    // GET /livres
    static async Task GetAllLivresAsync()
    {
        var response = await client.GetFromJsonAsync<MediaDto[]>("livres");
        if (response == null || response.Length == 0)
        {
            Console.WriteLine("Aucun livre trouvé.");
            return;
        }

        foreach (var livre in response)
        {
            Console.WriteLine($"ID: {livre.Id}, Titre: {livre.Title}, Auteur: {livre.Author}");
        }
    }

    // GET /livres/filter?start=xxx&end=yyy
    static async Task SearchLivresAsync()
    {
        Console.Write("Recherche par début de mot (facultatif) : ");
        var start = Console.ReadLine() ?? string.Empty;

        Console.Write("Recherche par fin de mot (facultatif) : ");
        var end = Console.ReadLine() ?? string.Empty;

        var url = $"livres/filter?start={Uri.EscapeDataString(start)}&end={Uri.EscapeDataString(end)}";
        var response = await client.GetFromJsonAsync<MediaDto[]>(url);

        if (response == null || response.Length == 0)
        {
            Console.WriteLine("Aucun livre trouvé.");
            return;
        }

        foreach (var livre in response)
        {
            Console.WriteLine($"ID: {livre.Id}, Titre: {livre.Title}, Auteur: {livre.Author}");
        }
    }

    // POST /livres
    static async Task AddLivreAsync()
    {
        Console.Write("Titre : ");
        var title = Console.ReadLine() ?? string.Empty;

        Console.Write("Auteur : ");
        var author = Console.ReadLine() ?? string.Empty;

        var dto = new CreateMediaDto(title, author);

        var response = await client.PostAsJsonAsync("livres", dto);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("✅ Livre ajouté !");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"❌ Erreur : {error}");
        }
    }

    // PUT /livres/{id}
    // PUT /livres/{id}

static async Task UpdateLivreAsync(string url, string id)
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{url}/{id}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound )
            {
                Console.WriteLine("Livre non trouvé.");
            } else {
                Console.WriteLine("Livre trouvé :");
                Console.WriteLine(responseBody);

                Console.Write("Entrez le nouveau titre du livre : ");
                string newTitle = Console.ReadLine();
                Console.Write("Entrez le nouvel auteur : ");
                string newAuthor = Console.ReadLine();
                var content = new StringContent(
                    $"{{\"id\":\"{id}\",\"title\":\"{newTitle}\", \"author\":\"{newAuthor}\"}}", 
                    System.Text.Encoding.UTF8, 
                    "application/json");

                var updateResponse = await client.PutAsync($"{url}/{id}", content);

                string updateResponseBody = await updateResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Livre mis à jour :");
                Console.WriteLine(updateResponseBody);
            }
        }

    // DELETE /livres/{id}
    static async Task DeleteLivreAsync()
    {
        Console.Write("ID du livre à supprimer : ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ ID invalide.");
            return;
        }

        var response = await client.DeleteAsync($"livres/{id}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("✅ Livre supprimé !");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"❌ Erreur : {error}");
        }
    }
}