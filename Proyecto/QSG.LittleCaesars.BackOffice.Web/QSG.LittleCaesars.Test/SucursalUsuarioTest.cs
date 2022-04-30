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
    public class SucursalUsuarioTest
    {

        //public void GetTicket()
        //{
        //    Console.WriteLine("Inicio Consola Get");

        //    var tck = new Ticket();

        //    tck.TicketID = 1;
        //    tck.CajaID = 2;
        //    tck.Importe = 100.25;
        //    tck.Sucursal = new Sucursal() { SucursalID = 1 };


        //    var dal = new TicketDAL();
        //    string msg = string.Empty;
        //    int usRqst = 1;

        //    var tk = dal.GetTicket(tck, usRqst, ref msg);

        //    Console.WriteLine(tk.Sucursal.Nombre);
        //    Console.WriteLine("Fin Prueba DAL");

        //    Console.ReadLine();


        //    var sv = new ServiceImplementation();

        //    var tr = new TicketRequest();

        //    //tr.Ticket = tck;
        //    //tr.MessageOperationType = MessageOperationType.Query;
        //    tr.Ticket.Importe = 0;

        //    var response = sv.TicketMessage(tr);

        //    Console.WriteLine(response.FriendlyMessage);

        //    if (response.Ticket.Sucursal != null)
        //        Console.WriteLine(response.Ticket.Sucursal.Nombre);
        //    Console.WriteLine("Fin Prueba Mensaje");

        //    Console.ReadLine();

        //}

        public void Save()
        {
            Console.WriteLine("Inicio Prueba DAL Save Sucursal Usuario");

            var dal = new SucursalUsuarioDAL();
            string msg = string.Empty;
            var lst = new List<SucursalUsuario>();


            lst.Add(new SucursalUsuario() { OperationType = OperationType.Delete, UsuarioPermisoID = 13, Sucursal = new Sucursal() { SucursalID = 2 } });
            //lst.Add(new SucursalUsuario() { OperationType = OperationType.New, UsuarioPermisoID = 13, Sucursal = new Sucursal() { SucursalID = 1 },CodUsAlta="1" });
            lst.Add(new SucursalUsuario() { OperationType = OperationType.New, UsuarioPermisoID = 13, Sucursal = new Sucursal() { SucursalID = 12 }, CodUsAlta = "1" });
            

            Console.WriteLine("Grabado.. ");
            dal.SaveSucursalesUsuario(lst, ref msg);

            Console.WriteLine("Fin Grabado Msg: " + msg);


            Console.ReadLine();
        }

        //public void ReporteTicket()
        //{
        //    Console.WriteLine("Inicio Prueba DAL Reporte");
        //    var tck = new TicketFilter();
            
        //    string msg = string.Empty;

        //    //tck.TicketID = 4;
        //    //tck.CajaID = 2;
        //    //tck.Importe = 203.750;
        //    //tck.Sucursal = new Sucursal() { SucursalID = 1 };
        //    //tck.FechaCaptura = DateTime.Now;
        //    //tck.HoraVta = DateTime.Now.ToString("HH:mm:ss");
        //    //tck.Cajero = "La mas trucha";

        //    tck.FechaCaptura = new DateTime(2014, 10, 25);
        //    tck.FechaCapturaHasta = new DateTime(2014, 10, 25);

        //    tck.FechaFactura  = new DateTime(2014, 10, 25);
        //    tck.FechaFacturaHasta = new DateTime(2014, 10, 25);

        //    //var dal = new TicketDAL();

        //    //var tkl = dal.GetTickets(tck, ref msg);

        //    var SI = new ServiceImplementation();

        //    //TicketResponse trp = SI.TicketMessage(new TicketRequest() {Filters = tck, MessageOperationType = MessageOperationType.Report});
        //    //var tkl = trp.Tickets;

        //    //foreach (Ticket t in tkl)
        //    //    Console.WriteLine("TicketID: " + t.TicketID.ToString());

        //    Console.WriteLine("Fin Prueba DAL Reporte");
        //    Console.ReadLine();


        //}
    }
}
