using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.Common.Constants
{
    public class Generales
    {
        #region Generales

        public const string strConn = "strConnection";
        #endregion

        #region Mensajes
        public const string msgSigInfo = "Favor de proporcionar la siguiente información: ";
        public const string msgNoGrabo = "La Información no se Grabo: ";
        public const string msgNoInfoAGrabar = "No se envío información a grabar. ";
        public const string msgFaltaDBName = "Favor de Proporcionar la Empresa con la que se trabajara";
        public const string msgConsultaExito = "La Consulta finalizada";
        public const string msgGraboExito = "La informacion se Grabo correctamente";
        public const string msgUsuarioRequest = "No se ha indicado el usuario que solicita la Consulta.";

        #endregion Mensajes

        #region Schemas
        public const string dbo = ".dbo.";
        public const string SElite = ".Selite.";
        #endregion
    }
}
