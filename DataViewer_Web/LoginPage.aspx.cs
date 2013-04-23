using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web
{
	public partial class LoginPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void On_LoginButton_Click(object sender, EventArgs e)
		{
			Administrator admin = new Administrator() { Username = Username_TextBox.Text, Password = Password_TextBox.Text };
			Administrator admin_check = Administrator.Get_ByUsername(Username_TextBox.Text);
			if (admin.Equals(admin_check))
			{
				Session["Administrator"] = admin_check;
				Response.Redirect("/Index.aspx");
			}
			else
			{
				Help_Label.Visible = true;
			}
		}
	}
}