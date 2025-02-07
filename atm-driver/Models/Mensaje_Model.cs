using atm_driver.Clases;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace atm_driver.Models
{
    [Table("Mensajes")] // Nombre de la tabla en la base de datos
    public class Mensaje_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrementable
        public int mensaje_id { get; set; }
        public string? mensaje { get; set; }
        public string? origen { get; set; }
        public DateTime hora_entrada { get; set; }

        
        public Servicio_Model? servicio_id { get; set; }
        
    }
}
