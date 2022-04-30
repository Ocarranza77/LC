using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class ResetPasswordRequest
    {
        public Usuario_MPH Usuario { get; set; }

        public ResetPassword Reset { get; set; }
    }
}