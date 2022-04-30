using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class ResetPassword
    {
        public bool Bloquear { get; set; }

        public bool ContrasenaXSistema { get; set; }

        public bool ContrasenaXAdmin { get; set; }

        public string ClaveAcceso { get; set; }

        public string FechaReseteo { get; set; }

        public string Administrador { get; set; }

        public bool EnviarContrasena { get; set; }
    }
}