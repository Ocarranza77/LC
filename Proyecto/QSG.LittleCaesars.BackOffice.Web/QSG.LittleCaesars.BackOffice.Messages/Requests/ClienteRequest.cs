using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class ClienteRequest : BaseRequest
    {
        public Cliente Cliente { get; set; }

        public List<Cliente> Clientes { get; set; }
    }
}
