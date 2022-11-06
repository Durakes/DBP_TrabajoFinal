using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Estanterium
    {
        public int IdEstanteria { get; set; } 
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El campo nombre estanteria es obligatorio")]
        public string? NomEstanteria { get; set; }
        //Fecha de creacion se cambio de Date a String
        public string? FechaCreacion { get; set; }
        public bool? EsEditable { get; set; }
    }
   
}
