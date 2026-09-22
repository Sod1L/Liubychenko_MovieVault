using Microsoft.EntityFrameworkCore;
using MovieVault.Models;

namespace MovieVault.Data;

public class MovieVaultContext : DbContext
{
    public MovieVaultContext(DbContextOptions<MovieVaultContext> options) : base(options) { }

    public DbSet<Title> Titles => Set<Title>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        
    }
}