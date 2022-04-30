using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.Common.Entities
{
    public class CatalogoTipo: BaseEntity
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Abr { get; set; }
        public bool Activo { get; set; }
        //public DateTime? FechaAlta { get; set; }        

    }
}
