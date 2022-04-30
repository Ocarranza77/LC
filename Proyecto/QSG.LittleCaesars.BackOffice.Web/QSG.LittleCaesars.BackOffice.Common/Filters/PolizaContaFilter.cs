using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Common.Filters
{
    public class PolizaContaFilter: PolizaConta
    {
        public DateTime FechaFin { get; set; }
        public DateTime FechaAltaFin { get; set; }
        public DateTime FechaUMFin { get; set; }
    }
}
