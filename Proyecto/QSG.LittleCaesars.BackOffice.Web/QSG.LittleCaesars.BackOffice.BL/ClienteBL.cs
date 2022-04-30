using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class ClienteBL
    {
        #region Public
        public Cliente GetCliente(Cliente cliente, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new Cliente();
            var dal = new ClienteDAL();

            msg = SatinizateQuery(cliente);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;
                
                return result;
            }

            result = dal.GetCliente(cliente, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool SaveCliente(Cliente cliente, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new ClienteDAL();

            msg = Satinizate(cliente);
            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SaveCliente(cliente, ref friendlyMessage);

            return result;
        }

        public bool SaveClientes(List<Cliente> clientes, ref string friendlyMessage)
        {
            string msg = string.Empty;
            string msgGral = string.Empty;
            int count = 0;
            var result = false;
            var dal = new ClienteDAL();

            foreach (Cliente cli in clientes)
            {
                count++;   
                msg = Satinizate(cli);

                if (msg != string.Empty)
                    msgGral += " Del Cliente (" + count.ToString() + "): " + msg + "; "; 
            }
            
            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SaveClientes(clientes, ref friendlyMessage);

            return result;
        }
        #endregion

        #region Private
        private string SatinizateQuery(Cliente cliente)
        {
            string msg = string.Empty;

            if (cliente.RFC == string.Empty)
                msg += "RFC, ";

            return msg;
        }

        private string Satinizate(Cliente cliente)
        {
            string msg = string.Empty;

            if (cliente.RFC == string.Empty)
                msg += "RFC, ";

            if (cliente.RazonSocial == string.Empty)
                msg += "Rason Social, ";

            if (cliente.Email1 == string.Empty)
                msg += "Correo Electronico, ";

            return msg;
        }

        #endregion

    }
}
