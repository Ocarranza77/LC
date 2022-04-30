using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.QSystem.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class CorteSucursalResponse: BaseResponse
    {
        //public CorteSucursal CorteSucursal { get; set; }

        public List<CorteSucursal> CorteSucursales { get; set; }

        public List<CboTipo> CorteSucursalesStt { get; set; }
        public string XML { get; set; }
    }
}
