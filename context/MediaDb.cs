using Microsoft.EntityFrameworkCore;

class MediaDb : DbContext
{
    // public MediaDb(DbContextOptions<MediaDb> options)
    //     : base(options) { }

    public DbSet<Media> Medias => Set<Media>();
}