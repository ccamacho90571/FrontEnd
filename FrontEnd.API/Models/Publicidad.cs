
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Publicidad

    {
        public int CodPublicidad { get; set; }
        public int CodEmpresa { get; set; }
        public string RutaArchivo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
    }
}
