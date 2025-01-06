using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace atm_driver.Models
{
    // Clase modelo para la tabla SistemasComunicacion en SQL Server
    [Table("Sistemas_Comunicacion")] // Nombre de la tabla en la base de datos
    public class Sistemas_Comunicacion_Model
    {
        [Key] // Llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrementable
        public int Id { get; set; }

        [Required] // Campo obligatorio
        public int TipoConexion { get; set; }

        [Required]
        public int TipoConexionTcp { get; set; }

        [MaxLength(50)] // Longitud máxima de la cadena
        public string? DireccionIp { get; set; }

        public int PuertoTcp { get; set; }
        public int SocketTcp { get; set; }

        [MaxLength(100)]
        public string? ServidorSoapApi { get; set; }

        public int PuertoSoapApi { get; set; }

        [MaxLength(20)]
        public string? Encabezado { get; set; }

        public int LongitudPaquete { get; set; }
        public bool Ebcdc { get; set; }
        public bool Empaquetado { get; set; }
    }
}
