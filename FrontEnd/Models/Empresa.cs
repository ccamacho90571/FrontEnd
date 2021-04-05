using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            Boleteria = new HashSet<Boleteria>();
            ControlAforo = new HashSet<ControlAforo>();
            Publicidad = new HashSet<Publicidad>();
            Usuarios = new HashSet<Usuarios>();
        }

        public int CodEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public int ReservasUsuario { get; set; }

        public virtual ICollection<Boleteria> Boleteria { get; set; }
        public virtual ICollection<ControlAforo> ControlAforo { get; set; }
        public virtual ICollection<Publicidad> Publicidad { get; set; }
        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}
