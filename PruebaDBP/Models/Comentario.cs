using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Comentario
    {
        [Required(ErrorMessage = "El campo ID comentario es obligatorio")]
        public int IdComentario { get; set; }
        [Required(ErrorMessage = "El campo ID usuario es obligatorio")]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El campo ID pelicula es obligatorio")]
        public int IdPelicula { get; set; }
        [Required(ErrorMessage = "El campo texto es obligatorio")]
        public string? Texto { get; set; }
        [Required(ErrorMessage = "El campo fecha de publicacion es obligatorio")]
        public DateOnly? FechaPublicacion { get; set; }
        [Required(ErrorMessage = "El campo estado es obligatorio")]
        public bool? Estado { get; set; }
    }
}
