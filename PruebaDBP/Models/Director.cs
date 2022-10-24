using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Director
    {
        public int IdDirector { get; set; }
        public int? IdMejorPelicula { get; set; }
        public string? NomDirector { get; set; }
        public string? BioDirector { get; set; }
        public string? UrlFoto { get; set; }
    }
}
