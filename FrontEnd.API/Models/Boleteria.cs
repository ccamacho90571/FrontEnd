
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Boleteria
    {

        public int CodBoleteria { get; set; }
        public int CodEmpresa { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre de este tiquete")]
        [Display(Name = "Descripción")]
        [DataType(DataType.Text)]

        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe ingresar el precio de esta entrada")]
        [DataType(DataType.Currency)]

        public int Costo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
        public virtual ICollection<BoleteriaReservados> BoleteriaReservados { get; set; }
    }
}
