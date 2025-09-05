using Microsoft.EntityFrameworkCore;
using RegistrosJugadores.DAL;
using RegistrosJugadores.Models;
using System.Linq.Expressions;

namespace RegistrosJugadores.Services
{
    public class JugadoresServices(IDbContextFactory<Contexto> DbFactory)
    {
        public class JugadoresService(IDbContextFactory<Contexto> DbFactory)
        {
            public async Task<bool> Guardar(Jugadores jugador)
            {
                if (!await Existe(jugador.JugadorId))
                {
                    return await Insertar(jugador);
                }
                else
                {
                    return await Modificar(jugador);
                }
            }

            public async Task<bool> Existe(int JugadorId)
            {
                await using var contexto = await DbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.AnyAsync(j => j.JugadorId == JugadorId);
            }

            public async Task<bool> ExisteNombre(string Nombre)
            {
                await using var contexto = await DbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.AnyAsync(j => j.Nombre.ToLower() == Nombre.ToLower());
            }

            private async Task<bool> Insertar(Jugadores jugador)
            {
                await using var contexto = await DbFactory.CreateDbContextAsync();
                contexto.Jugadores.Add(jugador);
                return await contexto.SaveChangesAsync() > 0;
            }

            private async Task<bool> Modificar(Jugadores jugador)
            {
                await using var contexto = await DbFactory.CreateDbContextAsync();
                contexto.Update(jugador);
                return await contexto.SaveChangesAsync() > 0;
            }

            public async Task<Jugadores?> Buscar(int JugadorId)
            {
                await using var contexto = await DbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.FirstOrDefaultAsync(j => j.JugadorId == JugadorId);
            }

            public async Task<bool> Eliminar(int JugadorId)
            {
                await using var contexto = await DbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.AsNoTracking().Where(j => j.JugadorId == JugadorId).ExecuteDeleteAsync() > 0;
            }

            public async Task<List<Jugadores>> Listar(Expression<Func<Jugadores, bool>> criterio)
            {
                await using var contexto = await DbFactory.CreateDbContextAsync();
                return await contexto.Jugadores.Where(criterio).AsNoTracking().ToListAsync();
            }
        }

    }
}

