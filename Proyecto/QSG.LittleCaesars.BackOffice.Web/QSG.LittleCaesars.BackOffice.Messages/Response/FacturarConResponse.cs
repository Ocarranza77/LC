using QSG.LittleCaesars.BackOffice.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class FacturarConResponse: BaseResponse
    {
        public FacturarCon Datos { get; set; }

        public Sucursal Sucursal { get; set; }

        public EmpresaCliente Empresa { get; set; }
    }
}
