using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRequests = QSG.QSystem.Messages.Requests;

namespace QSG.QSystem.Messages.Requests
{
    public class PaisRequest : QRequests.BaseRequest
    {
        public Pais Pais { get; set; }

        public List<Pais> Paises { get; set; }

        public bool GetCboNacionalidad { get; set; }

        public bool GetCbo { get; set; }

    }
}
