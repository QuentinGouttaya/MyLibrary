using Microsoft.EntityFrameworkCore;
using Models;
using Models.Ebook;
using Models.Paperbook;


public class MediaDb : DbContext
{
    public MediaDb(DbContextOptions<MediaDb> options)
        : base(options) { }

    public DbSet<Media> Medias => Set<Media>();
    public DbSet<Ebook> Ebooks => Set<Ebook>();
    public DbSet<PaperBook> PaperBooks => Set<PaperBook>();
}