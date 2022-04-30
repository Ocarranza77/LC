using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.QSystem.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class CuentaBancoResponse:BaseResponse
    {
        public CuentaBanco CuentaBanco { get; set; }
        public List<CuentaBanco> CuentaBancos { get; set; }

        public List<CboTipo> CboInis { get; set; }

        public int CuentaBancoID { get; set; }
    }
}
