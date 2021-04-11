using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Usuarios
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public bool Tipo { get; set; }
        public int CodEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
