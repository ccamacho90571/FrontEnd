using System;
using System.Collections.Generic;


namespace FrontEnd.API.Models
{
    public partial class Usuarios
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public bool Tipo { get; set; }
        public int? CodEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
    }
}
