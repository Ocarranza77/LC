using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class RepFacturaPGRequest: BaseRequest
    {
        public DateTime fecha { get; set; }
    }
}
