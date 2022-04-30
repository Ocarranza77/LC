using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using QSG.QAGC.Messages;
using QSG.QAGC.Messages.Requests;
using QSG.QAGC.Messages.Response;
using QagcRq = QSG.QAGC.Messages.Requests;
using QagcRp = QSG.QAGC.Messages.Response;
using QSG.QSystem.Common.Entities;
using QSG.QSystem.Common.Entities.SElite;
using QSG.QSystem.Messages;
using QSG.QSystem.Messages.Requests;
using QSG.QSystem.Messages.Response;
using QubicPortal.Model;
using QubicPortal.Model.Messages;
using QSG.QSystem.Common.Enums;


namespace QubicPortal
{
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class QubicService
    {
        //SecurityBusiness _securityBusiness = new SecurityBusiness();
        ServiceQbic _securityBusiness = new ServiceQbic();
        MessageQAGC _messageQAGC = new MessageQAGC();

        [WebGet(UriTemplate = "/Initialize", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<InitResponse> Initialize()
        {
            QubicResponse<InitResponse> response = new QubicResponse<InitResponse>();

            response.ResponseCode = "200";

            try
            {
                //TODO: Call business objects...
                response.Content = new InitResponse();
                response.Content.Version = "2.0.0";
                response.Content.HidePIN = true;

            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }


            return response;
        }

        [WebInvoke(UriTemplate = "/Authenticate", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<LoginResponse> Authenticate(QubicRequest<LoginRequest> request)
        {

            QubicResponse<LoginResponse> response = new QubicResponse<LoginResponse>();
            response.ResponseCode = "200";

            try
            {
                response.Content = Execute<LoginResponse, LoginRequest>(this._securityBusiness, request.RequestMessage);

                if (response.Content.ResultType == QSG.QSystem.Common.Enums.MessageResultType.Failure) 
                    throw new ApplicationException("Fallo el login, contactar al administrador del sistema.");                

                if (response.Content.Usuarios.Count > 0 && response.Content.Usuarios[0].Bloqueo == true)
                    throw new ApplicationException("Este usuario se encuentra bloqueado, contactar al administrador del sistema.");


                //Credenciales invalidas
                if (response.Content.Usuarios.Count == 0 )
                {
                    int trys = 1;

                    if (HttpContext.Current.Session["trys"] != null)
                    {
                        trys = Convert.ToInt32(HttpContext.Current.Session["trys"]);
                    }

                    trys++;

                    HttpContext.Current.Session.Add("trys", trys);

                    if (trys > 3)
                    {
                        string msg = string.Empty;
                        this._securityBusiness.BloquearUsuario(request.RequestMessage.Email, ref msg);
                        throw new ApplicationException("Ha intentando por tercera vez credenciales invalidas, el usuario ha sido bloqueado, contacte al adminsitrador del sistema");
                    }
                    else
                    {
                        throw new ApplicationException("Las credenciales proporcionadas no son validas.");
                    }
                }

                //Validando si esta activo
                if (response.Content.Usuarios.Count > 1)
                {
                    int i = 1;
                    do
                    {
                        if (response.Content.Usuarios[i - 1].Activo == false)
                            response.Content.Usuarios.RemoveAt(i - 1);
                        else
                            i++;

                    } while (response.Content.Usuarios.Count >= i);

                    if (response.Content.Usuarios.Count == 0)
                        throw new ApplicationException("Este usuario se encuentra dado de baja, contactar al administrador del sistema.");
                }

                //TODO: Revisar que onda con esa asignacion temprana del usuario
                //this.LoggedUser = response.Content.LoggedUser;
                this.LoggedUsers = response.Content.Usuarios;

                HttpContext.Current.Session.Add("usertoken", response.Content.Token );
            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Implementar el log.
            }
            
            return response;
        }

        private Usuario LoggedUser 
        {
            set 
            {
                HttpContext.Current.Session.Add("userLogged", value);
            }
            get 
            { 
                return HttpContext.Current.Session["userLogged"] as Usuario;
            }
        }

        private List<Usuario> LoggedUsers
        {
            set
            {
                HttpContext.Current.Session.Add("userLogged", value);
            }
            get
            {
                return HttpContext.Current.Session["userLogged"] as List<Usuario>;
            }
        }

        private List<App> Menus
        {
            set
            {
                HttpContext.Current.Session.Add("Menus", value);
            }
            get
            {
                return HttpContext.Current.Session["Menus"] as List<App>;
            }
        }

        [WebInvoke(UriTemplate = "/Authorize", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<LoginResponse> Authorize(QubicRequest<LoginRequest> request)
        {
            QubicResponse<LoginResponse> response = new QubicResponse<LoginResponse>();
            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                /*
                var usuarioClon = LoggedUsers.Where(x => x.UsuarioID == request.RequestMessage.UsuarioID).FirstOrDefault();
                LoggedUser = usuarioClon.Clone();
                LoggedUsers = null; */
                LoggedUser = LoggedUsers.Where(x => x.UsuarioID == request.RequestMessage.UsuarioID).FirstOrDefault();

                response.Content = Execute<LoginResponse, LoginRequest>(this._securityBusiness, request.RequestMessage);
                //response.Content.LoggedUser = this.LoggedUser;
                response.Content.Usuarios = new List<Usuario>() { LoggedUser };
//                HttpContext.Current.Session.Add("companies", response.Content.Companies);
                HttpContext.Current.Session.Add("companies", response.Content.Empresas);
                Menus = response.Content.Apps;
            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Implementar el log.
            }

            return response;
        }


        [WebInvoke(UriTemplate = "/GetMenuForCompany", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<LoginResponse> GetMenuForCompany(QubicRequest<LoginRequest> request)
        {
            QubicResponse<LoginResponse> response = new QubicResponse<LoginResponse>();
            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                response.Content = Execute<LoginResponse, LoginRequest>(this._securityBusiness, request.RequestMessage);
                Menus = response.Content.Apps;

                //response.Content.LoggedUser = this.LoggedUser;
                //HttpContext.Current.Session.Add("companies", response.Content.Companies);
            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Implementar el log.
            }

            return response;
        }

        [WebGet(UriTemplate = "/CloseSession", ResponseFormat = WebMessageFormat.Json)]
        public bool CloseSession()
        {
            HttpContext.Current.Session.Clear();
            return true;
        }

        #region ALTA DE USUARIOS

        //GET ALL USERS
        [WebInvoke(UriTemplate = "/users", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<UsuarioResponse> GetAllUsers(QubicRequest<UsuarioRequest> request)
        {
            QubicResponse<UsuarioResponse> response = new QubicResponse<UsuarioResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    response.ResponseCode = "403";
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                response.Content = Execute<UsuarioResponse, UsuarioRequest>(this._securityBusiness, request.RequestMessage);

            }
            catch (Exception)
            {
                if (response.ResponseCode != "403")
                    response.ResponseCode = "500";

                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }


            return response;
        }

        //SAVING USER
        [WebInvoke(UriTemplate = "/user", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<UsuarioResponse> Save(QubicRequest<UsuarioRequest> request)
        {
            QubicResponse<UsuarioResponse> response = new QubicResponse<UsuarioResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    response.ResponseCode = "403";
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                response.Content = Execute<UsuarioResponse, UsuarioRequest>(this._securityBusiness, request.RequestMessage);

            }
            catch (Exception)
            {
                if (response.ResponseCode != "403")
                    response.ResponseCode = "500";

                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/deleteuser", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<UsuarioResponse> DeleteUser(QubicRequest<UsuarioRequest> request)
        {
            QubicResponse<UsuarioResponse> response = new QubicResponse<UsuarioResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                response.Content = Execute<UsuarioResponse, UsuarioRequest>(this._securityBusiness, request.RequestMessage);

            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }


        #endregion END ALTA DE USUARIOS

        #region RESETEO DE CONTRASENA

        //GET ALL USERS BY CRITERIA
        [WebInvoke(UriTemplate = "/searchusers", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<UsuarioResponse> GetAllUsersByCriteria(QubicRequest<UsuarioRequest> request)
        {
            QubicResponse<UsuarioResponse> response = new QubicResponse<UsuarioResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    response.ResponseCode = "403";
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                response.Content = Execute<UsuarioResponse, UsuarioRequest>(this._securityBusiness, request.RequestMessage);

            }
            catch (Exception)
            {
                if (response.ResponseCode != "403")
                    response.ResponseCode = "500";

                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/resetpassword", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<UsuarioResponse> ResetPassword(QubicRequest<UsuarioRequest> request)
        {
            QubicResponse<UsuarioResponse> response = new QubicResponse<UsuarioResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                if (request.RequestMessage.Usuario.ContrasenaXSistema)
                    request.RequestMessage.Usuario.UsPwd = "@QU" + DateTime.Now.ToString("fff") + "_b" + DateTime.Now.ToString("s");

                response.Content = Execute<UsuarioResponse, UsuarioRequest>(this._securityBusiness, request.RequestMessage);

            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
            }
            catch (Exception)
            {
                if (response.ResponseCode != "403")
                    response.ResponseCode = "500";

                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }


        #endregion RESETEO DE CONTRASENA

        #region SECURITY

        [WebInvoke(UriTemplate = "/getsecurityfilters", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<SeguridadPermisosResponse> GetSecurityFilters(QubicRequest<SeguridadPermisosRequest> request)
        {
            QubicResponse<SeguridadPermisosResponse> response = new QubicResponse<SeguridadPermisosResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                response.Content = Execute<SeguridadPermisosResponse, SeguridadPermisosRequest>(this._securityBusiness, request.RequestMessage);

            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/getsecurityinfo", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<SeguridadPermisosResponse> GetSecurityInfo(QubicRequest<SeguridadPermisosRequest> request)
        {
            QubicResponse<SeguridadPermisosResponse> response = new QubicResponse<SeguridadPermisosResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                response.Content = Execute<SeguridadPermisosResponse, SeguridadPermisosRequest>(this._securityBusiness, request.RequestMessage);

            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }


        #endregion SECURITY

 
        #region QUBIC FORM

        [WebInvoke(UriTemplate = "/initQbicForm", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<InitFormResponse> InitializeQbicForm(QubicRequest<InitFormRequest> request)
        {
            QubicResponse<InitFormResponse> response = new QubicResponse<InitFormResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                response.Content = InitFormMessage(request.RequestMessage);//Execute<InitFormResponse, InitFormRequest>(this._securityBusiness, request.RequestMessage);

            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }


        #endregion END QUBIC FORM


        #region private implementation...

        private bool ValidateIfTokenIsValid(string token)
        {
            string storedToken = string.Empty;

            if (HttpContext.Current.Session["usertoken"] != null)
            {
                storedToken = HttpContext.Current.Session["usertoken"].ToString();
            }

            return storedToken.Equals(token);
        }

        private T Execute<T, U>(object bo, U requestMessage)
        {
            string methodName = requestMessage.GetType().Name.Replace("Request", "") + "Message";

            //MethodInfo method = bo.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(AuthenticateRequest) }, null);

            MethodInfo[] ms = bo.GetType().GetMethods();
            MethodInfo method = ms.First(x => x.Name == methodName);

            object[] parameters = new object[1];
            parameters[0] = requestMessage;

            return (T) method.Invoke(bo, parameters);
        }

        private InitFormResponse InitFormMessage(InitFormRequest request)
        {
            var response = new InitFormResponse();
            string app = string.Empty;
            string md = string.Empty;
            string frm = string.Empty;
            bool find = false;
            Menu mnFrm = new Menu();

            var empr = LoggedUser.ClienteQ.Empresas.Where(x=> x.EmpresaID == request.CompanyID).FirstOrDefault();

            response.Company = new QubicPortal.Model.Messages.Company { Id = empr.EmpresaID, Name = empr.Nombre, Logo = empr.NombreBD};
            response.User = LoggedUser;
            var mn = Menus;

            foreach (App a in Menus)
            {
                app = a.Descripcion;
                md = string.Empty;
                frm = string.Empty;
                foreach (Menu m in (a.Menus == null ? new List<Menu>() : a.Menus))
                {
                    md = m.Descripcion;
                    if (m.MenuID == request.ControlID)
                    {
                        find = true;
                        mnFrm = m;
                        break;
                    }
                    foreach (Menu p in (m.Hijos == null ? new List<Menu>() : m.Hijos))
                    {
                        frm = p.Descripcion;
                        if (p.MenuID == request.ControlID)
                        {
                            find = true;
                            mnFrm = p;
                            break;
                        }
                    }
                    if (find)
                        break;
                }
                if (find)
                    break;
            }


            response.ControlName = mnFrm.Forma.FormPath;
            
            if( string.IsNullOrEmpty(frm))
                response.Path = new List<string>() { app, "", md };
            else
                response.Path = new List<string>() { app, md, frm };


            response.Permits = new List<string>();// { mnFrm.Permiso_Add, mnFrm.Permiso_Delete, mnFrm.Permiso_Export, mnFrm.Permiso_Print, mnFrm.Permiso_Update };
            if (mnFrm.Permiso_Add > 0)
                response.Permits.Add("Save");
            if (mnFrm.Permiso_Delete > 0)
                response.Permits.Add("Delete");
            if (mnFrm.Permiso_Export > 0)
                response.Permits.Add("Export");
            if (mnFrm.Permiso_Print > 0)
                response.Permits.Add("Print");
            if (mnFrm.Permiso_Update > 0)
                response.Permits.Add("??");
            
            response.SpetialPermits = mnFrm.Permisos_Especiales.Select(x => x.Nombre).ToList();
            response.AcctionForm = mnFrm.Forma.Acciones;
            return response;

        }
        #endregion  private implementation...


        // MOVER A UN SERVICIO DE QAGC

        #region Offices Catalog

        [WebInvoke(UriTemplate = "/loadCatalog", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<LoadCatalogResponse> LoadCatalogInfo(QubicRequest<LoadCatalogRequest> request)
        {
            QubicResponse<LoadCatalogResponse> response = new QubicResponse<LoadCatalogResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                //response.Content = Execute<LoadCatalogResponse, LoadCatalogRequest>(this._securityBusiness, request.RequestMessage);

                //TODO: This code must be executed in the business component...
                //----------------------
                LoadCatalogResponse responseFake = new LoadCatalogResponse();
                List<object> items = new List<object>();

                if (request.RequestMessage.CatalogName == "monedas")
                {
                    items.Add(new Moneda { MonedaID = 1, Nombre = "Pesos" });
                    items.Add(new Moneda { MonedaID = 2, Nombre = "Dolares" });
                }

                if (request.RequestMessage.CatalogName == "oficinas")
                {
                    items.Add(new CatalogoTipo { ID = 1, Abr = "CM", Nombre = "Ciudad de Mexico" });
                    items.Add(new CatalogoTipo { ID = 2, Abr = "TJ", Nombre = "Tijuana" });
                    items.Add(new CatalogoTipo { ID = 3, Abr = "GD", Nombre = "Guadalajara" });
                    items.Add(new CatalogoTipo { ID = 4, Abr = "TP", Nombre = "Tepic" });
                }

                if (request.RequestMessage.CatalogName == "listas")
                {
                    items.Add(new Lista { ListID = 1, Activo = true, CodUsAltaNombre = "Majahide Payan", FechaAlta = "Nov.01.2015", Nombre = "Ciudad de Mexico", VigenciaDesde = "01/01/2016", VigenciaHasta = "01/15/2016", Moneda = new Moneda { MonedaID = 1, Nombre = "Pesos" }, Oficinas = new List<CatalogoTipo>() });
                    ((Lista)items[items.Count - 1]).Oficinas.Add(new CatalogoTipo { ID = 1, Nombre = "Ciudad de Mexico", Abr = "CM" });
                    ((Lista)items[items.Count - 1]).Oficinas.Add(new CatalogoTipo { ID = 2, Nombre = "Tijuana", Abr = "TJ" });

                    items.Add(new Lista { ListID = 2, Activo = true, CodUsAltaNombre = "Carlos Soto", FechaAlta = "Dic.03.2015", Nombre = "Tijuana", VigenciaDesde = "01/01/2016", VigenciaHasta = "04/15/2016", Moneda = new Moneda { MonedaID = 1, Nombre = "Dolares" }, Oficinas = new List<CatalogoTipo>() });
                    ((Lista)items[items.Count - 1]).Oficinas.Add(new CatalogoTipo { ID = 3, Nombre = "Guadalajara", Abr = "GD" });
                    ((Lista)items[items.Count - 1]).Oficinas.Add(new CatalogoTipo { ID = 4, Nombre = "Tepic", Abr = "TP" });

                    items.Add(new Lista { ListID = 3, Activo = false, CodUsAltaNombre = "Omar Carranza", FechaAlta = "Ene.01.2015", Nombre = "Guadalajara", VigenciaDesde = "10/01/2016", VigenciaHasta = "11/15/2016", Moneda = new Moneda { MonedaID = 1, Nombre = "Dolares" }, Oficinas = new List<CatalogoTipo>() });
                    ((Lista)items[items.Count - 1]).Oficinas.Add(new CatalogoTipo { ID = 1, Nombre = "Ciudad de Mexico", Abr = "CM" });

                    items.Add(new Lista { ListID = 4, Activo = true, CodUsAltaNombre = "Gerardo Cabrera", FechaAlta = "Jun.20.1960", Nombre = "Tepic", VigenciaDesde = "01/01/2016", VigenciaHasta = "12/15/2016", Moneda = new Moneda { MonedaID = 1, Nombre = "Pesos" }, Oficinas = new List<CatalogoTipo>() });
                    ((Lista)items[items.Count - 1]).Oficinas.Add(new CatalogoTipo { ID = 1, Nombre = "Ciudad de Mexico", Abr = "CM" });
                    ((Lista)items[items.Count - 1]).Oficinas.Add(new CatalogoTipo { ID = 4, Nombre = "Tepic", Abr = "TP" });
                }

                var rst = new JavaScriptSerializer().Serialize(items);

                responseFake.CatalogItems = rst;

                //----------------------

                response.Content = responseFake;

            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/saveCatalog", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<SaveCatalogResponse> SaveCatalogInfo(QubicRequest<SaveCatalogRequest> request)
        {
            QubicResponse<SaveCatalogResponse> response = new QubicResponse<SaveCatalogResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                //TODO: Some component implementation is missing...
                //response.Content = Execute<SaveCatalogResponse, SaveCatalogRequest>(this._securityBusiness, request.RequestMessage);

                response.ResponseCode = "300";
                response.FriendlyMessage = "La(s) Oficina(s) o Sucursal(es) (TJ, CM) esta(n) en uso, por favor verifique... gracias";

            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        #endregion End Office Catalog


        #region QAGC
        [WebInvoke(UriTemplate = "/catArticulos", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<ArticuloResponse> CatArticulos(QubicRequest<ArticuloRequest> request)
        {
            QubicResponse<ArticuloResponse> response = new QubicResponse<ArticuloResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");

                response.Content = Execute<ArticuloResponse, ArticuloRequest>(this._messageQAGC, request.RequestMessage);

                response.FriendlyMessage = response.Content.FriendlyMessage;

                if (response.Content.ResultType != MessageResultType.Sucess)
                {
                    response.ResponseCode = "300";
                    response.MessageError = response.Content.MessageError;
                }
                else
                    response.FriendlyMessage = "Operacion Exitosa!";
                

            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
                response.MessageError = ex.HResult.ToString() + " " + ex.Message;
            }
            catch (Exception ex)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                response.MessageError = ex.HResult.ToString() + " " + ex.Message;
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/catalogoTipos", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<QagcRp.CatalogoTipoResponse> CatalogoTipos(QubicRequest<QagcRq.CatalogoTipoRequest> request)
        {
            QubicResponse<QagcRp.CatalogoTipoResponse> response = new QubicResponse<QagcRp.CatalogoTipoResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");

                response.Content = Execute<QagcRp.CatalogoTipoResponse, QagcRq.CatalogoTipoRequest>(this._messageQAGC, request.RequestMessage);

                if (!string.IsNullOrEmpty(response.Content.FriendlyMessage))
                {
                    response.ResponseCode = "300";
                    response.FriendlyMessage = response.Content.FriendlyMessage;
                }

            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/listaPrecios", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<ListaResponse> ListaPrecios(QubicRequest<ListaRequest> request)
        {
            QubicResponse<ListaResponse> response = new QubicResponse<ListaResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");

                response.Content = Execute<ListaResponse, ListaRequest>(this._messageQAGC, request.RequestMessage);

                response.FriendlyMessage = response.Content.FriendlyMessage;

                if (response.Content.ResultType != MessageResultType.Sucess)
                {
                    response.ResponseCode = "300";
                    response.MessageError = response.Content.MessageError;
                }
                else
                    response.FriendlyMessage = "Operacion Exitosa!";

            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/catalogoFactores", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<QagcRp.FactorResponse> CatalogoFactores(QubicRequest<QagcRq.FactorRequest> request)
        {
            QubicResponse<QagcRp.FactorResponse> response = new QubicResponse<QagcRp.FactorResponse>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");

                response.Content = Execute<QagcRp.FactorResponse, QagcRq.FactorRequest>(this._messageQAGC, request.RequestMessage);

                response.FriendlyMessage = response.Content.FriendlyMessage;

                if (response.Content.ResultType != MessageResultType.Sucess)
                {
                    response.ResponseCode = "300";
                    response.MessageError = response.Content.MessageError;
                }
                else
                    response.FriendlyMessage = "Operacion Exitosa!";


            }
            catch (ApplicationException ex)
            {
                response.ResponseCode = "403";
                response.FriendlyMessage = ex.Message;
                response.MessageError = ex.HResult.ToString() + " " + ex.Message;
            }
            catch (Exception ex)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                response.MessageError = ex.HResult.ToString() + " " + ex.Message;
                //TODO: Log Exception...
            }

            return response;
        }

        #endregion
        
        #region Ejemplo Servicio Fake con la pantalla de Factores

        [WebInvoke(UriTemplate = "/factores", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<FactorResponseFake> LoadFactores(QubicRequest<FactorRequestFake> request)
        {
            QubicResponse<FactorResponseFake> response = new QubicResponse<FactorResponseFake>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                FactorResponseFake res = new FactorResponseFake();
                res.Factores = new List<Factor>();

                //TODO: Fill some hardcoded factors...
                res.Factores.Add(new Factor { Activo = true, FactorID = 1, Orden = 1, Nombre = "Importacion", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 1.05f, Formula = "", FormulaDes = "", ValorFinal = 0, UserID = "Carlos Soto", CDate = "Ene 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 10, Orden = 2, Nombre = "Costo fabricante", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 120f, Formula = "", FormulaDes = "", ValorFinal = 120, UserID = "Omar Carranza", CDate = "Feb 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 11, Orden = 3, Nombre = "Desc fab a CAS", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 1, Formula = "", FormulaDes = "", ValorFinal = 1, UserID = "Carlos Soto", CDate = "Abr 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 9, Orden = 4, Nombre = "Costo USA", Regla = true, ValorTipo = FactorValorTipo.Importe, Valor = 0, Formula = "cod10*cod11", FormulaDes = "", ValorFinal = 0, UserID = "Omar Carranza", CDate = "May 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 7, Orden = 5, Nombre = "IGI", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 1, Formula = "", FormulaDes = "", ValorFinal = 1, UserID = "Gerardo Cabrera", CDate = "Jun 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 2, Orden = 6, Nombre = "Costo Import", Regla = true, ValorTipo = FactorValorTipo.Importe, Valor = 0, Formula = "cod9*cod7", FormulaDes = "", ValorFinal = 0, UserID = "Carlos Soto", CDate = "Jul 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 3, Orden = 9, Nombre = "Indirecto operacion", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 0.7f, Formula = "", FormulaDes = "", ValorFinal = 0.7f, UserID = "Omar Carranza", CDate = "Ago 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 8, Orden = 10, Nombre = "Indirecto utilidades", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 0.7f, Formula = "", FormulaDes = "", ValorFinal = 0.7f, UserID = "Gerardo Cabrera", CDate = "Sep 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 4, Orden = 11, Nombre = "Costo Tijuana", Regla = true, ValorTipo = FactorValorTipo.Importe, Valor = 0, Formula = "cod2*cod1", FormulaDes = "", ValorFinal = 0, UserID = "Carlos Soto", CDate = "Oct 10, 2016" });
                res.Factores.Add(new Factor { Activo = false, FactorID = 12, Orden = 20, Nombre = "Inactive Sample", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 0, Formula = "", FormulaDes = "", ValorFinal = 0, UserID = "Omar Carranza", CDate = "Nov 10, 2016" });

                response.Content = res;
            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/savefactores", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<FactorResponseFake> SaveFactores(QubicRequest<FactorRequestFake> request)
        {
            QubicResponse<FactorResponseFake> response = new QubicResponse<FactorResponseFake>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                FactorResponseFake res = new FactorResponseFake();
                res.Factores = new List<Factor>();

                //TODO: Fill some hardcoded factors...
                res.Factores.Add(new Factor { Activo = true, FactorID = 1, Orden = 1, Nombre = "Importacion", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 1.05f, Formula = "", FormulaDes = "", ValorFinal = 0, UserID = "Carlos Soto", CDate = "Ene 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 10, Orden = 2, Nombre = "Costo fabricante", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 120f, Formula = "", FormulaDes = "", ValorFinal = 120, UserID = "Omar Carranza", CDate = "Feb 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 13, Orden = 3, Nombre = "Sample Factor", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 1, Formula = "", FormulaDes = "", ValorFinal = 1, UserID = "Gerardo Cabrera", CDate = "Mar 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 11, Orden = 4, Nombre = "Desc fab a CAS", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 1, Formula = "", FormulaDes = "", ValorFinal = 1, UserID = "Carlos Soto", CDate = "Abr 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 9, Orden = 5, Nombre = "Costo USA", Regla = true, ValorTipo = FactorValorTipo.Importe, Valor = 0, Formula = "cod10*cod11", FormulaDes = "", ValorFinal = 0, UserID = "Omar Carranza", CDate = "May 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 7, Orden = 6, Nombre = "IGI", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 1, Formula = "", FormulaDes = "", ValorFinal = 1, UserID = "Gerardo Cabrera", CDate = "Jun 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 2, Orden = 7, Nombre = "Costo Import", Regla = true, ValorTipo = FactorValorTipo.Importe, Valor = 0, Formula = "cod9*cod7", FormulaDes = "", ValorFinal = 0, UserID = "Carlos Soto", CDate = "Jul 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 3, Orden = 9, Nombre = "Indirecto operacion", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 0.7f, Formula = "", FormulaDes = "", ValorFinal = 0.7f, UserID = "Omar Carranza", CDate = "Ago 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 8, Orden = 10, Nombre = "Indirecto utilidades", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 0.7f, Formula = "", FormulaDes = "", ValorFinal = 0.7f, UserID = "Gerardo Cabrera", CDate = "Sep 10, 2016" });
                res.Factores.Add(new Factor { Activo = true, FactorID = 4, Orden = 11, Nombre = "Costo Tijuana", Regla = true, ValorTipo = FactorValorTipo.Importe, Valor = 0, Formula = "cod2*cod1", FormulaDes = "", ValorFinal = 0, UserID = "Carlos Soto", CDate = "Oct 10, 2016" });
                res.Factores.Add(new Factor { Activo = false, FactorID = 12, Orden = 20, Nombre = "Inactive Sample", Regla = false, ValorTipo = FactorValorTipo.Importe, Valor = 0, Formula = "", FormulaDes = "", ValorFinal = 0, UserID = "Omar Carranza", CDate = "Nov 10, 2016" });

                response.Content = res;
            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/deletefactor", ResponseFormat = WebMessageFormat.Json)]
        public QubicResponse<FactorResponseFake> DeleteFactor(QubicRequest<FactorRequestFake> request)
        {
            QubicResponse<FactorResponseFake> response = new QubicResponse<FactorResponseFake>();

            response.ResponseCode = "200";

            try
            {
                if (!ValidateIfTokenIsValid(request.Token))
                {
                    throw new ApplicationException("El token actual no es valido, favor de iniciar sesion nuevamente.");
                }

                FactorResponseFake res = new FactorResponseFake();
            }
            catch (Exception)
            {
                response.ResponseCode = "500";
                response.FriendlyMessage = "Se ha producido una falla interna en el sistema.";
                //TODO: Log Exception...
            }

            return response;
        }

        #endregion

    }
}
