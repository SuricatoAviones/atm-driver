using atm_driver.Clases;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace atm_driver.Models
{
    [Table("Mensajes")] // Nombre de la tabla en la base de datos

    public class Mensaje_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mensaje_id { get; set; }
        public string? mensaje { get; set; }
        public bool? origen { get; set; }
        public DateTime hora_entrada { get; set; }
        [ForeignKey("Servicios")]
        [Column("servicio_id")]
        public int? servicio_id { get; set; }
    }
}
