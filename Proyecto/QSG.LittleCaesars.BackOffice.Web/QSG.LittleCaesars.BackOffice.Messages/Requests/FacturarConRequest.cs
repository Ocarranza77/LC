using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class FacturarConRequest: BaseRequest
    {
        public int SucursalID { get; set; }
    }
}
