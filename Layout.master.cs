using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.IO;

public partial class Layout : System.Web.UI.MasterPage
{
    public OracleConnection conn = new OracleConnection();
    private MainClass appC = new MainClass();
    //Saber el nombre del URL en la que estamos
    string URL = HttpContext.Current.Request.Url.AbsoluteUri;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
                CadenaDeConexion();
        }
    }
                                                               
    private void CadenaDeConexion()
    {

    }
}
