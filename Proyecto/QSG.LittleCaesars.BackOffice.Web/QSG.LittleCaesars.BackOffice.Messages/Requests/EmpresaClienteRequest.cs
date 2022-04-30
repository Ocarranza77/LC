using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSG.LittleCaesars.BackOffice.Common.Entities;

namespace QSG.LittleCaesars.BackOffice.Messages.Requests
{
    public class EmpresaClienteRequest:BaseRequest
    {
        /// <summary>
        /// Para Grabar
        /// </summary>
        public EmpresaCliente EmpresaCliente { get; set; }

        /// <summary>
        /// Para regresar el catalogo de las Empresas del Cliente
        /// </summary>
        public bool GetEmpresasCliente { get; set; }

        /// <summary>
        /// Para consulta de una Empresa Cliente en particular
        /// </summary>
        public int EmpresaID { get; set; }


        /// <summary>
        /// Para obtener el listado de las empresas Contpaq 
        /// </summary>
        public bool GetEmpresasContpaq { get; set; }

        /// <summary>
        /// Para obtener el Catalogo contable usado por la empresa Contpaq (Requiere: BaseDatos, SoloCatalogo; Retorna tambien: CuentasNoEncontradas, Mascarilla)
        /// </summary>
        public bool GetCatalogoCuentasContpaq { get; set; } 

        /// <summary>
        /// Como parametro para la consulta del Catalogo contable
        /// </summary>
        public string BaseDatos { get; set; }
        /// <summary>
        /// Como parametro para la consulta del Catalogo contable
        /// </summary>
        public bool SoloCatalogo { get; set; }


        /// <summary>
        /// Para obtener un listado de las plantillas de polizas de ingresos donde es usada la cuenta.
        /// </summary>
        public string CuentaContableUsadaEn { get; set; }


        /// <summary>
        /// Indicador para crear el linkServer con contpaq (al activarlo se requeriran: ServidorContpaq, UsuarioContpaq, PasswordUsuarioContpaq)
        /// </summary>
        public bool CrearLinkContpaq { get; set; }

        /// <summary>
        /// Nombre del servidor (junto con la instancia); Como parametro para la creacion del LinkServer de contpaq
        /// </summary>
        public string ServidorContpaq { get; set; }
        /// <summary>
        /// Nombre del usuario de la Base de datos contpaq; Como parametro para la creacion del LinkServer de contpaq
        /// </summary>
        public string UsuarioContpaq { get; set; }
        /// <summary>
        /// Password del usuario de la base de datos contpaq; Como parametro para la creacion del LinkServer de contpaq
        /// </summary>
        public string PasswordUsuarioContpaq { get; set; }


    }
}
