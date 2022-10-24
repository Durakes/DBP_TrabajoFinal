using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PruebaDBP.Models
{
    public partial class Idioma
    {
        [Required(ErrorMessage = "El campo ID idioma es obligatorio")]
        public int IdIdioma { get; set; }
        [Required(ErrorMessage = "El campo nombre del idioma es obligatorio")]
        public string? NomIdioma { get; set; }
    }
}
