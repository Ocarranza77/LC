using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Enums;

namespace QSG.QSystem.Common.Entities
{
    public class BaseEntity
    {
        public OperationType OperationType { get; set; }

        public string CodUsAlta { get; set; }
        public string CodUsAltaNombre { get; set; }

        public string CodUsUM { get; set; }
        public string CodUsUMNombre { get; set; }

        public string CodUsAct { get; set; }

        public int UsuarioID { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechauM { get; set; }
 
    }
}
