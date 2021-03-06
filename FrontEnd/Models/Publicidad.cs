using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.Models
{
    public partial class Publicidad
    {
        public int CodPublicidad { get; set; }
        public int CodEmpresa { get; set; }
        public string RutaArchivo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
    }
}
