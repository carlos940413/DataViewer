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
        private static string RandomString(int length)
        {
            List<char> letters = new List<char>();
            for (char letter = 'a'; letter <= 'z'; letter++)
                letters.Add(letter);
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < length; i++)
                sb.Append(letters[rand.Next(26)]);
            return sb.ToString();
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

        public static void GenerateTeam(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Team t = new Team();
                t.TeamName = RandomString(20);
                t.Save();
            }
        }

        public static void GenerateProject(int count)
        {
            List<Team> teams = Team.Get_All();
            List<Company> companies = Company.Get_All();
            Random rand=new Random();
            for (int i = 0; i < count; i++)
            {
                Project p = new Project();
                p.ProjectName = RandomString(15);
                p.Team = teams[rand.Next(teams.Count)];
                p.Company = companies[rand.Next(companies.Count)];
                p.StartOn = new DateTime(rand.Next(2000, 2013), rand.Next(1, 13), rand.Next(1, 29));
                p.EndOn_Plan = p.StartOn.AddDays(rand.Next(100, 1000));
                p.Location_East = (double)rand.Next(1183667, 1192333) / 10000;
                p.Location_North = (double)rand.Next(312333, 326167) / 10000;
                p.Save();
            }
        }

        static void Main(string[] args)
        {
            //GenerateCompany(3);
            //GenerateTeam(3);
            GenerateProject(5);
        }
    }
}
