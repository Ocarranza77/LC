using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class SecurityBusiness
    {
        public AuthenticateResponse AuthenticateMessage(AuthenticateRequest request)
        {
            AuthenticateResponse response = new AuthenticateResponse();

            if (request.UserName.Equals("gato@qubic.com"))
            {
                response.Token = "1234-1234578-454545-5545";
                response.Brokers = new List<Broker>();
                response.Brokers.Add(new Broker { ID = 1, Name = "Broker 1", UserIdentificator = "sdsf3453sdfgdfg645645g" });
                response.LoggedUser = new Usuario_MPH { CorreoElectronico = request.UserName, Nombre = "Usuario Gato..." };
            }
            else if (request.UserName.Equals("perro@qubic.com"))
            {
                response.Token = "1234-1234578-454545-5545";
                response.Brokers = new List<Broker>();
                response.Brokers.Add(new Broker { ID = 1, Name = "Broker 1", UserIdentificator = "asdasddfgl5345kjsdf" });
                response.Brokers.Add(new Broker { ID = 2, Name = "Broker 2", UserIdentificator = "adfs3453sdfs5435354" });
                response.LoggedUser = new Usuario_MPH { CorreoElectronico = request.UserName, Nombre = "Usuario Perro..." };
            }
            else if (request.UserName.Equals("blocked@qubic.com"))
            {
                response.Blocked = true;
            }
            else
            {
                response.InvalidCredentials = true;
            }

            return response;
        }

        public void BlockUser(string userid)
        {
        }

        public AuthorizeResponse AuthorizeMessage(AuthorizeRequest request)
        {
            AuthorizeResponse response = new AuthorizeResponse();

            response.Companies = new List<Company>();
            response.Companies.Add(new Company { Id = 1, Name = "Little Caesars", Logo = "images/logo.png", CssFile = "css/companies/company01.css" });
            response.Companies.Add(new Company { Id = 2, Name = "Sucursal Centro", Logo = "images/Qlogo.png", CssFile = "css/companies/company02.css" });
            response.Companies.Add(new Company { Id = 3, Name = "La Presa", Logo = "images/logo.png", CssFile = "css/companies/company01.css" });
            response.Companies.Add(new Company { Id = 4, Name = "Clamatos la 20", Logo = "images/Qlogo.png", CssFile = "css/companies/company02.css" });

            response.MenuOptions = new List<MenuOption>();
            response.MenuOptions.Add(new MenuOption { ShortName = "mnuFavoritos", DisplayName = "Favoritos", Type = MenuType.Application });
            response.MenuOptions[0].Options = new List<MenuOption>();
            response.MenuOptions[0].Options.Add(new MenuOption { DisplayName = "Favorito 1", Type = MenuType.Option, Action="#" });
            response.MenuOptions[0].Options.Add(new MenuOption { DisplayName = "Favorito 2", Type = MenuType.Option, Action = "#" });

            response.MenuOptions.Add(new MenuOption { ShortName = "mnuM1", DisplayName = "Menu 1", Type = MenuType.Application });
            response.MenuOptions[1].Options = new List<MenuOption>();
            response.MenuOptions[1].Options.Add(new MenuOption { ShortName = "mnuM11", DisplayName = "M1 1", Type = MenuType.Module });
            response.MenuOptions[1].Options[0].Options = new List<MenuOption>();
            response.MenuOptions[1].Options[0].Options.Add(new MenuOption { DisplayName = "Option - M1.1 - 1", Type = MenuType.Option, Action = "#" });
            response.MenuOptions[1].Options[0].Options.Add(new MenuOption { DisplayName = "Option - M1.1 - 2", Type = MenuType.Option, Action = "#" });
            response.MenuOptions[1].Options.Add(new MenuOption { ShortName = "mnuM12", DisplayName = "M1 2", Type = MenuType.Module });
            response.MenuOptions[1].Options[1].Options = new List<MenuOption>();
            response.MenuOptions[1].Options[1].Options.Add(new MenuOption { DisplayName = "Option - M1.2 - 1", Type = MenuType.Option, Action = "#" });
            response.MenuOptions[1].Options[1].Options.Add(new MenuOption { DisplayName = "Option - M1.2 - 2", Type = MenuType.Option, Action = "#" });

            response.MenuOptions.Add(new MenuOption { ShortName = "mnuM2", DisplayName = "Menu 2", Type = MenuType.Application });
            response.MenuOptions[2].Options = new List<MenuOption>();
            response.MenuOptions[2].Options.Add(new MenuOption { ShortName = "mnuM21", DisplayName = "M2 1", Type = MenuType.Module });
            response.MenuOptions[2].Options[0].Options = new List<MenuOption>();
            response.MenuOptions[2].Options[0].Options.Add(new MenuOption { DisplayName = "Option - M2.1 - 1", Type = MenuType.Option, Action = "#" });
            response.MenuOptions[2].Options[0].Options.Add(new MenuOption { DisplayName = "Option - M2.1 - 2", Type = MenuType.Option, Action = "#" });
            response.MenuOptions[2].Options.Add(new MenuOption { ShortName = "mnuM22", DisplayName = "M2 2", Type = MenuType.Module });
            response.MenuOptions[2].Options[1].Options = new List<MenuOption>();
            response.MenuOptions[2].Options[1].Options.Add(new MenuOption { DisplayName = "Option - M2.2 - 1", Type = MenuType.Option, Action = "#" });
            response.MenuOptions[2].Options[1].Options.Add(new MenuOption { DisplayName = "Option - M2.2 - 2", Type = MenuType.Option, Action = "#" });

            response.MenuOptions.Add(new MenuOption { ShortName = "mnuM3", DisplayName = "Menu 3", Type = MenuType.Application });
            response.MenuOptions[3].Options = new List<MenuOption>();
            response.MenuOptions[3].Options.Add(new MenuOption { ShortName = "mnuM31", DisplayName = "M3 1", Type = MenuType.Module });
            response.MenuOptions[3].Options[0].Options = new List<MenuOption>();
            response.MenuOptions[3].Options[0].Options.Add(new MenuOption { DisplayName = "Option - M3.1 - 1", Type = MenuType.Option, Action = "#" });
            response.MenuOptions[3].Options[0].Options.Add(new MenuOption { DisplayName = "Option - M3.1 - 2", Type = MenuType.Option, Action = "#" });
            response.MenuOptions[3].Options.Add(new MenuOption { ShortName = "mnuM32", DisplayName = "M3 2", Type = MenuType.Module });
            response.MenuOptions[3].Options[1].Options = new List<MenuOption>();
            response.MenuOptions[3].Options[1].Options.Add(new MenuOption { DisplayName = "Option - M3.2 - 1", Type = MenuType.Option, Action = "abcde" });
            response.MenuOptions[3].Options[1].Options.Add(new MenuOption { DisplayName = "Option - M3.2 - 2", Type = MenuType.Option, Action = "#" });

            response.MenuOptions.Add(new MenuOption { ShortName = "mnuQ1", DisplayName = "Qubic 1", Type = MenuType.QubicMenu });
            response.MenuOptions[4].Options = new List<MenuOption>();
            response.MenuOptions[4].Options.Add(new MenuOption { DisplayName = "MQ1 1", Type = MenuType.QubicOption, Action = "#" });
            response.MenuOptions[4].Options.Add(new MenuOption { DisplayName = "MQ1 2", Type = MenuType.QubicOption, Action = "#" });

            response.MenuOptions.Add(new MenuOption { ShortName = "mnuQ2", DisplayName = "Seguridad", Type = MenuType.QubicMenu });
            response.MenuOptions[5].Options = new List<MenuOption>();
            response.MenuOptions[5].Options.Add(new MenuOption { DisplayName = "Registro de usuarios", Type = MenuType.QubicOption, Action = "1234567890" });
            response.MenuOptions[5].Options.Add(new MenuOption { DisplayName = "MQ2 2", Type = MenuType.QubicOption, Action = "9876543210" });

            response.Notifications = new List<Notification>();
            response.Notifications.Add(new Notification { Title = "Notification 1", CompanyName = "Company Name 1", NotificationType = "message-preview-info", Description = "Sample description for notification 1..." });
            response.Notifications.Add(new Notification { Title = "Notification 2", CompanyName = "Company Name 2", NotificationType = "message-preview-warning", Description = "Sample description for notification 2..." });
            response.Notifications.Add(new Notification { Title = "Notification 3", CompanyName = "Company Name 3", NotificationType = "message-preview-danger", Description = "Sample description for notification 3..." });

            return response;
        }

        #region Users...

        public GetUsersResponse GetUsersMessage(GetUsersRequest request)
        {
            GetUsersResponse response = new GetUsersResponse();


            response.Usuarios = new List<Usuario_MPH>();

            response.Usuarios.Add(new Usuario_MPH { Codigo = 1, Nombre = "Usuario 1", Alias = "sin alias", Puesto = "demo", CorreoElectronico = "demo@qubic.com", Perfil = PerfilType.Administrador, Status = StatusType.Activo, EnviarContrasena = true, SolicitarCambioContrasena = true });
            response.Usuarios.Add(new Usuario_MPH { Codigo = 2, Nombre = "Usuario 2", Alias = "sin alias 2", Puesto = "demo 2", CorreoElectronico = "demo2@qubic.com", Perfil = PerfilType.Usuario, Status = StatusType.Inactivo, EnviarContrasena = false, SolicitarCambioContrasena = false });


            return response;
        
        }

        public SaveUserResponse SaveUserMessage(SaveUserRequest request)
        {
            SaveUserResponse response = new SaveUserResponse();

            return response;
        }

        #endregion Users...

        #region Reset PWD

        public SearchUsersResponse SearchUsersMessage(SearchUsersRequest request)
        {
            SearchUsersResponse response = new SearchUsersResponse();


            response.Usuarios = new List<Usuario_MPH>();

            response.Usuarios.Add(new Usuario_MPH { Codigo = 1, Nombre = "S Usuario 1", Alias = "sin alias", Puesto = "demo", CorreoElectronico = "demo@qubic.com", Perfil = PerfilType.Administrador, Status = StatusType.Activo, EnviarContrasena = true, SolicitarCambioContrasena = true });
            response.Usuarios.Add(new Usuario_MPH { Codigo = 2, Nombre = "S Usuario 2", Alias = "sin alias 2", Puesto = "demo 2", CorreoElectronico = "demo2@qubic.com", Perfil = PerfilType.Usuario, Status = StatusType.Inactivo, EnviarContrasena = false, SolicitarCambioContrasena = false });


            return response;

        }

        public ResetPasswordResponse ResetPasswordMessage(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new ResetPasswordResponse();

            return response;

        }

        #endregion Reset PWD

        #region SecurityAccess

        public SecurityFiltersResponse SecurityFiltersMessage(SecurityFiltersRequest request)
        {
            SecurityFiltersResponse response = new SecurityFiltersResponse();

            response.Filters = new SecurityFilters();
            response.Filters.Companies = new List<Company>();
            response.Filters.Applications = new List<MenuOption>();

            response.Filters.Companies.Add(new Company { Id = -1, Name = "-[ TODAS ]-" });
            response.Filters.Companies.Add(new Company { Id = 1, Name = "Company 1" });
            response.Filters.Companies.Add(new Company { Id = 2, Name = "Company 2" });

            response.Filters.Applications.Add(new MenuOption { Id = -1, ShortName = "", DisplayName = "-[ TODAS ]-" });
            response.Filters.Applications.Add(new MenuOption { Id = 1, ShortName = "C1A1", DisplayName = "C1 Application 1" });
            response.Filters.Applications.Add(new MenuOption { Id = 2, ShortName = "C1A2", DisplayName = "C1 Application 2" });

            return response;
        }

        public GetSecurityAccessResponse GetSecurityAccessMessage(GetSecurityAccessRequest request)
        {
            GetSecurityAccessResponse response = new GetSecurityAccessResponse();
            List<dynamic> finalData = new List<dynamic>();

            dynamic data = new
            {
                CompanyId = 1,
                CompanyName = "Company 1",
                ApplicationId = 1,
                ApplicationName = "Application 1",
                ModuleId = 1,
                ModuleName = "Module 1",
                ScreenId = 1,
                ScreenName = "Form 1",
                Read = true,
                Write = true,
                Print = true,
                Delete = true,
                Export = true,
                OthersValue = "O1, O3",
                Others = new List<dynamic>() { new { Id = "O1", Name = "Other 1", Granted = true }, new { Id = "O2", Name = "Other 2", Granted = false }, new { Id = "O3", Name = "Other 3", Granted = true } }
            };
            finalData.Add(data);

            data = new
            {
                CompanyId = 2,
                CompanyName = "Company 2",
                ApplicationId = 2,
                ApplicationName = "Application 1",
                ModuleId = 2,
                ModuleName = "Module 2",
                ScreenId = 2,
                ScreenName = "Form 2",
                Read = false,
                Write = false,
                Print = false,
                Delete = false,
                Export = false,
                OthersValue = "O1",
                Others = new List<dynamic>() { new { Id = "O1", Name = "Other 1", Granted = true }, new { Id = "O2", Name = "Other 2", Granted = false }, new { Id = "O3", Name = "Other 3", Granted = false } }
            };
            finalData.Add(data);

            data = new
            {
                CompanyId = 2,
                CompanyName = "Company 2",
                ApplicationId = 2,
                ApplicationName = "Application 2",
                ModuleId = 2,
                ModuleName = "Module 2",
                ScreenId = 2,
                ScreenName = "Form 2",
                Read = false,
                Write = false,
                Print = false,
                Delete = false,
                Export = false,
                OthersValue = "O1",
                Others = new List<dynamic>() { new { Id = "O1", Name = "Other 1", Granted = true }, new { Id = "O2", Name = "Other 2", Granted = false }, new { Id = "O3", Name = "Other 3", Granted = false } }
            };
            finalData.Add(data);

            data = new
            {
                CompanyId = 2,
                CompanyName = "Company 2",
                ApplicationId = 2,
                ApplicationName = "Application 2",
                ModuleId = 2,
                ModuleName = "Module 2",
                ScreenId = 2,
                ScreenName = "Form 2",
                Read = false,
                Write = false,
                Print = false,
                Delete = false,
                Export = false,
                OthersValue = "O1",
                Others = new List<dynamic>() { new { Id = "O1", Name = "Other 1", Granted = true }, new { Id = "O2", Name = "Other 2", Granted = false }, new { Id = "O3", Name = "Other 3", Granted = false } }
            };
            finalData.Add(data);


            string json = Newtonsoft.Json.JsonConvert.SerializeObject(finalData);

            response.SecurityInfo = json;


            return response;
        }

        #endregion SecurityAccess

        #region Qubic Form

        public InitFormResponse InitFormMessage(InitFormRequest request)
        {

            var controlid = request.ControlID;

            InitFormResponse response = new InitFormResponse();

            response.Company = new Company { Id = 123, Name = "Little Caesars", Logo = "images/logo.png" };
            response.Permits = new List<string>() { "Save", "Search", "Report", "Clear" };
//            response.Permits = new List<int>() { 1, 1, 1, 1 };

            if (request.ControlID == 1) 
            {
                response.ControlName = "ctl_regusuarios.html";
                response.Path = new List<string>() { "Aplicacion 1", "Modulo 1", "Forma 1" };
            }
            if (request.ControlID == 2) 
            {
                response.ControlName = "ctl_resetpwd.html";
                response.Path = new List<string>() { "Aplicacion 1", "Modulo 1", "Forma 2" };
            }
            if (request.ControlID == 3) 
            {
                response.ControlName = "ctl_security.html";
                response.Path = new List<string>() { "Aplicacion 1", "Modulo 1", "Forma 3" };
            }

            return response;

        }

        #endregion Qubic Form
    }
}