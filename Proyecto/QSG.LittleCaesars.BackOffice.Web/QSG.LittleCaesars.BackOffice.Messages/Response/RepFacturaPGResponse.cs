using QSG.LittleCaesars.BackOffice.Common.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class RepFacturaPGResponse: BaseResponse
    {
        public List<RepFacturaPG> Reporte { get; set; }
    }
}
