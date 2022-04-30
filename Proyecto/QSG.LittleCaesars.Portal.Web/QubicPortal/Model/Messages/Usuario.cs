using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class Usuario_MPH
    {
        public int Codigo { get; set; }

        public string Nombre { get; set; }

        public string Alias { get; set; }

        public PerfilType Perfil { get; set; } 

        public StatusType Status { get; set; } 

        public string CorreoElectronico { get; set; }

        public string Puesto { get; set; }

        public string ClaveAcceso { get; set; }

        public bool SolicitarCambioContrasena { get; set; }

        public bool EnviarContrasena { get; set; }

        public string FAlta { get; set; }
    }
}