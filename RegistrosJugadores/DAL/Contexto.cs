using Microsoft.EntityFrameworkCore;
using RegistrosJugadores.Models;

namespace RegistrosJugadores.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }
    public DbSet<Jugadores> Jugadores { get; set; }
    public DbSet<Partidas> Partidas { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Jugadores>()
            .HasIndex(J => J.Nombre)
            .IsUnique();
        base.OnModelCreating(modelBuilder);
    }

}

