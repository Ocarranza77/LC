using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.QSystem.Common.Entities.SElite
{
    [Serializable, XmlRoot("Usuario")]
    public class Usuario : BaseEntity
    {
        //public int UsuarioID { get; set; }
        public string Email { get; set; }
        public string UsPwd { get; set; }
        public string Alias { get; set; }
        //public DateTime FechaAlta { get; set; }
        public string Titulo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaBaja { get; set; }
        public string PhotoPath { get; set; }
        public bool Activo { get; set; }
        public bool Bloqueo { get; set; }
        public int Tipo { get; set; }
        public int ShortCutMenuTpe { get; set; }
        public Persona Persona { get; set; }
        public ClienteQ ClienteQ { get; set; }



        public string Clave { get; set; }
        public string TipoPeriodo { get; set; }
        public string PrimerLogin { get; set; }
        public string Dias { get; set; }
        public string Fecha { get; set; }
        //public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public string Adscripcion { get; set; }
        public string NoEmpleado { get; set; }

    }
}
