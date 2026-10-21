using Microsoft.EntityFrameworkCore;
using MovieVault.Models;

namespace MovieVault.Data;

public class MovieVaultContext : DbContext
{
    public MovieVaultContext(DbContextOptions<MovieVaultContext> options) : base(options) { }

    public DbSet<Title> Titles => Set<Title>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Viewing> Viewings => Set<Viewing>();

    protected override void OnModelCreating(ModelBuilder b)
    {

        b.Entity<Title>()
            .HasIndex(x => new { x.Name, x.ReleaseYear })
            .IsUnique();

        b.Entity<Title>()
            .HasOne(x => x.Genre)
            .WithMany()
            .HasForeignKey(x => x.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Viewing>()
            .HasOne(v => v.Title)
            .WithMany(t => t.Viewings)
            .HasForeignKey(v => v.TitleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
