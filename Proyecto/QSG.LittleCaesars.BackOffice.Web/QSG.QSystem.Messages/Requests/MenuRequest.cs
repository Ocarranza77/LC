using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Entities;

namespace QSG.QSystem.Messages.Requests
{
    public class MenuRequest : BaseRequest
    {
        public MenuP Menu { get; set; }
    }
}
