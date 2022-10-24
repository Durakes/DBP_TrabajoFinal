using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Director
    {
        [Required(ErrorMessage = "El campo ID director es obligatorio")]
        public int IdDirector { get; set; }
        [Required(ErrorMessage = "El campo ID mejor pelicula es obligatorio")]
        public int? IdMejorPelicula { get; set; }
        [Required(ErrorMessage = "El campo nombre del director  es obligatorio")]
        public string? NomDirector { get; set; }
        [Required(ErrorMessage = "El campo biografia del director es obligatorio")]
        public string? BioDirector { get; set; }
        [Required(ErrorMessage = "El campo foto es obligatorio")]
        public string? UrlFoto { get; set; }
    }
}
