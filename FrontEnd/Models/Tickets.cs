using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.Models
{
    public partial class Tickets
    {
        public Tickets()
        {
            BoleteriaReservados = new HashSet<BoleteriaReservados>();
        }

        public int CodTicket { get; set; }
        public string Nreserva { get; set; }
        public string Usuario { get; set; }
        public int CodEmpresa { get; set; }
        public DateTime Fecha { get; set; }
        public int? Estado { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
        public virtual Usuarios UsuarioNavigation { get; set; }
        public virtual ICollection<BoleteriaReservados> BoleteriaReservados { get; set; }
    }
}
