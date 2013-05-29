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
        #region Properties
        /// <summary>
        /// 管理员ID
        /// </summary>
        private int _ID;
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

        /// <summary>
        /// 管理员用户名
        /// </summary>
		private string _Username;
		public string Username
		{
			get { return _Username; }
			set { _Username = value; }
		}

        /// <summary>
        /// 管理员密码
        /// </summary>
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
        #endregion

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

        /// <summary>
        /// 通过用户名获取管理员
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>返回相应的管理员，若没有，返回空</returns>
		public static Administrator Get_ByUsername(string username)
		{
			List<Administrator> admins = toList(DBHelper.SelectCommand("Administrator_username", CommandType.StoredProcedure,
				new SqlParameter("@username", username)));
			if (admins.Count == 0)
				return null;
			return admins[0];
		}

        /// <summary>
        /// 判断两个管理员用户名与密码是否相同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>如果管理员用户名与密码相同，返回true，否则返回false</returns>
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
