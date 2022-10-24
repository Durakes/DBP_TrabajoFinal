using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Pelicula
    {
        [Required(ErrorMessage = "El campo ID pelicula es obligatorio")]
        public int IdPelicula { get; set; }
        [Required(ErrorMessage = "El campo ID idioma es obligatorio")]
        public int? IdIdioma { get; set; }
        [Required(ErrorMessage = "El campo nombre pelicula es obligatorio")]
        public string? NomPelicula { get; set; }
        [Required(ErrorMessage = "El campo fecha de esterno es obligatorio")]
        public short? FechaEstreno { get; set; }
        [Required(ErrorMessage = "El campo duracion en minutos es obligatorio")]
        public int? DuracionMin { get; set; }
        [Required(ErrorMessage = "El campo sumilla es obligatorio")]
        public string? Sumilla { get; set; }
        [Required(ErrorMessage = "El campo foto de perfil es obligatorio")]
        public string? UrlFoto { get; set; }
    }
}
