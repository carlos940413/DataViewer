﻿using System;
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
		private Team()
		{
			_ID = 0;
			TeamName = "";
			LegalRepresentative = "";
			Address = "";
		}

        /// <summary>
        /// 创建施工队
        /// </summary>
        /// <param name="teamLevel">施工队资质等级</param>
        /// <param name="teamType">施工队分包类型</param>
        /// <returns>创建成功，返回新对象，ID为0，施工队名称为空。创建不成功，返回Null</returns>
		public static Team CreateTeam(TeamLevel teamLevel, TeamType teamType)
		{
			if (teamLevel != null && teamType != null && teamLevel.ID != 0 && teamType.ID != 0)
				return new Team() { TeamType = teamType, TeamLevel = teamLevel };
			return null;
		}

        #region Properties
        /// <summary>
        /// 施工队ID
        /// </summary>
        private int _ID;
		public int ID
		{
			get { return _ID; }
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

        /// <summary>
        /// 法人代表
        /// </summary>
		private string _LegalRepresentative;
		public string LegalRepresentative
		{
			get { return _LegalRepresentative; }
			set { _LegalRepresentative = value; }
		}

        /// <summary>
        /// 施工单位地址
        /// </summary>
		private string _Address;
		public string Address
		{
			get { return _Address; }
			set { _Address = value; }
		}

        /// <summary>
        /// 资质等级
        /// </summary>
		private TeamLevel _TeamLevel;
		public TeamLevel TeamLevel
		{
			get { return _TeamLevel; }
			set { _TeamLevel = value; }
		}

        /// <summary>
        /// 分包类型
        /// </summary>
		private TeamType _TeamType;
		public TeamType TeamType
		{
			get { return _TeamType; }
			set { _TeamType = value; }
		}
        #endregion

		public void Save()
		{
			if (ID == 0)
				_ID = DBHelper.InsertCommand("Team_Insert", CommandType.StoredProcedure,
					new SqlParameter("@teamname", TeamName),
					new SqlParameter("@legalrepresentative", LegalRepresentative),
					new SqlParameter("@address", Address),
					new SqlParameter("@teamlevelid", TeamLevel.ID),
					new SqlParameter("@teamtypeid", TeamType.ID));
			else
				DBHelper.UpdateDeleteCommand("Team_Update", CommandType.StoredProcedure,
					new SqlParameter("@id", ID),
					new SqlParameter("@teamname", TeamName),
					new SqlParameter("@legalrepresentative", LegalRepresentative),
					new SqlParameter("@address", Address),
					new SqlParameter("@teamlevelid", TeamLevel.ID),
					new SqlParameter("@teamtypeid", TeamType.ID));
		}

		public override bool Equals(object obj)
		{
			Team team = obj as Team;
			if (team == null || team.ID != this.ID)
				return false;
			else
				return true;
		}

		private static List<Team> toList(DataTable dt)
		{
			List<Team> result = new List<Team>();
			foreach (DataRow row in dt.Rows)
			{
				Team team = new Team();
				team._ID = Int32.Parse(row["id"].ToString());
				team.TeamName = row["teamname"].ToString();
				team.LegalRepresentative = row["legalrepresentative"].ToString();
				team.Address = row["address"].ToString();
				team.TeamLevel = TeamLevel.Get_ByID(Int32.Parse(row["teamlevelid"].ToString()));
				team.TeamType = TeamType.Get_ByID(Int32.Parse(row["teamtypeid"].ToString()));
				result.Add(team);
			}
			return result;
		}

		/// <summary>
		/// 根据ID查找施工队信息
		/// </summary>
		/// <param name="id">施工队ID</param>
		/// <returns>返回待查找的施工队, 如果未找到, 返回Null</returns>
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

        /// <summary>
        /// 通过模糊查询施工队名称获取施工队
        /// </summary>
        /// <param name="teamName">要查询的字符串</param>
        /// <returns></returns>
		public static List<Team> Get_ByFuzzyTeamName(string teamName)
		{
			return toList(DBHelper.SelectCommand("Team_teamnameFuzzy", CommandType.StoredProcedure,
				new SqlParameter("@teamname", teamName)));
		}
	}
}
