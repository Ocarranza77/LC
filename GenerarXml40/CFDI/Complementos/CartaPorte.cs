using System;
using System.Collections.Generic;

namespace GenerarXml40
{
    public partial class CartaPorte
    {
        public CartaPorte()
        {
            this.Version = "2.0";
        }

        public Ubicaciones Ubicaciones { get; set; }
        public Mercancias Mercancias { get; set; }
        public FiguraTransporte FiguraTransporte { get; set; }
        public string Version { get; set; }
        public string TranspInternac { get; set; }
        public string EntradaSalidaMerc { get; set; }
        public string PaisOrigenDestino { get; set; }
        public string ViaEntradaSalida { get; set; }
        public decimal TotalDistRec { get; set; }
    }
    public partial class Ubicaciones
    {
        public List<Ubicacion> Ubicacion { get; set; } = new List<Ubicacion>();
    }
    public partial class Ubicacion
    {
        public Domicilio Domicilio { get; set; }
        public string TipoUbicacion { get; set; }
        public string IDUbicacion { get; set; }
        public string RFCRemitenteDestinatario { get; set; }
        public string NombreRemitenteDestinatario { get; set; }
        public string NumRegIdTrib { get; set; }
        public string ResidenciaFiscal { get; set; }
        public string NumEstacion { get; set; }
        public string NombreEstacion { get; set; }
        public string NavegacionTrafico { get; set; }
        public DateTime FechaHoraSalidaLlegada { get; set; }
        public string TipoEstacion { get; set; }
        public decimal DistanciaRecorrida { get; set; }
    }
    public partial class Domicilio
    {
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string Colonia { get; set; }
        public string Localidad { get; set; }
        public string Referencia { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string CodigoPostal { get; set; }
    }
    public partial class Mercancias
    {
        public List<Mercancia> Mercancia { get; set; } = new List<Mercancia>();
        public Autotransporte Autotransporte { get; set; }
        public TransporteMaritimo TransporteMaritimo { get; set; }
        public TransporteAereo TransporteAereo { get; set; }
        public TransporteFerroviario TransporteFerroviario { get; set; }
        public decimal PesoBrutoTotal { get; set; }
        public string UnidadPeso { get; set; }
        public decimal PesoNetoTotal { get; set; }
        public int NumTotalMercancias { get; set; }
        public decimal CargoPorTasacion { get; set; }
    }
    public partial class Mercancia
    {
        public List<Pedimentos> Pedimentos { get; set; } = new List<Pedimentos>();
        public List<GuiasIdentificacion> GuiasIdentificacion { get; set; } = new List<GuiasIdentificacion>();
        public List<CantidadTransporta> CantidadTransporta { get; set; } = new List<CantidadTransporta>();
        public DetalleMercancia DetalleMercancia { get; set; } = new DetalleMercancia();
        public string BienesTransp { get; set; }
        public string ClaveSTCC { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public string ClaveUnidad { get; set; }
        public string Unidad { get; set; }
        public string Dimensiones { get; set; }
        public string MaterialPeligroso { get; set; }
        public string CveMaterialPeligroso { get; set; }
        public string Embalaje { get; set; }
        public string DescripEmbalaje { get; set; }
        public decimal PesoEnKg { get; set; }
        public decimal ValorMercancia { get; set; }
        public string Moneda { get; set; }
        public string FraccionArancelaria { get; set; }
        public string UUIDComercioExt { get; set; }
    }
    public partial class Pedimentos
    {
        public string Pedimento { get; set; }
    }
    public partial class GuiasIdentificacion
    {
        public string NumeroGuiaIdentificacion { get; set; }
        public string DescripGuiaIdentificacion { get; set; }
        public decimal PesoGuiaIdentificacion { get; set; }
    }
    public partial class CantidadTransporta
    {
        public decimal Cantidad { get; set; }
        public string IDOrigen { get; set; }
        public string IDDestino { get; set; }
        public string CvesTransporte { get; set; }
    }
    public partial class DetalleMercancia
    {
        public string UnidadPesoMerc { get; set; }
        public decimal PesoBruto { get; set; }
        public decimal PesoNeto { get; set; }
        public decimal PesoTara { get; set; }
        public int NumPiezas { get; set; }
    }
    public partial class Autotransporte
    {
        public IdentificacionVehicular IdentificacionVehicular { get; set; } = new IdentificacionVehicular();
        public Seguros Seguros { get; set; } = new Seguros();
        public Remolques Remolques { get; set; } = new Remolques();
        public string PermSCT { get; set; }
        public string NumPermisoSCT { get; set; }
    }
    public partial class IdentificacionVehicular
    {
        public string ConfigVehicular { get; set; }
        public string PlacaVM { get; set; }
        public int AnioModeloVM { get; set; }
    }
    public partial class Seguros
    {
        public string AseguraRespCivil { get; set; }
        public string PolizaRespCivil { get; set; }
        public string AseguraMedAmbiente { get; set; }
        public string PolizaMedAmbiente { get; set; }
        public string AseguraCarga { get; set; }
        public string PolizaCarga { get; set; }
        public decimal PrimaSeguro { get; set; } = -1m;
    }
    public partial class Remolques
    {
        public List<Remolque> Remolque { get; set; } = new List<Remolque>();
    }
    public partial class Remolque
    {
        public string SubTipoRem { get; set; }
        public string Placa { get; set; }
    }
    public partial class TransporteMaritimo
    {
        public List<Contenedor> Contenedor { get; set; } = new List<Contenedor>();
        public string PermSCT { get; set; }
        public string NumPermisoSCT { get; set; }
        public string NombreAseg { get; set; }
        public string NumPolizaSeguro { get; set; }
        public string TipoEmbarcacion { get; set; }
        public string Matricula { get; set; }
        public string NumeroOMI { get; set; }
        public int AnioEmbarcacion { get; set; }
        public string NombreEmbarc { get; set; }
        public string NacionalidadEmbarc { get; set; }
        public decimal UnidadesDeArqBruto { get; set; }
        public string TipoCarga { get; set; }
        public string NumCertITC { get; set; }
        public decimal Eslora { get; set; } = -1m;
        public decimal Manga { get; set; } = -1m;
        public decimal Calado { get; set; } = -1m;
        public string LineaNaviera { get; set; }
        public string NombreAgenteNaviero { get; set; }
        public string NumAutorizacionNaviero { get; set; }
        public string NumViaje { get; set; }
        public string NumConocEmbarc { get; set; }
    }
    public partial class Contenedor
    {
        public string MatriculaContenedor { get; set; }
        public string TipoContenedor { get; set; }
        public string NumPrecinto { get; set; }
    }
    public partial class TransporteAereo
    {
        public string PermSCT { get; set; }
        public string NumPermisoSCT { get; set; }
        public string MatriculaAeronave { get; set; }
        public string NombreAseg { get; set; }
        public string NumPolizaSeguro { get; set; }
        public string NumeroGuia { get; set; }
        public string LugarContrato { get; set; }
        public string CodigoTransportista { get; set; }
        public string RFCEmbarcador { get; set; }
        public string NumRegIdTribEmbarc { get; set; }
        public string ResidenciaFiscalEmbarc { get; set; }
        public string NombreEmbarcador { get; set; }
    }
    public partial class TransporteFerroviario
    {
        public List<DerechosDePaso> DerechosDePaso { get; set; } = new List<DerechosDePaso>();
        public List<Carro> Carro { get; set; } = new List<Carro>();
        public string TipoDeServicio { get; set; }
        public string TipoDeTrafico { get; set; }
        public string NombreAseg { get; set; }
        public string NumPolizaSeguro { get; set; }
    }
    public partial class DerechosDePaso
    {
        public string TipoDerechoDePaso { get; set; }
        public decimal KilometrajePagado { get; set; }
    }
    public partial class Carro
    {
        public List<CarroContenedor> Contenedor { get; set; } = new List<CarroContenedor>();
        public string TipoCarro { get; set; }
        public string MatriculaCarro { get; set; }
        public string GuiaCarro { get; set; }
        public decimal ToneladasNetasCarro { get; set; }
    }
    public partial class CarroContenedor
    {
        public string TipoContenedor { get; set; }
        public decimal PesoContenedorVacio { get; set; }
        public decimal PesoNetoMercancia { get; set; }
    }
    public partial class FiguraTransporte
    {
        public List<TiposFigura> TiposFigura { get; set; } = new List<TiposFigura>();
    }
    public partial class TiposFigura
    {
        public List<PartesTransporte> PartesTransporte { get; set; } = new List<PartesTransporte>();
        public Domicilio Domicilio { get; set; } = new Domicilio();
        public string TipoFigura { get; set; }
        public string RFCFigura { get; set; }
        public string NumLicencia { get; set; }
        public string NombreFigura { get; set; }
        public string NumRegIdTribFigura { get; set; }
        public string ResidenciaFiscalFigura { get; set; }
    }
    public partial class PartesTransporte
    {
        public string ParteTransporte { get; set; }
    }

}