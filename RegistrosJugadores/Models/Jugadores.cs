using System.ComponentModel.DataAnnotations;

namespace RegistrosJugadores.Models
{
    public class Jugadores
    {
        [Key]
        public int JugadorId { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido")]

        public String Nombre { get; set; }

        public int Partidas { get; set; }


    }
}
