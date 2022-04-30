using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.Common.Entities
{
    public class CboTipo
    {
        /// <summary>
        /// Llave primaria  si es numerica se debe castear.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Nombre largo del dato
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Abreviatura del dato (segunda opcion para desplegar.)
        /// </summary>
        public string Abr { get; set; }
    }
}
