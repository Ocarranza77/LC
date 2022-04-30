using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class DepositoRequest: BaseRequest
    {
        /// <summary>
        /// Necesaria para el grabado
        /// </summary>
        public List<CorteSucursal> CorteSucursales { get; set; }

        /// <summary>
        /// Necesaria para la Consulta; Regresa una lista de Cortes Sucursales (Dailys)
        /// </summary>
        public DateTime Fecha { get; set; }

        public bool GetCbo { get; set; }
    }
}
