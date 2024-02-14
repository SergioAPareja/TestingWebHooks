//Namespace Declaration
using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;
using System.Web;
using System.Web.UI;

public partial class _Default : System.Web.UI.Page
{
    private MainClass appC = new MainClass();

    public OracleConnection conn = new OracleConnection();
    private OracleTransaction trans = null;

    private DataTable dt = new DataTable();
    private string HostName = HttpContext.Current.Request.Url.Host;
    private Encoding enc = new UTF8Encoding(true, true);

    private bool DEBUG = false;

    //Handle first page load + postback
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime currentTime = DateTime.Now;
        dateHeader.InnerText = currentTime.ToString();
        if (!IsPostBack)
        {
            //Corre al inicio + en refresh solamente
            //Frontloading data
            Session["Sortinggridview"] = (Session["Sortinggridview"] == null) ? "Subject ASC,Titulo ASC,COLLEGE ASC,Sesión ASC,Salón ASC,CRN ASC,Créditos ASC,Entrada ASC,DATE ASC,Salida ASC,ScheduleType ASC,Nivel ASC" : Session["Sortinggridview"];
            CadenaDeConexion();
            ConnectionName();
            loadCrigView();
        }
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    //Handle DB connection
    private void CadenaDeConexion()
    {
        try
        {
            Session["Conexion"] = appC.getConnection("2");
        }
        catch (Exception ex)
        {
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            if (error.Contains("Object reference not set") || error.Contains("ConnectionString"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", @"swal({title: 'Error: #01',
                                                                                            text: " + ex + @",
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
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: 'Error master page: #01', text:'" + error + "', type: ''});", true);
        }
    }

    private void loadCrigView()
    {
        try
        {
            if (DEBUG) { }
            else
            {
                //SQL string
                string script = @"SELECT CASE
                       WHEN SSBSECT_CRSE_NUMB < '500'  THEN 'Undergraduate' -- Undergraduate
                       WHEN SSBSECT_CRSE_NUMB >= '500' THEN 'Gradute' -- Graduate
                   END AS NIVEL,
                   SSBSECT_TERM_CODE AS TERMINO,
                   STVTERE_DESC AS TERMINO_DESC,
                   SSBSECT_CAMP_CODE || ' ' || STVCAMP_DESC AS CAMPUS,
                   NVL(SSBOVRR_DEPT_CODE, SCBCRSE_DEPT_CODE) || ' ' || STVDEPT_DESC AS DEPARTAMENTO,
                   SSBSECT_PTRM_CODE AS PTRM,
                   CASE
                       WHEN SUBSTR(SSBSECT_TERM_CODE, -1) = '3' THEN SOBPTRM_DESC
                   ELSE STVPTRM_DESC
                   END AS PTRM_DESC,
                   TO_CHAR(SOBPTRM_START_DATE,'fmday, dd ""de"" month ""de"" yyyy','nls_date_language=spanish') AS PTRM_START_DATE,
                   TO_CHAR(SOBPTRM_END_DATE, 'fmday, dd ""de"" month ""de"" yyyy', 'nls_date_language=spanish') AS PTRM_END_DATE,
                   NVL(SSBOVRR_COLL_CODE, NVL(SCBCRSE_COLL_CODE, '00')) AS COLL_CODE,
                   NVL(SSBOVRR_COLL_CODE, NVL(SCBCRSE_COLL_CODE, '00')) || '-' || STVCOLL_DESC AS COLL_DESC,
                    SSBSECT_SESS_CODE AS SESS,
                    CASE
                       WHEN SSBSECT_SESS_CODE = 'D' THEN 'Día'
                       WHEN SSBSECT_SESS_CODE = 'E' THEN 'Noche'
                       WHEN SSBSECT_SESS_CODE = 'A' THEN 'Tarde y Noche'
                   ELSE INITCAP(STVSESS_DESC)
                   END AS SESS_DESC,
                   SSBSECT_SUBJ_CODE || '-' || SSBSECT_CRSE_NUMB AS SUBJ_CRSE,
                   SSBSECT_CRSE_NUMB AS CRSE_NUMB,
                   SSBSECT_CRN AS CRN,
                   SSBSECT_SCHD_CODE || ' ' || STVSCHD_DESC AS SCHD,
                   NVL(SSBSECT_CREDIT_HRS, NVL(SCBCRSE_CREDIT_HR_LOW, 0)) AS CREDITS,
                   SCBCRSE_TITLE AS TITLE,
                   SSRMEET_BLDG_CODE  AS BUILDING,
                   SSRMEET_ROOM_CODE  AS ROOM,
                   SSRMEET_SUN_DAY    AS SUN,
                   SSRMEET_MON_DAY    AS MON,
                   SSRMEET_TUE_DAY    AS TUE,
                   SSRMEET_WED_DAY    AS WED,
                   SSRMEET_THU_DAY    AS THU,
                   SSRMEET_FRI_DAY    AS FRI,
                   SSRMEET_SAT_DAY    AS SAT,
                   TO_CHAR(TO_DATE(SSRMEET_BEGIN_TIME, 'HH24MI'), 'HH12:MI PM') AS HR_ENTRADA,
                   TO_CHAR(TO_DATE(SSRMEET_END_TIME, 'HH24MI'), 'HH12:MI PM')   AS HR_SALIDA,
                   GUBINST_STREET_LINE1 AS INSTITUTION
                   FROM SSBSECT, SCBCRSE B, SSBOVRR, SSRMEET, SOBPTRM, GUBINST,
                   STVCAMP, STVCOLL, STVDEPT, STVPTRM, STVSCHD, STVSESS, STVTERE
                   WHERE SSBSECT_TERM_CODE BETWEEN '202401' AND '202401'
                   AND SSBSECT_PTRM_CODE LIKE '%'
                   AND SSBSECT_CAMP_CODE LIKE '%'
                   AND SSBSECT_CAMP_CODE NOT IN(SELECT STVEUCP_CODE FROM STVEUCP)
                   AND SSBSECT_SSTS_CODE = 'A'
                   AND SSBSECT_SCHD_CODE NOT LIKE 'N%'
                   AND(SSBSECT_SCHD_CODE LIKE '%A'
                   OR SSBSECT_SCHD_CODE LIKE '%B'
                   OR SSBSECT_SCHD_CODE LIKE '%C'
                   OR SSBSECT_SCHD_CODE LIKE '%F'
                   OR SSBSECT_SCHD_CODE LIKE '%G'
                   OR SSBSECT_SCHD_CODE LIKE '%I'
                   OR SSBSECT_SCHD_CODE LIKE '%L'
                   OR SSBSECT_SCHD_CODE LIKE '%P'
                   OR SSBSECT_SCHD_CODE LIKE '%S'
                   OR SSBSECT_SCHD_CODE LIKE '%U'
                   OR SSBSECT_SCHD_CODE LIKE '%UE'
                   OR SSBSECT_SCHD_CODE LIKE '%UG'
                   OR SSBSECT_SCHD_CODE LIKE '%UL'
                   OR SSBSECT_SCHD_CODE LIKE '%US'
                   OR SSBSECT_SCHD_CODE LIKE '%UW'
                   OR SSBSECT_SCHD_CODE LIKE '%W')
                   AND SCBCRSE_CRSE_NUMB = SSBSECT_CRSE_NUMB
                   AND SCBCRSE_SUBJ_CODE = SSBSECT_SUBJ_CODE
                   AND SCBCRSE_EFF_TERM = (SELECT MAX(A.SCBCRSE_EFF_TERM)
                                         FROM SCBCRSE A
                                        WHERE A.SCBCRSE_CRSE_NUMB = SSBSECT_CRSE_NUMB
                                          AND A.SCBCRSE_SUBJ_CODE = SSBSECT_SUBJ_CODE
                                          AND A.SCBCRSE_EFF_TERM <= SSBSECT_TERM_CODE)
                   AND NVL(SCBCRSE_COLL_CODE, '00') LIKE '%'
                   AND NVL(SCBCRSE_DEPT_CODE, '0000') LIKE '%'
                   AND SCBCRSE_COLL_CODE <> 'Z'
                   AND SSBOVRR_TERM_CODE(+) = SSBSECT_TERM_CODE
                   AND SSBOVRR_CRN(+) = SSBSECT_CRN
                   AND SSRMEET_TERM_CODE(+) = SSBSECT_TERM_CODE
                   AND SSRMEET_CRN(+) = SSBSECT_CRN
                   AND SOBPTRM_TERM_CODE = SSBSECT_TERM_CODE
                   AND SOBPTRM_PTRM_CODE = SSBSECT_PTRM_CODE
                   AND STVCAMP_CODE = NVL(SSBSECT_CAMP_CODE, '00')
                   AND STVCOLL_CODE = NVL(SSBOVRR_COLL_CODE, NVL(SCBCRSE_COLL_CODE, '00'))
                   AND STVDEPT_CODE(+) = NVL(SSBOVRR_DEPT_CODE, SCBCRSE_DEPT_CODE)
                   AND STVPTRM_CODE = SSBSECT_PTRM_CODE
                   AND STVSCHD_CODE = SSBSECT_SCHD_CODE
                   AND STVSESS_CODE(+) = SSBSECT_SESS_CODE
                   AND STVTERE_CODE = SSBSECT_TERM_CODE
                   ORDER BY NIVEL, TERMINO, CAMPUS, DEPARTAMENTO, PTRM, SESS, SUBJ_CRSE";

                DataTable newDataTable = new DataTable();

                newDataTable.Columns.Add("SUBJ_CRSE", typeof(String));
                newDataTable.Columns.Add("TITLE", typeof(String));
                newDataTable.Columns.Add("COLL_DESC", typeof(String));
                newDataTable.Columns.Add("SESS_DESC", typeof(String));
                newDataTable.Columns.Add("BUILDING", typeof(String));
                newDataTable.Columns.Add("CRN", typeof(String));
                newDataTable.Columns.Add("CREDITS", typeof(String));
                newDataTable.Columns.Add("HR_ENTRADA", typeof(String));
                newDataTable.Columns.Add("HR_SALIDA", typeof(String));
                newDataTable.Columns.Add("DO", typeof(String));
                newDataTable.Columns.Add("LU", typeof(String));
                newDataTable.Columns.Add("MA", typeof(String));
                newDataTable.Columns.Add("MI", typeof(String));
                newDataTable.Columns.Add("JU", typeof(String));
                newDataTable.Columns.Add("VI", typeof(String));
                newDataTable.Columns.Add("SA", typeof(String));
                newDataTable.Columns.Add("SCHD", typeof(String));
                newDataTable.Columns.Add("NIVEL", typeof(String));
                newDataTable.Columns.Add("PTRM", typeof(String));

                newDataTable = appC.retornarDT(script);

                if (newDataTable.Rows.Count == 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: '¡Alerta!', text:'No hemos encontrado resultados.', type: ''});", true);
                else
                {
                    //gvResultados.DataSource = newDataTable;
                    Session["gvResultados"] = newDataTable;
                    if (newDataTable != null)
                    {
                        DataView dataView = newDataTable.DefaultView;
                        dataView.Sort = "SUBJ_CRSE ASC";
                        gvResultados.DataSource = dataView;
                        gvResultados.DataBind();
                    }
                    //gvResultados.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", @"swal({title: '',
                                                                                            text:" + ex + @",
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
    }

    protected void gvResultados_Sorting(object sender, EventArgs e)
    {
        string[] columns = new string[15];
        columns[0] = "COLLEGE";
        columns[1] = "Titulo";
        columns[2] = "Sesión";
        columns[3] = "Subject";
        columns[4] = "Salón";
        columns[5] = "CRN";
        columns[6] = "Créditos";
        columns[7] = "Entrada";
        columns[8] = "DATE";
        columns[9] = "Salida";
        columns[10] = "ScheduleType";
        columns[11] = "Nivel";
        columns[13] = "PTRM";
        columns[14] = "Departamento";
        string rowSortIn = "";

        foreach (string filter in columns)
        {
            switch (filter)
            {
                case "Subject":
                    rowSortIn = (SubjectSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "SUBJ_CRSE " + SubjectSorting.SelectedValue + ",";
                    break;

                case "Titulo":
                    rowSortIn = (TituloSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "TITLE " + TituloSorting.SelectedValue + ",";
                    break;

                case "Sesión":
                    rowSortIn = (SesiónSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "SESS_DESC " + SesiónSorting.SelectedValue + ",";
                    break;

                case "COLLEGE":
                    rowSortIn = (CollegeSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "COLL_DESC " + CollegeSorting.SelectedValue + ",";
                    break;

                case "Salón":
                    rowSortIn = (SalónSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "BUILDING " + SalónSorting.SelectedValue + ",";
                    break;

                case "CRN":
                    rowSortIn = (CRNSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "CRN " + CRNSorting.SelectedValue + ",";
                    break;

                case "Créditos":
                    rowSortIn = (CréditosSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "CREDITS " + CréditosSorting.SelectedValue + ",";
                    break;

                case "Entrada":
                    rowSortIn = (EntradaSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "HR_ENTRADA " + EntradaSorting.SelectedValue + ",";
                    break;

                case "Salida":
                    rowSortIn = (SalidaSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "HR_ENTRADA " + SalidaSorting.SelectedValue + ",";
                    break;

                case "DATE":

                    break;

                case "ScheduleType":
                    rowSortIn = (ScheduleSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "SCHD " + ScheduleSorting.SelectedValue + ",";
                    break;

                case "Nivel":
                    rowSortIn = (NivelSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "NIVEL " + NivelSorting.SelectedValue + ",";
                    break;

                case "PTRM":
                    rowSortIn = (PTRMSorting.SelectedValue == "0") ? rowSortIn : rowSortIn + "PTRM " + PTRMSorting.SelectedValue + ",";
                    break;
            }
        }

        rowSortIn = (rowSortIn.EndsWith(",")) ? rowSortIn.Remove(rowSortIn.Length - 1) : rowSortIn;

        Session["Sortinggridview"] = rowSortIn;
        DataTable dataTable = Session["gvResultados"] as DataTable;

        if (dataTable != null)
        {
            DataView dataView = dataTable.DefaultView;
            dataView.Sort = rowSortIn;
            gvResultados.DataSource = dataView;
            gvResultados.DataBind();
        }
    }

    //All filters found in the sidebar have this method for filtering
    protected void sidebarSearch_TextChanged(object sender, EventArgs e)
    {
        string rowFilterIn = "";

        try
        {
            DataTable dt = (DataTable)Session["gvResultados"];

            if (dt == null)
            {
            }
            else
            {
                rowFilterIn = (string.IsNullOrEmpty(College.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "COLL_DESC", College.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(Titulo.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "TITLE", Titulo.Text.ToUpper()); ;
                rowFilterIn = (string.IsNullOrEmpty(Titulo.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "SESS_DESC", Sesión.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(Subject.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "SUBJ_CRSE", Subject.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(Salón.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "BUILDING", Salón.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(CRN.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "CRN", CRN.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(Créditos.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "CREDITS", Créditos.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(Entrada.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "HR_ENTRADA", Entrada.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(Salida.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "HR_SALIDA", Salida.Text.ToUpper());

                rowFilterIn = (!dayDo.Checked) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "SUN", "N");
                rowFilterIn = (!dayLu.Checked) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "MON", "M");
                rowFilterIn = (!dayMa.Checked) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "TUE", "T");
                rowFilterIn = (!dayMi.Checked) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "WED", "W");
                rowFilterIn = (!dayJu.Checked) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "THU", "R");
                rowFilterIn = (!dayVi.Checked) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "FRI", "F");
                rowFilterIn = (!daySa.Checked) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "SAT", "S");

                rowFilterIn = (string.IsNullOrEmpty(Schedule.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "SCHD", Schedule.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(Nivel.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "NIVEL", Nivel.Text.ToUpper());
                rowFilterIn = (string.IsNullOrEmpty(PTRM.Text)) ? rowFilterIn : rowFilterIn + string.Format("{0} LIKE '%{1}%' AND ", "PTRM", PTRM.Text.ToUpper());
                rowFilterIn = string.IsNullOrEmpty(rowFilterIn) ? rowFilterIn : rowFilterIn.Remove(rowFilterIn.Length - 5, 5);
                //Keep previous filters if already exists, must follow format "Column LIKE Column.Text AND ..."
                dt.DefaultView.RowFilter = rowFilterIn;
                gvResultados.DataSource = dt;
                Session["gvResultados"] = dt;
                gvResultados.DataBind();
            }
        }
        catch (Exception exx)
        {
            appC.errorBasesDeDatos(this);
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        DataTable dataTable = Session["gvResultados"] as DataTable;

        College.Text = string.Empty;
        Titulo.Text = string.Empty;
        Sesión.Text = string.Empty;
        Subject.Text = string.Empty;
        Salón.Text = string.Empty;
        CRN.Text = string.Empty;
        Créditos.Text = string.Empty;
        Entrada.Text = string.Empty;
        Salida.Text = string.Empty;

        Nivel.Text = string.Empty;

        dataTable.DefaultView.RowFilter = "";
        gvResultados.DataSource = dataTable;
        Session["gvResultados"] = dataTable;
        gvResultados.DataBind();
    }

    protected void gotoTop(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
    }

    private void ConnectionName()
    {
        //Mostramos el nombre de la base de datos.
        try
        {
            conn.ConnectionString = Session["Conexion"].ToString();
            OracleCommand cmd = new OracleCommand("SELECT GUBINST_STREET_LINE1 AS RECINTO, GUBINST_INSTANCE_NAME AS INSTANCIA FROM GUBINST");
            conn.Open();
            cmd.Connection = conn;
            OracleDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            if (rdr.HasRows == true)
            {
                string inst = (string)(Session["INST"]) as string;
                string acceso = (string)(Session["ACCESS_LEVEL"]) as string;
                /*                lblUsername.Text = "<b>Usuario: </b>" + (string)(Session["USERNAME"]) as string;
                                lblConnectionName.Text = "<b>Conexión: </b>" + rdr["INSTANCIA"].ToString().ToUpper();
                                lblRecinto.Text = "<b>Recinto o localidad: </b> Administración Central";
                                lblFechaModificado.Text = "<b>Actualización: </b>" + "03/24/2022";
                                if (acceso != null)
                                    lblTipoAcceso.Text = "<b>Acceso: </b>" + acceso.Replace("FULL", "Super Admin").Replace("NULL", "Administrador");
                                else
                                    lblTipoAcceso.Text = "<b>Acceso: </b>Usuario Regular";*/
            }
        }
        catch (Exception ex)
        {
            //appC.SendException(ex.Message + " | " + ex.Source + " | " + ex.InnerException, "ConnectionName()", Session["USERNAME"].ToString(), URL);
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            if (error.Contains("Object reference not set") || error.Contains("ConnectionString"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", @"swal({title: 'Error: #02',
                                                                                            text: " + ex + @",
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
        }
        finally
        {
            conn.Close();
        }
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

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        string mediaType = "D";
        string urlTarget = HttpContext.Current.Request.Url.AbsoluteUri.Replace("Index", "PagePDF");
        string fileName = "Programación de Cursos - " + DateTime.Now.ToString();
        string fileTarget = "null";

        DataTable newDataTable = new DataTable();

        newDataTable.Columns.Add("SUBJ_CRSE", typeof(String));
        newDataTable.Columns.Add("TITLE", typeof(String));
        newDataTable.Columns.Add("COLL_DESC", typeof(String));
        newDataTable.Columns.Add("SESS_DESC", typeof(String));
        newDataTable.Columns.Add("BUILDING", typeof(String));
        newDataTable.Columns.Add("CRN", typeof(String));
        newDataTable.Columns.Add("CREDITS", typeof(String));
        newDataTable.Columns.Add("HR_ENTRADA", typeof(String));
        newDataTable.Columns.Add("HR_SALIDA", typeof(String));
        newDataTable.Columns.Add("SUN", typeof(String));
        newDataTable.Columns.Add("MON", typeof(String));
        newDataTable.Columns.Add("TUE", typeof(String));
        newDataTable.Columns.Add("WED", typeof(String));
        newDataTable.Columns.Add("THU", typeof(String));
        newDataTable.Columns.Add("FRI", typeof(String));
        newDataTable.Columns.Add("SAT", typeof(String));
        newDataTable.Columns.Add("SCHD", typeof(String));
        newDataTable.Columns.Add("NIVEL", typeof(String));
        newDataTable.Columns.Add("PTRM", typeof(String));

        DataRow newRow = newDataTable.NewRow();

        newRow[0] = Subject.Text;
        newRow[1] = Titulo.Text;
        newRow[2] = College.Text;
        newRow[3] = Sesión.Text;
        newRow[4] = Salón.Text;
        newRow[5] = CRN.Text;
        newRow[6] = Créditos.Text;
        newRow[7] = Entrada.Text;
        newRow[8] = Salida.Text;
        newRow[9] = (dayDo.Checked) ? "Do" : "";
        newRow[10] = (dayLu.Checked) ? "Lu" : "";
        newRow[11] = (dayMa.Checked) ? "Ma" : "";
        newRow[12] = (dayMi.Checked) ? "Mi" : "";
        newRow[13] = (dayJu.Checked) ? "Ju" : "";
        newRow[14] = (dayVi.Checked) ? "Vi" : "";
        newRow[15] = (daySa.Checked) ? "Sa" : "";
        newRow[16] = Schedule.Text;
        newRow[17] = PTRM.Text;
        newRow[18] = Nivel.Text;

        newDataTable.Rows.Add(newRow);

        Application["Search"] = newDataTable;
        Application["gvResultados"] = Session["gvResultados"];
        Application["Sortinggridview"] = Session["Sortinggridview"];

        appC.convertirPDF(mediaType, urlTarget, fileName, fileTarget);

        Application.RemoveAll();
    }
}