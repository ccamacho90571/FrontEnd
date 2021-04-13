using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.API.Models
{
    public partial class ControlAforo
    {
        public int CodControl { get; set; }
        public int CodEmpresa { get; set; }
        public int NumeroDia { get; set; }
        public int NumeroAforo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
    }
}
