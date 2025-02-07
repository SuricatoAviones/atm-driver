using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Transacciones")]
    public class Transacciones_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("transaccion_id")]
        public int transaccion_id { get; set; }

        [Column("codigo")]
        [StringLength(50)] // Ajusta la longitud según sea necesario
        public string? codigo { get; set; }

        [Column("fecha_operacion")]
        public DateTime? fecha_operacion { get; set; }

        [Column("fecha_negocio")]
        public DateTime? fecha_negocio { get; set; }

        [Column("fecha_mensaje_recibido")]
        public DateTime? fecha_mensaje_recibido { get; set; }

        [Column("fecha_mensaje")]
        public DateTime? fecha_mensaje { get; set; }

        [Column("monto")]
        public int? monto { get; set; }

        [Column("trace")]
        [StringLength(100)] // Ajusta la longitud según sea necesario
        public string? trace { get; set; }

        [Column("codigo_respuesta")]
        public int? codigo_respuesta { get; set; }

        [Column("tipo_cuenta")]
        [StringLength(50)]
        public string? tipo_cuenta { get; set; }

        [Column("origen")]
        [StringLength(50)]
        public string? origen { get; set; }

        [Column("tipo_cuenta_destino")]
        [StringLength(50)]
        public string? tipo_cuenta_destino { get; set; }

        [Column("numero_autorizacion")]
        public int? NumeroAutorizacion { get; set; }

        [Column("tarjeta")]
        [StringLength(50)]
        public string? tarjeta { get; set; }

        [Column("pin")]
        [StringLength(50)]
        public string? pin { get; set; }

        // Claves Foráneas
         /*/[ForeignKey("CodigoMoneda")]
        [Column("codigo_moneda_id")]
        public int? CodigoMonedaId { get; set; }

        [ForeignKey("Cajero")]
        [Column("cajero_id")]
        public int? CajeroId { get; set; }*/

        // Propiedades de navegación
        //public virtual CodigoMoneda CodigoMoneda { get; set; }
        //public virtual Cajero Cajero { get; set; }
    }
}
