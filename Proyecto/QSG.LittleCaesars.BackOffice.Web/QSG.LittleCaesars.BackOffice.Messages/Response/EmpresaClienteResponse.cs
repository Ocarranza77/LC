using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Response
{
   public  class EmpresaClienteResponse:BaseResponse
    {
       /// <summary>
       /// Retorno de la consulta de la Empresa Cliente
       /// </summary>
       public EmpresaCliente EmpresaCliente { get; set; }

        /// <summary>
        /// Retorno de Todas las Empresas del Cliente
        /// </summary>
        public List<EmpresaCliente> EmpresasCliente { get; set; }

        /// <summary>
        /// Reporto del guardado de la empresa cliente
        /// </summary>
        public bool GraboEmpresaCliente { get; set; }

       /// <summary>
       /// Listado de las empresas contpaq  (consulta de GetEmpresasContpaq)
       /// </summary>
       public List<EmpresaCliente> EmpresasContpaq { get; set; }

       /// <summary>
       /// Catalogo de cuentas contables (consulta de  GetCatalogoCuentasContpaq)
       /// </summary>
       public List<CatalogoContable> CatalogoCuentasContpaq { get; set; }

       /// <summary>
       /// Catalogo de cuentas contables (consulta de  GetCatalogoCuentasContpaq)
       /// </summary>
       public List<CatalogoContable> CuentasNoEncontradas { get; set; }

       /// <summary>
       /// Catalogo de cuentas contables (consulta de  GetCatalogoCuentasContpaq)
       /// </summary>
       public string Mascarilla { get; set; }

       /// <summary>
       /// Listado de las plantillas de polizas de ingreso donde se utiliza una cta contable ( consulta de CuentaContableUsadaEn)
       /// </summary>
       public List<PlantillaPolizaIngreso> CuentasContpaqUsadasEn { get; set; }


       /// <summary>
       /// Indicador si el linkServer con contpaq se creo sin problemas.
       /// </summary>
       public bool LinkContpaqCreado { get; set; }

    }
}
