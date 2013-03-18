using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
    public class Company : IEntity
    {
        public Company()
        {
            ID = 0;
            CompanyName = "";
        }

        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>
        /// 企业名称
        /// </summary>
        private string _CompanyName;
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        public void Save()
        {
            if (ID == 0)
                ID = DBHelper.InsertCommand("Company_Insert", CommandType.StoredProcedure,
                    new SqlParameter("@companyname", CompanyName));
            else
                DBHelper.UpdateCommand("Company_Update", CommandType.StoredProcedure,
                    new SqlParameter("@id", ID),
                    new SqlParameter("@companyname", CompanyName));
        }

        private static List<Company> toList(DataTable dt)
        {
            List<Company> result = new List<Company>();
            foreach (DataRow row in dt.Rows)
            {
                Company company = new Company();
                company.ID = Int32.Parse(row["id"].ToString());
                company.CompanyName = row["companyname"].ToString();
                result.Add(company);
            }
            return result;
        }

        /// <summary>
        /// 通过ID查找企业
        /// </summary>
        /// <param name="id">所查找的企业ID</param>
        /// <returns>返回待查找的企业, 如果未找到, 返回ID为0的初始化对象</returns>
        public static Company Get_ByID(int id)
        {
            List<Company> temp = toList(DBHelper.SelectCommand("Company_id", CommandType.StoredProcedure,
                new SqlParameter("@id", id)));
            if (temp.Count != 0)
                return temp[0];
            return new Company();
        }

        /// <summary>
        /// 获取所有的企业机构
        /// </summary>
        /// <returns>如果没有企业, 返回count为0的List</returns>
        public static List<Company> Get_All()
        {
            return toList(DBHelper.SelectCommand("Company_all", CommandType.StoredProcedure));
        }
    }
}
