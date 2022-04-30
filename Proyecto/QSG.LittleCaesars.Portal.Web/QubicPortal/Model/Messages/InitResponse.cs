using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace QubicPortal.Model.Messages
{
    [DataContract]
    public class InitResponse
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public bool HidePIN { get; set; }
    }
}