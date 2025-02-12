using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Models
{
    [Table("Keys")] // Especifica el nombre de la tabla en la base de datos
    public class Keys_Model
    {
        [Key] // Indica que esta propiedad es la clave primaria
        [Column("key_id")] // Especifica el nombre de la columna en la base de datos
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int key_id { get; set; }

        [Column("clave_comunicacion")] // Especifica el nombre de la columna en la base de datos
        public string clave_comunicacion { get; set; }

        [Column("clave_masterKey")] // Especifica el nombre de la columna en la base de datos
        public string clave_masterKey { get; set; }
    }
}