using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Comentario
    {
        public int IdComentario { get; set; }
        public int IdUsuario { get; set; }
        public int IdPelicula { get; set; }
        [Required(ErrorMessage = "El campo texto es obligatorio")]
        public string? Texto { get; set; }
        public string? FechaPublicacion { get; set; }
        public bool? Estado { get; set; }
    }
}
