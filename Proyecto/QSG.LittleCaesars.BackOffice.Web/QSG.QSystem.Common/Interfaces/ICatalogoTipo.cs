using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.Common.Interfaces
{
    interface ICatalogoTipo
    {
        int ID { get; set; }
        string Nombre { get; set; }
        string Abr { get; set; }
        bool Activo { get; set; }
        //DateTime FechaAlta { get; set; }        
    }
}
