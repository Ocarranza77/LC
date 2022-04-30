using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public List<MenuOption> Applications { get; set; }

        public string CssFile { get; set; }
    }
}