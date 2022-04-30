using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Common.Reports
{
    public class RepIngresosDSM
    {
        /// <summary>
        /// Str = Sucursal
        /// Dub = Importe
        /// </summary>
        public List<BaseStrDub> Dia { get; set; }

        /// <summary>
        /// Str = Dia de la semana
        /// Dub = Importe
        /// </summary>
        public List<BaseStrDub> Semana { get; set; }

        /// <summary>
        /// Str = Mes del año
        /// Dub = Importe
        /// </summary>
        public List<BaseStrDub> Mes { get; set; }
 
    }
}
