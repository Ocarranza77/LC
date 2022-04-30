using System.Runtime.Serialization;
using QSG.QAGC.Common.Enums;

namespace QubicPortal.Model
{
    [DataContract]
    public class Factor
    {
        [DataMember]
        public int FactorID { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Abr { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Regla { get; set; }
        [DataMember]
        public FactorValorTipo ValorTipo { get; set; }
        [DataMember]
        public float Valor { get; set; }
        [DataMember]
        public string Formula { get; set; }
        [DataMember]
        public string FormulaDes { get; set; }
        [DataMember]
        public int Orden { get; set; }
        [DataMember]
        public bool Activo { get; set; }
        [DataMember]
        public float ValorFinal { get; set; }
        
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string CDate { get; set; }

    }
}
