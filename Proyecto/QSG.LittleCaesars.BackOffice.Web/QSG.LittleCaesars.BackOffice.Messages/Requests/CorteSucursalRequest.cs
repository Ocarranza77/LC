using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class CorteSucursalRequest: BaseRequest
    {
        public List<CorteSucursal> CorteSucursales { get; set; }

        public CorteSucursalFilter Filters { get; set; }

        /// <summary>
        /// Necesario para la busqueda de los Stt de los CortesSucurslaes (junto con SucursalesID)
        /// </summary>
        public DateTime FechaVta { get; set; }
        /// <summary>
        /// Necesario para la busqueda de los Stt de los CortesSucurslaes (junto con FechaVta)
        /// </summary>
        public List<int> SucursalesID { get; set; }
        public bool ReturnXML { get; set; }
    }
}
