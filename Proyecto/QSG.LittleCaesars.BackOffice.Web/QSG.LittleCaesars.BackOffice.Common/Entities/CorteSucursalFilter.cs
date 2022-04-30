using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("CorteSucursalFilter")]
    public class CorteSucursalFilter : CorteSucursal
    {
        public DateTime? FechaVtaHasta { get; set; }
        public string Sucursales { get; set; }
        public decimal TotalHasta { get; set; }
    }
}
