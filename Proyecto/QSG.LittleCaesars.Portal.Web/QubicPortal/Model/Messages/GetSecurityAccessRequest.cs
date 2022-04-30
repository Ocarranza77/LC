using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class GetSecurityAccessRequest
    {
        public string CompanyId { get; set; }

        public string ApplicationId { get; set; }

        public string UserIdentificator { get; set; }
    }
}