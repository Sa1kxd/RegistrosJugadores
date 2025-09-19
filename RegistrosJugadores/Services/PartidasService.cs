using Microsoft.EntityFrameworkCore;
using RegistrosJugadores.DAL;
using RegistrosJugadores.Models;
using System.Linq.Expressions;

namespace RegistrosJugadores.Services;

public class PartidasService
{
    private readonly IDbContextFactory<Contexto> _dbFactory;
    private readonly ILogger<PartidasService> _logger;

    public PartidasService(IDbContextFactory<Contexto> dbFactory, ILogger<PartidasService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    public async Task<bool> Guardar(Partidas partida)
    {
        if (!await Existe(partida.PartidaId))
        {
            return await Insertar(partida);
        }
        else
        {
            return await Modificar(partida);
        }
    }

    public async Task<bool> Existe(int partidaId)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Partidas.AnyAsync(p => p.PartidaId == partidaId);
    }

    private async Task<bool> Insertar(Partidas partida)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            contexto.Partidas.Add(partida);
            var result = await contexto.SaveChangesAsync() > 0;

            if (result)
            {
                partida.PartidaId = partida.PartidaId;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al insertar partida");
            return false;
        }
    }

    private async Task<bool> Modificar(Partidas partida)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var partidaExistente = await contexto.Partidas.FirstOrDefaultAsync(p => p.PartidaId == partida.PartidaId);

            if (partidaExistente == null)
            {
                _logger.LogWarning("Partida con ID {PartidaId} no encontrada para modificar", partida.PartidaId);
                return false;
            }

            partidaExistente.EstadoTablero = partida.EstadoTablero;
            partidaExistente.EstadoPartida = partida.EstadoPartida;
            partidaExistente.TurnoJugadorId = partida.TurnoJugadorId;
            partidaExistente.GanadorId = partida.GanadorId;
            partidaExistente.FechaFin = partida.FechaFin;

            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar partida {PartidaId}", partida.PartidaId);
            return false;
        }
    }

    public async Task<Partidas?> Buscar(int partidaId)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Partidas
                .Include(p => p.Jugador1)
                .Include(p => p.Jugador2)
                .Include(p => p.Ganador)
                .FirstOrDefaultAsync(p => p.PartidaId == partidaId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar partida {PartidaId}", partidaId);
            return null;
        }
    }

    public async Task<bool> Eliminar(int partidaId)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Partidas
                .Where(p => p.PartidaId == partidaId)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar partida {PartidaId}", partidaId);
            return false;
        }
    }

    public async Task<List<Partidas>> Listar(Expression<Func<Partidas, bool>> criterio)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Partidas
                .Include(partida => partida.Jugador1)
                .Include(partida => partida.Jugador2)
                .Include(partida => partida.Ganador)
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar partidas");
            return new List<Partidas>();
        }
    }

    public async Task<bool> ActualizarEstadisticasAsync(Partidas partida)
    {
        try
        {
            if (partida.EstadoPartida != "Finalizada" && partida.EstadoPartida != "Empate")
            {
                _logger.LogWarning("Intento de actualizar estadísticas de partida no finalizada: {PartidaId}", partida.PartidaId);
                return false;
            }

            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var jugadorX = await contexto.Jugadores.FirstOrDefaultAsync(j => j.JugadorId == partida.Jugador1Id);
            var jugadorO = await contexto.Jugadores.FirstOrDefaultAsync(j => j.JugadorId == partida.Jugador2Id);

            if (jugadorX == null || jugadorO == null)
            {
                _logger.LogError("No se encontraron los jugadores para la partida {PartidaId}. JugadorX: {JugadorXId}, JugadorO: {JugadorOId}",
                    partida.PartidaId, partida.Jugador1Id, partida.Jugador2Id);
                return false;
            }

            jugadorX.Jugadas++;
            jugadorO.Jugadas++;

            if (partida.GanadorId != null)
            {
                if (partida.GanadorId == jugadorX.JugadorId)
                {
                    jugadorX.Victorias++;
                    jugadorO.Derrotas++;
                    _logger.LogInformation("Jugador {JugadorX} ganó contra {JugadorO} en partida {PartidaId}",
                        jugadorX.Nombre, jugadorO.Nombre, partida.PartidaId);
                }
                else if (partida.GanadorId == jugadorO.JugadorId)
                {
                    jugadorO.Victorias++;
                    jugadorX.Derrotas++;
                    _logger.LogInformation("Jugador {JugadorO} ganó contra {JugadorX} en partida {PartidaId}",
                        jugadorO.Nombre, jugadorX.Nombre, partida.PartidaId);
                }
                else
                {
                    _logger.LogWarning("GanadorId {GanadorId} no coincide con ningún jugador de la partida {PartidaId}",
                        partida.GanadorId, partida.PartidaId);
                    return false;
                }
            }
            else
            {
                jugadorX.Empates++;
                jugadorO.Empates++;
                _logger.LogInformation("Empate entre {JugadorX} y {JugadorO} en partida {PartidaId}",
                    jugadorX.Nombre, jugadorO.Nombre, partida.PartidaId);
            }

            var filasAfectadas = await contexto.SaveChangesAsync();

            if (filasAfectadas > 0)
            {
                _logger.LogInformation("Estadísticas actualizadas correctamente para partida {PartidaId}", partida.PartidaId);
                return true;
            }
            else
            {
                _logger.LogWarning("No se actualizaron las estadísticas para partida {PartidaId}", partida.PartidaId);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando estadísticas para partida {PartidaId}", partida.PartidaId);
            return false;
        }
    }

    public async Task<bool> RecalcularEstadisticasJugadorAsync(int jugadorId)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            var jugador = await contexto.Jugadores.FirstOrDefaultAsync(j => j.JugadorId == jugadorId);
            if (jugador == null) return false;
            var partidasJugador = await contexto.Partidas
                .Where(p => (p.Jugador1Id == jugadorId || p.Jugador2Id == jugadorId) &&
                           (p.EstadoPartida == "Finalizada" || p.EstadoPartida == "Empate"))
                .ToListAsync();

            jugador.Jugadas = partidasJugador.Count;
            jugador.Victorias = partidasJugador.Count(p => p.GanadorId == jugadorId);
            jugador.Derrotas = partidasJugador.Count(p => p.GanadorId != null && p.GanadorId != jugadorId);
            jugador.Empates = partidasJugador.Count(p => p.GanadorId == null);

            await contexto.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recalculando estadísticas del jugador {JugadorId}", jugadorId);
            return false;
        }
    }
}