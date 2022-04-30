using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class PlantillaContaIngresosRequest : BaseRequest
    {
        /// <summary>
        /// ID de la sucursal, cada sucursal solo puede tener una Plantilla de Poliza de Ingresos
        /// </summary>
        public int SucursalID { get; set; }

        /// <summary>
        /// Para obtener el reporte de todas las Plantillas de polizas de ingresos
        /// </summary>
        public bool GetPlantillasContaIngresos { get; set; }


        /// <summary>
        /// Para Guardar la plantilla de Poliza de Ingresos.
        /// </summary>
        public PlantillaPolizaIngreso PlantillaPolizaIngresos { get; set; }


    }
}
