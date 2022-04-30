using QSG.LittleCaesars.BackOffice.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using System.Configuration;
using XPESD.Tools.DBHelper;
using System.Data;
using System.Data.SqlClient;
using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.DAL
{
    public class TicketDAL
    {
        private string _strConnection = string.Empty;

        public TicketDAL()
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
        }

        public TicketDAL(string strConnection)
        {
            _strConnection = strConnection;
        }

        



        public Ticket GetTicket(Ticket ticket, int usRqst, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new Ticket();

            DBHelper dbHelper = new DBHelper(_strConnection);
            dbHelper.CreateParameter<int>("@TicketID", ticket.TicketID);
            dbHelper.CreateParameter<int>("@SucursalID", ticket.Sucursal.SucursalID);
            dbHelper.CreateParameter<int>("@CajaID", ticket.CajaID);
           // dbHelper.CreateParameter<string>("@Folio", ticket.Folio);
            dbHelper.CreateParameter<double>("@Importe", ticket.Importe);
            //dbHelper.CreateParameter<string>("@FechaVta",ticket.FechaVta.ToString("yyyy-MM-dd"));// String.Format("0:yyyy-mm-dd", ticket.FechaVta));
            //var date = ticket.FechaVta.ToString("yyyy-MM-dd");
            dbHelper.CreateParameter<int>("@usRqst", usRqst);
            DataSet ds = dbHelper.ExecuteDataset("getTicket");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DateTime dt;
                DataRow row = ds.Tables[0].Rows[0];
                result.TicketID = Convert.ToInt32(row["TicketID"]);
                result.CajaID = Convert.ToInt32(row["CajaID"]);
                result.Importe = Convert.ToDouble(row["Importe"]);
                result.Cajero = row["Cajero"].ToString();
                result.CodUsuario  = Convert.ToInt32(row["CodUsuario"]);
                DateTime.TryParse(row["FechaCaptura"].ToString(), out dt);
                result.FechaCaptura = dt;
                DateTime.TryParse(row["FechaFactura"].ToString(), out dt);
                result.FechaFactura = dt;
                DateTime.TryParse(row["FechaVta"].ToString(), out dt);
                result.FechaVta = dt;
                result.HoraVta = row["HoraVta"].ToString();
                result.Anterior = row["Anterior"].ToString();
                result.CodUsAlta = row["CodUsuario"].ToString();
                result.CodUsAltaNombre = row["NomUsuario"].ToString();
                result.FolioFactura = row["FolioFactura"].ToString();
                result.UUID = row["UUID"].ToString();
                result.FacturaXML = row["FacturaXML"].ToString();
                result.AcuseXML = row["AcuseXML"].ToString();
                result.MotivoCancelacion = row["MotivoCancelacion"].ToString();
                DateTime.TryParse(row["FechaCancelacion"].ToString(), out dt);
                result.FechaCancelacion = dt;
                result.RFC = row["RFC"].ToString();
                result.MetodoPago = new MetodoPagoSAT() { CodMetodoP = row["CodMetodoP"].ToString(), Descripcion = row["MPSATDescripcion"].ToString() };

            }
            
            // Sucursal
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[1].Rows != null && ds.Tables[1].Rows.Count > 0)
            {
                var suc = new Sucursal();
                DataRow row = ds.Tables[1].Rows[0];

                suc.SucursalID = Convert.ToInt32(row["SucursalID"]);
                suc.Nombre = row["Nombre"].ToString();
                suc.Abr = row["Abr"].ToString();
                suc.Descripcion = row["Descripcion"].ToString();

                suc.Serie = row["Serie"].ToString();
                suc.Calle = row["Calle"].ToString();
                suc.NoInt = row["NoInt"].ToString();
                suc.NoExt = row["NoExt"].ToString();
                suc.Colonia = row["Colonia"].ToString();
                suc.Delegacion = row["Delegacion"].ToString();
                suc.Ciudad = row["Ciudad"].ToString();
                suc.Municipio = row["Municipio"].ToString();
                suc.Estado = row["Estado"].ToString();
                suc.CP = row["CP"].ToString();
              
                result.Sucursal = suc;
            }

            //Cliente
            var cli = new Cliente();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[2].Rows != null && ds.Tables[2].Rows.Count > 0)
            {
               // var cli = new Cliente();
                DataRow row = ds.Tables[2].Rows[0];

                cli.RFC  = row["RFC"].ToString();
                cli.RazonSocial = row["RazonSocial"].ToString();
                cli.Calle = row["Calle"].ToString();
                cli.NoInt = row["NoInt"].ToString();
                cli.NoExt = row["NoExt"].ToString();
                cli.Colonia = row["Colonia"].ToString();
                cli.Delegacion = row["Delegacion"].ToString();
                cli.Ciudad = row["Ciudad"].ToString();
                cli.Municipio = row["Municipio"].ToString();
                cli.Estado = row["Estado"].ToString();
                cli.CP = row["CP"].ToString();
                cli.Contacto = row["Contacto"].ToString();
                
               
                cli.Email1 = row["Email1"].ToString();
                cli.Email2 = row["Email2"].ToString();
                cli.Email3 = row["Email3"].ToString();
            

                result.Cliente = cli;
            }
            result.Cliente = cli;
            return result;
        }

        public List<Ticket> GetTickets(TicketFilter ticket, int usRqst, ref string friendlyMessage, int tipoTicket = 0)
        {
            var result = new List<Ticket>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(ticket);
            dbHelper.AddParameter(prmData);
            
            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);
            dbHelper.CreateParameter<int>("@usRqst", usRqst);
            dbHelper.CreateParameter<int>("@TipoTicket", tipoTicket);

            DataSet ds = dbHelper.ExecuteDataset("getTickets");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DateTime dt;
                Ticket tk;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    tk = new Ticket();
                    tk.TicketID = Convert.ToInt32(row["TicketID"]);
                    tk.CajaID = Convert.ToInt32(row["CajaID"]);
                    tk.Importe = Convert.ToDouble(row["Importe"]);
                    tk.Cajero = row["Cajero"].ToString();
                    tk.CodUsuario = Convert.ToInt32(row["CodUsuario"]);
                    DateTime.TryParse(row["FechaCaptura"].ToString(), out dt);
                    tk.FechaCaptura = dt;
                    DateTime.TryParse(row["FechaFactura"].ToString(), out dt);
                    tk.FechaFactura = dt;
                    DateTime.TryParse(row["FechaVta"].ToString(), out dt);
                    tk.FechaVta = dt;
                    tk.HoraVta = row["HoraVta"].ToString();
                    tk.CodUsAlta = row["CodUsuario"].ToString();
                    tk.CodUsAltaNombre = row["NomUsuario"].ToString();
                    tk.FolioFactura = row["FolioFactura"].ToString();
                    tk.UUID = row["UUID"].ToString();
                    tk.RFC = row["RFC"].ToString();
                    DateTime.TryParse(row["FechaCancelacion"].ToString(), out dt);
                    tk.FechaCancelacion = dt;
                    tk.MotivoCancelacion = row["MotivoCancelacion"].ToString();

                    //tk.MetodoPago =  row["CodMetodoP"].ToString();
                    tk.MetodoPago = new MetodoPagoSAT() { CodMetodoP = row["CodMetodoP"].ToString(), Descripcion = row["MPSATDescripcion"].ToString() };


                    //tk.FacturaXML = row["FacturaXML"].ToString();
                    //tk.AcuseXML = row["AcuseXML"].ToString();


                    tk.Sucursal = new Sucursal();
                    tk.Sucursal.SucursalID = (int)row["SucursalID"];
                    tk.Sucursal.Nombre = row["Nombre"].ToString();
                    tk.Sucursal.Abr = row["Abr"].ToString();
                    /*
                    tk.Sucursal.Descripcion = row["Descripcion"].ToString();
                    tk.Sucursal.Serie = row["Serie"].ToString();
                    tk.Sucursal.Calle = row["Calle"].ToString();
                    tk.Sucursal.NoInt = row["NoInt"].ToString();
                    tk.Sucursal.NoExt = row["NoExt"].ToString();
                    tk.Sucursal.Colonia = row["Colonia"].ToString();
                    tk.Sucursal.Delegacion = row["Delegacion"].ToString();
                    tk.Sucursal.Ciudad = row["Ciudad"].ToString();
                    tk.Sucursal.Municipio = row["Municipio"].ToString();
                    tk.Sucursal.Estado = row["Estado"].ToString();
                    tk.Sucursal.CP = row["CP"].ToString();
                    */
                    tk.Sucursal.Empresa = new EmpresaCliente();
                    tk.Sucursal.Empresa.EmpresaID = row.Field<Int32>("EmpresaID");
                    tk.Sucursal.Empresa.Nombre = row["ECNombre"].ToString();
                    tk.Sucursal.Empresa.RFC = row["ECRFC"].ToString();
                    tk.Sucursal.Empresa.UserPak = row["ECUserPak"].ToString();
                    tk.Sucursal.Empresa.ClavePak = row["ECClavePak"].ToString();


                    tk.Cliente = new Cliente();
                    tk.Cliente.RFC = row["RFC"].ToString();
                    tk.Cliente.RazonSocial = row["RazonSocial"].ToString();
                    /*
                    tk.Cliente.Calle = row["Calle"].ToString();
                    tk.Cliente.NoInt = row["NoInt"].ToString();
                    tk.Cliente.NoExt = row["NoExt"].ToString();
                    tk.Cliente.Colonia = row["Colonia"].ToString();
                    tk.Cliente.Delegacion = row["Delegacion"].ToString();
                    tk.Cliente.Ciudad = row["Ciudad"].ToString();
                    tk.Cliente.Municipio = row["Municipio"].ToString();
                    tk.Cliente.Estado = row["Estado"].ToString();
                    tk.Cliente.CP = row["CP"].ToString();
                    tk.Cliente.Contacto = row["Contacto"].ToString();
                    */
                    tk.Cliente.Email1 = row["Email1"].ToString();
                    /*tk.Cliente.Email2 = row["Email2"].ToString();
                    tk.Cliente.Email3 = row["Email3"].ToString();*/
                    result.Add(tk);
                }

            }


            return result;
        }

        public bool SaveTicket(Ticket ticket, ref string friendlyMessage)
        {
            bool result = false;
            string Msg = string.Empty;
            DBHelper dbHelper = new DBHelper(_strConnection);
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(ticket);
            //string serializer = dbHelper.SerializeToXML(ticket);
            //serializer = serializer.Substring(serializer.IndexOf("<Ticket"));
            //prmData.Value = serializer;
            dbHelper.AddParameter(prmData);
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.ExecuteCommand("SaveTicket");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }

        public bool SaveTickets(List<Ticket> tickets, ref string friendlyMessage)
        {
            bool result = false; 
            string Msg = string.Empty;
            DBHelper dbHelper = new DBHelper(_strConnection);
            
            SqlParameter prmData = new SqlParameter();
            prmData.ParameterName = "@prmXML";
            prmData.SqlDbType = SqlDbType.Xml;
            prmData.Value = dbHelper.SerializeToXML(tickets);
            dbHelper.AddParameter(prmData);

            dbHelper.CreateParameter<string>("@Entidad","ArrayOfTicket/Ticket");
            dbHelper.CreateParameter<string>("@Msg", Msg, System.Data.ParameterDirection.Output);
            dbHelper.ExecuteCommand("SaveTicket");

            result = true;
            if (Msg != String.Empty)
            {
                friendlyMessage = Msg;
                result = false;
            }

            return result;
        }

        public List<Ticket> GetBitacoraTicket(string llave, ref string friendlyMessage)
        {
            var result = new List<Ticket>();
            string msg = string.Empty;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();
            dbHelper.CreateParameter<string>("@Llave", llave);


            DataSet ds = dbHelper.ExecuteDataset("BitacoraTicket");

            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DateTime dt;
                Ticket tk;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string[] campos;
                    string campo = string.Empty;

                    tk = new Ticket();
                    campo = row["Llave"].ToString();

                    campos = campo.Split('|');

                    tk.TicketID = Convert.ToInt32(campos[0]);
                    tk.CajaID = Convert.ToInt32(campos[2]);
                    tk.Importe = Convert.ToDouble(campos[3]);
                    tk.Cajero = row["Cajero"].ToString();
                    tk.CodUsuario = Convert.ToInt32(row["CodUsuario"]);
                    DateTime.TryParse(row["FechaCaptura"].ToString(), out dt);
                    tk.FechaCaptura = dt;
                    DateTime.TryParse(row["FechaFactura"].ToString(), out dt);
                    tk.FechaFactura = dt;
                    DateTime.TryParse(campos[4], out dt);
                    tk.FechaVta = dt;
                    tk.HoraVta = row["HoraVta"].ToString();
                    tk.CodUsAlta = row["CodUsuario"].ToString();
                    tk.CodUsAltaNombre = row["NomUsuario"].ToString();



                    tk.Sucursal = new Sucursal();
                    tk.Sucursal.SucursalID = Convert.ToInt32(campos[1]);
                    tk.Sucursal.Abr = row["AbrSuc"].ToString();
                    tk.Sucursal.Nombre = row["NomSuc"].ToString();

                    result.Add(tk);
                }

            }


            return result;
        }

    }


}
