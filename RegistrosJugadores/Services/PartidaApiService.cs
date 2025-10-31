using RegistrosJugadores.DTO;
using System.Net.Http;

namespace RegistrosJugadores.Services;

public interface IPartidaApiService
{
    Task<Resource<PartidaResponse>> GetPartidasAsync(int partidaId);
}

public class PartidaApiService(HttpClient _httpClient) : IPartidaApiService
{
    
   public async Task<Resource<PartidaResponse>> GetPartidasAsync(int partidaId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<PartidaResponse>($"api/Partidas/{partidaId}");
            return new Resource<PartidaResponse>.Success(response!);
        }
        catch (Exception ex)
        {
            return new Resource<PartidaResponse>.Error(ex.Message);
        }
    }
}
