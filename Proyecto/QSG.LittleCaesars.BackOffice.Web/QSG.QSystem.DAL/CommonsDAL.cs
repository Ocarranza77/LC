using QSG.QSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSG.QSystem.DAL
{
    public class CommonsDAL
    {
        public List<CboTipo> CboFill(DataTable dt, bool abr = true)
        {
            var lista = new List<CboTipo>();
            CboTipo ct;

            foreach (DataRow dr in dt.Rows)
            {
                ct = new CboTipo();

                ct.ID = dr["ID"].ToString();
                ct.Nombre = dr["Nombre"].ToString();
                if(abr)
                  //  ct.Abr = dr["Abr"].ToString();

                lista.Add(ct);
            }

            return lista;
        }

        public Domicilio DomicilioFill(DataRow row)
        {
            var dom = new Domicilio();
            //DateTime dt;

            dom.DomicilioID = Convert.ToInt32(row["DomicilioID"]);
            dom.Calle = row["Calle"].ToString();
            dom.Colonia = row["Colonia"].ToString();
            dom.CP = row["CP"].ToString();
            dom.Delegacion = row["Delegacion"].ToString();
            //DateTime.TryParse(row["FechaAlta"].ToString() , out dt);
            dom.FechaAlta = row.Field<DateTime>("FechaAlta");//dt;
            dom.Municipio = row["Municipio"].ToString();
            dom.NoExt = row["NoExt"].ToString();
            dom.NoInt = row["NoInt"].ToString();

            // Domicilio Tipo
            if (row["DomicilioTipoID"] != DBNull.Value)
            {
                dom.DomicilioTipo = new DomicilioTipo();
                dom.DomicilioTipo.DomicilioTipoID = Convert.ToInt32(row["DomicilioTipoID"]);
                dom.DomicilioTipo.Nombre = row["DomTipoNom"].ToString();
                dom.DomicilioTipo.Abr = row["DomTipoAbr"].ToString();
            }

            // Ciudad
            if (row["CiudadID"] != DBNull.Value)
            {
                dom.Ciudad = new Ciudad();
                dom.Ciudad.CiudadID = Convert.ToInt32(row["CiudadID"]);
                dom.Ciudad.Nombre = row["CiudadNom"].ToString();
                dom.Ciudad.Abr = row["CiudadAbr"].ToString();
            }

            // Estado
            if (row["EstadoID"] != DBNull.Value)
            {
                dom.Ciudad.Estado = new Estado();
                dom.Ciudad.Estado.EstadoID = Convert.ToInt32(row["EstadoID"]);
                dom.Ciudad.Estado.Nombre = row["EstadoNom"].ToString();
                dom.Ciudad.Estado.Abr = row["EstadoAbr"].ToString();
            }

            // Pais
            if (row["EstadoID"] != DBNull.Value)
            {
                dom.Ciudad.Estado.Pais = new Pais();
                dom.Ciudad.Estado.Pais.PaisID = Convert.ToInt32(row["PaisID"]);
                dom.Ciudad.Estado.Pais.Nombre = row["PaisNom"].ToString();
                dom.Ciudad.Estado.Pais.Abr = row["PaisAbr"].ToString();
            }

            return dom;

        }

        public Telefono TelefonoFill(DataRow row)
        {
            var tel = new Telefono();
            //DateTime dt;

            tel.TelefonoID = Convert.ToInt32(row["TelefonoID"]);
            tel.Lada = row["Lada"].ToString();
            tel.Numero = row["Numero"].ToString();
            tel.Notas = row["Notas"].ToString();
            //DateTime.TryParse(row["FechaAlta"].ToString(), out dt);
            tel.FechaAlta = row.Field<DateTime>("FechaAlta");//dt;

            // Telefono Tipo
            if (row["TelefonoTipoID"] != DBNull.Value)
            {
                tel.TelefonoTipo = new TelefonoTipo();
                tel.TelefonoTipo.TelefonoTipoID = Convert.ToInt32(row["TelefonoTipoID"]);
                tel.TelefonoTipo.Nombre = row["TipoNom"].ToString();
                tel.TelefonoTipo.Abr = row["TipoAbr"].ToString();
            }

            // Compañia Telefono
            if (row["CompaniaID"] != DBNull.Value)
            {
                tel.CompaniaTelefono = new TelefonoCompania();
                tel.CompaniaTelefono.TelefonoCompaniaID = Convert.ToInt32(row["CompaniaID"]);
                tel.CompaniaTelefono.Nombre = row["CompaniaNom"].ToString();
                tel.CompaniaTelefono.Abr = row["CompaniaAbr"].ToString();
            }
            return tel;
        }

        public void PersonaFill(DataRow row, Persona persona) //, ref Personas persona)
       // public Personas PersonaFill(DataRow row) //, ref Personas persona)
        {
            //DateTime dt;
             //Personas persona = new Personas();
            // Persona
            persona.PersonaID = row.Field<int>("PersonaID"); //Convert.ToInt32(row["PersonaID"]);
            persona.Nombre = row["Nombre"].ToString();
            persona.Paterno = row["Paterno"].ToString();
            persona.Materno = row["Materno"].ToString();
            persona.FechaNac = row.Field<DateTime?>("FechaNac");
            //if (row["FechaNac"] != DBNull.Value)
            //{
            //    DateTime.TryParse((string)row["FechaNac"].ToString(), out dt);
            //    persona.FechaNac = dt;
            //}
            persona.RFC = row["RFC"].ToString();
            persona.CURP = row["CURP"].ToString();
            persona.Email = row["Email"].ToString();
            persona.UsuarioID = row["UsuarioID"] != DBNull.Value ? Convert.ToInt32(row["UsuarioID"]) : 0;
            persona.RestringidoA = row["RestringidoA"].ToString();

            //      Persona Tipo
            if (row["PersonaTipoID"] != DBNull.Value)
            {
                persona.PersonaTipo = new PersonaTipo();
                persona.PersonaTipo.PersonaTipoID = Convert.ToInt32(row["PersonaTipoID"]);
                persona.PersonaTipo.Nombre = row["TipoPersonaNom"].ToString();
                persona.PersonaTipo.Abr = row["TipoPersonaAbr"].ToString();
            }

            //      Persona Sexo
            if (row["SexoID"] != DBNull.Value)
            {
                persona.Sexo = new PersonaSexo();
                persona.Sexo.SexoID = Convert.ToInt32(row["SexoID"]);
                persona.Sexo.Nombre = row["SexoNom"].ToString();
                persona.Sexo.Abr = row["SexoAbr"].ToString();
            }

            //      Nacionalidad
            if (row["NacionalidadID"] != DBNull.Value)
            {
                persona.Nacionalidad = new Pais();
                persona.Nacionalidad.PaisID = Convert.ToInt32(row["NacionalidadID"]);
                persona.Nacionalidad.Nombre = row["Nacionalidad"].ToString();
            }

            //      Ciudad Nacimiento
            if (row["CiudadNacID"] != DBNull.Value)
            {
                persona.CiudadNac = new Ciudad();
                persona.CiudadNac.CiudadID = Convert.ToInt32(row["CiudadNacID"]);
                persona.CiudadNac.Nombre = row["CiudadNom"].ToString();
                persona.CiudadNac.Abr = row["CiudadAbr"].ToString();
            }

            //      Estado Nacimiento
            if (row["EstadoNacID"] != DBNull.Value)
            {
                persona.EstadoNac = new Estado();
                persona.EstadoNac.EstadoID = Convert.ToInt32(row["EstadoNacID"]);
                persona.EstadoNac.Nombre = row["EstadoNom"].ToString();
                persona.EstadoNac.Abr = row["EstadoAbr"].ToString();
            }

            //      Nacionalidad
            if (row["PaisNacID"] != DBNull.Value)
            {
                persona.PaisNac = new Pais();
                persona.PaisNac.PaisID = Convert.ToInt32(row["PaisNacID"]);
                persona.PaisNac.Nombre = row["PaisNom"].ToString();
                persona.PaisNac.Nombre = row["PaisAbr"].ToString();
            }

            //return persona;

        }

        #region Obtener Dato
        //public T? GD<T>(DataRow row, string fiel, bool nulo)
        //{
        //    T t;
        //    if (row[fiel] == DBNull.Value)
        //        if (nulo)
        //            return null;

        //    t = (T)row[fiel];

        //    return t; 
        //}
        #endregion


    }
}
