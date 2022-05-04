using System.Collections.Generic;

namespace GenerarXml40
{
    public class Concepto
    {
        public string ClaveProdServ { get; set; } = string.Empty;
        public string NoIdentificacion { get; set; } = string.Empty;
        public decimal Cantidad { get; set; } = 0m;
        public string ClaveUnidad { get; set; } = string.Empty;
        public string Unidad { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal ValorUnitario { get; set; } = 0m;
        public decimal Importe { get; set; } = 0m;
        public decimal Descuento { get; set; } = 0m;
        public string ObjetoImp { get; set; } = string.Empty;
        public ImpuestosC Impuestos { get; set; } = new ImpuestosC();
        public ACuentaTercerosC ACuentaTerceros { get; set; } = new ACuentaTercerosC();
        public List<InformacionAduaneraC> InformacionAduanera { get; set; } = new List<InformacionAduaneraC>();
        public List<CuentaPredialC> CuentaPredial { get; set; } = new List<CuentaPredialC>();
        /************************* Falta agregar la clase ComplementoConcepto *************************************/

        public ComplementoC Complemento { get; set; } = new ComplementoC();

        public List<ParteC> Parte { get; set; } = new List<ParteC>();


    }
}