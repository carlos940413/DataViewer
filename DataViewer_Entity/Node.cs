using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
    public class Node : IEntity
    {
        public Node()
        {
            ID = 0;
            Project = null;
            Description = "";
        }

        #region Properties
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>
        /// 节点所属的施工工程
        /// </summary>
        private Project _Project;
        public Project Project
        {
            get { return _Project; }
            set { _Project = value; }
        }

        /// <summary>
        /// 节点的描述信息(限制为500个字符)
        /// </summary>
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        /// <summary>
        /// 标记节点的经度位置
        /// </summary>
        private double _Location_East;
        public double Location_East
        {
            get { return _Location_East; }
            set { _Location_East = value; }
        }

        /// <summary>
        /// 标记节点的纬度位置
        /// </summary>
        private double _Location_North;
        public double Location_North
        {
            get { return _Location_North; }
            set { _Location_North = value; }
        }
        #endregion

        public void Save()
        {
            if (ID == 0)
                ID = DBHelper.InsertCommand("Node_Insert", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", Project.ID),
                    new SqlParameter("@description", Description));
            else
                DBHelper.UpdateCommand("Node_Update", CommandType.StoredProcedure,
                    new SqlParameter("@id", ID),
                    new SqlParameter("@projectid", Project.ID),
                    new SqlParameter("@description", Description));
        }

        private static List<Node> toList(DataTable dt)
        {
            List<Node> result = new List<Node>();
            foreach (DataRow row in dt.Rows)
            {
                Node node = new Node();
                node.ID = Int32.Parse(row["id"].ToString());
                node.Project = Project.Get_ByID(Int32.Parse(row["projectid"].ToString()));
                node.Description = row["description"].ToString();
                result.Add(node);
            }
            return result;
        }

        public static List<Node> Get_All()
        {
            return toList(DBHelper.SelectCommand("Node_all", CommandType.StoredProcedure));
        }
    }
}
