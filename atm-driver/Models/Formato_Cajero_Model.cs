using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace atm_driver.Models
{
    [Table("Formato_Cajero")]
    public class Formato_Cajero_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("formato_cajero_id")]
        public int formato_cajero_id { get; set; }

        [Column("nombre")]
        [StringLength(255)] // Ajusta la longitud según sea necesario
        public string? nombre { get; set; }

        [Column("descripcion")]
        [StringLength(500)] // Ajusta la longitud según sea necesario
        public string? descripcion { get; set; }
    }
}
