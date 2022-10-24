using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaEstanterium
    {
        public int IdPelicula { get; set; }
        public int IdEstanteria { get; set; }
        //Fecha de Valoracion se cambio de Date a String
        public string FechaAgregacion { get; set; }
    }
}
