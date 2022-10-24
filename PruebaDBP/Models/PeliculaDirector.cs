using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaDirector
    {
        public int IdPelicula { get; set; }
        public int IdDirector { get; set; }
    }
}
