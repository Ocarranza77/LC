using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Entities;

namespace QSG.QSystem.Messages.Response
{
    public class MetodoPagoSATResponse:BaseResponse
    {
        public List<MetodoPagoSAT> MetodoPagos { get; set; }
        public List<CboTipo> ListCboTipo { get; set; }
    }
}
