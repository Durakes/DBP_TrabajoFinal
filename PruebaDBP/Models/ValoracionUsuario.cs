using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class ValoracionUsuario
    {
        public int IdPelicula { get; set; }
        public int IdUsuario { get; set; }
        public int? Valoracion { get; set; }
        //Fecha de Valoracion se cambio de Date a String
        public string? FechaValoracion { get; set; }
        public int? EstaVisto { get; set; }
    }
}
