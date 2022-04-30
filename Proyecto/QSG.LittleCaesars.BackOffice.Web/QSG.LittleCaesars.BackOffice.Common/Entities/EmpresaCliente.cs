using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("EmpesaCliente")]
    public class EmpresaCliente:BaseEntity
    {
        public string Nombre { get; set; }
        public int EmpresaID { get; set; }

        public string Calle { get; set; }
        public string NoInt { get; set; }
        public string NoExt { get; set; }
        public string Colonia { get; set; }
        public string Delegacion { get; set; }
        public string Ciudad { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string CP { get; set; }
        
        public string RFC { get; set; }
        public string DB { get; set; }

        public int EmpresaContpaqID { get; set; }
        public string EmpresaContpaqName { get; set; }
        public string EmpresaContpaqRutaBD { get; set; }
        public string EmpresaContpaqMascarilla { get; set; }

        public string Serao { get; set; }
        public string PassKey { get; set; }
        public string UserPak { get; set; }
        public string ClavePak { get; set; }
        public string CertificadoCer { get; set; }
        public string CertificadoKey { get; set; }

        public string CertificadoCerSerie { get; set; }
        public string CertificadoCer64bits { get; set; }

        public List<Sucursal> Sucursales { get; set; }

    }
}
