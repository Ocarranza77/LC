using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSG.LittleCaesars.BackOffice.Common.Entities
{
    [Serializable, XmlRoot("CorteSucursal")]
    public class CorteSucursal:BaseEntity
    {

        private double _TotalDeudorEnPesos = 0;
        private double _TotalDepositosP = 0;
        private double _TotalDepositosD = 0;
        private double _TotalPorDepositarP = 0;
        private double _TotalPorDepositarD = 0;

        public DateTime FechaVta { get; set; }
        public Sucursal Sucursal { get; set; }
        public int NoZ { get; set; }
        public double EfectivoZ { get; set; }
        public double Total { get; set; }
        public double TotalTCredito { get; set; }
        public double TotalTDebito { get; set; }
        public double TotalOtraFormaPago { get; set; }
        public double PesosADeposito { get; set; }
        public double DolarADeposito { get; set; }
        public double TC { get; set; }
        public double PesosSB { get; set; }
        public double DolarSB { get; set; }
        public double Gastos { get; set; }
        public double Ajuste { get; set; }
        public string Comentarios { get; set; }
        public string Supervisor { get; set; }
        public string CajeroCorto { get; set; }
        public string Stt { get; set; }
        public string DeudorNombre { get; set; }
        public double DeudorPesos { get; set; }
        public double DeudorDolar { get; set; }
        public int CodUsuario { get; set; }
        public double Faltante { get { return Ajuste < 0 ? Ajuste * -1 : 0; } set { Ajuste = value > 0 ? value * -1 : Ajuste; } } //{ get { return Ajuste < 0 ? Ajuste * -1 : 0; } set { Ajuste = value * -1; } }
        public double Sobrante { get { return Ajuste >= 0 ? Ajuste : 0; } set { Ajuste = value; } } 

        public string FolioFactura { get; set; }
        

        public List<CorteSucursalDeposito> Depositos { get; set; }

        
        public double TotalDepositosP {
            get
            {
                double result = 0;
                if (_TotalDepositosP == 0)
                {
                    if (Depositos != null)
                        foreach (CorteSucursalDeposito itm in Depositos)
                            result += itm.CuentaBanco.Moneda.MonedaID == 1 ? itm.Importe : 0;
                }
                else
                {
                    result = _TotalDepositosP;
                }

                return result;
            } 
            set{ _TotalDepositosP = value;  } 
        }

        public double TotalDepositosD {
            get
            {
                double result = 0;
                if (_TotalDepositosD == 0)
                {
                    if (Depositos != null)
                        foreach (CorteSucursalDeposito itm in Depositos)
                            result += itm.CuentaBanco.Moneda.MonedaID == 2 ? itm.Importe : 0;
                }
                else
                {
                    result = _TotalDepositosD;
                }

                return result;
            }
            set{ _TotalDepositosD = value; } 
        }

        public double TotalPorDepositarP { 
            get{double result = 0;

            if (_TotalPorDepositarP == 0)
                result = ((PesosADeposito + PesosSB) - TotalDepositosP) - DeudorPesos;
                /* result =  (PesosADeposito + (DolarADeposito * TC))  
                   - (TotalDepositosP + (TotalDepositosD * TC))
                   - (DeudorPesos + (DeudorDolar * TC));*/

                

                return result;    
            } 
            set{ _TotalPorDepositarP = value; } 
        } //Esto en realidad es desglosarlo con formula

        public double TotalPorDepositarD
        {
            get
            {
                double result = 0;

                if (_TotalPorDepositarD == 0)
                    result = ((DolarADeposito + DolarSB) - TotalDepositosD) - DeudorDolar;
                /* result =  (PesosADeposito + (DolarADeposito * TC))  
                   - (TotalDepositosP + (TotalDepositosD * TC))
                   - (DeudorPesos + (DeudorDolar * TC));*/



                return result;
            }
            set { _TotalPorDepositarD = value; }
        } //Esto en realidad es desglosarlo con formula

       /* public double TotalDeudorEnPesos { 
            get{double result = 0;

                if(_TotalDeudorEnPesos == 0)
                      result =  (PesosADeposito + (DolarADeposito * TC))  
                        - (TotalDepositosP + (TotalDepositosD * TC))
                        - (DeudorPesos + (DeudorDolar * TC));

                return result;    
            } 
            set{ _TotalDeudorEnPesos = value; } 
        } //Falta formula para convertir los dolares
        */
        
    }
}
