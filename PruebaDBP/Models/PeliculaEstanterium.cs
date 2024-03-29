﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class PeliculaEstanterium
    {
        public int IdPelicula { get; set; }
        public int IdEstanteria { get; set; }
        //Fecha de Valoracion se cambio de Date a String
        public string? FechaAgregacion { get; set; }
    }
    public class LibreriaPelis
    {
        //public List<PeliculaLib> listPelis { get; set; }
        public Paginador<PeliLib> listPelis { get; set; }
        public List<Estanterium> listLib { get; set; }
        public Estanterium LibAct{ get; set; }
        
    }


    public class PeliLib
    {
        public int IdPelicula { get; set; }
        public int? IdTmdb { get; set; }
        public string? NomPelicula { get; set; }
        public string? NomDirector { get; set; }
        public string? UrlFoto { get; set; }
        public string? FechaAgregacion { get; set; }
        public int? DuracionMin { get; set; }
        public string? Sumilla { get; set; }
        public decimal? Valoracion { get; set; }
        public List<Director> directores{get;set;}
        public ValoracionUsuario valoracion;


    }
}
