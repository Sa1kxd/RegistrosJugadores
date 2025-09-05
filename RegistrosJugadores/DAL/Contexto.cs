using Microsoft.EntityFrameworkCore;
using RegistrosJugadores.Models;

namespace RegistrosJugadores.DAL
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }
    
        public DbSet<Jugadores> Jugadores { get; set; }
    }
}
