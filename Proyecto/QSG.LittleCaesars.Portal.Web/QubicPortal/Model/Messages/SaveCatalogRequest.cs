using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class SaveCatalogRequest
    {
        public string CatalogName { get; set; }
        public string CatalogItems { get; set; }
    }
}