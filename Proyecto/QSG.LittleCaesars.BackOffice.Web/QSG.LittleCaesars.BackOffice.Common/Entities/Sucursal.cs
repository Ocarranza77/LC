//using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("Sucursal")]
    public class Sucursal:QSystem.Common.Entities.BaseEntity
    {
        public  int SucursalID { get; set; }
        public string Nombre { get; set; }
        public string Abr { get; set; }
        public string Descripcion { get; set; }
        
        public string Calle { get; set; }
        public string NoInt { get; set; }
        public string NoExt { get; set; }
        public string Colonia { get; set; }
        public string Delegacion { get; set; }
        public string Ciudad { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string CP { get; set; }

        public string Serie { get; set; }

        public double Iva { get; set; }

        public EmpresaCliente Empresa { get; set; }
    }
}
