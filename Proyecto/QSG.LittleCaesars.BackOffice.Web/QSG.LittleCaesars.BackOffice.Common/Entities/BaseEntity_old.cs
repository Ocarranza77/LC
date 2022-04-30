using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    public class BaseEntity_old
    {
        public OperationType OperationType { get; set; }
        
        public string CodUsAlta { get; set; }
        public string CodUsAltaNombre { get; set; }

        public string CodUsUM { get; set; }
        public string CodUsUMNombre { get; set; }

        public string CodUsAct { get; set; }

    }
}
