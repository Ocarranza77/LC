using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Entities.SElite;

namespace QSG.QSystem.Messages.Requests
{
    public class UsuarioRequest : BaseRequest
    {
        public Usuario Usuario { get; set; }
    }
}
