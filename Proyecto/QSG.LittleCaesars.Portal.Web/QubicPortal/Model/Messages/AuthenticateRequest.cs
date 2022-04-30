using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace QubicPortal.Model.Messages
{
    [DataContract]
    public class AuthenticateRequest
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string PIN { get; set; }
    }
}