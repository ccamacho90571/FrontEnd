using System;
using System.Collections.Generic;
<<<<<<< HEAD

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.API.Models
{
    public partial class ControlAforo
=======
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class ControlAforo
>>>>>>> 5edace1f7eda4c76f842137c3499f2fe47e10079
    {
        public int CodControl { get; set; }
        public int CodEmpresa { get; set; }
        public int NumeroDia { get; set; }
        public int NumeroAforo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
    }
}
