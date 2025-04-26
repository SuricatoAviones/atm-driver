using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models.Models_Auxiliares
{
    [Table("Transaccion_codigo")] // Nombre de la tabla en la base de datos
    public class Transaccion_Codigo_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int transaccion_codigo_id { get; set; }

        [Required]
        [Column("codigo_operacion")]
        [MaxLength(10)]
        public string codigo_operacion { get; set; }

        [Required]
        [Column("tipo_transaccion")]
        [MaxLength(2)]
        public string tipo_transaccion { get; set; }

        [Required]
        [Column("tipo_cuenta_origen")]
        [MaxLength(2)]
        public string tipo_cuenta_origen { get; set; }

        [Required]
        [Column("tipo_cuenta_destino")]
        [MaxLength(2)]
        public string tipo_cuenta_destino { get; set; }

        [Required]
        [Column("pos_ini")]
        [MaxLength(1)]
        public int pos_ini { get; set; }

        [Required]
        [Column("descripcion")]
        [MaxLength(100)]
        public string descripcion { get; set; }

    }
}
