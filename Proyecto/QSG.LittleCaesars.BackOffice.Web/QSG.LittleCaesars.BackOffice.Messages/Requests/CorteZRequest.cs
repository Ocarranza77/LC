using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class CorteZRequest: BaseRequest
    {
        public CorteZ CorteZ { get; set; }

        public List<CorteZ> CorteZs { get; set; }

        public CorteZFilter Filters { get; set; }

        public bool ReturnXML{ get; set; }
    }
}
