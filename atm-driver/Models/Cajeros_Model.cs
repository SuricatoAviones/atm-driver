using atm_driver.Clases;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace atm_driver.Models
{
    [Table("Cajeros")] // Nombre de la tabla en la base de datos
    public class Cajeros_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrementable
        public int cajero_id { get; set; }

        [MaxLength(100)]
        public string? nombre { get; set; }
        public string? codigo { get; set; }
        public string? marca { get; set; }
        public string? modelo { get; set; }
        public string? localizacion { get; set; }
        public string? estado { get; set; }
        public string? direccion_ip { get; set; }
        public Download_Model? download_id { get; set; }
        public Keys_Model? key_id { get; set; }

    }

}
