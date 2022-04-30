using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.Common.Entities
{
    public class CheckListTipo: BaseEntity
    {
        public int ID { get; set; }
        public string Grupo { get; set; }
        public string Elemento { get; set; }
        public bool Activo { get; set; }
        //public DateTime? FechaAlta { get; set; }

    }
}
