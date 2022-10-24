using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaCategorium
    {
        [Required(ErrorMessage = "El campo ID pelicula es obligatorio")]
        public int IdPelicula { get; set; }
        [Required(ErrorMessage = "El campo ID categoria es obligatorio")]
        public int IdCategoria { get; set; }
    }
}
