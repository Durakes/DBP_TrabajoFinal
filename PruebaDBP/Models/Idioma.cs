using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PruebaDBP.Models
{
    public partial class Idioma
    {
        public int IdIdioma { get; set; }
        public string? NomIdioma { get; set; }
        public string? Abreviacion { get; set; }

        public Idioma(string nombre, string abrv)
        {
            NomIdioma = nombre;
            Abreviacion = abrv;
        }

        public Idioma(){}
    }
}
