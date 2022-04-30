using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace QSG.LittleCaesars.BackOffice.BL
{
    public class Global
    {
        public static int GetSafeID(object value)
        {
            try
            {
                if (value == null)
                {
                    return 0;
                }
                return Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en GetSafeID, Error: " + ex.Message);
            }
        }

        public static decimal GetSafeValue(object value)
        {
            string msj;
            try
            {
                if (value != null)
                {
                    try
                    {
                        return (decimal)value;
                    }
                    catch (Exception ex)
                    {
                        msj = ex.Message;
                        return Convert.ToDecimal(0);
                    }
                }
                else
                {
                    return Convert.ToDecimal(0);
                }
            }
            catch (Exception ex)
            {
                msj = ex.Message;
                return Convert.ToDecimal(0);
            }
        }

        //public static void Imprimir(System.Windows.Forms.Form Padre, string nomReport, Telerik.WinControls.UI.RadGridView grid)
        //{
        //    try
        //    {
        //        RadGridReportingLite.RadGridReport Reporte = new RadGridReportingLite.RadGridReport(nomReport);

        //        RadGridReportingLite.frmOpciones frm = new RadGridReportingLite.frmOpciones();
        //        if (frm.ShowDialog(Padre) == System.Windows.Forms.DialogResult.OK)
        //        {
        //            Reporte.FitToPageSize = frm.FitToPage;
        //            Reporte.LeftMargin = frm.MargenIzquierdo;
        //            Reporte.RightMargin = frm.MargenDerecho;
        //            Reporte.TopMargin = frm.MargenSuperior;
        //            Reporte.BottomMargin = frm.MargenInferior;
        //            Reporte.PaperKind = frm.PaperKind;
        //            Reporte.PageLandScape = frm.IsLandScape;
        //            Reporte.ReportFormShow(Padre, grid);
        //        }
        //        //Me.RadGridView1.TableElement.ResumeLayout(True, True)
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al generar reporte: " + ex.Message);
        //    }
        //}

        public  bool ComprobarCampo(object Campo)
        {
            bool bRes = true;
            if (Campo != null)
            {
                if (Convert.IsDBNull(Campo))
                {
                    bRes = false;
                }
                else
                {
                    if (Campo.ToString().Trim().Length == 0)
                    {
                        bRes = false;
                    }
                }
            }
            else
            {
                bRes = false;
            }
            return bRes;
        }

        public static byte[] Compress(string filename)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                ZipStorer zip;
                zip = ZipStorer.Create(ms, "UMBRALLSoft");
                zip.EncodeUTF8 = true;
                zip.AddFile(ZipStorer.Compression.Deflate, filename, Path.GetFileName(filename), "");
                zip.Close();
                byte[] data = (byte[])ms.ToArray();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al compactar el documento Connector" + Environment.NewLine + "Error: " + ex.Message);
            }
        }

        public static void DeCompress(byte[] bytInput, string _rutaXMLSALIDA)
        {
            try
            {
                bool result;
                Stream ms = new MemoryStream(bytInput);
                ZipStorer zip = ZipStorer.Open(ms, FileAccess.Read);
                List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
                foreach (ZipStorer.ZipFileEntry entry in dir)
                {
                    result = zip.ExtractFile(entry, _rutaXMLSALIDA);
                    break;
                }
                zip.Close();
                ms.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al descompactar el documento timbrado Connector" + Environment.NewLine + "Error: " + ex.Message);
            }
        }



    }
}
