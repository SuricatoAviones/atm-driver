using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Denominaciones_Monedas")]
    public class Denominaciones_Monedas_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("denominacion_moneda_id")]
        public int denominacion_moneda_id { get; set; }

        [Column("nombre")]
        [StringLength(255)] 
        public string? nombre { get; set; }

        [Column("descripcion")]
        [StringLength(500)] 
        public string? descripcion { get; set; }

        [Column("siglas")]
        [StringLength(500)]
        public string? siglas { get; set; }

        [Column("codigo")]
        [StringLength(500)]
        public string? codigo { get; set; }
    }
}
