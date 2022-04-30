using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class LoadCatalogRequest
    {
        public string CatalogName { get; set; }
        //TODO Add Filters if needed, maybe a dictionary can be usefull...
    }
}