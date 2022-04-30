using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class AuthorizeRequest
    {
        public string UserIdentificator { get; set; }

        public int CompanyId { get; set; }
    }
}