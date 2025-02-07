using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Control")]
public class Control_Model
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("control")]
    public int control_id { get; set; }

    [Required]
    [Column("codigo_institucion")]
    public int codigo_institucion { get; set; }

    [Column("nombre")]
    [StringLength(255)] // Ajusta la longitud según sea necesario
    public string? nombre { get; set; }

    [Column("fecha_negocio")]
    public DateTime fecha_negocio { get; set; }

    [Column("estado_sistema")]
    [StringLength(50)] // Ajusta la longitud según sea necesario
    public string? estado_sistema { get; set; }
}
