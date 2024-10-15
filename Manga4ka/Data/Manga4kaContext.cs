using Manga4ka.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Manga4ka.Data;

public class Manga4kaContext : DbContext
{
    public Manga4kaContext(DbContextOptions<Manga4kaContext> options) : base(options)
    {

    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Manga> Manga { get; set; }

    public virtual DbSet<Genre> Genre { get; set; }

    public virtual DbSet<MangaGenre> MangaGenres { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<FavoriteManga> FavoriteManga { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MangaGenre>()
            .HasKey(mg => new { mg.MangaId, mg.GenreId });

        modelBuilder.Entity<MangaGenre>()
            .HasOne(mg => mg.Manga)
            .WithMany(m => m.MangaGenres)
            .HasForeignKey(mg => mg.MangaId);

        modelBuilder.Entity<MangaGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MangaGenres)
            .HasForeignKey(mg => mg.GenreId);
    }
}
