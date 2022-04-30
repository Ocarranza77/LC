using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Common.Reports
{
    public class RepFacturaPG
    {
        public string Sucursal { get; set; }
        public int SucursalID { get; set; }
        public double TotalVenta { get; set; }
        public double ImporteFacturaCliente { get; set; }
        public double ImporteFacturaClienteCancelada { get; set; }
        public double ImporteFacturaPG { get; set; }
        public double ImporteFacturaPGCancelada { get; set; }
        public string FoliosFacturaPG {get; set;}
        public int NumeroFacturasPG {get; set;}
        public string Empresa { get; set; }

    }
}
