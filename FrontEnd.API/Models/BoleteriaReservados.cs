using System;
using System.Collections.Generic;
<<<<<<< HEAD

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.API.Models
{
    public partial class BoleteriaReservados
=======
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class BoleteriaReservados
>>>>>>> 5edace1f7eda4c76f842137c3499f2fe47e10079
    {
        public int CodBoletaReservado { get; set; }
        public int CodBoleteria { get; set; }
        public int CodTickets { get; set; }
        public int? Cantidad { get; set; }

        public virtual Boleteria CodBoleteriaNavigation { get; set; }
        public virtual Tickets CodTicketsNavigation { get; set; }
    }
}
