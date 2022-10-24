using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaCategorium
    {
        public int IdPelicula { get; set; }
        public int IdCategoria { get; set; }
    }
}
