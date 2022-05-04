using System;
using System.Collections.Generic;

namespace QSG.LittleCaesars.BackOffice.Common.SAT_XML_Entities
{
    public class Pagos
    {
        public Pagos()
        {
            this.Version = "2.0";
        }

        public string Version { get; set; } = "2.0";
        public PTotales Totales { get; set; } = new PTotales();
        public List<Pago> Pago { get; set; } = new List<Pago>();
    }
    public class PTotales
    {
        public PTotales()
        {
        }

        public decimal TotalRetencionesIVA { get; set; } = 0m;
        public decimal TotalRetencionesISR { get; set; } = 0m;
        public decimal TotalRetencionesIEPS { get; set; } = 0m;
        public decimal TotalTrasladosBaseIVA16 { get; set; } = 0m;
        public decimal TotalTrasladosImpuestoIVA16 { get; set; } = 0m;
        public decimal TotalTrasladosBaseIVA8 { get; set; } = 0m;
        public decimal TotalTrasladosImpuestoIVA8 { get; set; } = 0m;
        public decimal TotalTrasladosBaseIVA0 { get; set; } = 0m;
        public decimal TotalTrasladosImpuestoIVA0 { get; set; } = 0m;
        public decimal TotalTrasladosBaseIVAExento { get; set; } = 0m;
        public decimal MontoTotalPagos { get; set; } = 0m;
    }
    public class Pago
    {
        public Pago()
        {
        }

        public DateTime FechaPago { get; set; }
        public string FormaDePagoP { get; set; } = string.Empty;
        public string MonedaP { get; set; } = string.Empty;
        public decimal TipoCambioP { get; set; } = 0m;
        public decimal Monto { get; set; } = 0m;
        public string NumOperacion { get; set; } = string.Empty;
        public string RfcEmisorCtaOrd { get; set; } = string.Empty;
        public string NomBancoOrdExt { get; set; } = string.Empty;
        public string CtaOrdenante { get; set; } = string.Empty;
        public string RfcEmisorCtaBen { get; set; } = string.Empty;
        public string CtaBeneficiario { get; set; } = string.Empty;
        public string TipoCadPago { get; set; } = string.Empty;
        public string CertPago { get; set; } = string.Empty;
        public string CadPago { get; set; } = string.Empty;
        public string SelloPago { get; set; } = string.Empty;
        public List<PDoctoRelacionado> DoctoRelacionado { get; set; } = new List<PDoctoRelacionado>();
        public ImpuestosP Impuestos { get; set; } = null;
    }
    public class PDoctoRelacionado
    {
        public PDoctoRelacionado()
        {
        }

        public string IdDocumento { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public string Folio { get; set; } = string.Empty;
        public string MonedaDR { get; set; } = string.Empty;
        public decimal EquivalenciaDR { get; set; } = 0m;
        public string NumParcialidad { get; set; } = string.Empty;
        public decimal ImpSaldoAnt { get; set; } = 0m;
        public decimal ImpPagado { get; set; } = 0m;
        public decimal ImpSaldoInsoluto { get; set; } = 0m;
        public string ObjetoImpDR { get; set; } = string.Empty;

        public ImpuestosDR ImpuestosDR { get; set; } = null;
    }
    public class ImpuestosDR 
    {
        public ImpuestosDR()
        {
        }

        public RetencionesDR RetencionesDR { get; set; } = null;
        public TrasladosDR TrasladosDR { get; set; } = null;
    }
    public class RetencionesDR
    {
        public RetencionesDR()
        {
        }

        public List<RetencionDR> RetencionDR { get; set; } = null;
    }
    public class RetencionDR
    {
        public RetencionDR()
        {
        }

        public decimal BaseDR { get; set; } = 0m;
        public string ImpuestoDR { get; set; } = string.Empty;
        public string TipoFactorDR { get; set; } = string.Empty;
        public decimal TasaOCuotaDR { get; set; } = 0m;
        public decimal ImporteDR { get; set; } = 0m;
    }
    public class TrasladosDR
    {
        public TrasladosDR()
        {
        }

        public List<TrasladoDR> TrasladoDR { get; set; } = null;
    }
    public class TrasladoDR
    {
        public TrasladoDR()
        {
        }

        public decimal BaseDR { get; set; } = 0m;
        public string ImpuestoDR { get; set; } = string.Empty;
        public string TipoFactorDR { get; set; } = string.Empty;
        public decimal TasaOCuotaDR { get; set; } = 0m;
        public decimal ImporteDR { get; set; } = 0m;
    }
    public class ImpuestosP
    {
        public ImpuestosP()
        {
        }

        public RetencionesP RetencionesP { get; set; } = null;
        public TrasladosP TrasladosP { get; set; } = null;
    }
    public class RetencionesP
    {
        public RetencionesP()
        {
        }

        public List<RetencionP> RetencionP { get; set; } = null;
    }
    public class RetencionP
    {
        public RetencionP()
        {
        }

        public string ImpuestoP { get; set; } = string.Empty;
        public decimal ImporteP { get; set; } = 0m;
    }
    public class TrasladosP
    {
        public TrasladosP()
        {
        }

        public List<TrasladoP> TrasladoP { get; set; } = null;

    }    
    public class TrasladoP
    {
        public TrasladoP()
        {
        }

        public decimal BaseP { get; set; } = 0m;
        public string ImpuestoP { get; set; } = string.Empty;
        public string TipoFactorP { get; set; } = string.Empty;
        public decimal TasaOCuotaP { get; set; } = 0m;
        public decimal ImporteP { get; set; } = 0m;
    }

}