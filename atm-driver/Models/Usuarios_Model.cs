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

        public Rol_Model? rol_id { get; set; }
    }
}
