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
        [Required(ErrorMessage = "El campo nombre de la pelicula es obligatorio")]
        public string? NomPelicula { get; set; }
        [Required(ErrorMessage = "El campo fecha de esteno es obligatorio")]
        public string? FechaEstreno { get; set; }
        [Required(ErrorMessage = "El campo duracion en minutos es obligatorio")]
        public int? DuracionMin { get; set; }
        [Required(ErrorMessage = "El campo sumilla es obligatorio")]
        public string? Sumilla { get; set; }
        [Required(ErrorMessage = "El campo UrlFoto es obligatorio")]
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
    public partial class IndexPelicula
    {
        public Pelicula objPelicula;
        public Usuario usuario;
        public List<Estanterium> listEstanterias;
    }
}
