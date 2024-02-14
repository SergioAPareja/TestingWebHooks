using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net;
using System.Web.UI;
using Winnovative.WnvHtmlConvert;
using Winnovative;
using System.Net.Mail;
using System.Data;
using ClosedXML.Excel;
using System.Data.OracleClient;
using OpenXmlPowerTools;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Drawing;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using GemBox.Document;

//En esta clase se encuentran las conexiones a la base de datos y las hojas de estilo de cada institución.
public class MainClass
{
    public OracleConnection conn = new OracleConnection();

    string HostName = HttpContext.Current.Request.Url.Host;

    //Nombre de la aplicación:
    public string APP_NAME = "DirectorioUAGM";

    //Elegir si la base de datos es Prueba desarrollo 'DES', prueba de Usuario 'TEST' y Producción 'PROD'. 
    //Si colocas AUTO la aplicación cambiara la conexión de acuerdo al servidor.
    string TypeConn = "PROD";
    //Códigos de la aplicación en OCIT Security:
    string APP_CODE_PROD = "218";
    string APP_CODE_TEST = "217";

    //Obtener código de aplicación en OCIT Security:
    public string getAppCode()
    {
        string APP_CODE = "";

        if (TypeConn == "PROD" || (HostName == "ociteapps.uagm.edu" && TypeConn == "AUTO") || (HostName == "ocitlocalapps.uagm.edu" && TypeConn == "AUTO"))
        {
            APP_CODE = APP_CODE_PROD;
        }
        else if (TypeConn == "TEST" || (HostName == "ocit-test.uagm.edu" && TypeConn == "AUTO"))
        {
            APP_CODE = APP_CODE_TEST;
        }
        else
        {
            APP_CODE = APP_CODE_TEST;
        }

        return APP_CODE;

    }

    //Base de datos estudiantiles
    public string getConnection(string inst)
    {
        string connString = "";
        if (TypeConn == "PROD" || (HostName == "ociteapps.uagm.edu" && TypeConn == "AUTO") || (HostName == "ocitlocalapps.uagm.edu" && TypeConn == "AUTO"))
        {
            //Case para definir y conexión a la base de dato de la insitución apropiada.
            switch (inst)
            {
                //Institución = UNE
                case "2":
                    connString = "DATA SOURCE=UTTEST.WORLD; PASSWORD=devprog123*; USER ID=OCIT_PROG_UMET";
                    break;

                //Institución = UMET
                case "4":
                    connString = "DATA SOURCE=UTTEST.WORLD; PASSWORD=time121517*; USER ID=webprog_umet";
                    break;

                //Institución = TURABO
                case "6":
                    connString = "DATA SOURCE=UTTEST.WORLD.WORLD; PASSWORD=time121517*; USER ID=webprog_ut";
                    break;

                //Institución = VIRTUAL
                case "8":
                    connString = "DATA SOURCE=UTTEST.WORLD; PASSWORD=time021218*; USER ID=webprog_uagm";
                    break;

            }
        }
        else if (TypeConn == "TEST" || (HostName == "ocit-test.uagm.edu" && TypeConn == "AUTO"))
        {
            //Case para definir conexión a la base de dato de la institución apropiada.
            switch (inst)
            {
                //Institución = UNE
                case "2":

                    connString = "DATA SOURCE=UMTEST.world; PASSWORD=time121517*; PERSIST SECURITY INFO=True; USER ID=webprog_une";
                    break;

                //Institución = UMET
                case "4":
                    connString = "DATA SOURCE=UMTEST.world; PASSWORD=time121517*; PERSIST SECURITY INFO=True; USER ID=WEBPROG_UMET";
                    break;

                //Institución = TURABO
                case "6":
                    connString = "DATA SOURCE=UMTEST.world; PASSWORD=time121517*; PERSIST SECURITY INFO=True; USER ID=WEBPROG_UT";
                    break;

                //Institución = VIRTUAL
                case "8":
                    connString = "DATA SOURCE=UMTEST.world; PASSWORD=time021218*; PERSIST SECURITY INFO=True; USER ID=WEBPROG_UAGM";
                    break;
            }
        }
        else
        {
            //Institución = STUDES
            switch (inst)
            {
                //Institución = UNE
                case "2":
                    connString = "DATA SOURCE=UTTEST.WORLD; PASSWORD=time121517*; USER ID=webprog_une";
                    break;

                //Institución = UMET
                case "4":
                    connString = "DATA SOURCE=UTTEST.WORLD; PASSWORD=time121517*; USER ID=webprog_umet";
                    break;

                //Institución = TURABO
                case "6":
                    connString = "DATA SOURCE=UTTEST.WORLD.WORLD; PASSWORD=time121517*; USER ID=webprog_ut";
                    break;

                //Institución = VIRTUAL
                case "8":
                    connString = "DATA SOURCE=UTTEST.WORLD; PASSWORD=time021218*; USER ID=webprog_uagm";
                    break;
            }
        }
        HttpContext.Current.Session["Conexion"] = connString;
        return connString;
    }

    //Hoja de estilos
    public string GetStyle(string inst)
    {
        string StyleSheet = "";
        StyleSheet = "<link href='https://ociteapps.uagm.edu/OCITStyle/css/CENTRAL.css' rel='stylesheet'/>";
        StyleSheet += "<link href='https://ociteapps.uagm.edu/OCITStyle/css/Structure.css' rel='stylesheet'/>";
        return StyleSheet;
    }

    public DataTable loadDT(string query)
    {
        try
        {
            conn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
            OracleCommand cmd = new OracleCommand(query);
            cmd.Connection = conn;
            conn.Open();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            return null;
        }
        finally
        {
            conn.Close();
        }
    }
    //Este es el que se esta utilizando.
    public string retunrHTML(string nombre, System.Web.UI.WebControls.FileUpload fUploadDocx)
    {
        try
        {
            if (System.IO.Path.GetExtension(fUploadDocx.FileName).ToUpper() != ".DOCX")
                return "Error: El formato del documento debe ser: docx.";
            else
            {
                string filepath = HttpContext.Current.Server.MapPath("Uploads/" + fUploadDocx.FileName);
                fUploadDocx.SaveAs(filepath);
                //Convertir DOCX a HTML
                ComponentInfo.SetLicense("DN-2020Feb10-2020Mar10-phmjgUFMDcpONnB02FCIteIxjkvSNE8c3uwofeZRtfLSlG/FAgzC9kssHdryDRZ33DUH9VD8dzhi17QY5FiJ7n8yV/Q==B");
                var document = DocumentModel.Load(filepath);
                var saveOptions = new HtmlSaveOptions()
                {
                    HtmlType = HtmlType.Html,
                    EmbedImages = true,
                    UseSemanticElements = true
                };

                //Save DocumentModel object to HTML (or MHTML) file.
                document.Save(HttpContext.Current.Server.MapPath("Uploads/" + nombre + ".html"), new HtmlSaveOptions() { EmbedImages = true });

                //Leemos el html convertido de word
                System.Threading.Thread.Sleep(1000);
                string path = HttpContext.Current.Server.MapPath("Uploads/" + nombre + ".html");
                StreamReader reader = new StreamReader(path);
                string template = reader.ReadToEnd();
                reader.Close();
                return template;
            }
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            return "";
        }
    }
    public bool ConvertHtmlToWord(string name, string text)
    {
        string filePath = HttpContext.Current.Server.MapPath("Doc/Doc1.docx");
        try
        {
            //Creamos el Word
            File.Copy(HttpContext.Current.Server.MapPath("~/Doc/wordTemplate.docx"), filePath);

            //Insertamos el HTML en el Word.
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                int altChunkIdCounter = 1;
                int blockLevelCounter = 1;
                string altChunkId = String.Format("AltChunkId{0}", altChunkIdCounter++);

                //Import data as html content using Altchunk
                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);

                using (Stream chunkStream = chunk.GetStream(FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter stringWriter = new StreamWriter(chunkStream, System.Text.Encoding.UTF8)) //Encoding.UTF8 is important to remove special characters
                    {
                        if (text.Contains("html"))
                            stringWriter.Write(text);
                        else
                            stringWriter.Write("<html><head><style type='text/css'></style></head><body style='font-family:Trebuchet MS;font-size:.9em;'>" + text + "</body></html>");
                    }
                }

                AltChunk altChunk = new AltChunk();
                altChunk.Id = altChunkId;

                mainPart.Document.Body.InsertAt(altChunk, blockLevelCounter++);
                mainPart.Document.Save();
            }

            //Descargarmos el archivo
            DownloadFile(filePath, name + ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            return true;
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            return false;
        }
        finally
        {
            //Lo eliminamos
            File.Delete(filePath);
        }
    }

    public void DownloadFile(string completeFilePath, string fileName, string contentType)
    {
        Stream iStream = null;
        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];
        // Length of the file:
        int length;
        // Total bytes to read:
        long dataToRead;

        try
        {
            // Open the file.
            iStream = new FileStream(completeFilePath, FileMode.Open,
            FileAccess.Read, FileShare.Read);
            // Total bytes to read:
            dataToRead = iStream.Length;
            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            // Read the bytes.
            while (dataToRead > 0)
            {
                // Verify that the client is connected.
                if (HttpContext.Current.Response.IsClientConnected)
                {
                    // Read the data in buffer.
                    length = iStream.Read(buffer, 0, 10000);
                    // Write the data to the current output stream.
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                    // Flush the data to the HTML output.
                    HttpContext.Current.Response.Flush();
                    buffer = new Byte[10000];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    //prevent infinite loop if user disconnects
                    dataToRead = -1;
                }
            }
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
            HttpContext.Current.Response.Close();
        }
    }
    public string returnWordToHTML(System.Web.UI.WebControls.FileUpload fUpload)
    {
        try
        {
            string fileExt = System.IO.Path.GetExtension(fUpload.FileName).ToUpper();
            if (!(fUpload.HasFile))
            {
                return "Debe seleccionar el archivo.";
            }
            else if (fileExt.ToUpper() != ".DOCX")
                return "Error: La extensión del documento debe ser docx.";
            else
            {
                string filepath = HttpContext.Current.Server.MapPath("Uploads/" + fUpload.FileName);
                fUpload.SaveAs(filepath);

                byte[] byteArray = File.ReadAllBytes(filepath);
                var source = System.IO.Packaging.Package.Open(filepath);
                var document = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(source);
                HtmlConverterSettings settings = new HtmlConverterSettings();
                XElement html = HtmlConverter.ConvertToHtml(document, settings);

                var writer = File.CreateText(HttpContext.Current.Server.MapPath("Uploads/test.html"));
                writer.WriteLine(html.ToString());
                writer.Dispose();
                writer.Close();

                //Leemos el html convertido de word
                System.Threading.Thread.Sleep(1000);
                string path = HttpContext.Current.Server.MapPath("Uploads/test.html");
                StreamReader reader = new StreamReader(path);
                string template = reader.ReadToEnd();
                reader.Close();               

                //System.Threading.Thread.Sleep(1000);
                //File.Delete(filepath);//Eliminamos el word
                //File.Delete(path);//Eliminamos el html                

                return template;
            }
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
        }
    }
    public string retornarValor(string query)
    {
        string valor = "";
        try
        {
            conn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
            OracleCommand cmd = new OracleCommand(query);
            conn.Open();
            cmd.Connection = conn;
            OracleDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            if (rdr.HasRows == true)
            {
                valor = rdr[0].ToString();
            }
            else
                valor = "";
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            valor = "";
        }
        finally
        {
            conn.Close();
        }
        return valor;
    }

    public bool oficialmenteMatriculado(string termino, string PIDM)
    {
        bool valor = false;
        try
        {
            conn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
            OracleCommand cmd = new OracleCommand(@"SELECT 'X'
                                                        FROM SFBETRM
                                                       WHERE SFBETRM_PIDM = '" + PIDM + @"'
                                                         AND (SUBSTR(SFBETRM_TERM_CODE, 1,4) = SUBSTR('" + termino + @"', 1, 4)  -- 202101
                                                         AND SUBSTR(SFBETRM_TERM_CODE, -1) = SUBSTR('" + termino + @"', -1))
                                                         AND SFBETRM_AR_IND = 'Y'
                                                         AND SFBETRM_ESTS_CODE IN ('EL', 'RS', 'RA')
                                                         AND EXISTS (SELECT 'X'
                                                                       FROM SFRSTCR
                                                                      WHERE SFRSTCR_TERM_CODE = SFBETRM_TERM_CODE
                                                                        AND SFRSTCR_PIDM = SFBETRM_PIDM
                                                                        AND SFRSTCR_RSTS_CODE = (SELECT STVRSTS_CODE
                                                                                                   FROM STVRSTS
                                                                                                  WHERE STVRSTS_CODE = SFRSTCR_RSTS_CODE
                                                                                                    AND STVRSTS_INCL_SECT_ENRL = 'Y'))");
            conn.Open();
            cmd.Connection = conn;
            OracleDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            if (rdr.HasRows == true)
            {
                valor = true;
            }
            else
                valor = false;
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            valor = false;
        }
        finally
        {
            conn.Close();
        }
        return valor;
    }

    public bool retornarValorBool(string query)
    {
        bool valor = false;
        try
        {
            conn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
            OracleCommand cmd = new OracleCommand(query);
            conn.Open();
            cmd.Connection = conn;
            OracleDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            if (rdr.HasRows == true)
            {
                valor = true;
            }
            else
                valor = false;
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            valor = false;
        }
        finally
        {
            conn.Close();
        }
        return valor;
    }
    public DataTable retornarDT(string query)
    {
        DataTable dt = new DataTable();
        try
        {
            conn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
            OracleCommand cmd = new OracleCommand(query);
            conn.Open();
            cmd.Connection = conn;
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }
        finally
        {
            conn.Close();
        }
        return dt;
    }
    public bool ejecutarScript(string query)
    {
        bool valor = false;
        try
        {
            conn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
            OracleCommand cmd = new OracleCommand(query);
            cmd.Connection = conn;
            conn.Open();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            conn.Close();
            valor = true;
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            valor = false;
        }
        finally
        {
            conn.Close();
        }
        return valor;
    }
     public bool sendEmail(string emailTo, string path, string usuario, string nombreArchivo, string mensaje, string localidad)
    {
        //FUNCION PARA ENVIAR EMAILS CON ARCHIVO
        try
        {
            string filename = HttpContext.Current.Server.MapPath(path);
            System.Net.Mail.Attachment data = new Attachment(filename, System.Net.Mime.MediaTypeNames.Application.Octet);
            MailMessage mail = new MailMessage();
            StringBuilder EMailBody = new System.Text.StringBuilder();
            mail.To.Add(emailTo);
            mail.From = new MailAddress("noreplyprog@uagm.edu");
            mail.Subject = "Acuse de Recibo";
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            EMailBody.Append(mensaje + "<br/><br/><br/>");
            EMailBody.Append(@"Favor de no contestar este email.");
            AlternateView HTMLEmail = AlternateView.CreateAlternateViewFromString(EMailBody.ToString(), null, "text/html");
            mail.AlternateViews.Add(HTMLEmail);
            SmtpClient client = new SmtpClient("webmail.uagm.edu");
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("noreplyprog@uagm.edu", "desembolsos", "desembolsos331");
            mail.Attachments.Add(data);

            //Instrucciones
            string filenameInstrucciones = "";
            if (localidad.Contains("DA") || localidad.Contains("DC") || localidad.Contains("MD") || localidad.Contains("MO") || localidad.Contains("SF") || localidad.Contains("TB"))//EU
                filenameInstrucciones = HttpContext.Current.Server.MapPath("Guia Rapida Uso por Primera Vez Laptop Estudiantes US.pdf");
            else
                filenameInstrucciones = HttpContext.Current.Server.MapPath("Guia Rapida Uso por Primera Vez Laptop Estudiantes.pdf");

            System.Net.Mail.Attachment data2 = new Attachment(filenameInstrucciones, System.Net.Mime.MediaTypeNames.Application.Octet);
            mail.Attachments.Add(data2);

            client.Send(mail);
            mail.Dispose();
            return true;
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            return false;
        }
    }
    
    public bool sendMessage(string emailTo, string mensaje, string subject)
    {
        //FUNCION PARA ENVIAR EMAILS CON ARCHIVO
        try
        {
            MailMessage mail = new MailMessage();
            StringBuilder EMailBody = new System.Text.StringBuilder();
            mail.To.Add(emailTo);
            mail.From = new MailAddress("OrdenesEjecutivas@uagm.edu");
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            string path = HttpContext.Current.Server.MapPath("assets/emailTemplate/index.html");
            StreamReader reader = new StreamReader(path);
            string template = reader.ReadToEnd();
            reader.Close();

            template = template.Replace("[#body]", mensaje + "<br/><br/><br/>Favor de no contestar este email.");
            template = template.Replace("[#BANNER]","https://ociteapps.suagm.edu//ordenesEjecutivas/assets/emailTemplate/banner.jpg");
            EMailBody.Append(template);
            AlternateView HTMLEmail = AlternateView.CreateAlternateViewFromString(EMailBody.ToString(), null, "text/html");
            mail.AlternateViews.Add(HTMLEmail);
            SmtpClient client = new SmtpClient("webmail.uagm.edu");
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("noreplyprog@uagm.edu", "desembolsos", "desembolsos331");
            client.Send(mail);
            mail.Dispose();
            return true;
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            return false;
        }
    }

    public bool convertirPDF(string dowloadOrSendPDF, string url, string nombre, string carpeta)
    {
        try
        {
            //Generar PDF:
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
            htmlToPdfConverter.LicenseKey = "2lRHVURVQkFBVUREW0VVRkRbREdbTExMTA==";
            htmlToPdfConverter.MediaType = "print";
            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            //Enable header and footer in the generated PDF document
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = false;
            htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 0;
            htmlToPdfConverter.PdfDocumentOptions.RightMargin = 0;
            htmlToPdfConverter.PdfDocumentOptions.TopMargin = 0;
            htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 0;

            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 0;

            htmlToPdfConverter.PdfDocumentOptions.AvoidImageBreak = true;

            //htmlToPdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.WhiteSmoke;
            //Numero de páginas
            TextElement footerPage = new TextElement(55, 40, "Page &p; of &P; ", new System.Drawing.Font(new System.Drawing.FontFamily("Arial"), 12, System.Drawing.GraphicsUnit.Point));
            footerPage.TextAlign = Winnovative.HorizontalTextAlign.Left;
            footerPage.ForeColor = System.Drawing.Color.Black;
            footerPage.EmbedSysFont = true;
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerPage);

            if (dowloadOrSendPDF.ToUpper() == "D")
            {
                //Descargar el PDF.
                string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                byte[] outPdfBuffer = htmlToPdfConverter.ConvertUrl(url);
                HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");
                HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=" + nombre + ".pdf; size={0}", outPdfBuffer.Length.ToString()));
                HttpContext.Current.Response.BinaryWrite(outPdfBuffer);
                HttpContext.Current.Response.End();
            }
            else
            {
                //htmlToPdfConverter.ConvertHtmlFileToFile(url, HttpContext.Current.Server.MapPath("preview/") + nombre + ".pdf");
                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.AddHeader("Content-Type", "binary/octet-stream");
                //HttpContext.Current.Response.End();

                //No descargamos el PDf por que se enviara por email.
                byte[] outPdfBuffer = htmlToPdfConverter.ConvertUrl(url);
                //Descargamos el pdf en la carpeta
                System.IO.FileStream oFileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath(carpeta + "/") + nombre + ".pdf", System.IO.FileMode.Create);
                oFileStream.Write(outPdfBuffer, 0, outPdfBuffer.Length);
                oFileStream.Flush();
                oFileStream.Close();
                oFileStream.Dispose();
            }
            return true;
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            return false;
        }
    }
    public bool exportaExcel(DataTable tabla, string nombreArchivo)
    {
        try
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(tabla, nombreArchivo);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xlsx");
                using (System.IO.MemoryStream MyMemoryStream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            return false;
        }
    }

    public void SendException(string Error, string NombreEventoFuncion, string NombreUsuario, string URL)
    {
        //Parametros a llenar.
        string Programador = "WEBPROG";
        string NombreAplicacion = "Aplicación";
        string EmailTo = "programacion@uagm.edu";
        string EmailFrom = "programacion@uagm.edu";

        try
        {
            Error = Error.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            MailMessage mail = new MailMessage();
            StringBuilder EMailBody = new System.Text.StringBuilder();
            mail.To.Add(EmailTo);
            mail.From = new MailAddress(EmailFrom);
            mail.Subject = "Error en la aplicación " + NombreAplicacion + " con el usuario " + NombreUsuario;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            EMailBody.Append(@"<html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:w=""urn:schemas-microsoft-com:office:word"" xmlns:m=""http://schemas.microsoft.com/office/2004/12/omml"" xmlns=""http://www.w3.org/TR/REC-html40""><head><meta http-equiv=Content-Type content=""text/html; charset=iso-8859-1""><meta name=Generator content=""Microsoft Word 15 (filtered medium)""><!--[if !mso]><style>v\:* {behavior:url(#default#VML);}o\:* {behavior:url(#default#VML);}w\:* {behavior:url(#default#VML);}.shape {behavior:url(#default#VML);}</style><![endif]--><style><!--/* Font Definitions */@font-face	{font-family:""Cambria Math"";panose-1:2 4 5 3 5 4 6 3 2 4;}@font-face{font-family:Calibri;	panose-1:2 15 5 2 2 2 4 3 2 4;}/* Style Definitions */p.MsoNormal, li.MsoNormal, div.MsoNormal{margin:0in;	margin-bottom:.0001pt;font-size:11.0pt;font-family:""Calibri"",sans-serif;}a:link, span.MsoHyperlink{mso-style-priority:99;color:#0563C1;text-decoration:underline;}a:visited, span.MsoHyperlinkFollowed{mso-style-priority:99;	color:#954F72;	text-decoration:underline;}p	{mso-style-priority:99;	mso-margin-top-alt:auto;	margin-right:0in;	mso-margin-bottom-alt:auto;	margin-left:0in;	font-size:12.0pt;	font-family:""Times New Roman"",serif;}p.MsoNoSpacing, li.MsoNoSpacing, div.MsoNoSpacing	{mso-style-priority:1;margin:0in;margin-bottom:.0001pt;text-align:justify;font-size:11.0pt;font-family:""Calibri"",sans-serif;}span.EmailStyle17	{mso-style-type:personal-compose;	font-family:""Calibri"",sans-serif;	color:windowtext;}.MsoChpDefault{mso-style-type:export-only;font-family:""Calibri"",sans-serif;}@page WordSection1	{size:8.5in 11.0in;	margin:1.0in 1.0in 1.0in 1.0in;}div.WordSection1{page:WordSection1;}--></style><!--[if gte mso 9]><xml><o:shapedefaults v:ext=""edit"" spidmax=""1026"" /></xml><![endif]--><!--[if gte mso 9]><xml><o:shapelayout v:ext=""edit""><o:idmap v:ext=""edit"" data=""1"" /></o:shapelayout></xml><![endif]--></head>");
            EMailBody.Append(@"<body bgcolor=""#eeeeee"" lang=EN-US link=""#0563C1"" vlink=""#954F72""><div class=WordSection1><p class=MsoNormal><o:p>&nbsp;</o:p></p><div align=center><table class=MsoNormalTable border=0 cellspacing=15 cellpadding=0 width=804 style='width:603.0pt;background:#333333'><tr><td width=780 style='width:585.0pt;padding:0in 0in 0in 0in'><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=""100%"" style='width:100%;background:white'><tr style='height:121.85pt'><td style='padding:0in 0in 0in 0in;height:0pt'></td></tr><tr style='height:206.1pt'><td style='padding:0in 0in 0in 0in;height:206.1pt'><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=""97%"" style='width:97.6%;border-collapse:collapse'><tr style='height:6.75pt'><td width=123 rowspan=2 valign=top style='width:92.0pt;padding:0in 5.4pt 0in 5.4pt;height:6.75pt'><p class=MsoNormal><span style='font-size:12.0pt;font-family:""Times New Roman"",serif'><img width=120 id=""Picture_x0020_3"" src=""cid:Logo"" alt=""Logo""></span><o:p></o:p></p></td><td width=760 valign=top style='width:570.35pt;padding:0in 5.4pt 0in 5.4pt;height:6.75pt'></td></tr><tr style='height:226.85pt'><td width=760 valign=top style='width:570.35pt;padding:0in 5.4pt 0in 5.4pt;height:226.85pt'><p class=MsoNormal>");
            EMailBody.Append(@"<span lang=ES-PR style='font-size:12.0pt'><p><b>Programador: </b>" + Programador + "</p> <p><b>Aplicación: </b> " + NombreAplicacion + "<p><b>Nombre del Usuario: </b>" + NombreUsuario + "</p></p><p><b>Evento o Función: </b>" + NombreEventoFuncion + "</p> <p><b>URL: </b>" + URL + "</p> <p><b>Error: </b>" + Error + "</p><br></span>");
            EMailBody.Append(@"<span style='font-size:12.0pt'></span></p></td></tr></table></td></tr><tr style='height:18.2pt'><td style='padding:0in 0in 0in 0in;height:18.2pt'></td></tr></table></td></tr></table></div><p align=center style='text-align:center'><span lang=ES style='font-size:8.0pt;font-family:""Arial"",sans-serif;color:#7F7F7F'>Copyright 2016 © Sistema Universitario Ana G. Méndez <br/>POWER BY | O C I T</span><span lang=ES-PR style='color:#7F7F7F'><o:p></o:p></span></p></div></body></html>");

            AlternateView HTMLEmail = AlternateView.CreateAlternateViewFromString(EMailBody.ToString(), null, "text/html");
            string imagePath = System.Web.HttpContext.Current.Server.MapPath("img/Logo.png");
            LinkedResource logo = new LinkedResource(imagePath);
            logo.ContentId = "Logo";
            HTMLEmail.LinkedResources.Add(logo);

            mail.AlternateViews.Add(HTMLEmail);
            SmtpClient client = new SmtpClient("mailaccess.uagm.edu");
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("", "", "AGM");
            client.Send(mail);
            mail.Dispose();
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }
    }

    public string decipherTerm(string term)
    {
        string year = term.Substring(0, 4);
        string mod = term.Substring(4, 2);
        switch (mod)
        {
            case "01":
                year = (int.Parse(year) - 1).ToString();
                mod = "Agosto - Diciembre";
                break;
            case "02":
                mod = "Enero - Mayo";
                break;
            case "03":
                mod = "Julio - Junio";
                break;
        }
        return year + " " + mod;
    }

    public void sesionCaducada(Page page)
    {
        ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alert", @"swal({title: '',
                                                                                            text: 'La sesión ha caducado.',
                                                                                            showCancelButton: false,
                                                                                            confirmButtonText: 'Ok',
                                                                                            allowEscapeKey: false,
                                                                                            allowOutsideClick: false,
                                                                                            allowEnterKey: false
                                                                                        }).then((result) => {
                                                                                            if (result.value) {
                                                                                                location.href = 'login.aspx';
                                                                                            }
                                                                                        });", true);
    }

    public void errorBasesDeDatos(Page page)
    {
        ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alert", @"swal({title: ,
                                                                                            text: 'Problemas con la base de datos, intente mas tarde.',
                                                                                            showCancelButton: false,
                                                                                            confirmButtonText: 'Ok',
                                                                                            allowEscapeKey: false,
                                                                                            allowOutsideClick: false,
                                                                                            allowEnterKey: false
                                                                                        }).then((result) => {
                                                                                            if (result.value) {
                                                                                                location.href =                                                                             'login.aspx';
                                                                                            }
                                                                                        });", true);
    }
}