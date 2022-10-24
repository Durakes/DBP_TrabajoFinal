using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class ValoracionUsuario
    {
        [Required(ErrorMessage ="El campo ID pelicula es obligatorio")]
        public int IdPelicula { get; set; }
        [Required(ErrorMessage = "El campo ID usuario es obligatorio")]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El campo valoracion es obligatorio")]
        public int? Valoracion { get; set; }
        [Required(ErrorMessage = "El campo fecha de valoracion es obligatorio")]
        public DateOnly? FechaValoracion { get; set; }
    }
}
