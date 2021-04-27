
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Empresa
    {

        public int CodEmpresa { get; set; }
        public string Nombre { get; set; }

        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }

        [Display(Name = "Reservas por usuario")]
        public int ReservasUsuario { get; set; }

        public virtual ICollection<Boleteria> Boleteria { get; set; }
        public virtual ICollection<ControlAforo> ControlAforo { get; set; }
        public virtual ICollection<Publicidad> Publicidad { get; set; }
        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}
