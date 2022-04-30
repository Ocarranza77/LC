using QSG.QAGC.Common.Entities;
using QSG.QSystem.Messages.Response;
using System.Collections.Generic;

namespace QubicPortal.Model.Messages
{
    public class FactorResponseFake : BaseResponse
    {
        public List<Factor> Factores { get; set; }

        public bool Save { get; set; }

        public bool Validacion { get; set; }
    }
}
