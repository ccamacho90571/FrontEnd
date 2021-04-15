using System;
using System.Collections.Generic;
<<<<<<< HEAD

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.API.Models
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

=======
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Empresa
    {
>>>>>>> 5edace1f7eda4c76f842137c3499f2fe47e10079
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
