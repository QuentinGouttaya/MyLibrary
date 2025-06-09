using Models.Media;
using System;
using System.Linq;


[Route("livres")]
public class LivresControllers : Controller
{
    public DbSet<Media> medias { get; set; }

    // Quentin me donnera les infos de sqlite
    // protected override void OnConfiguring(DbContextOptionsBuilder options)
    //     => options.UseSqlite("Data Source=ma_base.sqlite");


    [HttpGet()]
    public IActionResult getAllLivres()
    {   
        List<Media> livres = db.medias.ToList();
        return livres;
    }

    [HttpGet("{id}")]
    public IActionResult getLivres(int id)
    {   
        var livre = context.Media.Find(id)
        if (livre == null)
        {
            return NotFound();
        }
        return livre;
    }

    [HttpPost()]
    public IActionResult addLivres(Media newLivre)
    {
        try
        {
            context.Media.Add(newLivre);
            context.SaveChanges();
            return "Livre ajouté avec succès";
        }
        catch (Exception ex)
        {
            return "Erreur lors de l'ajout du livre: " + ex.Message;
        }
    }

    [HttpPut("{id}")]
    public IActionResult updateLivres(int id, Media updatedLivre)
    {
        var livre = context.Media.Find(id);
        if (livre == null)
        {
            return NotFound();
        }

        livre.Author = updatedLivre.Author;
        livre.Title = updatedLivre.Title;

        try
        {
            context.Update(livre);
            context.SaveChanges();
            return "Livre mis à jour avec succès";
        }
        catch (Exception ex)
        {
            return "Erreur lors de la mise à jour du livre: " + ex.Message;
        }
    }

    [HttpDelete("{id}")]
    public IActionResult deleteLivres(int id)
    {
        var livre = context.Media.Find(id);
        if (livre == null)
        {
            return NotFound();
        }

        try
        {
            context.Media.Remove(livre);
            context.SaveChanges();
            return "Livre supprimé avec succès";
        }
        catch (Exception ex)
        {
            return "Erreur lors de la suppression du livre: " + ex.Message;
        }
    }
}