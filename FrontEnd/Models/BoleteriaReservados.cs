using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.Models
{
    public partial class BoleteriaReservados
    {
        public int CodBoletaReservado { get; set; }
        public int CodBoleteria { get; set; }
        public int CodTickets { get; set; }
        public int? Cantidad { get; set; }

        public virtual Boleteria CodBoleteriaNavigation { get; set; }
        public virtual Tickets CodTicketsNavigation { get; set; }
    }
}
