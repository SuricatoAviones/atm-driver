using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Downloads")]

    public class Download_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("download_id")]
        public int download_id { get; set; }

        [Column("nombre")]
        [StringLength(255)] // Ajusta la longitud según sea necesario
        public string? nombre { get; set; }

        [Column("ruta")]
        [StringLength(500)] // Ajusta la longitud según sea necesario
        public string? ruta { get; set; }

        // Clave Foránea
        [ForeignKey("FormatoCajero")]
        [Column("formato_cajero_id")]
        public int? formato_cajero_id { get; set; }

        /*// Propiedad de navegación
        public virtual FormatoCajero FormatoCajero { get; set; }*/
    }
}
