using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
	public class SupervisionDepartment : IEntity
	{
		public SupervisionDepartment()
		{
			_ID = 0;
			DepartmentName = "";
			PhoneNumber = "";
		}

		private int _ID;
		public int ID
		{
			get { return _ID; }
		}

		private string _DepartmentName;
		public string DepartmentName
		{
			get { return _DepartmentName; }
			set { _DepartmentName = value; }
		}

		private string _PhoneNumber;
		public string PhoneNumber
		{
			get { return _PhoneNumber; }
			set { _PhoneNumber = value; }
		}

		public void Save()
		{
			if (ID == 0)
				_ID = DBHelper.InsertCommand("SupervisionDepartment_Insert", CommandType.StoredProcedure,
					new SqlParameter("@departnemtname", DepartmentName),
					new SqlParameter("@phonenumber", PhoneNumber));
			else
				DBHelper.UpdateDeleteCommand("SupervisionDepartment_Update", CommandType.StoredProcedure,
					new SqlParameter("@id", ID),
					new SqlParameter("@departmentname", DepartmentName),
					new SqlParameter("@phonenumber", PhoneNumber));
		}

		private static List<SupervisionDepartment> toList(DataTable dt)
		{
			List<SupervisionDepartment> result = new List<SupervisionDepartment>();
			foreach (DataRow row in dt.Rows)
			{
				SupervisionDepartment supervisionDepartment = new SupervisionDepartment();
				supervisionDepartment._ID = Int32.Parse(row["id"].ToString());
				supervisionDepartment.DepartmentName = row["departmentname"].ToString();
				supervisionDepartment.PhoneNumber = row["phonenumber"].ToString();
				result.Add(supervisionDepartment);
			}
			return result;
		}

		public static SupervisionDepartment Get_ByID(int id)
		{
			List<SupervisionDepartment> supervisionDepartments = toList(DBHelper.SelectCommand("SupervisionDepartment_id", CommandType.StoredProcedure,
				new SqlParameter("@id", id)));
			if (supervisionDepartments.Count == 0)
				return null;
			return supervisionDepartments[0];
		}
	}
}
