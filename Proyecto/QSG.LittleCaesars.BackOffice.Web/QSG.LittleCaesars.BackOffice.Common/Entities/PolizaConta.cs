using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("PolizaConta")]
    public class PolizaConta : BaseEntity
    {
        public string CodPoliza { get; set; }

        public string Tipo { get; set; }
        public string Poliza { get; set; }
        public DateTime Fecha { get; set; }
        public string Clase{ get; set; }
        public string Diario { get; set; }
        public string Concepto { get; set; }
        public string Sistema { get; set; }
        public string SttCon { get; set; }
        public string SttConIni { get; set; }
        public DateTime? FechaImpresa { get; set; }
        public bool CuadreConOrigen { get; set; }
        public float TC { get; set; }
        public float ImportePol { get; set; }
        public string Origen { get; set; }
        public string OrigenCodigo { get; set; }

        public List<PolizaContaDT> Asientos { get; set; }
        public List<PolizaContaStatus> Status { get; set; }

    }
}
