﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace atm_driver.Models
{
    [Table("Tipo_Mensaje")]
    public class Tipo_Mensaje_Model
    {
        [Key] // Llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrementable
        [Column("tipo_mensaje_id")]
        public int tipo_mensaje_id { get; set; }

        [Required]
        public string? nombre_tipo { get; set; }

        [Required]
        public string? descripcion {  get; set; }
    }
}
