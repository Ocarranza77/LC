using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QSG.QSystem.Common.Entities.SElite;

namespace QubicPortal.Model.Messages
{
    public class InitFormResponse
    {
        public Company Company { get; set; }

        public List<string> Path { get; set; }

        public string ControlName { get; set; }

        public List<string> Permits { get; set; }

        public List<string> SpetialPermits { get; set; }

        public List<string> AcctionForm { get; set; }

        public Usuario User { get; set; }

    }
}