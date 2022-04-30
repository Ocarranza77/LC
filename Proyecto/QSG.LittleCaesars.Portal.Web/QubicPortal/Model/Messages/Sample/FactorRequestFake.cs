using QSG.QAGC.Common.Entities;
using QSG.QSystem.Messages.Requests;
using System.Collections.Generic;

namespace QubicPortal.Model.Messages
{
    public class FactorRequestFake: BaseRequest
    {
        public Factor Factor { get; set; }

        public List<Factor> Factores { get; set; }

        /// <summary>
        /// Este Requere de que Factor sea mandado
        /// </summary>
        public bool ValidarFormula { get; set; }
    }
}
