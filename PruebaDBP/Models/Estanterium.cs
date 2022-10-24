using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Estanterium
    {
        [Required(ErrorMessage = "El campo ID estanteria es obligatorio")]
        public int IdEstanteria { get; set; }
        [Required(ErrorMessage = "El campo ID usuario es obligatorio")]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El campo nombre estanteria es obligatorio")]
        public string? NomEstanteria { get; set; }
        [Required(ErrorMessage = "El campo fecha de creacion es obligatorio")]
        public DateOnly? FechaCreacion { get; set; }
        [Required(ErrorMessage = "El campo esEditable es obligatorio")]
        public bool? EsEditable { get; set; }
    }
}
