using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class TicketBL
    {
        #region Public
        public Ticket GetTicket(Ticket ticket, int usRqst, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new Ticket();
            var dal = new TicketDAL();
            
            msg = SatinizateQuery(ticket);

            if (msg != string.Empty)
            {
                FriendlyMessage = FriendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            
            result = dal.GetTicket(ticket, usRqst, ref msg);
            FriendlyMessage = FriendlyMessage + msg;

            return result;
        }

        public List<Ticket> GetTickets(TicketFilter ticket, int usRqst, ref string FriendlyMessage, int tipoTicket)
        {
            string msg = string.Empty;
            var result = new List<Ticket>();
            var dal = new TicketDAL();

            //msg = SatinizateQuery(ticket);
            /*
            if (msg != string.Empty)
            {
                FriendlyMessage = FriendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            */

            result = dal.GetTickets(ticket, usRqst, ref msg, tipoTicket);
            FriendlyMessage = FriendlyMessage + msg;

            return result;
        }

        public bool SaveTicket(Ticket ticket, TicketSaveType saveType, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new TicketDAL();

            msg = SatinizateAlta(ticket);
            if (saveType == TicketSaveType.Factura)
                msg += SatinizateFactura(ticket);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            if (saveType == TicketSaveType.Alta)
            {
                ticket.FechaCaptura = DateTime.Now;
                //ToDo> tk.CodUsuario = ??
            }
            
            result = dal.SaveTicket(ticket, ref friendlyMessage);

            return result;
        }

        public bool SaveTickets(List<Ticket> tickets, TicketSaveType saveType, ref string friendlyMessage)
        {
            string msg = string.Empty;
            string msgGral = string.Empty;
            int count = 0;
            var result = false;
            var dal = new TicketDAL();

            foreach (Ticket tk in tickets)
            {
                count++;
                msg = SatinizateAlta(tk);
                if (saveType == TicketSaveType.Factura)
                    msg += SatinizateFactura(tk);

                if (msg != string.Empty)
                    msgGral += " Del Ticket (" + count.ToString() + "): " + msg + "; ";

                if (saveType == TicketSaveType.Alta)
                {
                    tk.FechaCaptura = DateTime.Now;
                    //ToDo> tk.CodUsuario = ??
                }
            }

            if (msgGral != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msgGral;

                return result;
            }

            result = dal.SaveTickets(tickets, ref friendlyMessage);

            return result;
        }

        public List<Ticket> GetBitacoraTicket(string llave, ref string FriendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<Ticket>();
            var dal = new TicketDAL();

            msg = SatinizateBitacora(llave);
            
            if (msg != string.Empty)
            {
                FriendlyMessage = FriendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }
            

            result = dal.GetBitacoraTicket(llave, ref msg);
            FriendlyMessage = FriendlyMessage + msg;

            return result;
        }

        #endregion

        #region Private
        private string SatinizateQuery(Ticket ticket)
        {
            string msg = string.Empty;
            
            if (ticket.TicketID == 0)
                msg += "Numero de Ticket, ";

            if (ticket.Sucursal.SucursalID == 0)
                msg += "Sucursal, ";

            if (ticket.CajaID < 0)
                msg += "Numero de caja, ";
            
          
           
            if (ticket.Importe == 0)
                msg += "Importe";
            
            return msg;
        }

        private string SatinizateAlta(Ticket ticket)
        { 
            string msg = string.Empty;

            if (ticket.TicketID == 0)
                msg += "Numero de Ticket, ";

            if (ticket.Sucursal.SucursalID == 0)
                msg += "Sucursal, ";

            if (ticket.CajaID == 0)
                msg += "Numero de caja, ";

            if (ticket.Importe == 0)
                msg += "Importe, ";

            if (ticket.FechaVta < new DateTime(2013, 01, 01))
                msg += "Fecha de Venta, ";

            if (ticket.OperationType == OperationType.Delete)
                if (ticket.Cliente != null && ticket.Cliente.RFC != string.Empty )
                    msg += "No se puede Eliminar un ticket ya facturado. ";


            DateTime dt;
            DateTime.TryParse(ticket.FechaCancelacion.ToString(), out dt);

            if (dt != null && dt < new DateTime(2010, 01, 01))
                ticket.FechaCancelacion = null;

            DateTime.TryParse(ticket.FechaFactura.ToString(), out dt);
            if (dt != null && dt < new DateTime(2010, 01, 01))
                ticket.FechaFactura = null;

            return msg;
        }

        private string SatinizateFactura(Ticket ticket)
        {
            string msg = string.Empty;

            if (ticket.FechaFactura < new DateTime(2013, 01, 01))
                msg += "Fecha de la Factura, ";

            if (ticket.FolioFactura == string.Empty)
                msg += "Folio de la Factura, ";

            if (!(ticket.Cliente != null && ticket.Cliente.RFC != string.Empty))
                msg += "Cliente (RFC), ";

            return msg;
        }

        private string SatinizateBitacora(string llave)
        {
            string msg = string.Empty;

            string[] campos;
            campos = llave.Split('|');

            if (campos.Count() < 4)
                msg = "La Llave no contiene todos los elementos de la busqueda";

            return msg;
        }

        #endregion 
    }

}
