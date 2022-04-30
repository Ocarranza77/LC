using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class Notification
    {
        public string Title { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string NotificationType { get; set; }
    }
}