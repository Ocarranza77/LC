using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRequests = QSG.QSystem.Messages.Requests;

namespace QSG.QSystem.Messages.Requests
{
    public class CatalogoTipoRequest : QRequests.BaseRequest
    {
        public CatalogoTipo CatalogoTipo { get; set; }

        public List<CatalogoTipo> CatalogoTipos { get; set; }

        public CatalogoType type { get; set; }

        public bool GetCbo { get; set; }

    }
}
