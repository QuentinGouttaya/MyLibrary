using System;
using System.Net.Http;
using System.Threading.Tasks;

public class Media 
{   public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

class Program
{
    static async Task Main()
    {
        string url = "http://localhost:5290/livres";
        bool quit = false;
        
        // Boucle qui permet d'intéragir avec l'utilisateur
        while (!quit) {

        Console.WriteLine("Bonjour sur la vue de Sirine !");
        Console.WriteLine("Appuyez sur une des numéros pour requéter l'API");
        Console.WriteLine("1. Obtenir tous les livres");
        Console.WriteLine("2. Obtenir un livre par ID"); 
        Console.WriteLine("3. Ajouter un livre");
        Console.WriteLine("4. Mettre à jour un livre");
        Console.WriteLine("5. Supprimer un livre");
        Console.WriteLine("q. Sortir");
        Console.Write("Votre choix : ");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await GetAllAsync(url);
                break;
            case "2":
                Console.Write("Entrez l'ID du livre : ");
                string id = Console.ReadLine();
                await GetByIdAsync(url, id);
                break;
            case "3":
                Console.Write("Entrez le titre du livre : ");
                string title = Console.ReadLine();
                Console.Write("Entrez l'auteur du livre : ");
                string author = Console.ReadLine();
                await AddAsync(url, title, author);
                break;
            case "4":
                Console.Write("Entrez l'ID du livre à mettre à jour : ");
                string updateId = Console.ReadLine();
                await UpdateAsync(url, updateId);
                break;
            case "5":
                Console.Write("Entrez l'ID du livre à supprimer : ");
                string deleteId = Console.ReadLine();
                await DeleteAsync(url, deleteId);
                break;
            case "q":
                Console.WriteLine("Au revoir !");
                quit = true;
                break;
            default:
                Console.WriteLine("Choix invalide.");
                break;
        }
            

        static async Task GetAllAsync(string url) {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Réponse de l'API :");
            Console.WriteLine(responseBody);
        }
        
        static async Task GetByIdAsync(string url, string id)
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{url}/{id}");

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Réponse de l'API :");
            Console.WriteLine(responseBody);
        }
        static async Task AddAsync(string url, string title, string author)
        {
            using HttpClient client = new HttpClient();
            var content = new StringContent($"{{\"title\":\"{title}\", \"author\":\"{author}\"}}", System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            // var response = await client.PostAsync(url, content);

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Livre ajouté :");
            Console.WriteLine(responseBody);
        }
        static async Task UpdateAsync(string url, string id)
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
        async Task DeleteAsync(string url, string id)
        {
            using HttpClient client = new HttpClient();
            var response = await client.DeleteAsync($"{url}/{id}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Livre supprimé avec succès.");
            }
            else
            {
                Console.WriteLine("Erreur lors de la suppression du livre.");
            }
        }
    }

}

}
