using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web.CompanyPage
{
	public partial class CompanyDetailsPage : System.Web.UI.Page
	{
		protected Company company = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			int id;
			if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
			{
				if (Session["Administrator"] == null)
					ChangeCompany_HyperLink.Visible = false;
				company = Company.Get_ByID(id);
			}
			if (company == null)
				Response.Redirect("/CompanyPage/CompanyPage.aspx");
		}
	}
}