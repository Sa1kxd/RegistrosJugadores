using RegistrosJugadores.DTO;

namespace RegistrosJugadores.Services;


public interface IMovimientoApiService
{
    Task<Resource<List<MovimientoResponse>>> GetMovimientosAsync(int partidaId);
    Task<Resource<bool>> PostMovimientosAsync(MovimientoRequest request);
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
            var result = await _httpClient.GetFromJsonAsync<List<MovimientoResponse>>($"api/Movimientos{partidaId}");
            return new Resource<List<MovimientoResponse>>.Success(result ?? new());
        }
        catch (Exception ex)
        {
            return new Resource<List<MovimientoResponse>>.Error(Message: ex.Message);
        }
    }

    public async Task<Resource<bool>> PostMovimientosAsync(MovimientoRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Movimientos", request);
            response.EnsureSuccessStatusCode();
            return new Resource<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return new Resource<bool>.Error(ex.Message);
        }
    }
}
