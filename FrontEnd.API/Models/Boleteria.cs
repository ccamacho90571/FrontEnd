﻿using System;
using System.Collections.Generic;
<<<<<<< HEAD

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FrontEnd.API.Models
{
    public partial class Boleteria
    {
        public Boleteria()
        {
            BoleteriaReservados = new HashSet<BoleteriaReservados>();
        }

=======
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.API.Models
{
    public class Boleteria
    {
>>>>>>> 5edace1f7eda4c76f842137c3499f2fe47e10079
        public int CodBoleteria { get; set; }
        public int CodEmpresa { get; set; }
        public string Descripcion { get; set; }
        public int Costo { get; set; }

        public virtual Empresa CodEmpresaNavigation { get; set; }
        public virtual ICollection<BoleteriaReservados> BoleteriaReservados { get; set; }
    }
}
