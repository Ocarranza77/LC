using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class MenuResponse:BaseResponse
    {

        public MenuP Menu { get; set; }
        public List<MenuP> Menus { get; set; }
    }
}
