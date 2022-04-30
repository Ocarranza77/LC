using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class UsuarioResponse:BaseResponse
    {
        public Usuario Usuario { get; set; }

        public List<Usuario> Usuarios { get; set; }
    }
}
