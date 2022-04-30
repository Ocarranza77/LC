using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class AuthorizeResponse
    {
        public List<Company> Companies { get; set; }

        public List<MenuOption> MenuOptions { get; set; }

        public Usuario_MPH LoggedUser { get; set; }

        public List<Notification> Notifications { get; set; }
    }
}