using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Entities;

namespace QSG.QSystem.Messages.Response
{
    public class MenuResponse : BaseResponse
    {

        public MenuP Menu { get; set; }
        public List<MenuP> Menus { get; set; }
    }
}
