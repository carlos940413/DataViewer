using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web
{
    public partial class DataProcessingPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime acquireTime = DateTime.Now;
            int total = Int32.Parse(Request.Params["total"].ToString());
            for (int i = 1; i <= total; i++)
            {
                Concentration.SubmitConcentration(new Node() { ID = Int32.Parse(Request.Params.Keys[i].ToString()) }, acquireTime, Double.Parse(Request.Params[i].ToString()));
            }
        }
    }
}