using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataViewer_Entity;

namespace DataGenerator
{
	class Program
	{
		private static Random rand = new Random();

		private static string RandomString(int length)
		{
			List<char> letters = new List<char>();
			for (char letter = 'a'; letter <= 'z'; letter++)
				letters.Add(letter);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < length; i++)
				sb.Append(letters[rand.Next(26)]);
			return sb.ToString();
		}

		private static string RandomPhone()
		{
			List<char> numbers = new List<char>();
			for (char letter = '0'; letter <= '9'; letter++)
				numbers.Add(letter);
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < 11; i++)
			{
				builder.Append(numbers[rand.Next(10)]);
			}
			return numbers.ToString();
		}

		public static void GenerateTeamLevel(int count)
		{
			for (int i = 0; i < count; i++)
			{
				TeamLevel teamLevel = new TeamLevel();
				teamLevel.LevelName = RandomString(10);
				teamLevel.Save();
			}
		}

		public static void GenerateTeamType(int count)
		{
			for (int i = 0; i < count; i++)
			{
				TeamType teamType = new TeamType();
				teamType.TypeName = RandomString(10);
				teamType.Save();
			}
		}

		public static void GenerateTeam(int count)
		{
			List<TeamLevel> teamLevels = TeamLevel.Get_All();
			List<TeamType> teamTypes = TeamType.Get_All();

			for (int i = 0; i < count; i++)
			{
				Team t = Team.CreateTeam(teamLevels[rand.Next(teamLevels.Count)], teamTypes[rand.Next(teamTypes.Count)]);
				t.Address = RandomString(200);
				t.LegalRepresentative = RandomString(10);
				t.TeamName = RandomString(20);
				t.Save();
			}
		}

		public static void GenerateCompany(int count)
		{
			for (int i = 0; i < count; i++)
			{
				Company c = new Company();
				c.CompanyName = RandomString(20);
				c.Save();
			}
		}

		public static void GenerateRegion(int count)
		{
			for (int i = 0; i < count; i++)
			{
				Region r = new Region();
				r.RegionName = RandomString(10);
				r.Save();
			}
		}

		public static void GenerateSupervisionDepartment(int count)
		{
			for (int i = 0; i < count; i++)
			{
				SupervisionDepartment sd = new SupervisionDepartment();
				sd.DepartmentName = RandomString(10);
				sd.PhoneNumber = RandomPhone();
				sd.Save();
			}
		}

		public static void GenerateProject(int count)
		{
			List<Company> companies = Company.Get_All();
			List<Region> regions = Region.Get_All();

			List<Team> teams = Team.Get_All();
			int teamCount = rand.Next(2, 4);
			Dictionary<Team, DutyOfficer> teamInformation = new Dictionary<Team, DutyOfficer>();
			for (int i = 0; i < teamCount; i++)
			{
				int index = rand.Next(teams.Count);
				teamInformation.Add(teams[index], new DutyOfficer() { PersonName = RandomString(10), PhoneNumber = RandomPhone() });
				teams.RemoveAt(index);
			}

			List<SupervisionDepartment> supervisionDepartments = SupervisionDepartment.Get_All();
			int supervisionDepartmentStartIndex = rand.Next(supervisionDepartments.Count);
			for (int i = 0; i < count; i++)
			{
				Project p = Project.CreateProject(
					companies[rand.Next(companies.Count)],
					regions[rand.Next(regions.Count)],
					teamInformation,
					supervisionDepartments.GetRange(supervisionDepartmentStartIndex, rand.Next(1, supervisionDepartments.Count - supervisionDepartmentStartIndex + 1)));
				p.ProjectName = RandomString(15);
				p.DutyOfficer.PersonName = RandomString(10);
				p.DutyOfficer.PhoneNumber = RandomPhone();
				p.StartOn = new DateTime(rand.Next(2000, 2013), rand.Next(1, 13), rand.Next(1, 29));
				p.EndOn_Plan = p.StartOn.AddDays(rand.Next(100, 1000));
				p.Location_East = (double)rand.Next(1183667, 1192333) / 10000;
				p.Location_North = (double)rand.Next(312333, 326167) / 10000;
				p.Node_Phone = RandomPhone();
				p.Save();
			}
		}

		public static void GenerateArea(int lowerCount, int upperCount)
		{
			List<Project> projects = Project.Get_All();
			foreach (Project project in projects)
			{
				int areaCount = rand.Next(lowerCount, upperCount + 1);
				for (int i = 0; i < areaCount; i++)
				{
					Area area = Area.CreateArea(project);
					area.AreaName = RandomString(10);
					area.Save();
				}
			}
		}

		public static void GenerateNode(int lowerCount, int upperCount)
		{
			List<Area> areas = Area.Get_All();
			foreach (Area area in areas)
			{
				int actualCount = rand.Next(lowerCount, upperCount + 1);
				for (int i = 0; i < actualCount; i++)
				{
					Node node = Node.CreateNode(area);
					node.HardwareID = i;
					node.Description = RandomString(200);
					node.Save();
				}
			}
		}

		public static void GenerateConcentration(int countPerNode)
		{
			List<Area> areas = Area.Get_All();
			foreach (Area area in areas)
			{
				List<Node> nodes = Node.Get_ByAreaID(area.ID);
				DateTime acquireTime = area.Project.StartOn.AddHours(rand.Next(23)).AddMinutes(rand.Next(60)).AddSeconds(rand.Next(60));
				for (int i = 0; i < countPerNode; i++)
				{
					acquireTime = acquireTime.AddMinutes(30);
					foreach (Node node in nodes)
					{
						Concentration.SubmitConcentration(node, acquireTime, rand.Next(200, 800));
					}
				}
			}
		}

		public static void GenerateAdministrator()
		{
			Administrator admin = new Administrator();
			admin.Username = Console.ReadLine();
			admin.Password = Console.ReadLine();
			admin.Save();
		}

		static void Main(string[] args)
		{
			int choice = 0;
			while (choice != 12)
			{
				#region Print Menu
				Console.WriteLine("1. 3 Companies");
				Console.WriteLine("2. 5 TeamTypes");
				Console.WriteLine("3. 5 TeamLevels");
				Console.WriteLine("4. 3 Teams");
				Console.WriteLine("5. 10 Regions");
				Console.WriteLine("6. 7 SupervisionDepartments");
				Console.WriteLine("7. 5 Projects");
				Console.WriteLine("8. 1 to 3 Areas");
				Console.WriteLine("9. 4 or 5 Nodes");
				Console.WriteLine("10. 50 Concentrations");
				Console.WriteLine("11. Add Administrator");
				Console.WriteLine("12. exit");
				#endregion
				choice = Int32.Parse(Console.ReadLine());
				switch (choice)
				{
					case 1:
						GenerateCompany(3);
						break;
					case 2:
						GenerateTeamType(5);
						break;
					case 3:
						GenerateTeamLevel(5);
						break;
					case 4:
						GenerateTeam(3);
						break;
					case 5:
						GenerateRegion(10);
						break;
					case 6:
						GenerateSupervisionDepartment(7);
						break;
					case 7:
						GenerateProject(5);
						break;
					case 8:
						GenerateArea(1, 3);
						break;
					case 9:
						GenerateNode(4, 5);
						break;
					case 10:
						GenerateConcentration(50);
						break;
					case 11:
						GenerateAdministrator();
						break;
					default:
						break;
				}
			}
		}
	}
}
