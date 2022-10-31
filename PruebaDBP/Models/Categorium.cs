using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Categorium
    {
        public int IdCategoria { get; set; }
        public string? NomCategoria { get; set; }
        public Categorium() { }
        public Categorium(int index,string? nomCategoria)
        {
            IdCategoria = index;
            NomCategoria = nomCategoria;
        }
        public Categorium(string? nomCategoria)
        {
            NomCategoria = nomCategoria;
        }
    }
}
