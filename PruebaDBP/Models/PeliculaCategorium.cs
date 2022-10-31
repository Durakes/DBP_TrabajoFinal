using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaCategorium
    {
        public int IdPelicula { get; set; }
        public int IdCategoria { get; set; }

        public PeliculaCategorium(int idPelicula, int idCategoria)
        {
            IdPelicula = idPelicula;
            IdCategoria = idCategoria;
        }

        public PeliculaCategorium() { }
    }
}
