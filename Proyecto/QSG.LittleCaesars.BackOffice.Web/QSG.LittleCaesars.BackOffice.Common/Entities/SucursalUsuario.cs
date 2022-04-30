using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("Sucursal")]
    public class SucursalUsuario:BaseEntity
    {
        public Sucursal Sucursal { get; set; }
        public int UsuarioPermisoID { get; set; }

        public string Nombre { get; set; }
        public DateTime FechaUM { get; set; }
    }
}
