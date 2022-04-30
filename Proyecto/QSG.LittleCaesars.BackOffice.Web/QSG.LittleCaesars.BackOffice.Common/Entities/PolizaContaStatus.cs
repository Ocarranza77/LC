﻿using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("PolizaContaStatus")]
    public class PolizaContaStatus : BaseEntity
    {
        public DateTime CodPoliza { get; set; }

        public string SttConDe { get; set; }
        public string SttConA { get; set; }
        public string Tipo { get; set; }

    }
}
