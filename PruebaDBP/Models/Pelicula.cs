using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Pelicula
    {
        public int IdPelicula { get; set; }
        public int? IdIdioma { get; set; }
        public string? NomPelicula { get; set; }
        public short? FechaEstreno { get; set; }
        public int? DuracionMin { get; set; }
        public string? Sumilla { get; set; }
        public string? UrlFoto { get; set; }
    }
}
