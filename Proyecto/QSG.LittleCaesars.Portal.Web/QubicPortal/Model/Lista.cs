using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QSG.QSystem.Common.Entities;

namespace QubicPortal.Model
{
    public class Lista
    {
        public bool Activo { get; set; }

        public int ListID { get; set; }

        public string Nombre { get; set; }

        public string Abr { get; set; }

        public Moneda Moneda { get; set; }

        public string VigenciaDesde { get; set; }

        public string VigenciaHasta { get; set; }

        public List<CatalogoTipo> Oficinas { get; set; }

        public string CodUsAltaNombre { get; set; }

        public string FechaAlta { get; set; }
    }
}