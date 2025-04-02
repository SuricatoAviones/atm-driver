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

        [ForeignKey("Cajeros")]
        [Column("cajero_id")]
        public int? cajero_id { get; set; }

        [ForeignKey("Servicios")]
        [Column("servicio_id")]
        public int? servicio_id { get; set; }

        // Propiedad de navegación
        public virtual Codigos_Evento_Model? Codigos_Evento { get; set; }
        public virtual Cajeros_Model? Cajeros { get; set; }
        public virtual Servicio_Model? Servicios { get; set; }



    }
}
