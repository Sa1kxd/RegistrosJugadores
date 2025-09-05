using System.ComponentModel.DataAnnotations;

namespace RegistrosJugadores.Models
{
    public class Jugadores
    {
        [Key]
        
        public int JugadorId { get; set; }
        
     
        [Required(ErrorMessage = "Este campo es requerido")]
        public String Nombre { get; set; }

        [Range(0,int.MaxValue, ErrorMessage = "Valor debe ser mayor a 0")]
        public int Partidas { get; set; }


    }
}
