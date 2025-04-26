using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Cajeros_Dispositivos")] // Nombre de la tabla en la base de datos
    public class Cajeros_Dispositivos_Model
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrementable
        public int cajero_dispositivo_id { get; set; }
        

        [Column("estado_dispositivo")]
        public int? estado_dispositivo { get; set; }

        [Column("estado_suministro")]
        public int? estado_suministro { get; set; }

        [ForeignKey("Dispositivos")]
        [Column("dispositivo_id")]
        public int? dispositivo_id { get; set; }


        [ForeignKey("Cajeros")]
        [Column("cajero_id")]
        public int? cajero_id { get; set; }

        // Propiedad de navegación
        public virtual Cajeros_Model? Cajero { get; set; }
        public virtual Dispositivos_Model? Dispositivo { get; set; }
    }
}
