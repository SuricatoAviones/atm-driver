using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Dispositivos")]
    public class Dispositivos_Model
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("dispositivo_id")]
        public int dispositivo_id { get; set; }

        [Column("nombre")]
        [StringLength(255)] // Ajusta la longitud según sea necesario
        public string? nombre { get; set; }


        [Column("codigo")]
        public int? codigo { get; set; }

       
    }
}
