using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Tipo_Eventos")]
    public class Tipo_Evento_Model
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tipo_evento_id { get; set; }

        [MaxLength(100)]
        [Column("nombre")]
        public string? nombre { get; set; }

        [MaxLength(100)]
        [Column("descripcion")]
        public string? descripcion { get; set; }
    }
}
