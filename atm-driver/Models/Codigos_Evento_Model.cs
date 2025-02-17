using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Codigos_Evento")]
    public class Codigos_Evento_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int codigo_evento_id { get; set; }

        [MaxLength(100)]
        [Column("nombre")]
        public string? nombre{ get; set; }

        [MaxLength(100)]
        [Column("descripcion")]
        public string? descripcion { get; set; }

        [ForeignKey("Tipo_Eventos")]
        [Column("tipo_evento_id")]
        public int? tipo_evento_id { get; set; }
    }
}
