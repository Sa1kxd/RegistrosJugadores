namespace RegistroJugadores.BlazorWasm.DTO;

public class PartidasRequest
{
    public int Jugador1Id { get; set; }
    public int? Jugador2Id { get; set; }
    public PartidasRequest(int jugador1Id, int? jugador2Id)
    {
        Jugador1Id = jugador1Id;
        Jugador2Id = jugador2Id;
    }
}
