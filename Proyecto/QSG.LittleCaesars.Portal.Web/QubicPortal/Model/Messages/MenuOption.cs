using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class MenuOption
    {

        public int Id { get; set; }

        public MenuType Type { get; set; }

        public string DisplayName { get; set; }

        public string ShortName { get; set; }

        public List<MenuOption> Options { get; set; }

        public string Action { get; set; }

        public List<Form> Forms { get; set; }

    }
}