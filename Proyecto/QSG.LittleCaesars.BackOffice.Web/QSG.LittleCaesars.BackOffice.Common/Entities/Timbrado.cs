using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    public class Timbrado
    {
        public string Estado { get; set; }
        public DateTime FechaTimbrado { get; set; }
        public string NumeroCertificadoSAT { get; set; }
        public string SelloCFD { get; set; }
        public string SelloSAT { get; set; }
        public string UUID { get; set; }
        public string XML { get; set; }
    }
}
