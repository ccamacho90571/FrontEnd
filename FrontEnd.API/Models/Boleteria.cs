
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Boleteria
    {

        public int CodBoleteria { get; set; }
        public int CodEmpresa { get; set; }
        public string Descripcion { get; set; }
        public int Costo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
        public virtual ICollection<BoleteriaReservados> BoleteriaReservados { get; set; }
    }
}
