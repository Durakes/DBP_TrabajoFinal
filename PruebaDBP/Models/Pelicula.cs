using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Pelicula
    {
        public int IdPelicula { get; set; }
        public int? IdTmdb { get; set; }
        public int? IdIdioma { get; set; }
        public string? NomPelicula { get; set; }
        public string? FechaEstreno { get; set; }
        public int? DuracionMin { get; set; }
        public string? Sumilla { get; set; }
        public string? UrlFoto { get; set; }


        public Pelicula(int? idTmdb, int? idIdioma, string? nomPelicula, string? fechaEstreno, int? duracionMin, string? sumilla, string? urlFoto)
        {
            IdTmdb = idTmdb;
            IdIdioma = idIdioma;
            NomPelicula = nomPelicula;
            FechaEstreno = fechaEstreno;
            DuracionMin = duracionMin;
            Sumilla = sumilla;
            UrlFoto = urlFoto;
        }

        public Pelicula() { }
    }
}
