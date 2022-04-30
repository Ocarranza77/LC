using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Entities;

namespace QSG.QSystem.Messages.Requests
{
    public class LoginRequest : BaseRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public int UsuarioID { get; set; }
    }
}
