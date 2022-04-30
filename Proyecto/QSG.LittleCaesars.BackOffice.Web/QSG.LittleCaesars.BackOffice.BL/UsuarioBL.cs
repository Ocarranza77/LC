using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using System.Text.RegularExpressions;
using QSG.QSystem.Common.Constants;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class UsuarioBL
    {
        public Usuario GetUsuario(Usuario user, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new Usuario();
            var dal = new UsuarioDAL();

            msg = Satinizate(user);

            if (msg != string.Empty)
            {
                FriendlyMessage = FriendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.GetUsuario(user, ref msg);
            FriendlyMessage = FriendlyMessage + msg;

            return result;
        }


        public bool SaveUsuarios(Usuario usuario, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new UsuarioDAL();

            //msg = SatinizateAlta(ticket);
            //if (saveType == TicketSaveType.Factura)
            //    msg += SatinizateFactura(ticket);

            //if (msg != string.Empty)
            //{
            //    friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

            //    return result;
            //}


            result = dal.SaveUsuario(usuario, ref friendlyMessage);

            return result;

        }

        public List<Usuario> GetUsuarios(Usuario usuario, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<Usuario>();
            var dal = new UsuarioDAL();

            //msg = SatinizateQuery(ticket);
            /*
            if (msg != string.Empty)
            {
                FriendlyMessage = FriendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            */

            result = dal.GetUsuarios(usuario, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;

        }

        private string Satinizate(Usuario user)
        {
            string msg = string.Empty;

            /*if (user.CodUsuario == 0) {
                msg += "Codigo Usuario";
            }*/

           
            if (user.Alias  == "")
            {
                msg += "Usuario  ";
            }
            if (user.Clave == "")
            {
                msg += "Contraseña";
            }


            




            return msg;
        }
    }
}
