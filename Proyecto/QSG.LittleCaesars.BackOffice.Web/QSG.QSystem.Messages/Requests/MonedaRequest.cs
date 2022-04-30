using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRequests = QSG.QSystem.Messages.Requests;

namespace QSG.QSystem.Messages.Requests
{
    public class MonedaRequest : QRequests.BaseRequest
    {
        public Moneda Moneda { get; set; }

        public List<Moneda> Monedas { get; set; }

        public bool GetCbo { get; set; }

    }
}
