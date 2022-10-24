using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaEstanterium
    {
        [Required(ErrorMessage = "El campo ID pelicula es obligatorio")]
        public int IdPelicula { get; set; }
        [Required(ErrorMessage = "El campo ID estanteria es obligatorio")]
        public int IdEstanteria { get; set; }
        [Required(ErrorMessage = "El campo fecha de agregacion es obligatorio")]
        public DateOnly? FechaAgregacion { get; set; }
    }
}
