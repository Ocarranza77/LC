using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace QubicPortal.Model
{
    [DataContract]
    public class QubicResponse<T>
    {
        [DataMember]
        public string ResponseCode { get; set; }

        [DataMember]
        public string FriendlyMessage { get; set; }

        [DataMember]
        public string MessageError { get; set; }

        [DataMember]
        public T Content { get; set; }

    }
}