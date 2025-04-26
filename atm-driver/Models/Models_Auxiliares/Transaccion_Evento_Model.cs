using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models.Models_Auxiliares
{
    [Table("Transaccion_evento")] // Nombre de la tabla en la base de datos

    public class Transaccion_Evento_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int transaccion_evento_id { get; set; }


        [Column("codigo")]
        [MaxLength(2)]
        public string? codigo { get; set; }

        [Required]
        [Column("estado")]
        [MaxLength(4)]
        public string? estado { get; set; }

        [Required]
        [Column("screen")]
        [MaxLength(4)]
        public string? screen { get; set; }

        [Column("descripcion")]
        [MaxLength(40)]
        public string? descripcion { get; set; }

    }
}
