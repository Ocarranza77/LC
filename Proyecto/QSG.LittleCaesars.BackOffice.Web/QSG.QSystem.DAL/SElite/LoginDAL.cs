using QSG.QSystem.Common.Constants;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Entities.SElite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPESD.Tools.DBHelper;

namespace QSG.QSystem.DAL.SElite
{
    public class LoginDAL
    {
        private string _strConnection = string.Empty;
        private string _DBName;

        public LoginDAL(string dbName)
        {
            _strConnection = ConfigurationManager.ConnectionStrings[Generales.strConn].ConnectionString;
            _DBName = dbName + ".SElite.";
        }
        public LoginDAL(string dbName, string srtConnection)
        {
            _strConnection = srtConnection;
            _DBName = dbName + ".SElite.";
        }

        public List<Usuario> Authentication (string email, string pwd, ref string friendlyMessage)
        {
            var result = new List<Usuario>();
            string msg = string.Empty;
            int numTable = 0;

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();

            dbHelper.CreateParameter<string>("@Email", email);
            dbHelper.CreateParameter<string>("@UsPwd", pwd);
            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "Authenticate");
            
            msg = dbHelper.GetParameterValue<string>("@Msg");
            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                int usID = 0;
                int cqID = 0;
                Usuario u = new Usuario();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (usID != Convert.ToInt32(row["UsuarioID"]))
                    {
                        u = new Usuario();
                        u.UsuarioID = Convert.ToInt32(row["UsuarioID"]);
                        usID = u.UsuarioID;
                        u.Email = row["Email"].ToString();
                        u.Nombre = row["UsuarioNombre"].ToString();
                        u.Alias = row["UsuarioAlias"].ToString();
                        u.Titulo = row["UsuarioTitulo"].ToString();
                        u.Activo = Convert.ToBoolean(row["UsuarioActivo"]);
                        u.Bloqueo = Convert.ToBoolean(row["Bloqueo"]);
                        u.Tipo = row["UsuarioTipo"] == DBNull.Value ? 0 : Convert.ToInt32(row["UsuarioTipo"]);
                        u.ShortCutMenuTpe = row["ShortCutMenuType"] == DBNull.Value ? 0 : Convert.ToInt32(row["ShortCutMenuType"]);

                        u.FechaAlta = Convert.ToDateTime(row["FechaAlta"].ToString());
                        u.FechaBaja = Convert.ToDateTime(row["FechaBaja"].ToString());
                        u.PhotoPath = row["PhotoPath"].ToString();

                        u.Persona = new Persona();
                        if (row["PersonaID"] != DBNull.Value && Convert.ToInt32(row["PersonaID"]) > 0)
                        {
                            u.Persona.PersonaID = Convert.ToInt32(row["PersonaID"]);
                            u.Persona.Nombre = row.Field<string>("Nombre");
                            u.Persona.Paterno = row["Paterno"].ToString();
                            u.Persona.Materno = row["Materno"].ToString();
                            u.Persona.FechaNac = row.Field<DateTime?>("FechaNac");
                        }

                        result.Add(u);
                    }


                    ClienteQ cq = new ClienteQ();
                    if (row["ClienteQID"] != DBNull.Value && Convert.ToInt32(row["ClienteQID"]) > 0 && Convert.ToInt32(row["ClienteQID"]) != cqID)
                    {
                        cq.ClienteID = Convert.ToInt32(row["ClienteID"]);
                        cqID = cq.ClienteID;
                        cq.Nombre = row.Field<string>("Nombre");
                        cq.Alias = row["ClienteAlias"].ToString();
                        cq.Activo = Convert.ToBoolean(row["ClienteActivo"]);
                        cq.Licencia = row["Licencia"].ToString();
                        cq.LogoPath = row["ClienteLogo"].ToString();
                        cq.Nombre = row["ClienteNombre"].ToString();

                        u.ClienteQ = cq;
                    }

                    Empresa e = new Empresa();
                    if (row["EmpresaID"] != DBNull.Value && Convert.ToInt32(row["EmpresaID"]) > 0)
                    {
                        e.EmpresaID = Convert.ToInt32(row["EmpresaID"]);
                        e.Nombre = row.Field<string>("EmpresaNombre");
                        e.Abr = row["EmpresaAbr"].ToString();
                        e.Activo = Convert.ToBoolean(row["EmpresaActivo"]);
                        e.NombreBD = row["NombreDB"].ToString();

                        u.ClienteQ.Empresas.Add(e);
                    }
                    
                }

                if (usID != 0)
                    result.Add(u);
                
            }

            return result;
        }

        public List<App> Authorize(int usID, ref string friendlyMessage)
        {
            var result = new List<App>();
            string msg = string.Empty;
            int numTable = 0;
            var padrePath = new Dictionary<string, List<int>>();

            DBHelper dbHelper = new DBHelper(_strConnection);

            SqlParameter prmData = new SqlParameter();

            dbHelper.CreateParameter<int>("@UsuarioID", usID);
            dbHelper.CreateParameter<string>("@Msg", msg, System.Data.ParameterDirection.Output);

            DataSet ds = dbHelper.ExecuteDataset(_DBName + "Authorize");

            msg = dbHelper.GetParameterValue<string>("@Msg");
            friendlyMessage = msg;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[numTable].Rows != null && ds.Tables[numTable].Rows.Count > 0)
            {
                //int usID = 0;
                //int cqID = 0;
                Menu m;
                Menu p;
                App a;
                
                List<int> lst;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    // Datos de la opcion del menu.
                    m = new Menu();

                    m.MenuID = Convert.ToInt32(row["MenuID"]);
                    m.AppID = row["AppID"].ToString();
                    m.Descripcion = row["Descripcion"].ToString();
                    if (row["PadreID"] != DBNull.Value)
                        m.PadreID = Convert.ToInt32(row["PadreID"]);

                    if (row["FormID"] != DBNull.Value)
                        m.Forma = new Forma()
                        {
                            FormaID = row["FormID"].ToString(),
                            Descripcion = row["FormaDescription"].ToString(),
                            Activo = Convert.ToBoolean(row["FormaActiva"]),
                            FormPath = row["FormaPath"].ToString(),
                            IconPath = row["FormaIconPath"].ToString()
                        };
                    m.Activo = Convert.ToBoolean(row["Activa"]);
                    m.Orden = Convert.ToInt32(row["Orden"]);

                    // Encontrar la App
                    //if (padrePath[row["AppID"].ToString()] != null)
                    if(padrePath.TryGetValue(row["AppID"].ToString(), out lst))
                    {
                        //lst = padrePath[row["AppID"].ToString()];
                        a = result[lst[0]];
                    }
                    else
                    {
                        a = new App()
                        {
                             AppID = row["AppID"].ToString(),
                             Abr = row["AppAbr"].ToString(),
                             Descripcion = row["AppDescription"].ToString(),
                             Activo = Convert.ToBoolean(row["AppActiva"]),
                             IconPath = row["AppIconPath"].ToString()
                        };
                        padrePath.Add(a.AppID, new List<int>() { result.Count()});

                        result.Add(a);
                    }

                    // Encontrar la ruta de la Opcion
                    if (row["PadreID"] == DBNull.Value)
                    {
                        if (a.Menus == null)
                            a.Menus = new List<Menu>();

                        padrePath.Add("M" + m.MenuID.ToString(), new List<int>() {a.Menus.Count()});
                        a.Menus.Add(m);
                    }
                    else
                    {
                        lst = padrePath["M" + row["PadreID"].ToString()]; //Lista que contiene la ruta para llegar a la opcion
                        List<int> nLst = new List<int>();

                        p = a.Menus[lst[0]];
                        nLst.Add(lst[0]);

                        for (int it = 1; it < lst.Count(); it++)
                        {
                            p = p.Hijos[lst[it]];
                            nLst.Add(lst[it]);
                        }

                        if (p.Hijos == null)
                            p.Hijos = new List<Menu>();

                        nLst.Add(p.Hijos.Count());
                        padrePath.Add("M" + m.MenuID.ToString(), nLst);
                        p.Hijos.Add(m);
                    }
                }
            }

            return result;
        }

    }
}
