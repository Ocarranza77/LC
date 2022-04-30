using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.QSystem.Common.Enums;


namespace QSG.LittleCaesars.Test
{
    public class TicketTest
    {

        public void GetTicket()
        {
            Console.WriteLine("Inicio Consola Get");

            var tck = new Ticket();

            tck.TicketID = 1;
            tck.CajaID = 2;
            tck.Importe = 100.25;
            tck.Sucursal = new Sucursal() { SucursalID = 1 };


            var dal = new TicketDAL();
            string msg = string.Empty;
            int usRqst = 1;

            var tk = dal.GetTicket(tck, usRqst, ref msg);

            Console.WriteLine(tk.Sucursal.Nombre);
            Console.WriteLine("Fin Prueba DAL");

            Console.ReadLine();


            var sv = new ServiceImplementation();

            var tr = new TicketRequest();

            //tr.Ticket = tck;
            //tr.MessageOperationType = MessageOperationType.Query;
            tr.Ticket.Importe = 0;

            var response = sv.TicketMessage(tr);

            Console.WriteLine(response.FriendlyMessage);

            if (response.Ticket.Sucursal != null)
                Console.WriteLine(response.Ticket.Sucursal.Nombre);
            Console.WriteLine("Fin Prueba Mensaje");

            Console.ReadLine();

        }

        public void SaveTicket()
        {
            Console.WriteLine("Inicio Prueba DAL Save");
            var tck = new Ticket();
            bool b;



//            TicketID	SucursalID	CajaID	Importe	FechaVta
//1	6	1	14.00	2014-11-14

            tck.TicketID = 1;
            tck.CajaID = 1;
            tck.Importe = 14.00;
            tck.Sucursal = new Sucursal() { SucursalID = 6 };
            tck.FechaCaptura = DateTime.Now;
            tck.FechaVta = new DateTime(2014,11,14);
            tck.HoraVta = DateTime.Now.ToString("HH:mm:ss");


            //tck.OperationType = BackOffice.Common.Enums.OperationType.New;
            tck.OperationType = OperationType.Edit;

            tck.Cajero = "Kakaroto";
            tck.FolioFactura = "199";
            tck.Cliente = new Cliente() { RFC = "RFC1234" };

            var dal = new TicketDAL();
            string msg = string.Empty;
            int usRqst = 1;
            #region Grabado de un Ticket
            
            var tk = dal.SaveTicket(tck,ref msg);// .GetTicket(tck, ref msg);

            Console.WriteLine("Grabado: " + tk.ToString());

            tck.HoraVta = "";
            var tkc = dal.GetTicket(tck, usRqst, ref msg);
            Console.WriteLine("Consulta Hora: " + tkc.Cliente.RFC );


            Console.WriteLine("Fin Prueba DAL Save individual");

            Console.ReadLine();
            
            #endregion

            #region Grabado en Masa
            /*
            // Prueba para grabar en masa
            var tkl = new List<Ticket>();
            for (int i = 1; i < 3; i++ )
            {
                tck = new Ticket();
                tck.TicketID = 4 + i;
                tck.CajaID = 1;
                tck.Sucursal = new Sucursal() { SucursalID = 1 };
                tck.Importe = 199;
                tck.Cajero = "La mas trucha";
                tck.FechaCaptura = DateTime.Now;
                tck.HoraVta = DateTime.Now.ToString("HH:mm:ss");
                tck.OperationType = BackOffice.Common.Enums.OperationType.New;
                tkl.Add(tck);
            }
            msg = "";
            b = dal.SaveTickets(tkl, ref msg);
            Console.WriteLine("Grabado: " + b.ToString() + ";   " + msg);
            Console.WriteLine("Fin Prueba DAL Save Masiva");
            Console.ReadLine();
            */
            #endregion


            #region Update
            /*
            Console.WriteLine("Prueba de Update ");
            tck.TicketID = 0;
            tck.CajaID = 1;
            tck.Importe = 43000;
            tck.Sucursal = new Sucursal() { SucursalID = 1 };
            tck.FechaVta  = new DateTime(2014,6,1);

            tck.Cajero = "Goku";
            tck.FechaCaptura = DateTime.Now;
            tck.HoraVta = "24:00:00";
            tck.CodUsuario = 2;

            tck.OperationType = BackOffice.Common.Enums.OperationType.Edit;

            var tk = dal.SaveTicket(tck, ref msg);// .GetTicket(tck, ref msg);

            Console.WriteLine("Grabado: " + tk.ToString());
            */
            #endregion

            #region Eliminar
            /*
            Console.WriteLine("Prueba de Borrado ");
            tck.TicketID = 0;
            tck.CajaID = 1;
            tck.Importe = 45000;
            tck.Sucursal = new Sucursal() { SucursalID = 1 };
            tck.FechaVta = new DateTime(2014, 6, 1);

            tck.Cajero = "Goku";
            tck.FechaCaptura = DateTime.Now;
            tck.HoraVta = "24:00:00";
            tck.CodUsuario = 2;

            tck.OperationType = BackOffice.Common.Enums.OperationType.Delete;

            var tk = dal.SaveTicket(tck, ref msg);// .GetTicket(tck, ref msg);

            Console.WriteLine("Grabado: " + tk.ToString());
            */
            #endregion
        }
        public void ReporteTicket()
        {
            Console.WriteLine("Inicio Prueba DAL Reporte");
            var tck = new TicketFilter();
            
            string msg = string.Empty;

            //tck.TicketID = 4;
            //tck.CajaID = 2;
            //tck.Importe = 203.750;
            //tck.Sucursal = new Sucursal() { SucursalID = 1 };
            //tck.FechaCaptura = DateTime.Now;
            //tck.HoraVta = DateTime.Now.ToString("HH:mm:ss");
            //tck.Cajero = "La mas trucha";

            tck.FechaCaptura = new DateTime(2014, 10, 25);
            tck.FechaCapturaHasta = new DateTime(2014, 10, 25);

            tck.FechaFactura  = new DateTime(2014, 10, 25);
            tck.FechaFacturaHasta = new DateTime(2014, 10, 25);

            //var dal = new TicketDAL();

            //var tkl = dal.GetTickets(tck, ref msg);

            var SI = new ServiceImplementation();

            //TicketResponse trp = SI.TicketMessage(new TicketRequest() {Filters = tck, MessageOperationType = MessageOperationType.Report});
            //var tkl = trp.Tickets;

            //foreach (Ticket t in tkl)
            //    Console.WriteLine("TicketID: " + t.TicketID.ToString());

            Console.WriteLine("Fin Prueba DAL Reporte");
            Console.ReadLine();


        }
    }
}
