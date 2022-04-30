using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using System.Xml;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class CorteZResponse: BaseResponse
    {
        public CorteZ CorteZ { get; set; }

        public List<CorteZ> CorteZs { get; set; }

        public string XML { get; set; }
    }
}
