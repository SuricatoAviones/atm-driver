using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Eventos")]

    public class Eventos_Model
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("evento_id")]
        public int evento_id { get; set; }
        [MaxLength(100)]
        [Column("nombre")]
        public string? nombre { get; set; }

        [MaxLength(100)]
        [Column("tipo")]
        public string? tipo { get; set; }

        [Column("fecha")]
        public DateTime fecha { get; set; }

        [MaxLength(100)]
        [Column("observaciones")]
        public string? observaciones { get; set; }

        [ForeignKey("Codigos_Evento")]
        [Column("codigo_evento_id")]
        public int?  codigo_evento_id { get; set; }
    
        public Cajeros_Model? cajero_id { get; set; }

        public Servicio_Model? servicio_id { get; set; }

        
    }
}
