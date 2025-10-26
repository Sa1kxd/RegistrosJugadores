using RegistroJugadores.BlazorWasm.DTO;
using RegistroJugadores.BlazorWasm.Shared;
using System.Net.Http.Json;

namespace RegistroJugadores.BlazorWasm.Services;

public interface IMovimientoAPIService
{
    Task<Resource<List<MovimientoResponse>>> GetMovimientosAsync(int partidaId);
    Task<Resource<bool>> PostMovimientoAsync(MovimientoRequest request);
}

public class MovimientoAPIService : IMovimientoAPIService
{
    private readonly HttpClient _httpClient;
    public MovimientoAPIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<Resource<List<MovimientoResponse>>> GetMovimientosAsync(int partidaId)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<MovimientoResponse>>($"api/Movimientos/{partidaId}");
            return new Resource<List<MovimientoResponse>>.Success(result ?? new());
        }
        catch (Exception ex)
        {
            return new Resource<List<MovimientoResponse>>.Error(ex.Message);
        }
    }

    public async Task<Resource<bool>> PostMovimientoAsync(MovimientoRequest request)
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
