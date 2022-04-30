using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.QSystem.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class DepositoResponse: BaseResponse
    {
        //public CorteSucursal CorteSucursal { get; set; }

        public List<CorteSucursal> CorteSucursales { get; set; }

        public Dictionary<string, List<CboTipo>> CboInis { get; set; }
    }
}
