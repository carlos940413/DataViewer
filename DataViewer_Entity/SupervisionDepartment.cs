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

        #region Properties
        /// <summary>
        /// 监管部门ID
        /// </summary>
		private int _ID;
		public int ID
		{
			get { return _ID; }
		}

        /// <summary>
        /// 监管部门名称
        /// </summary>
		private string _DepartmentName;
		public string DepartmentName
		{
			get { return _DepartmentName; }
			set { _DepartmentName = value; }
		}

        /// <summary>
        /// 监管部门电话号码
        /// </summary>
		private string _PhoneNumber;
		public string PhoneNumber
		{
			get { return _PhoneNumber; }
			set { _PhoneNumber = value; }
		}
        #endregion

		public void Save()
		{
			if (ID == 0)
				_ID = DBHelper.InsertCommand("SupervisionDepartment_Insert", CommandType.StoredProcedure,
					new SqlParameter("@departmentname", DepartmentName),
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

        /// <summary>
        /// 根据ID获得监管部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public static SupervisionDepartment Get_ByID(int id)
		{
			List<SupervisionDepartment> supervisionDepartments = toList(DBHelper.SelectCommand("SupervisionDepartment_id", CommandType.StoredProcedure,
				new SqlParameter("@id", id)));
			if (supervisionDepartments.Count == 0)
				return null;
			return supervisionDepartments[0];
		}

        /// <summary>
        /// 获取所有监管部门
        /// </summary>
        /// <returns></returns>
		public static List<SupervisionDepartment> Get_All()
		{
			return toList(DBHelper.SelectCommand("SupervisionDepartment_all", CommandType.StoredProcedure));
		}
	}
}
