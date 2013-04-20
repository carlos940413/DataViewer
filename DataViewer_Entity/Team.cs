using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
    public class Team : IEntity
    {
        public Team()
        {
            ID = 0;
            TeamName = "";
        }

        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>
        /// 施工队名称
        /// </summary>
        private string _TeamName;
        public string TeamName
        {
            get { return _TeamName; }
            set { _TeamName = value; }
        }

        public void Save()
        {
            if (ID == 0)
                ID = DBHelper.InsertCommand("Team_Insert", CommandType.StoredProcedure,
                    new SqlParameter("@teamname", TeamName));
            else
                DBHelper.UpdateDeleteCommand("Team_Update", CommandType.StoredProcedure,
                    new SqlParameter("@id", ID),
                    new SqlParameter("@teamname", TeamName));
        }

        private static List<Team> toList(DataTable dt)
        {
            List<Team> result = new List<Team>();
            foreach (DataRow row in dt.Rows)
            {
                Team team = new Team();
                team.ID = Int32.Parse(row["id"].ToString());
                team.TeamName = row["teamname"].ToString();
                result.Add(team);
            }
            return result;
        }

        /// <summary>
        /// 根据ID查找施工队信息
        /// </summary>
        /// <param name="id">施工队ID</param>
        /// <returns>返回待查找的施工队, 如果未找到, 返回ID为0的初始化对象</returns>
        public static Team Get_ByID(int id)
        {
            List<Team> temp = toList(DBHelper.SelectCommand("Team_id", CommandType.StoredProcedure,
                new SqlParameter("@id", id)));
            if (temp.Count != 0)
                return temp[0];
            return null;
        }

        /// <summary>
        /// 获取所有的施工队
        /// </summary>
        /// <returns>如果没有施工队, 返回count为0的List</returns>
        public static List<Team> Get_All()
        {
            return toList(DBHelper.SelectCommand("Team_all", CommandType.StoredProcedure));
        }
    }
}
