using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
	public class Administrator : IEntity
	{
		public Administrator()
		{
			ID = 0;
			Username = "";
			Password = "";
		}

		private int _ID;
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private string _Username;
		public string Username
		{
			get { return _Username; }
			set { _Username = value; }
		}

		private string _Password;
		public string Password
		{
			get { return _Password; }
			set
			{
				byte[] input = Encoding.Default.GetBytes(value);
				byte[] output = (new MD5CryptoServiceProvider()).ComputeHash(input);
				_Password = BitConverter.ToString(output).Replace("-","");
			}
		}

		public void Save()
		{
			if (ID == 0)
				ID = DBHelper.InsertCommand("Administrator_Insert", CommandType.StoredProcedure,
					new SqlParameter("@username", Username),
					new SqlParameter("@password", Password));
			else
				DBHelper.UpdateDeleteCommand("Administrator_Update", CommandType.StoredProcedure,
					new SqlParameter("@id", ID),
					new SqlParameter("@username", Username),
					new SqlParameter("@password", Password));
		}

		private static List<Administrator> toList(DataTable dt)
		{
			List<Administrator> result = new List<Administrator>();
			foreach (DataRow row in dt.Rows)
			{
				Administrator admin = new Administrator();
				admin.ID = Int32.Parse(row["id"].ToString());
				admin.Username = row["username"].ToString();
				admin._Password = row["password"].ToString();
				result.Add(admin);
			}
			return result;
		}

		public static Administrator Get_ByUsername(string username)
		{
			List<Administrator> admins = toList(DBHelper.SelectCommand("Administrator_username", CommandType.StoredProcedure,
				new SqlParameter("@username", username)));
			if (admins.Count == 0)
				return null;
			return admins[0];
		}

		public override bool Equals(object obj)
		{
			Administrator admin = obj as Administrator;
			if (admin == null)
				return false;
			else if (admin.Username == this.Username && admin.Password == this.Password)
				return true;
			return false;
		}
	}
}
