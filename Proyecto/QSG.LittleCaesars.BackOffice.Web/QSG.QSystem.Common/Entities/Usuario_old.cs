using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities
{
    [Serializable, XmlRoot("Usuario")]
    public class Usuario_old : BaseEntity
    {
        public int CodUsuario { get; set; }
        public string Alias { get; set; }
        public string Clave { get; set; }
        public string TipoPeriodo { get; set; }
        public string PrimerLogin { get; set; }
        public string Dias { get; set; }
        public string Fecha { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public string Adscripcion { get; set; }
        public string NoEmpleado { get; set; }

    }
}
