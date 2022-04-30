using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;

namespace QSG.LittleCaesars.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var tt = new TicketTest();
            //var c = new ClienteTest();

            //tt.GetTicket();


            //tt.SaveTicket();
            //tt.ReporteTicket();
            //c.Savecliente();



            //var cst = new CorteSucursalTest();
            //cst.ReporteCorteSucursal();

            var us = new SucursalUsuarioTest();
            us.Save();


        }
    }

}
