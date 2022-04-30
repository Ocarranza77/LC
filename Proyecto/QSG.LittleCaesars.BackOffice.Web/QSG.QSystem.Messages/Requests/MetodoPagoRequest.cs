﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.QSystem.Common.Entities;

namespace QSG.QSystem.Messages.Requests
{
    public class MetodoPagoSATRequest:BaseRequest
    {
        public MetodoPagoSAT MetodoPago { get; set; }
        public List<MetodoPagoSAT> MetodoPagos { get;set; }
        public bool CboIni { get; set; }

    }
}
