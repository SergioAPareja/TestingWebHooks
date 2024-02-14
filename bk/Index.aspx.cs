using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using Winnovative;
using System.DirectoryServices;
using System.Threading.Tasks;
using System.Text;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Math;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    /*
     Abrebiaturas de AD
     cn=container
     ou=grupo
     dc=domain
     * */
    public OracleConnection conn = new OracleConnection();
    private MainClass appC = new MainClass();
    OracleTransaction trans = null;
    string HostName = HttpContext.Current.Request.Url.Host;
    Encoding enc = new UTF8Encoding(true, true);
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);


        if (!IsPostBack)
        {
            CadenaDeConexion();
            ConnectionName();
            //((Label)Master.FindControl("lblTituloPantalla")).Text = "Cuadro telefónico: (787)751-1372";
            //SHERLYN SOLICITO ELIMINAR LOS FILTROS Y QUE APARECIERAN TODOS LOS DATOS AL CARGAR LA PAGINA.
            //loadCrigView();
            

            if ((string)(Session["USERNAME"]) as string == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", @"swal({title: '',
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

        }



    }
    private void CadenaDeConexion()
    {
        try
        {
            Session["Conexion"] = appC.getConnection("2");
        }
        catch (Exception ex)
        {
            //appC.SendException(ex.Message + " | " + ex.Source + " | " + ex.InnerException, "CadenaDeConexion()", Session["USUARIO"].ToString(), URL);
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            if (error.Contains("Object reference not set") || error.Contains("ConnectionString"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", @"swal({title: 'Error: #01',
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
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: 'Error master page: #01', text:'" + error + "', type: ''});", true);
        }
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> autoCompleteOficina(string prefixText)
    {
        //FUNCIÓN DE AUTOCOMPLETE
        List<string> autoComplete = new List<string>();
        DataTable dt = new DataTable();
        OracleConnection cn = new OracleConnection();
        cn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
        cn.Open();
        prefixText = prefixText.Replace(" ", "%");
        OracleCommand cmdSPRIDEN = new OracleCommand(@"SELECT DISTINCT
                                                            UPPER(NVL(YTVORGN_TITLE, '')) AS OFFICE
                                                            FROM PEBEMPL, NBRJOBS A, SPRIDEN, FIMSMGR.YTVORGN D
                                                            WHERE PEBEMPL_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
                                                            AND A.NBRJOBS_PIDM = PEBEMPL_PIDM
                                                            AND A.NBRJOBS_STATUS <> 'T'
                                                            AND A.NBRJOBS_SUFF = '00'
                                                            AND A.NBRJOBS_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
                                                            AND A.NBRJOBS_EFFECTIVE_DATE = (SELECT MAX(C.NBRJOBS_EFFECTIVE_DATE)
                                                            FROM NBRJOBS C
                                                            WHERE C.NBRJOBS_PIDM = A.NBRJOBS_PIDM
                                                            AND C.NBRJOBS_POSN = A.NBRJOBS_POSN
                                                            AND C.NBRJOBS_SUFF = A.NBRJOBS_SUFF
                                                            AND C.NBRJOBS_EFFECTIVE_DATE <= SYSDATE)
                                                            AND EXISTS (SELECT 'X'
                                                            FROM NBRBJOB
                                                            WHERE NBRBJOB_PIDM = A.NBRJOBS_PIDM
                                                            AND NBRBJOB_POSN = A.NBRJOBS_POSN
                                                            AND NBRBJOB_SUFF = A.NBRJOBS_SUFF
                                                            AND NBRBJOB_CONTRACT_TYPE = 'P'
                                                            AND (NBRBJOB_END_DATE IS NULL
                                                            OR NBRBJOB_END_DATE > SYSDATE))
                                                            AND SPRIDEN_PIDM = PEBEMPL_PIDM
                                                            AND SPRIDEN_CHANGE_IND IS NULL
                                                            --
                                                            AND YTVORGN_ORGN_CODE(+) = PEBEMPL_ORGN_CODE_HOME
                                                            AND UPPER(TRIM(REPLACE((YTVORGN_TITLE),' ', ' '))) LIKE UPPER(TRIM('%" + prefixText + @"%'))
                                                            --
                                                            ORDER BY OFFICE", cn);
        OracleDataAdapter adpSPRIDEN = new OracleDataAdapter(cmdSPRIDEN);
        adpSPRIDEN.Fill(dt);
        if (dt.Rows.Count == 0)
        {
            autoComplete.Add("No hay datos.");
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                autoComplete.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["OFFICE"].ToString(), dt.Rows[i]["OFFICE"].ToString()));
            }
        }
        return autoComplete;
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> autoCompleteNombre(string prefixText)
    {
        //FUNCIÓN DE AUTOCOMPLETE
        List<string> autoComplete = new List<string>();
        DataTable dt = new DataTable();
        OracleConnection cn = new OracleConnection();
        cn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
        cn.Open();
        prefixText = prefixText.Replace(" ", "%");
        OracleCommand cmdSPRIDEN = new OracleCommand(@"SELECT SPRIDEN_ID, SPRIDEN_PIDM AS PIDM, REPLACE((SPRIDEN_FIRST_NAME || ' ' || SPRIDEN_MI || ' ' || SPRIDEN_LAST_NAME),' ', ' ') USER_NAME, USERNAME, GOREMAL_EMAIL_ADDRESS
 FROM
      (SELECT SPRIDEN_ID, SPRIDEN_FIRST_NAME, SPRIDEN_MI, SPRIDEN_LAST_NAME, GOREMAL_EMAIL_ADDRESS,
              UPPER(SUBSTR(GOREMAL_EMAIL_ADDRESS, 0 ,INSTR(GOREMAL_EMAIL_ADDRESS, '@',1)-1)) AS USERNAME, SPRIDEN_PIDM
        FROM SPRIDEN, GOREMAL
       WHERE UPPER(TRIM(REPLACE((SPRIDEN_FIRST_NAME || ' ' || SPRIDEN_MI || ' ' || SPRIDEN_LAST_NAME),' ', ' '))) LIKE UPPER(TRIM('%" + prefixText + @"%'))
         AND SPRIDEN_CHANGE_IND IS NULL
         AND GOREMAL_PIDM = SPRIDEN_PIDM
         AND GOREMAL_EMAL_CODE = 'AGM'
		 --
         AND EXISTS (SELECT 'X'
                       FROM PEBEMPL, NBRJOBS A
                      WHERE PEBEMPL_PIDM = SPRIDEN_PIDM
                        AND PEBEMPL_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
                        AND A.NBRJOBS_PIDM = PEBEMPL_PIDM
                        AND A.NBRJOBS_STATUS <> 'T'
                        AND A.NBRJOBS_SUFF = '00'
                        AND A.NBRJOBS_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
                        AND A.NBRJOBS_EFFECTIVE_DATE = (SELECT MAX(C.NBRJOBS_EFFECTIVE_DATE)
                                                          FROM NBRJOBS C
                                                         WHERE C.NBRJOBS_PIDM = A.NBRJOBS_PIDM
                                                           AND C.NBRJOBS_POSN = A.NBRJOBS_POSN
                                                           AND C.NBRJOBS_SUFF = A.NBRJOBS_SUFF
                                                           AND C.NBRJOBS_EFFECTIVE_DATE <= SYSDATE)
                        AND EXISTS (SELECT 'X'
                                      FROM NBRBJOB
                                     WHERE NBRBJOB_PIDM = A.NBRJOBS_PIDM
                                       AND NBRBJOB_POSN = A.NBRJOBS_POSN
                                       AND NBRBJOB_SUFF = A.NBRJOBS_SUFF
                                       AND NBRBJOB_CONTRACT_TYPE = 'P'
                                       AND (NBRBJOB_END_DATE IS NULL
                                         OR NBRBJOB_END_DATE > SYSDATE)))
		 --
       ORDER BY SPRIDEN_FIRST_NAME, SPRIDEN_LAST_NAME)
--
WHERE ROWNUM <= 50", cn);
        OracleDataAdapter adpSPRIDEN = new OracleDataAdapter(cmdSPRIDEN);
        adpSPRIDEN.Fill(dt);
        if (dt.Rows.Count == 0)
        {
            autoComplete.Add("No hay datos.");
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                autoComplete.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i][2].ToString(), dt.Rows[i][0].ToString()));
            }
        }
        return autoComplete;
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> autoCompletePuesto(string prefixText)
    {
        //FUNCIÓN DE AUTOCOMPLETE
        List<string> autoComplete = new List<string>();
        DataTable dt = new DataTable();
        OracleConnection cn = new OracleConnection();
        cn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
        cn.Open();
        prefixText = prefixText.Replace(" ", "%");
        OracleCommand cmdSPRIDEN = new OracleCommand(@"SELECT DISTINCT NVL(TRIM(REPLACE(NTVPOSN_TITLE, CHR(09))), TRIM(NBBPOSN_TITLE)) AS POSITION_TITLE
                                                         FROM PEBEMPL, NBRJOBS A, NBBPOSN, NTVPOSN, SPRIDEN
                                                        WHERE PEBEMPL_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
                                                          AND A.NBRJOBS_PIDM = PEBEMPL_PIDM
                                                          AND A.NBRJOBS_STATUS <> 'T'
                                                          AND A.NBRJOBS_SUFF = '00'
                                                          AND A.NBRJOBS_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
                                                          AND A.NBRJOBS_EFFECTIVE_DATE = (SELECT MAX(C.NBRJOBS_EFFECTIVE_DATE)
                                                                                            FROM NBRJOBS C
                                                                                           WHERE C.NBRJOBS_PIDM = A.NBRJOBS_PIDM
                                                                                             AND C.NBRJOBS_POSN = A.NBRJOBS_POSN
                                                                                             AND C.NBRJOBS_SUFF = A.NBRJOBS_SUFF
                                                                                             AND C.NBRJOBS_EFFECTIVE_DATE <= SYSDATE)
                                                          AND EXISTS (SELECT 'X'
                                                                        FROM NBRBJOB
                                                                       WHERE NBRBJOB_PIDM = A.NBRJOBS_PIDM
                                                                         AND NBRBJOB_POSN = A.NBRJOBS_POSN
                                                                         AND NBRBJOB_SUFF = A.NBRJOBS_SUFF
                                                                         AND NBRBJOB_CONTRACT_TYPE = 'P'
                                                                         AND (NBRBJOB_END_DATE IS NULL
                                                                           OR NBRBJOB_END_DATE > SYSDATE))
                                                       --
                                                          AND SPRIDEN_PIDM = PEBEMPL_PIDM
                                                          AND SPRIDEN_CHANGE_IND IS NULL
                                                          AND NBBPOSN_POSN = A.NBRJOBS_POSN
                                                          AND NTVPOSN_POSN(+) = A.NBRJOBS_POSN
                                                          AND (TRIM(UPPER(NBBPOSN_TITLE))  LIKE TRIM(UPPER('%' || '" + prefixText + @"' || '%')) OR UPPER(TRIM(NBBPOSN_TITLE)) LIKE TRIM(UPPER('%' || '" + prefixText + @"' || '%')))
                                                        ORDER BY POSITION_TITLE", cn);
        OracleDataAdapter adpSPRIDEN = new OracleDataAdapter(cmdSPRIDEN);
        adpSPRIDEN.Fill(dt);
        if (dt.Rows.Count == 0)
        {
            autoComplete.Add("No hay datos.");
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                autoComplete.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i][0].ToString(), dt.Rows[i][0].ToString()));
            }
        }
        return autoComplete;
    }

    [Obsolete]
    private void gridViewWithFilter()
    {
      

        try
        {
            conn.ConnectionString = HttpContext.Current.Session["Conexion"].ToString();
            OracleCommand cmd = new OracleCommand(@"SELECT DISTINCT
       CASE
          CASE
             WHEN NBRJLBD_PERCENT = 50 THEN SUBSTR(NBRJOBS_ORGN_CODE_TS, 1, 1)
             WHEN SUBSTR(NBRJLBD_FUND_CODE, 1, 1) = '2' THEN SUBSTR(NVL(NBRJLBD_FUND_CODE, '*'), 3, 1)
          ELSE SUBSTR(PEBEMPL_ORGN_CODE_HOME, 1, 1)
          END
          WHEN 'A' THEN 'Administración Central'
          WHEN 'E' THEN 'Recinto de Carolina'
          WHEN 'M' THEN 'Recinto de Cupey'
          WHEN 'T' THEN 'Recinto de Gurabo'
          WHEN 'V' THEN 'Recinto Online'
         --WHEN 'C' THEN 'Sistema TV'
          WHEN 'O' THEN 'Arecibo Observatory'
       ELSE 'UAGM'
       END AS INSTITUTION,
--
       CASE UPPER(YTVORGN_TITLE)
              WHEN 'TO BE ASSIGNED' THEN
                         (SELECT FTVORGN_TITLE
                        FROM FTVORGN A
                       WHERE A.FTVORGN_ORGN_CODE = PEBEMPL_ORGN_CODE_HOME
                         AND A.FTVORGN_COAS_CODE = '4'
                         AND A.FTVORGN_STATUS_IND = 'A'
                         AND A.FTVORGN_EFF_DATE = (SELECT MAX(B.FTVORGN_EFF_DATE)
                                                     FROM FTVORGN B
                                                    WHERE B.FTVORGN_COAS_CODE = A.FTVORGN_COAS_CODE
                                                      AND B.FTVORGN_ORGN_CODE = A.FTVORGN_ORGN_CODE))
       ELSE NVL(YTVORGN_TITLE, '')
       END AS OFFICE, --- Oficina
       SPRIDEN_ID AS ID,
       INITCAP(TRIM(SPRIDEN_FIRST_NAME ||' '|| SPRIDEN_LAST_NAME)) AS EMPLOYEE_NAME,
       NVL(INITCAP(TRIM(SPBPERS_LEGAL_NAME)),
           INITCAP(TRIM(SPRIDEN_FIRST_NAME ||' '|| REPLACE(SPRIDEN_MI, '.')) ||' '||
                        SPRIDEN_LAST_NAME)) AS NOMBRE_COMPLETO,
       NVL(TRIM(NTVPOSN_TITLE), NBBPOSN_TITLE) AS POSITION_TITLE,

       NVL((SELECT LOWER(GOREMAL_EMAIL_ADDRESS)
              FROM GOREMAL
             WHERE GOREMAL_PIDM = SPRIDEN_PIDM
               AND GOREMAL_EMAL_CODE = 'AGM'
               AND GOREMAL_STATUS_IND = 'A'
               AND GOREMAL_PREFERRED_IND = 'Y'), '') AS EMAIL_AGM
  FROM PEBEMPL, NBRJOBS A, NBRJLBD B, NBBPOSN, NTVPOSN, SPRIDEN, SPBPERS, FIMSMGR.YTVORGN D
 WHERE PEBEMPL_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
   AND A.NBRJOBS_PIDM = PEBEMPL_PIDM
   AND A.NBRJOBS_STATUS <> 'T'
   AND A.NBRJOBS_SUFF = '00'
--
   AND ( (   :P_TYPE = 'F' -- facultad
          AND A.NBRJOBS_ECLS_CODE IN ('AJ', 'FA', 'FM', 'FS', 'FV') )
      OR (   :P_TYPE = 'A' -- administrativo
       AND A.NBRJOBS_ECLS_CODE IN ('AD', 'FT', 'HP', 'HT') )  )
--
   AND A.NBRJOBS_EFFECTIVE_DATE = (SELECT MAX(C.NBRJOBS_EFFECTIVE_DATE)
                                     FROM NBRJOBS C
                                    WHERE C.NBRJOBS_PIDM = A.NBRJOBS_PIDM
                                      AND C.NBRJOBS_POSN = A.NBRJOBS_POSN
                                      AND C.NBRJOBS_SUFF = A.NBRJOBS_SUFF
                                      AND C.NBRJOBS_EFFECTIVE_DATE <= SYSDATE)
   AND EXISTS (SELECT 'X'
                 FROM NBRBJOB
                WHERE NBRBJOB_PIDM = A.NBRJOBS_PIDM
                  AND NBRBJOB_POSN = A.NBRJOBS_POSN
                  AND NBRBJOB_SUFF = A.NBRJOBS_SUFF
                  AND NBRBJOB_CONTRACT_TYPE = 'P'
                  AND (NBRBJOB_END_DATE IS NULL
                    OR NBRBJOB_END_DATE > SYSDATE))
   AND NBRJLBD_PIDM = A.NBRJOBS_PIDM
   AND NBRJLBD_POSN = A.NBRJOBS_POSN
   AND NBRJLBD_SUFF = A.NBRJOBS_SUFF
   AND NBRJLBD_EFFECTIVE_DATE = (SELECT MAX(D.NBRJLBD_EFFECTIVE_DATE)
                                   FROM POSNCTL.NBRJLBD D
                                  WHERE D.NBRJLBD_PIDM = A.NBRJOBS_PIDM
                                    AND D.NBRJLBD_POSN = A.NBRJOBS_POSN
                                    AND D.NBRJLBD_SUFF = A.NBRJOBS_SUFF)
   AND NBRJLBD_PERCENT = (SELECT MAX(D.NBRJLBD_PERCENT)
                            FROM POSNCTL.NBRJLBD D
                           WHERE D.NBRJLBD_PIDM = A.NBRJOBS_PIDM
                             AND D.NBRJLBD_POSN = A.NBRJOBS_POSN
                             AND D.NBRJLBD_SUFF = A.NBRJOBS_SUFF
                             AND D.NBRJLBD_EFFECTIVE_DATE = B.NBRJLBD_EFFECTIVE_DATE)
   AND SPRIDEN_PIDM = PEBEMPL_PIDM
   AND SPRIDEN_CHANGE_IND IS NULL
   AND SPBPERS_PIDM(+) = PEBEMPL_PIDM
   AND UPPER(TRIM(REPLACE((SPRIDEN_FIRST_NAME || ' ' || SPRIDEN_MI || ' ' || SPRIDEN_LAST_NAME),' ', ' '))) LIKE UPPER(TRIM('%')) --('%"" + txtNombre.Text + @""%'))
   AND NBBPOSN_POSN = A.NBRJOBS_POSN
   AND NTVPOSN_POSN(+) = A.NBRJOBS_POSN
   AND YTVORGN_ORGN_CODE(+) = PEBEMPL_ORGN_CODE_HOME
--
   AND CASE
          WHEN NBRJLBD_PERCENT = 50 THEN SUBSTR(NBRJOBS_ORGN_CODE_TS, 1, 1)
          WHEN SUBSTR(NBRJLBD_FUND_CODE, 1, 1) = '2' THEN SUBSTR(NVL(NBRJLBD_FUND_CODE, '*'), 3, 1)
       ELSE SUBSTR(PEBEMPL_ORGN_CODE_HOME, 1, 1)
       END LIKE :P_INST
--
   AND UPPER(NVL(YTVORGN_TITLE, '%')) LIKE UPPER('%') --('"" + txtOficina.Text + @""%')
   AND (UPPER(NBBPOSN_TITLE) LIKE UPPER('%') OR UPPER(NTVPOSN_TITLE) LIKE UPPER('%'))
 ORDER BY INSTITUTION, OFFICE, EMPLOYEE_NAME


");

            if (rdlFacultad.SelectedValue.Equals("%") || rdlFacultad.SelectedValue.Equals("E") || rdlFacultad.SelectedValue.Equals("M") || rdlFacultad.SelectedValue.Equals("T"))
            {
                cmd.Parameters.AddWithValue(":P_TYPE", "F");
                cmd.Parameters.AddWithValue(":P_INST", rdlFacultad.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue(":P_TYPE", "A");
                cmd.Parameters.AddWithValue(":P_INST", rdlAdminitrativo.SelectedValue);
            }




            cmd.Connection = conn;
            conn.Open();

            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            

            var newDataTable = new DataTable();
            DataRow row = newDataTable.NewRow();
            newDataTable.Columns.Add("INSTITUTION", typeof(String));
            newDataTable.Columns.Add("OFFICE", typeof(String));
            newDataTable.Columns.Add("EMPLOYEE_NAME", typeof(String));
            newDataTable.Columns.Add("NOMBRE_COMPLETO", typeof(String));
            newDataTable.Columns.Add("ID", typeof(String));
            newDataTable.Columns.Add("POSITION_TITLE", typeof(String));
            newDataTable.Columns.Add("EMAIL_AGM", typeof(String));
            newDataTable.Columns.Add("EXTENSION", typeof(String));
            newDataTable.Columns.Add("TEAMS", typeof(String));
            

            foreach (DataRow item in dt.Rows)
            {
                if (item["INSTITUTION"].ToString().Equals("Administracion Central"))
                {


                    if (item["NOMBRE_COMPLETO"].ToString().Equals("Rafael A Nadal Arcelay") || item["NOMBRE_COMPLETO"].ToString().Equals("Gino Q Natalicchio") || item["NOMBRE_COMPLETO"].ToString().Equals("Jose F Mendez Mendez"))
                    {


                        newDataTable.Rows.Add(item["INSTITUTION"].ToString().Replace(item["INSTITUTION"].ToString(), "Administración Central"),
                                        item["OFFICE"].ToString(),
                                        item["NOMBRE_COMPLETO"].ToString(),
                                        item["EMPLOYEE_NAME"].ToString(),
                                        item["ID"].ToString(),
                                        item["POSITION_TITLE"].ToString(),
                                        item["EMAIL_AGM"].ToString()

                                        //retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                                        //"https://teams.microsoft.com/L/Chat/0/0?users=" + item["EMAIL_AGM"].ToString()

                                        //"" + ""




                                        );


                    }
                    else
                    {
                        newDataTable.Rows.Add(item["INSTITUTION"].ToString().Replace(item["INSTITUTION"].ToString(), "Administración Central"),
                                       item["OFFICE"].ToString(),
                                       item["NOMBRE_COMPLETO"].ToString(),
                                       item["EMPLOYEE_NAME"].ToString(),
                                       item["ID"].ToString(),
                                       item["POSITION_TITLE"].ToString(),
                                       item["EMAIL_AGM"].ToString(),
                                       //retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                                       retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                                       "https://teams.microsoft.com/L/Chat/0/0?users=" + item["EMAIL_AGM"].ToString()
                                       //"" + ""
                                       );
                    }

                }
                else
                {
                    newDataTable.Rows.Add(
                                        item["INSTITUTION"].ToString(),
                                        item["OFFICE"].ToString(),
                                        item["NOMBRE_COMPLETO"].ToString(),
                                        item["EMPLOYEE_NAME"].ToString(),
                                        item["ID"].ToString(),
                                        item["POSITION_TITLE"].ToString(),
                                        item["EMAIL_AGM"].ToString(),
                                        retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                                        "https://teams.microsoft.com/L/Chat/0/0?users=" + item["EMAIL_AGM"].ToString()
                                        );
                }
                //row["NombreColumna"].ToString();
                //newDataTable.Rows.Add(
                //    item["INSTITUTION"].ToString(),
                //    item["OFFICE"].ToString(),
                //    item["NOMBRE_COMPLETO"].ToString(),
                //    item["EMPLOYEE_NAME"].ToString(),
                //    item["ID"].ToString(),
                //    item["POSITION_TITLE"].ToString(),
                //    item["EMAIL_AGM"].ToString(),
                //    retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                //    "https://teams.microsoft.com/L/Chat/0/0?users=" + item["EMAIL_AGM"].ToString()
                //    );

                //row = newDataTable.NewRow();
                //row["EXTENSION"] = "CustName";
                //newDataTable.Rows.Add(newDataTable);

            }


            if (newDataTable.Rows.Count == 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: '¡Alerta!', text:'No hemos encontrado resultados.', type: ''});", true);
            else
            {

                lblTotal.Text = newDataTable.Rows.Count.ToString();
                gvResultados.DataSource = newDataTable;
                Session["gvResultados"] = newDataTable;
                gvResultados.DataBind();

            }



        }
        catch (Exception ex)
        {

            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: 'Error', text:'" + error + "', type: ''});", true);
        }
        finally
        {
            
            conn.Close();

        }



    }
    private void loadCrigView()
    {
        try
        {

            string id = "%", scriptSearch = "";
            gvResultados.DataSource = null;
            gvResultados.DataBind();

            /*if (txtNombre.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: '¡Alerta!', text:'El nombre es requerido.', type: ''});", true);
                return;
            }
            else if (txtNombre.Text.Contains("-") == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: '¡Alerta!', text:'Seleccione el nombre en la lista.', type: ''});", true);
                return;
            }*/

            if (txtPuesto.Text == "")
                txtPuesto.Text = "%";
            if (txtOficina.Text == "")
                txtOficina.Text = "%";
            if (txtNombre.Text == "")
                txtNombre.Text = "%";

            /*if (txtNombre.Text != "" && txtNombre.Text.Contains("-") == true)
            {
                string[] ID = txtNombre.Text.Split('-');
                id = ID[0].ToString();
                scriptSearch = "AND UPPER(TRIM(SPRIDEN_ID)) LIKE TRIM(UPPER('" + id.Trim() + @"%'))";
            }
            else
            {
                scriptSearch = "AND UPPER(TRIM(REPLACE((SPRIDEN_FIRST_NAME || ' ' || SPRIDEN_MI || ' ' || SPRIDEN_LAST_NAME),' ', ' '))) LIKE UPPER(TRIM('%" + txtNombre.Text + @"%'))";
            }*/

            string script = @"SELECT DISTINCT
                                           CASE
                                                  CASE
                                                     WHEN NBRJLBD_PERCENT = 50 THEN SUBSTR(NBRJOBS_ORGN_CODE_TS, 1, 1)
                                                     WHEN SUBSTR(NBRJLBD_FUND_CODE, 1, 1) = '2' THEN SUBSTR(NVL(NBRJLBD_FUND_CODE, '*'), 3, 1)
                                                  ELSE SUBSTR(PEBEMPL_ORGN_CODE_HOME, 1, 1)
                                                  END
                                              WHEN 'A' THEN 'Administración Central'
                                              WHEN 'E' THEN 'Recinto de Carolina'
                                              WHEN 'M' THEN 'Recinto de Cupey'
                                              WHEN 'T' THEN 'Recinto de Gurabo'
                                              WHEN 'V' THEN 'Recinto Online'
                                             --WHEN 'C' THEN 'Sistema TV'
                                              WHEN 'O' THEN 'Arecibo Observatory'
                                           ELSE 'UAGM'
                                           END AS INSTITUTION,
                                    --
                                    --      NVL(YTVORGN_TITLE, '') AS OFFICE,
                                          CASE UPPER(YTVORGN_TITLE)
                                                  WHEN 'TO BE ASSIGNED' THEN
                                                             (SELECT FTVORGN_TITLE
                                                            FROM FTVORGN A
                                                           WHERE A.FTVORGN_ORGN_CODE = PEBEMPL_ORGN_CODE_HOME
                                                             AND A.FTVORGN_COAS_CODE = '4'
                                                             AND A.FTVORGN_STATUS_IND = 'A'
                                                             AND A.FTVORGN_EFF_DATE = (SELECT MAX(B.FTVORGN_EFF_DATE)
                                                                                         FROM FTVORGN B
                                                                                        WHERE B.FTVORGN_COAS_CODE = A.FTVORGN_COAS_CODE
                                                                                          AND B.FTVORGN_ORGN_CODE = A.FTVORGN_ORGN_CODE))
                                           ELSE NVL(YTVORGN_TITLE, '')
                                            END AS OFFICE, --- Oficina
                                           SPRIDEN_ID AS ID,
                                           INITCAP(TRIM(SPRIDEN_FIRST_NAME ||' '|| SPRIDEN_LAST_NAME)) AS EMPLOYEE_NAME,
                                           INITCAP(TRIM(SPRIDEN_FIRST_NAME ||' '|| REPLACE(SPRIDEN_MI, '.')) ||' '|| SPRIDEN_LAST_NAME) AS NOMBRE_COMPLETO,
                                           NVL(TRIM(NTVPOSN_TITLE), NBBPOSN_TITLE) AS POSITION_TITLE,
                                           --
--                                           NVL((SELECT LOWER(GWRUSER_EMAIL_ADDR)
--                                                  FROM GWRUSER
--                                                 WHERE GWRUSER_ID = SPRIDEN_ID
--                                                   AND GWRUSER_REC_TYPE = '3'
--                                                   AND NVL(GWRUSER_AD_IND,'N') = 'Y'),
                                           NVL((SELECT LOWER(GOREMAL_EMAIL_ADDRESS)
                                                  FROM GOREMAL
                                                 WHERE GOREMAL_PIDM = SPRIDEN_PIDM
                                                   AND GOREMAL_EMAL_CODE = 'AGM'
                                                   AND GOREMAL_STATUS_IND = 'A'
                                                   AND GOREMAL_PREFERRED_IND = 'Y'), '') AS EMAIL_AGM
                                      FROM PEBEMPL, NBRJOBS A, NBRJLBD B, NBBPOSN, NTVPOSN, SPRIDEN, FIMSMGR.YTVORGN D
                                     WHERE PEBEMPL_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
                                       AND A.NBRJOBS_PIDM = PEBEMPL_PIDM
                                       AND A.NBRJOBS_STATUS <> 'T'
                                       AND A.NBRJOBS_SUFF = '00'
                                       AND A.NBRJOBS_ECLS_CODE NOT IN ('PT', 'WE', 'WM', 'WR', 'WS', 'WT', 'WU')
                                       AND A.NBRJOBS_EFFECTIVE_DATE = (SELECT MAX(C.NBRJOBS_EFFECTIVE_DATE)
                                                                         FROM NBRJOBS C
                                                                        WHERE C.NBRJOBS_PIDM = A.NBRJOBS_PIDM
                                                                          AND C.NBRJOBS_POSN = A.NBRJOBS_POSN
                                                                          AND C.NBRJOBS_SUFF = A.NBRJOBS_SUFF
                                                                          AND C.NBRJOBS_EFFECTIVE_DATE <= SYSDATE)
                                       AND EXISTS (SELECT 'X'
                                                     FROM NBRBJOB
                                                    WHERE NBRBJOB_PIDM = A.NBRJOBS_PIDM
                                                      AND NBRBJOB_POSN = A.NBRJOBS_POSN
                                                      AND NBRBJOB_SUFF = A.NBRJOBS_SUFF
                                                      AND NBRBJOB_CONTRACT_TYPE = 'P'
                                                      AND (NBRBJOB_END_DATE IS NULL
                                                        OR NBRBJOB_END_DATE > SYSDATE))
                                    --
                                       AND NBRJLBD_PIDM = A.NBRJOBS_PIDM
                                       AND NBRJLBD_POSN = A.NBRJOBS_POSN
                                       AND NBRJLBD_SUFF = A.NBRJOBS_SUFF
                                       AND NBRJLBD_EFFECTIVE_DATE = (SELECT MAX(D.NBRJLBD_EFFECTIVE_DATE)
                                                                       FROM POSNCTL.NBRJLBD D
                                                                      WHERE D.NBRJLBD_PIDM = A.NBRJOBS_PIDM
                                                                        AND D.NBRJLBD_POSN = A.NBRJOBS_POSN
                                                                        AND D.NBRJLBD_SUFF = A.NBRJOBS_SUFF)
                                       AND NBRJLBD_PERCENT = (SELECT MAX(D.NBRJLBD_PERCENT)
                                                                FROM POSNCTL.NBRJLBD D
                                                               WHERE D.NBRJLBD_PIDM = A.NBRJOBS_PIDM
                                                                 AND D.NBRJLBD_POSN = A.NBRJOBS_POSN
                                                                 AND D.NBRJLBD_SUFF = A.NBRJOBS_SUFF
                                                                 AND D.NBRJLBD_EFFECTIVE_DATE = B.NBRJLBD_EFFECTIVE_DATE)
                                                       AND SPRIDEN_PIDM = PEBEMPL_PIDM
                                                       AND SPRIDEN_CHANGE_IND IS NULL
                                                       AND UPPER(TRIM(REPLACE((SPRIDEN_FIRST_NAME || ' ' || SPRIDEN_MI || ' ' || SPRIDEN_LAST_NAME),' ', ' '))) LIKE UPPER(TRIM('%')) --('%" + txtNombre.Text + @"%'))
                                                       AND NBBPOSN_POSN = A.NBRJOBS_POSN
                                                       AND NTVPOSN_POSN(+) = A.NBRJOBS_POSN
                                                       AND YTVORGN_ORGN_CODE(+) = PEBEMPL_ORGN_CODE_HOME
                                                       AND CASE
                                                            WHEN NBRJLBD_PERCENT = 50 THEN SUBSTR(NBRJOBS_ORGN_CODE_TS, 1, 1)
                                                            WHEN SUBSTR(NBRJLBD_FUND_CODE, 1, 1) = '2' THEN SUBSTR(NVL(NBRJLBD_FUND_CODE, '*'), 3, 1)
                                                        ELSE SUBSTR(PEBEMPL_ORGN_CODE_HOME, 1, 1)
                                                       END  LIKE UPPER('%') --('" + ddlInstitucion.SelectedValue + @"')
                                                       AND UPPER(NVL(YTVORGN_TITLE, '%')) LIKE UPPER('%') --('" + txtOficina.Text + @"%')
                                                       --AND (UPPER(NBBPOSN_TITLE) LIKE UPPER('" + txtPuesto.Text + @"%') OR UPPER(NTVPOSN_TITLE) LIKE UPPER('" + txtPuesto.Text + @"%'))
                                                       AND (UPPER(NBBPOSN_TITLE) LIKE UPPER('%') OR UPPER(NTVPOSN_TITLE) LIKE UPPER('%'))
                                                     ORDER BY INSTITUTION, OFFICE, EMPLOYEE_NAME";
            DataTable dt = appC.retornarDT(script);


            //Tabla nueva con la extensión telefónica tomada de AD.
            DataTable newDataTable = new DataTable();
            DataRow row = newDataTable.NewRow();
            newDataTable.Columns.Add("INSTITUTION", typeof(String));
            newDataTable.Columns.Add("OFFICE", typeof(String));
            newDataTable.Columns.Add("EMPLOYEE_NAME", typeof(String));
            newDataTable.Columns.Add("NOMBRE_COMPLETO", typeof(String));
            newDataTable.Columns.Add("ID", typeof(String));
            newDataTable.Columns.Add("POSITION_TITLE", typeof(String));
            newDataTable.Columns.Add("EMAIL_AGM", typeof(String));
            newDataTable.Columns.Add("EXTENSION", typeof(String));
            newDataTable.Columns.Add("TEAMS", typeof(String));

            foreach (DataRow item in dt.Rows)
            {
                if (item["INSTITUTION"].ToString().Equals("Administracion Central"))
                {


                    if (item["NOMBRE_COMPLETO"].ToString().Equals("Rafael A Nadal Arcelay") || item["NOMBRE_COMPLETO"].ToString().Equals("Gino Q Natalicchio") || item["NOMBRE_COMPLETO"].ToString().Equals("Jose F Mendez Mendez"))
                    {


                        newDataTable.Rows.Add(item["INSTITUTION"].ToString().Replace(item["INSTITUTION"].ToString(), "Administración Central"),
                                        item["OFFICE"].ToString(),
                                        item["NOMBRE_COMPLETO"].ToString(),
                                        item["EMPLOYEE_NAME"].ToString(),
                                        item["ID"].ToString(),
                                        item["POSITION_TITLE"].ToString(),
                                        item["EMAIL_AGM"].ToString()

                                        //retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                                        //"https://teams.microsoft.com/L/Chat/0/0?users=" + item["EMAIL_AGM"].ToString()

                                        //"" + ""




                                        );


                    }
                    else
                    {
                        newDataTable.Rows.Add(item["INSTITUTION"].ToString().Replace(item["INSTITUTION"].ToString(), "Administración Central"),
                                       item["OFFICE"].ToString(),
                                       item["NOMBRE_COMPLETO"].ToString(),
                                       item["EMPLOYEE_NAME"].ToString(),
                                       item["ID"].ToString(),
                                       item["POSITION_TITLE"].ToString(),
                                       item["EMAIL_AGM"].ToString(),
                                       //retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                                       retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                                       "https://teams.microsoft.com/L/Chat/0/0?users=" + item["EMAIL_AGM"].ToString()
                                       //"" + ""
                                       );
                    }

                }
                else
                {
                    newDataTable.Rows.Add(
                                        item["INSTITUTION"].ToString(),
                                        item["OFFICE"].ToString(),
                                        item["NOMBRE_COMPLETO"].ToString(),
                                        item["EMPLOYEE_NAME"].ToString(),
                                        item["ID"].ToString(),
                                        item["POSITION_TITLE"].ToString(),
                                        item["EMAIL_AGM"].ToString(),
                                        retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                                        "https://teams.microsoft.com/L/Chat/0/0?users=" + item["EMAIL_AGM"].ToString()
                                        );
                }
                //row["NombreColumna"].ToString();
                //newDataTable.Rows.Add(
                //    item["INSTITUTION"].ToString(),
                //    item["OFFICE"].ToString(),
                //    item["NOMBRE_COMPLETO"].ToString(),
                //    item["EMPLOYEE_NAME"].ToString(),
                //    item["ID"].ToString(),
                //    item["POSITION_TITLE"].ToString(),
                //    item["EMAIL_AGM"].ToString(),
                //    retornarExtensionAD(item["EMAIL_AGM"].ToString()),
                //    "https://teams.microsoft.com/L/Chat/0/0?users=" + item["EMAIL_AGM"].ToString()
                //    );

                //row = newDataTable.NewRow();
                //row["EXTENSION"] = "CustName";
                //newDataTable.Rows.Add(newDataTable);

            }


            if (newDataTable.Rows.Count == 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: '¡Alerta!', text:'No hemos encontrado resultados.', type: ''});", true);
            else
            {

                lblTotal.Text = newDataTable.Rows.Count.ToString();
                gvResultados.DataSource = newDataTable;
                Session["gvResultados"] = newDataTable;
                gvResultados.DataBind();

            }
        }
        catch (Exception ex)
        {
            //appC.SendException(ex.Message + " | " + ex.Source + " | " + ex.InnerException, "CadenaDeConexion()", Session["USUARIO"].ToString(), URL);
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: 'Error', text:'" + error + "', type: ''});", true);
        }
    }
    private string retornarExtensionAD(string email)
    {
        try
        {
            string[] username = email.Split('@');
            DirectoryEntry ConnectionAD = new DirectoryEntry("LDAP://agm.su", "directory_ldap", "$%H8x7wQ*W100#", AuthenticationTypes.Secure);
            DirectorySearcher search = new DirectorySearcher(ConnectionAD);
            search.Filter = String.Format("(SAMAccountName={0})", username[0].ToString());
            search.PropertiesToLoad.Add("telephoneNumber");
            SearchResult result = search.FindOne();

            //Obtener atributos
            //if (result.Properties["telephoneNumber"].Count > 0)
            //    return result.Properties["telephoneNumber"][0].ToString();
            //else return "";
            return "";


        }
        catch (Exception ex)
        {
            //appC.SendException(ex.Message + " | " + ex.Source + " | " + ex.InnerException, "CadenaDeConexion()", Session["USUARIO"].ToString(), URL);
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({title: 'Error', text:'" + error + "', type: ''});", true);
            return "";
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        loadCrigView();
    }




    protected void gvResultados_Sorting(object sender, GridViewSortEventArgs e)
    {
        string institution = "INSTITUTION";
        string position = "POSITION_TITLE";
        string office = "OFFICE";
        string employee = "NOMBRE_COMPLETO";
        string employeeName = "EMPLOYEE_NAME";
        string email = "EMAIL_AGM";
        string extension = "EXTENSION";

        DataTable dataTable = Session["gvResultados"] as DataTable;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            dataView.RowFilter = string.Format("{0} LIKE '%{7}%' OR {1} LIKE '%{7}%' OR {2} LIKE '%{7}%' OR {3} LIKE '%{7}%' OR {4} LIKE '%{7}%' OR {5} LIKE '%{7}%' OR {6} LIKE '%{7}%'", institution, position, office, employeeName, employee, email, extension, myInputSearch.Text);

            dataView.Sort = e.SortExpression + " " + ConvertSort(e.SortDirection);
            gvResultados.DataSource = dataView;
            gvResultados.DataBind();
        }


    }

    private string ConvertSort(System.Web.UI.WebControls.SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        string Sorting = Session["Sortinggridview"] as string;
        switch (Sorting)
        {
            case "ASC":
                newSortDirection = "DESC";
                Session["Sortinggridview"] = "DESC";
                break;

            case "DESC":
                newSortDirection = "ASC";
                Session["Sortinggridview"] = "ASC";
                break;
            default:
                Session["Sortinggridview"] = "ASC";
                newSortDirection = "ASC";
                break;
        }
        return newSortDirection;
    }


    protected void inputSearch_TextChanged(object sender, EventArgs e)
    {


        string institution = "INSTITUTION";
        string position = "POSITION_TITLE";
        string office = "OFFICE";
        string employee = "NOMBRE_COMPLETO";
        string employeeName = "EMPLOYEE_NAME";
        string email = "EMAIL_AGM";
        string extension = "EXTENSION";


        try
        {

            DataTable dt = (DataTable)Session["gvResultados"];

            if (dt == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", @"swal({title: '',
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
            else
            {
                dt.DefaultView.RowFilter = string.Format("{0} LIKE '%{7}%' OR {1} LIKE '%{7}%' OR {2} LIKE '%{7}%' OR {3} LIKE '%{7}%' OR {4} LIKE '%{7}%' OR {5} LIKE '%{7}%' OR {6} LIKE '%{7}%'", institution, position, office, employeeName, employee, email, extension, myInputSearch.Text);

                gvResultados.DataSource = dt;
                Session["gvResultados"] = null;
                Session["gvResultados"] = dt;
                gvResultados.DataBind();
            }






        }
        catch (Exception exx)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", @"swal({title: ,
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


    protected void rdlFacultad_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["gvResultados"] = null;
        gridViewWithFilter();
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
                lblUsername.Text = "<b>Usuario: </b>" + (string)(Session["USERNAME"]) as string;
                lblConnectionName.Text = "<b>Conexión: </b>" + rdr["INSTANCIA"].ToString().ToUpper();
                lblRecinto.Text = "<b>Recinto o localidad: </b> Administración Central";
                lblFechaModificado.Text = "<b>Actualización: </b>" + "03/24/2022";
                if (acceso != null)
                    lblTipoAcceso.Text = "<b>Acceso: </b>" + acceso.Replace("FULL", "Super Admin").Replace("NULL", "Administrador");
                else
                    lblTipoAcceso.Text = "<b>Acceso: </b>Usuario Regular";


            }
        }
        catch (Exception ex)
        {
            //appC.SendException(ex.Message + " | " + ex.Source + " | " + ex.InnerException, "ConnectionName()", Session["USERNAME"].ToString(), URL);
            string error = ex.Message.Replace("\n", ". ").Replace("'", "").Replace(".\r", "");
            if (error.Contains("Object reference not set") || error.Contains("ConnectionString"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", @"swal({title: 'Error: #02',
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
        }

        finally
        {
            conn.Close();
        }
    }
    protected void btnToogleMenu_Click(object sender, EventArgs e)
    {
        ltlToogleMenu.Text = "";
        if (pnlSideBar.Visible == false)
        {
            pnlSideBar.Visible = true;
            btnToogleMenu.Text = "menu_open";
        }
        else
        {
            pnlSideBar.Visible = false;
            btnToogleMenu.Text = "menu";
            ltlToogleMenu.Text = "<style>.main-panel{width: 100%;}</style>";
        }
    }
}