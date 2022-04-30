using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Enums;

namespace QSG.QSystem.Messages.Response
{
    public class BaseResponse
    {
        public MessageResultType ResultType { get; set; }

        public string FriendlyMessage { get; set; }
    }
}
