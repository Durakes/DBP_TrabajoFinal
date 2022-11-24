using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PruebaDBP.Models
{
    public partial class Idioma
    {
        public int IdIdioma { get; set; }
        [Required(ErrorMessage = "El campo nombre del idioma es obligatorio")]
        public string? NomIdioma { get; set; }
        [Required(ErrorMessage = "El campo abreviacion es obligatorio")]
        public string? Abreviacion { get; set; }

        public Idioma(string nombre, string abrv)
        {
            NomIdioma = nombre;
            Abreviacion = abrv;
        }

        public Idioma(){}
    }
}
