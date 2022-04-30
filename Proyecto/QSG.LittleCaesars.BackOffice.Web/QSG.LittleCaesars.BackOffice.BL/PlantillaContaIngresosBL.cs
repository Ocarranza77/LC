using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Constants;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.QSystem.Common.Constants;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class PlantillaContaIngresosBL
    {
        private string _DBName;

        public PlantillaContaIngresosBL(string dbName)
        {
            this._DBName = dbName;
        }


        #region Public
        public PlantillaPolizaIngreso GetPlantillaContaIngresos(int sucursalID, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new PlantillaPolizaIngreso();
            var dal = new PlantillaContaIngresosDAL(_DBName);

            msg = SatinizateQuery(sucursalID);

            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.GetPlantillaContaIngresos(sucursalID, ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public List<PlantillaPolizaIngreso> GetPlantillasContaIngresos(ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = new List<PlantillaPolizaIngreso>();
            var dal = new PlantillaContaIngresosDAL(_DBName);

            result = dal.GetPlantillasContaIngresos(ref msg);
            friendlyMessage = friendlyMessage + msg;

            return result;
        }

        public bool SavePlantillaContaIngresos(PlantillaPolizaIngreso item, ref string friendlyMessage)
        {
            string msg = string.Empty;
            var result = false;
            var dal = new PlantillaContaIngresosDAL(_DBName);

            msg = Satinizate(item);
            if (msg != string.Empty)
            {
                friendlyMessage = friendlyMessage + Generales.msgSigInfo + msg;

                return result;
            }

            result = dal.SavePlantillaPolizaContable(item, ref friendlyMessage);

            return result;
        }

        #endregion

        #region Private

        private string SatinizateQuery(int sucursalID)
        {
            string msg = string.Empty;


            if (sucursalID == 0)
                msg += "Sucursal, ";

            return msg;
        }

        private string Satinizate(PlantillaPolizaIngreso item)
        {
            string msg = string.Empty;

            string msgA = string.Empty;
            string msgA1 = "Las Siguientes Cuentas no son Afectables: ";
            string msgE = string.Empty;
            string msgE1 = "Las siguientes Cuentas no se encuentras en el Catalogo Contable: ";

            var cat = new List<CatalogoContable>();
            var cc = new CatalogoContable();
            var catNoUsado = new List<CatalogoContable>();
            var msk = string.Empty;
            var empDAL = new EmpresaClienteDAL(_DBName);
            var emp = new EmpresaCliente();
            // TODO: Modificar para hacerlo multi empresa, esta como una sola empresa.
            emp = empDAL.GetEmpresa(1, ref msg);

            cat = empDAL.GetCuentasContpaq(emp.EmpresaContpaqRutaBD, false, ref catNoUsado, ref msk, ref msg);


            if (item.Sucursal.SucursalID == 0)
                msg += "SucursalID, ";
            if (!string.IsNullOrEmpty(item.Nombre) )
                msg += "Nombre Poliza, ";

            var asientosC = item.Asientos.Where(a => !string.IsNullOrEmpty(a.CtaContable)).ToList();
            var asientosCC = item.Asientos.Where(a => !string.IsNullOrEmpty(a.CtaComplementearia)).ToList();

            asientosC.AddRange(asientosCC);

            foreach(PlantillaPolizaIngresoAsiento a in asientosC)
            {
                cc = cat.Where(c => c.Cuenta == a.CtaContable).FirstOrDefault();
                if (cc == null)
                    msgE += a.CtaContable;
                else
                {
                    if (!cc.Afectable)
                        msgA += a.CtaContable;
                }
            }

            var bcoC = item.CtasBancarias.Where(a => !string.IsNullOrEmpty(a.CtaContable)).ToList();
            var bcoCC = item.CtasBancarias.Where(a => !string.IsNullOrEmpty(a.CtaComplementearia)).ToList();

            bcoC.AddRange(bcoCC);

            foreach (PlantillaPolizaIngresoCtaBco a in bcoC)
            {
                cc = cat.Where(c => c.Cuenta == a.CtaContable).FirstOrDefault();
                if (cc == null)
                    msgE += a.CtaContable;
                else
                {
                    if (!cc.Afectable)
                        msgA += a.CtaContable;
                }
            }

            if (!string.IsNullOrEmpty(msgA))
                msg += msgA1 + msgA;

            if (!string.IsNullOrEmpty(msgE))
                msg += msgE1 + msgE;

            
            return msg;
        }

        #endregion

    }
}
