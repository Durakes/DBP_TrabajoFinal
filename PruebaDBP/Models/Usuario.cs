﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#nullable disable

namespace PruebaDBP.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El campo nombre de usuario es obligatorio")]
        public string? NomUsuario { get; set; }
        [Required(ErrorMessage = "El campo apellido usuario es obligatorio")]
        public string? ApeUsuario { get; set; }
        [Required(ErrorMessage = "El campo username es obligatorio")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "El campo contraseña es obligatorio")]
        public string? Contraseña { get; set; }
        [Required(ErrorMessage = "El campo fecha de nacimiento es obligatorio")]
        //Fecha de nacimiento se cambio de Date a String
        public string? FechaNacimiento { get; set; }
        public string? Descripcion { get; set; }
        //Fecha de creacion se cambio de Date a String
        public string? FechaCreacion { get; set; }
        public string? UrlFoto { get; set; }
    }
    public partial class UsuarioLogin
    {
        [Required(ErrorMessage = "El campo username es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "El campo contraseña es obligatorio")]
        public string Contraseña { get; set; }
        public string? FechaNacimiento { get; set; }

    }
    public partial class UsuarioLogin2
    {
        [Required(ErrorMessage = "El campo username es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "El campo Fecha de nacimiento es obligatorio")]
        public string? FechaNacimiento { get; set; }
        public int IdUsuario { get; set; }

    }
    public partial class UsuarioModif
    {
        public int IdUsuario { get; set; }

        public string? NomUsuario { get; set; }

        public string? ApeUsuario { get; set; }

        public string? Username { get; set; }

        public string? Descripcion { get; set; }
        public string? Contraseña { get; set; }
        public string? UrlFoto { get; set; }
        public string? ContraseñaActual { get; set; }
        public string? ContraseñaNueva1 { get; set; }
        public string? ContraseñaNueva2 { get; set; }

    }

    public partial class Cambio
    {
        
        [Required(ErrorMessage = "Modifique su contraseña")]
        public string? ContraseñaNueva1 { get; set; }
        [Required(ErrorMessage = "Confirme su contraseña")]
        public string? ContraseñaNueva2 { get; set; }
    }
    public class PeliAgre
    {
        public int IdPelicula { get; set; }
        public int? IdTmdb { get; set; }
        public string? NomPelicula { get; set; }
        public List<Director> directoresLista;
        public string? UrlFoto { get; set; }
        public string? FechaEstreno { get; set; }
        public string? Sumilla { get; set; }
        public string? estanteria { get; set; }
        public int IdEstanteria { get; set; }
        public string? FechaAgregar { get; set; }
        public ValoracionUsuario valoracion { get; set; }

    }

    public class PerfilPelis
    {
        public List<PeliAgre> Agregadas { get; set; }
        public int IdUsuario { get; set; }
        public string? NomUsuario { get; set; }

        public string? ApeUsuario { get; set; }

        public string? Username { get; set; }

        public string? Descripcion { get; set; }
        public string? Contraseña { get; set; }
        public string? UrlFoto { get; set; }
        public string? FechaCreacion { get; set; }
    }
    public partial class Recomendaciones
    {
        public Pelicula peli1 { get; set; }
        public Pelicula peli2 { get; set; }
        public Pelicula peli3 { get; set; }
    }
    
}
