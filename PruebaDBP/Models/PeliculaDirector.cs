using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaDirector
    {
        public int? IdPelicula { get; set; }
        public int? IdDirector { get; set; }

        public PeliculaDirector(int? idPelicula, int? idDirector)
        {
            IdPelicula = idPelicula;
            IdDirector = idDirector;
        }

        public PeliculaDirector() { }
    }
}
