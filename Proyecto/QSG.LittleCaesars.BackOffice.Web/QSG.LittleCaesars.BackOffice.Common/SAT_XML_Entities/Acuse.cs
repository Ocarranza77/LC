﻿using System;

namespace QSG.LittleCaesars.BackOffice.Common.SAT_XML_Entities
{
    public class Folios
    {
        public string UUID { get; set; } = string.Empty;
        public string EstatusUUID { get; set; } = string.Empty;
    }
    public class Acuse
    {
        public DateTime Fecha { get; set; }
        public string RfcEmisor { get; set; } = string.Empty;
        public string SelloDigitalSAT { get; set; } = string.Empty;
        public Folios Folios { get; set; } = new Folios();

    }
}