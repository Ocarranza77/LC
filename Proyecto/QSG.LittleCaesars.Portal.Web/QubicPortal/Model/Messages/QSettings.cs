using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class QSettings
    {
        public Dictionary<MenuType, ColorSettings> MenuSettings { get; set; } 
    }
}