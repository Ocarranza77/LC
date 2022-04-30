using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRequests = QSG.QSystem.Messages.Requests;

namespace QSG.QSystem.Messages.Requests
{
    public class CiudadRequest : QRequests.BaseRequest
    {
        public Ciudad Ciudad { get; set; }

        public List<Ciudad> Ciudades { get; set; }

        public bool GetCbo { get; set; }

    }
}
