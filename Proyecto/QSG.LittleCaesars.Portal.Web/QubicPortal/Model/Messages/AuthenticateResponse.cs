using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }

        public bool Blocked { get; set; }

        public bool InvalidCredentials { get; set; }

        public List<Broker> Brokers { get; set; }

        public Usuario_MPH LoggedUser { get; set; }
    }
}