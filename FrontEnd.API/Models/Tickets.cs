
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Tickets
    {

        public int CodTicket { get; set; }

        [Display(Name = "Código de reserva")]
        [DataType(DataType.Text)]
        public string Nreserva { get; set; }
        public string Usuario { get; set; }

        [Display(Name = "Lugar")]
        
        public int CodEmpresa { get; set; }
        public DateTime Fecha { get; set; }
        public int? Estado { get; set; }

        public int Espacio { get; set; }
        public virtual Empresa CodEmpresaNavigation { get; set; }
        public virtual Usuarios UsuarioNavigation { get; set; }
        public virtual ICollection<BoleteriaReservados> BoleteriaReservados { get; set; }
        

    }
}
