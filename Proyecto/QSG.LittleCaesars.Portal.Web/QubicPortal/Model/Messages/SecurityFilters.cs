using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class SecurityFilters
    {
        public List<Company> Companies { get; set; }

        public List<MenuOption> Applications { get; set; }

        public bool ShowUserProfile { get; set; }
    }
}