using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaDirector
    {
        [Required(ErrorMessage = "El campo ID pelicula es obligatorio")]
        public int IdPelicula { get; set; }
        [Required(ErrorMessage = "El campo ID director es obligatorio")]
        public int IdDirector { get; set; }
    }
}
