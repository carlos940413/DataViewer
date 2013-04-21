using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web.ManagementPage
{
	public partial class CompanyPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void On_SearchButton_Click(object sender, EventArgs e)
		{
			List<Company> companies = Company.Get_ByFuzzyCompanyName(CompanyName_TextBox.Text);
			if (companies.Count == 0)
			{
				Help_Label.Text = "建设单位不存在, 请重新键入关键字!";
				Help_Label.Visible = true;
				Company_ListView.Visible = false;
			}
			else
			{
				Help_Label.Visible = false;
				Company_ListView.Visible = true;
				Company_ListView.DataSource = companies;
				Company_ListView.DataBind();
			}
		}
	}
}