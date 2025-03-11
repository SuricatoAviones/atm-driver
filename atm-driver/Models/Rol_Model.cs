using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Roles")]
    public class Rol_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrementable
        public int rol_id { get; set; }

        public string? nombre { get; set; }
        public string? descripcion { get; set; }

        // Colección de usuarios con este rol
        public virtual ICollection<Usuarios_Model> Usuarios { get; set; }
    }
}
