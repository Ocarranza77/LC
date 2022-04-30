using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
   public class SucursalUsuarioRequest:BaseRequest
    {
       public int SucursalID { get; set; }
       public int UsuarioID { get; set; }

       public List<SucursalUsuario> SucursalesUsuario { get; set; }
    }
}
