using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QResponse = QSG.QSystem.Messages.Response;

namespace QSG.QSystem.Messages.Response
{
    public class CiudadResponse : QResponse.BaseResponse
    {
        public List<Ciudad> Ciudades { get; set; }

        public List<CboTipo> CboInis { get; set; }

        //Regresa valor solo en caso de alta
        public int CiudadID { get; set; }

    }
}
