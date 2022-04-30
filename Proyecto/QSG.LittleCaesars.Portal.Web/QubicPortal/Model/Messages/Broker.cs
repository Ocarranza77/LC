using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace QubicPortal.Model.Messages
{
    [DataContract]
    public class Broker
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string UserIdentificator { get; set; }
    }
}