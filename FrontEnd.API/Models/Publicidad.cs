
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Publicidad

    {
        public int CodPublicidad { get; set; }
        public int CodEmpresa { get; set; }

        [Display(Name = "Imagen")]
        [DataType(DataType.Text)]
        public string RutaArchivo { get; set; }

        [Required(ErrorMessage = "Debe cargar una imagen")]
        [Display(Name = "Imagen")]
        public IFormFile Archivo { get; set; }
        public virtual Empresa CodEmpresaNavigation { get; set; }
    }
}
