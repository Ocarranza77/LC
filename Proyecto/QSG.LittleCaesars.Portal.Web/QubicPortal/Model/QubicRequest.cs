using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace QubicPortal.Model
{
    [DataContract]
    public class QubicRequest<T>
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public T RequestMessage { get; set; }
    }
}