
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class ControlAforo

    {
        public int CodControl { get; set; }


        [Display(Name = "Institución")]
        public int CodEmpresa { get; set; }
        
        public string NombreDia
        {
            get
            {
                return TraerDia();
                
            }

            set { }
        }

        private string TraerDia()
        {
            string Dia = "";
            switch (NumeroDia)
            {
                case 1:
                    Dia = "Lunes";
                    break;
                case 2:
                    Dia = "Martes";
                    break;
                case 3:
                    Dia = "Miércoles";
                    break;
                case 4:
                    Dia = "Jueves";
                    break;
                case 5:
                    Dia = "Viernes";
                    break;
                case 6:
                    Dia = "Sábado";
                    break;
                case 7:
                    Dia = "Domingo";
                    break;
            }
            return Dia;
        }
        
        [Display(Name = "Día")]
        [Required(ErrorMessage = "Debe ingresar un día")]
        public int NumeroDia { get; set; }

        [Display(Name = "Cantidad de personas")]
        [Required(ErrorMessage = "Debe digitar una cantidad")]
        public int NumeroAforo { get; set; }


        [Display(Name = "Institución")]
        public virtual Empresa CodEmpresaNavigation { get; set; }

   


    }
}
