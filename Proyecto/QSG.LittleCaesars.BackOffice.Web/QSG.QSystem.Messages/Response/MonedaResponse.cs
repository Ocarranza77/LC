using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QResponse = QSG.QSystem.Messages.Response;

namespace QSG.QSystem.Messages.Response
{
    public class MonedaResponse : QResponse.BaseResponse
    {
        public List<Moneda> Monedas { get; set; }

        public List<CboTipo> CboInis { get; set; }

        //Regresa valor solo en caso de alta
        public int MonedaID { get; set; }

    }
}
