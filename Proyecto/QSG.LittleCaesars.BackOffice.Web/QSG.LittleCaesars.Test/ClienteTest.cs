using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.Test
{
    class ClienteTest
    {

        public void Savecliente() {

            Console.WriteLine("Inicio Prueba DAL Save");
            var tck = new Cliente();
            bool b;

            tck.RFC = "adfasfd";
            tck.RazonSocial = "venta de dulces";

           

            tck.OperationType = OperationType.New;

            var dal = new ClienteDAL();
            string msg = string.Empty;
            var tk = dal.SaveCliente (tck, ref msg);// .GetTicket(tck, ref msg);

            Console.WriteLine("Grabado: " + tk.ToString());
            Console.ReadLine();
        }

    }
}
