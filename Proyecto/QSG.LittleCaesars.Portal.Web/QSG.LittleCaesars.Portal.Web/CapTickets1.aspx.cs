using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using System.Globalization;
using QSG.LittleCaesars.BackOffice.Common.Entities;
using QSG.LittleCaesars.BackOffice.Messages;
using QSG.LittleCaesars.BackOffice.Messages.Requests;
using QSG.LittleCaesars.BackOffice.DAL;
using QSG.LittleCaesars.BackOffice.Messages.Response;
using QSG.LittleCaesars.BackOffice.Common.Enums;
using System.Web.UI.HtmlControls;

namespace QSG.LittleCaesars.Portal.Web
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Today;
            Session["pantalla"] = 1;
            if (Session["User"] != null)
            {
                ruta_app.Value = Session["Empresa"].ToString() + " | " + "Captura de Tickets";
                NickName.InnerText = Session["Nombre"].ToString();
                fechaCapt.Value = fecha.Day.ToString() + "/" + fecha.Month.ToString() + "/" + fecha.Year.ToString();
                usuario.Value = Session["Nombre"].ToString();
                //content_repT.InnerHtml = GetTickets(fechaCapt.Value);

                if (!IsPostBack)
                {
                    //  ClFechaCap.SelectedDate = DateTime.Today;


                    //  BtnFechaCap.Text = DateTime.Now.ToShortDateString();

                }

            }
            else
            {
                Response.Redirect("~/Account/Login.aspx", true);
            }
        }

        protected void btnFechaCaP_click(object sender, EventArgs e)
        {
            if (ClFechaCap.Visible)
            {
                ClFechaCap.Visible = false;
            }
            else
            {
                ClFechaCap.Visible = true;

            }

        }
        protected void ClFechaCap_SelectionChanged(object sender, EventArgs e)
        {
            BtnFechaCap.Text = ClFechaCap.SelectedDate.ToShortDateString();
            ClFechaCap.Visible = false;
        }


        protected void GetTickets(object sender, EventArgs e)
        {

            content_repT.InnerHtml = GetTickets(ClFechaCap.SelectedDate);
        }
        public static string GetTickets(DateTime fecha)
        {

            string html = "";

            html += " <ul runat='server' id='container_history' ><li runat='server' id='R0'>";
            html += "<ul runat='server' id='row'>";
            html += "<li class='column_ID'>No.</li>";
            html += "<li class='column_Edit'><img class='btnEdit' src='iconos/ic_edit1.png' title='Editar registro'/></li>";
            html += "<li class='column_Del'><img class='btnDel' src='iconos/ic_delete.png' title='Eliminar registro'/></li>";
            html += "<li class='column_Sucursal'>Sucursal</li>";
            html += "<li class='column_FechaT'>Fecha Ticket</li>";
            html += "<li class='column_HoraT' >Hora Ticket</li>";
            html += "<li class='column_FolioT'>Folio Ticket</li>";
            html += "<li class='column_Caja'># Caja</li>";
            html += "<li class='column_Cajero'>Nombre de Cajero</li>";
            html += "<li class='column_Importe'>Importe</li>";
            html += "<li class='column_UserCap'>Usuario Capturo</li>";
            html += "<li class='column_stt'></li>";
            html += "</ul> ";
            html += "</li>";



            var su = new Sucursal();
            var sv1 = new ServiceImplementation();
            var sr = new SucursalRequest();
            su.SucursalID = 0;
            sr.Sucursal = su;
            sr.MessageOperationType = BackOffice.Common.Enums.MessageOperationType.Report;
            var response = sv1.SucursalMessage(sr);





            var tck = new TicketFilter();



            tck.FechaVta = Convert.ToDateTime(fecha); // new DateTime(2014, 10, 24);



            var sv = new ServiceImplementation();

            TicketResponse tr = sv.TicketMessage(new TicketRequest() { Filters = tck, MessageOperationType = MessageOperationType.Report });



            var selec = "";
            var count = 0;

            foreach (Ticket ticket in tr.Tickets)
            {
                count++;
                html += "<li runat='server' class='row'>";
                html += "    <ul runat='server' >";
                html += "    <li runat='server' class='row_ID'><input  type='hidden' value='STT' runat='server' /></li>";
                html += "    <li runat='server' class='column_ID'>" + count + "</li>";
                html += "    <li runat='server' class='column_Edit'><img src='iconos/checkbox_unchecked (1).png' onclick='Edit_Click(event);' /></li>";
                html += "    <li runat='server' class='column_Del'><img src='iconos/checkbox_unchecked (1).png' onclick='Del_Click(event);'/></li>";
                html += "    <li runat='server' class='column_Sucursal' ><select runat='server' class='inactive' id='select_sucursal' disabled> ";

                foreach (Sucursal s in response.Sucursales)
                {
                    if (s.SucursalID == ticket.Sucursal.SucursalID)
                    {
                        selec = "selected";
                    }
                    html += "<option " + selec + ">" + s.Abr + "</option>";
                    selec = "";
                }

                html += " </select></li>";

                html += "<li runat='server' class='column_FechaT'><input runat='server' class='inactive' value='" + ticket.FechaVta.ToString("dd/MM/yyyy") + "' placeholder='Dia/Mes/Año'  onkeyup='ValDate(event);' onkeypress='return justNumbers(event);' maxlength='10' readonly/></li>";
                html += "<li runat='server' class='column_HoraT'><input runat='server' class='inactive' value='" + ticket.HoraVta + "'  placeholder='HH:MM' onkeyup='valida(event);' onkeypress='return justNumbers(event);' maxlength='5' readonly/> </li>";
                html += "<li runat='server' class='column_FolioT'><input runat='server' class='inactive' value='" + ticket.TicketID + "' readonly/></li>";
                html += "<li runat='server' class='column_Caja'><input runat='server' class='inactive' value='" + ticket.CajaID + "' readonly/>    </li>";
                html += "<li runat='server' class='column_Cajero'><input runat='server' class='inactive' value='" + ticket.Cajero + "' readonly/>   </li>";
                html += "<li runat='server' class='column_Importe'><input runat='server' class='inactive' value='" + ticket.Importe + "'  readonly/></li>";
                html += "<li runat='server' class='column_UserCap'><input runat='server' class='inactive' readonly/></li>";
                html += "<li runat='server' class='column_stt' ><input type='hidden' value='STT' /></li>";
                html += "</ul> ";
                html += "</li>";
            }

            return html + "</ul>";
        }
        protected void SaveTickets(object sender, EventArgs e)
        {
            var count = 0;
            var id = "";

            var cadena = "";
            var lstArray = new List<string>();
            var lstTicket = new List<Ticket>();
            var tck = new Ticket();


            foreach (Control crt in container_history.Controls)
            {
                if (crt is HtmlGenericControl)
                {

                    foreach (Control crtn in crt.Controls)
                    {
                        if (crtn is HtmlGenericControl)
                        {
                            var ul = (HtmlGenericControl)crtn;

                            if (ul.TagName == "ul")
                            {
                                //recorre ul
                                cadena = "";
                                foreach (Control crt3 in ul.Controls)
                                {
                                    if (crt3 is HtmlGenericControl)
                                    {
                                        mostTest.InnerText += crt3.ID;
                                        //recorre  li
                                        foreach (Control crt4 in crt3.Controls)
                                        {

                                            if (crt4 is HtmlInputText)
                                            {
                                                var crtp = (HtmlInputText)crt4;
                                                if (cadena != "") { cadena += ","; }
                                                cadena += crtp.Value;
                                            }

                                            if (crt4 is HtmlSelect)
                                            {
                                                var select = (HtmlSelect)crt4;
                                                if (cadena != "") { cadena += ","; }
                                                cadena += select.Value;

                                            }
                                        }



                                    }

                                }//recorre ul

                                if (cadena != "") { lstArray.Add(cadena); }

                            }



                        }


                    }//recorre la primera li
                }
            }
            /*
            foreach(string dat in lstArray){
                string[] datos = dat.Split(new char[] { ',' });
                
                if (datos[0] == "STT0")//si el registro es nuevo
                {
                    tck = new Ticket();
                    tck.Sucursal = new Sucursal() { SucursalID =Convert.ToInt32( datos[1]) };
                    tck.FechaVta = Convert.ToDateTime(datos[2]);
                    tck.HoraVta = datos[3];
                    tck.TicketID = Convert.ToInt32(datos[4]);
                    tck.CajaID = Convert.ToInt32(datos[5]);
                    tck.Cajero = datos[6];
                    tck.Importe = Convert.ToDouble(datos[7]);
                    tck.CodUsuario = Convert.ToInt32(datos[8]);
                    tck.FechaCaptura = DateTime.Now;
                
                   //tck.OperationType = BackOffice.Common.Enums.OperationType.New;
                   lstTicket.Add(tck);
                }

                if (datos[0] == "STT1")//si el registro es edit
                {
                    tck = new Ticket();
                    tck.Sucursal = new Sucursal() { SucursalID = Convert.ToInt32(datos[1]) };
                    tck.FechaVta = Convert.ToDateTime(datos[2]);
                    tck.HoraVta = datos[3];
                    tck.TicketID = Convert.ToInt32(datos[4]);
                    tck.CajaID = Convert.ToInt32(datos[5]);
                    tck.Cajero = datos[6];
                    tck.Importe = Convert.ToDouble(datos[7]);
                    tck.CodUsuario = Convert.ToInt32(datos[8]);
                    tck.FechaCaptura = DateTime.Now;

                    //tck.OperationType = BackOffice.Common.Enums.OperationType.New;
                    lstTicket.Add(tck);
                }
                if (datos[0] == "STT2")//si el registro es nuevo
                {
                    tck = new Ticket();
                    tck.Sucursal = new Sucursal() { SucursalID = Convert.ToInt32(datos[1]) };
                    tck.FechaVta = Convert.ToDateTime(datos[2]);
                    tck.HoraVta = datos[3];
                    tck.TicketID = Convert.ToInt32(datos[4]);
                    tck.CajaID = Convert.ToInt32(datos[5]);
                    tck.Cajero = datos[6];
                    tck.Importe = Convert.ToDouble(datos[7]);
                    tck.CodUsuario = Convert.ToInt32(datos[8]);
                    tck.FechaCaptura = DateTime.Now;

                    //tck.OperationType = BackOffice.Common.Enums.OperationType.New;
                    lstTicket.Add(tck);
                }
                
            }*/


        }
        protected void addTicket(object sender, EventArgs e)
        {
            var html = "";
            html += "<li class='row' runat='server' >";
            html += "    <ul runat='server' >";
            html += "    <li runat='server' class='row_ID'><input runat='server' value='STT0'/></li>";
            html += "    <li runat='server' class='column_ID'></li>";
            html += "    <li runat='server' class='column_Edit'><img src='iconos/checkbox_unchecked (1).png' onclick='Edit_Click(event);' /></li>";
            html += "    <li runat='server' class='column_Del'><img src='iconos/checkbox_unchecked (1).png' onclick='Del_Click(event);' /></li>";
            html += "    <li runat='server' class='column_Sucursal' ><select runat='server'  > ";
            html += "<option>1</option>";

            html += " </select></li>";

            html += "<li runat='server' class='column_FechaT'><input runat='server' placeholder='Dia/Mes/Año'  onkeyup='ValDate(event);' onkeypress='return justNumbers(event);' maxlength='10'  /></li>";
            html += "<li runat='server' class='column_HoraT'><input runat='server' placeholder='HH:MM' onkeyup='valida(event);' onkeypress='return justNumbers(event);' maxlength='5' /> </li>";
            html += "<li runat='server' class='column_FolioT'><input runat='server' /></li>";
            html += "<li runat='server' class='column_Caja'><input runat='server'   />    </li>";
            html += "<li runat='server' class='column_Cajero'><input runat='server'    />   </li>";
            html += "<li runat='server' class='column_Importe'><input runat='server'  /></li>";
            html += "<li runat='server' class='column_UserCap'><input runat='server'   /></li>";
            html += "<li runat='server' class='column_stt'><input  value='STT0' /></li>";
            html += "</ul> ";
            html += "</li>";


            HtmlGenericControl li_1 = new HtmlGenericControl("li");
            li_1.Controls.Add(new HtmlInputText() { Value = "STT0" });
            HtmlGenericControl li_2 = new HtmlGenericControl("li");

            HtmlGenericControl li_3 = new HtmlGenericControl("li");

            HtmlGenericControl li_4 = new HtmlGenericControl("li");

            HtmlGenericControl li_5 = new HtmlGenericControl("li");
            li_5.Controls.Add(new HtmlSelect());
            HtmlGenericControl li_6 = new HtmlGenericControl("li");
            li_6.Controls.Add(new HtmlInputText() { ID = "new" });
            HtmlGenericControl li_7 = new HtmlGenericControl("li");
            li_7.Controls.Add(new HtmlInputText());
            HtmlGenericControl li_8 = new HtmlGenericControl("li");
            li_8.Controls.Add(new HtmlInputText());
            HtmlGenericControl li_9 = new HtmlGenericControl("li");
            li_9.Controls.Add(new HtmlInputText());
            HtmlGenericControl li_10 = new HtmlGenericControl("li");
            li_10.Controls.Add(new HtmlInputText());
            HtmlGenericControl li_11 = new HtmlGenericControl("li");
            li_11.Controls.Add(new HtmlInputText());
            HtmlGenericControl li_12 = new HtmlGenericControl("li");
            li_12.Controls.Add(new HtmlInputText());
            HtmlGenericControl li_13 = new HtmlGenericControl("li");
            li_13.Controls.Add(new HtmlInputText());


            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.TagName = "ul";

            ul.Controls.Add(li_1);
            ul.Controls.Add(li_2);
            ul.Controls.Add(li_3);
            ul.Controls.Add(li_4);
            ul.Controls.Add(li_5);
            ul.Controls.Add(li_6);
            ul.Controls.Add(li_7);
            ul.Controls.Add(li_8);
            ul.Controls.Add(li_9);
            ul.Controls.Add(li_10);
            ul.Controls.Add(li_11);
            ul.Controls.Add(li_12);
            ul.Controls.Add(li_13);


            HtmlGenericControl li1 = new HtmlGenericControl("li");
            li1.TagName = "li";
            li1.Attributes["id"] = "newid";
            li1.Controls.Add(ul);


            container_history.Controls.Add(new LiteralControl(html));


        }


    }
}