using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Usuarios")]
    public class Usuarios_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int usuario_id { get; set; }

        public string? nombre { get; set; }

        public string? email { get; set; }

        public string? password { get; set; }

        // Clave externa
        public int? rol_id { get; set; }

        // Propiedad de navegación
        [ForeignKey("rol_id")]
        public virtual Rol_Model? Rol { get; set; }
    }
}