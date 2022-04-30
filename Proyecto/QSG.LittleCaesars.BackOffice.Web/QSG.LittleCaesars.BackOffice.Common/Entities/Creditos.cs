using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
   public class Creditos
    {
       public bool EnUso { get; set; }
       public DateTime? FechaActivacion { get; set; }
       public DateTime? FechaVencimiento { get; set; }
       public string Paquete { get; set; }
       public int Timbres { get; set; }
       public int TimbresRestantes { get; set; }
       public int TimbresUsados { get; set; }
       public bool Vigente { get; set; }
    }
}
