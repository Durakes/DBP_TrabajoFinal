using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaDBP.Models
{
    public partial class Categorium
    {
        [Required(ErrorMessage = "El campo ID Categoria es obligatorio")]
        public int IdCategoria { get; set; }
        [Required(ErrorMessage = "El campo nombre de categoria es obligatorio")]
        public string? NomCategoria { get; set; }
    }
}
