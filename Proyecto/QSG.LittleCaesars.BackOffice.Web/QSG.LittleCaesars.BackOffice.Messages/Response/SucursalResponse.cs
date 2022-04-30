using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class SucursalResponse:BaseResponse
    {
        public Sucursal Sucursal { get; set; }
        public List<Sucursal> Sucursales { get; set; }
    }
}
