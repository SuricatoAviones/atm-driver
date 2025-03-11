
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace atm_driver.Models
{
    [Table("Servicios")]
    public class Servicio_Model
    {
        [Key] // Llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrementable
        public int servicio_id { get; set; }

        [Required]
        public string? codigo { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public string? estado { get; set; }

        public int? tiempo_espera_uno { get; set; }

        public int? tiempo_espera_dos {  get; set; }
        
        [Column("tipo_mensaje_id")]
        public Tipo_Mensaje_Model? tipo_mensaje_id { get; set; }

        [Column("tipo_comunicacion_id")]
        public Sistemas_Comunicacion_Model? sistema_comunicacion_id  { get; set; }  
    }
}
