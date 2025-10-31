using RegistrosJugadores.DTO;
using System.Net.Http;

namespace RegistrosJugadores.Services;


public interface IMovimientoApiService
{
    Task<Resource<List<MovimientoResponse>>> GetMovimientosAsync(int partidaId);
    Task<Resource<MovimientoResponse>> PostMovimientosAsync(int partidaId, string jugador, int posicionFila, int posicionColumna);
}

public class MovimientoApiService : IMovimientoApiService
{
    private readonly HttpClient _httpClient;

    public MovimientoApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Resource<List<MovimientoResponse>>> GetMovimientosAsync(int partidaId)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<MovimientoResponse>>($"/api/Movimientos/{partidaId}");
            return new Resource<List<MovimientoResponse>>.Success(result ?? new());
        }
        catch (Exception ex)
        {
            return new Resource<List<MovimientoResponse>>.Error(Message: ex.Message);
        }
    }

    public async Task<Resource<MovimientoResponse>> PostMovimientosAsync(int partidaId, string jugador, int posicionFila, int posicionColumna)
    {
        var request = new MovimientoRequest(partidaId, jugador, posicionFila, posicionColumna);
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Movimientos", request);
            response.EnsureSuccessStatusCode();
            return new Resource<MovimientoResponse>.Success(null!);
        }
        catch (HttpRequestException ex)
        {
            return new Resource<MovimientoResponse>.Error($"Error de red: {ex.Message}");
        }
        catch (NotSupportedException)
        {
            return new Resource<MovimientoResponse>.Error("Respuesta inválida del servidor.");
        }
    }
}

