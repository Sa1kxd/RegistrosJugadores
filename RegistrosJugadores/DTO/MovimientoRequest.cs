namespace RegistrosJugadores.DTO;

public class MovimientoRequest
{
    public int PartidaId { get; set; }

    public string Jugador { get; set; }

    public int PosicionFila { get; set; }

    public int PosicionColumna { get; set; }
}
