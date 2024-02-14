using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;
using System.Web;

//! REMEMBER: CAMBIAR TODOS LOS CATCH A SUS ESTADOS NORMALES
public partial class _Default : System.Web.UI.Page
{
    public OracleConnection conn = new OracleConnection();
    private MainClass appC = new MainClass();
    private DataTable dt = new DataTable();
    private Encoding enc = new UTF8Encoding(true, true);
    private string HostName = HttpContext.Current.Request.Url.Host;
    private OracleTransaction trans = null;

    protected void btnPDF_Click(object sender, EventArgs e)
    {
    }

    protected void gvResultados_Sorting(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Corre al inicio + en refresh solamente
            //Frontloading data
            Application["Sortinggridview"] = (Application["Sortinggridview"] == null) ? "Subject ASC,Titulo ASC,COLLEGE ASC,Sesión ASC,Salón ASC,CRN ASC,Créditos ASC,Entrada ASC,DATE ASC,Salida ASC,ScheduleType ASC,Nivel ASC" : Application["Sortinggridview"];
            gvResultados.DataSource = Application["gvResultados"] as DataTable;
            filterTable.DataSource = Application["Search"] as DataTable;

            gvResultados.DataBind();
            filterTable.DataBind();
            dateHeader.InnerText = DateTime.Now.ToString();
        }
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void sidebarSearch_TextChanged(object sender, EventArgs e)
    {
    }
}