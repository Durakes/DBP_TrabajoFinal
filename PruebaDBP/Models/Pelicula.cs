﻿using System;
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
        [Required(ErrorMessage = "El campo valoracion es obligatorio")]
        public decimal? Valoracion { get; set; }
        [Required(ErrorMessage = "El campo sumilla es obligatorio")]
        public string? Sumilla { get; set; }
        [Required(ErrorMessage = "El campo UrlFoto es obligatorio")]
        public string? UrlFoto { get; set; }


        public Pelicula(int? idTmdb, int? idIdioma, string? nomPelicula, string? fechaEstreno, decimal? valoracion, string? sumilla, string? urlFoto)
        {
            IdTmdb = idTmdb;
            IdIdioma = idIdioma;
            NomPelicula = nomPelicula;
            FechaEstreno = fechaEstreno;
            Valoracion = valoracion;
            Sumilla = sumilla;
            UrlFoto = urlFoto;
        }

        public Pelicula() { }
    }
    public partial class ComentarioIndex
    {
        public Comentario CometariosUsers;
        public ValoracionUsuario UsuarioValor;
        public Usuario User;
    }
    public partial class IndexPelicula
    {
        public Pelicula objPelicula;
        public Usuario usuario;
        public ValoracionUsuario valoracion;
        public List<Estanterium> listEstanterias;
        public List<Director> listDirectores;
        public List<String> listCategoria;
        public List<ComentarioIndex> listComentario;

    }
    public partial class PeliculaTop
    {
        public Pelicula PeliTop;
        public List<Director> directoresLista;
        public List<String> listaCategorias;
    }
}
