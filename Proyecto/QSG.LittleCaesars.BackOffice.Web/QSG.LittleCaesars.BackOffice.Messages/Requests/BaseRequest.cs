using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using QSG.QSystem.Common.Enums;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class BaseRequest
    {
        public string Token { get; set; }

        public MessageOperationType MessageOperationType { get; set; }

        public Dictionary<string, string> OtherParams { get; set; }

        public string BDName { get; set; }

        public int UserIDRqst { get; set; }
   
    }
}
