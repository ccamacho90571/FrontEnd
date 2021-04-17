
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class BoleteriaReservados
    {
        public int CodBoletaReservado { get; set; }
        public int CodBoleteria { get; set; }
        public int CodTickets { get; set; }
        public int? Cantidad { get; set; }

        public virtual Boleteria CodBoleteriaNavigation { get; set; }
        public virtual Tickets CodTicketsNavigation { get; set; }
    }
}
