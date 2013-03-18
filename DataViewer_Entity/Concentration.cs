using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
    public class Concentration : IEntity
    {
        /// <summary>
        /// 向数据库提交采集的数据
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="acquireOn">采集时间</param>
        /// <param name="concentration">粉尘浓度</param>
        public static void SubmitConcentration(Node node, DateTime acquireOn, double concentration)
        {
            if (node.ID == 0 || acquireOn == DateTime.MinValue)
                return;
            Concentration result = new Concentration();
            result.Node = node;
            result.AcquireOn = acquireOn;
            result.Amount = concentration;
            result.Save();
        }

        private Concentration() { }

        /// <summary>
        /// 采集该数据的节点
        /// </summary>
        private Node _Node;
        public Node Node
        {
            get { return _Node; }
            set { _Node = value; }
        }

        /// <summary>
        /// 采集时间
        /// </summary>
        private DateTime _AcquireOn;
        public DateTime AcquireOn
        {
            get { return _AcquireOn; }
            set { _AcquireOn = value; }
        }

        /// <summary>
        /// 粉尘浓度
        /// </summary>
        private double _Amount;
        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        public void Save()
        {
            DBHelper.UpdateCommand("Concentration_Insert", CommandType.StoredProcedure,
                new SqlParameter("@nodeid", Node.ID),
                new SqlParameter("@acquireon", AcquireOn),
                new SqlParameter("@amount", Amount));
        }
    }
}
