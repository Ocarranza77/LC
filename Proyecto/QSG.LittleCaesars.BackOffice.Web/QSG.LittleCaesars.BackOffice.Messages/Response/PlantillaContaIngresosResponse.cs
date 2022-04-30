using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
    public class PlantillaContaIngresosResponse : BaseResponse
    {
       /// <summary>
        /// Retorno de la consulta de la SucursalID (que corresponte con su plantilla de poliza de ingresos)
       /// </summary>
        public PlantillaPolizaIngreso PlantillaContaIngresos { get; set; }

       /// <summary>
       /// Reporte del guardado de la Plantilla
       /// </summary>
       public bool GraboPlantilla { get; set; }

       /// <summary>
       /// Listado de las Plantillas Contrables de Ingresos  (consulta de GetPlantillasContaIngresos)
       /// </summary>
       public List<PlantillaPolizaIngreso> PlantillasContaIngresos { get; set; }

    }
}
