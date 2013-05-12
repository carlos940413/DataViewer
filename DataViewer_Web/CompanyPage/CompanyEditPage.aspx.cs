using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web.CompanyPage
{
	public partial class CompanyEditPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				int id;
				Company company = null;
				if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
					company = Company.Get_ByID(id);
				if (company != null)
				{
					Back_HyperLink.NavigateUrl = "/CompanyPage/CompanyDetailsPage.aspx?id=" + company.ID;

					CompanyName_TextBox.Text = company.CompanyName;
					LegalRepresentative_TextBox.Text = company.LegalRepresentative;
					Address_TextBox.Text = company.Address;
				}
				else
				{
					Back_HyperLink.NavigateUrl = "/CompanyPage/CompanyPage.aspx";
				}
			}
		}

		protected void On_SubmitButton_Click(object sender, EventArgs e)
		{
			int id;
			Company company = null;
			if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
				company = Company.Get_ByID(id);
			if (company == null)
				company = new Company();
			company.CompanyName = CompanyName_TextBox.Text;
			company.Address = Address_TextBox.Text;
			company.LegalRepresentative = LegalRepresentative_TextBox.Text;
			company.Save();
			Response.Redirect("/CompanyPage/CompanyDetailsPage.aspx?id=" + company.ID);
		}
	}
}