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
        public int sistema_comunicacion_id { get; set; }

        [Required] // Campo obligatorio
        public int tipo_conexion { get; set; }

        [Required]
        public int tipo_conexion_tcp { get; set; }

        [MaxLength(50)] // Longitud máxima de la cadena
        public string? direccion_ip { get; set; }

        public string? puerto_tcp { get; set; }
        public string? socket_tcp { get; set; }

        [MaxLength(100)]
        public string? servidor_soap_api { get; set; }

        public int? puerto_soap_api { get; set; }

        [MaxLength(20)]
        public string? encabezado { get; set; }

        public int longitud_paquete { get; set; }
        public bool ebcdc { get; set; }
        public bool empaquetado { get; set; }
    }
}
