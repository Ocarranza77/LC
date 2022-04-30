﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Entities.SElite;

namespace QSG.QSystem.Messages.Response
{
    public class LoginResponse : BaseResponse
    {
        public List<Usuario> Usuarios { get; set; }

        public List<App> Apps { get; set; }
    }
}
