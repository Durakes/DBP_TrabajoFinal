using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Usuario
    {
        [Required(ErrorMessage = "El campo ID usuario es obligatorio")]
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
        public DateOnly? FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El campo descripcion es obligatorio")]
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "El campo fecha de creacion es obligatorio")]
        public DateOnly? FechaCreacion { get; set; }
        [Required(ErrorMessage = "El campo foto de perfil es obligatorio")]
        public string? UrlFoto { get; set; }
    }
}
