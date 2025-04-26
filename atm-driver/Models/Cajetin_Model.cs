using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace atm_driver.Models
{
    [Table("Cajetines")] // Nombre de la tabla en la base de datos
    public class Cajetin_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrementable
        public int cajetin_id { get; set; }

        [MaxLength(100)]
        public int? denominacion { get; set; }

        [MaxLength(100)]
        public string? tipo_denominacion { get; set; }

        [MaxLength(100)]
        public string? tipo { get; set; }
        public int cantidad_disponible { get; set; }
        public int cantidad_dispensada { get; set; }
        public int cantidad_rechazada { get; set; }

        public int cantidad_ultima_transaccion { get; set; }

        public int numero_cajetin { get; set; }

        public DateTime fecha_habil { get; set; }

        [ForeignKey("Cajeros_Dispositivos")]
        [Column("cajero_dispositivo_id")]
        public int cajero_dispositivo_id { get; set; }

        [ForeignKey("Denominaciones_Monedas")]
        [Column("denominacion_moneda_id")]
        public int? denominacion_moneda_id { get; set; }

        // Propiedad de navegación
        public virtual Cajeros_Dispositivos_Model? Cajeros_Dispositivos { get; set; }

        public virtual Denominaciones_Monedas_Model? Denominaciones_Monedas { get; set; }
    }
}
