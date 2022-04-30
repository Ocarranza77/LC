using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class SucursalUsuarioResponse:BaseResponse
    {
        
        public List<SucursalUsuario> SucursalesUsuario { get; set; }
    }
}
