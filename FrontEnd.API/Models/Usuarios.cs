using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrontEnd.API.Models
{
    public partial class Usuarios
    {
        public string Usuario { get; set; }

        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }
        public bool Tipo { get; set; }
        public int? CodEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }

        [Display(Name = "Contraseña actual")]
        public string ContrasenaAnterior { get; set; }

        [Display(Name = "Repetir contraseña")]
        public string Contrasena2 { get; set; }

        [Display(Name = "Empresa")]
        public virtual Empresa CodEmpresaNavigation { get; set; }
    }
}
