using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Categorium
    {
        public int IdCategoria { get; set; }
        public string? NomCategoria { get; set; }
        public int? IdTmdbCategoria { get; set; }
        public Categorium() { }
        public Categorium(int index,string? nomCategoria, int? idTmdb)
        {
            IdCategoria = index;
            NomCategoria = nomCategoria;
            IdTmdbCategoria = idTmdb;
        }
        public Categorium(int? idTmdb, string? nomCategoria)
        {
            IdTmdbCategoria = idTmdb;
            NomCategoria = nomCategoria;
        }
    }
}
