using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FastMember;

namespace LearnDotNet
{
    class OrmTest: IRun
    {
        public Methods Methods { get; } = new Methods();
        
        private int iterations = 1000;

        public void Run(string method, params object[] parameters)
        {
            long time;
            switch (method)
            {
                case "GetEmployeesWithAdo":
                    time = 0;
                    for (int i = 0; i < iterations; i++)
                    {
                       time +=  GetEmployeesWithAdo();
                    }
                    Console.WriteLine("ADO net time taken:{0}", time/(iterations));
                    break;
                case "GetEmployeesWithEFramework":
                    time = 0;
                    for (int i = 0; i < iterations; i++)
                    {
                        time += GetEmployeesWithEFramework();
                    }
                    Console.WriteLine("Entity Framework:{0}", time / (iterations));
                    
                    // CompareEntityAdoDapper();
                    break;
                case "GetEmployeesWithDappper":
                    time = 0;
                    for (int i = 0; i < iterations; i++)
                    {
                        time += GetEmployeesWithDappper();
                    }
                    Console.WriteLine("Dapper:{0}", time / (iterations));
                    break;
            }
        }

        public static long GetEmployeesWithAdo()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var dt = new DataTable();
            var conn = new SqlConnection(
                @"Data Source = (local); Initial Catalog = AdventureWorks2012; Integrated Security = True;");
            var adaptor = new SqlDataAdapter("GetAllPersons", conn);
            adaptor.Fill(dt);
            var accessor = TypeAccessor.Create(typeof(GetAllPersons_Result));
            MemberSet members = accessor.GetMembers();

            var list = new List<GetAllPersons_Result>();
            foreach(DataRow row in dt.Rows)
            {
                var person = new GetAllPersons_Result();
                foreach (var member in members)
                {
                    if (row[member.Name] != DBNull.Value)
                    {
                        accessor[person, member.Name] = row[member.Name];
                    }
                }
                list.Add(person);
            }

            stopWatch.Stop();
            //Console.WriteLine("Ado Net time taken:{0}",stopWatch.ElapsedMilliseconds);
            return stopWatch.ElapsedTicks;
            //Console.Read();

        }

        public static long GetEmployeesWithEFramework()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var context = new AdventureWorks2012Entities1();
            List<GetAllPersons_Result> list = context.GetAllPersons().ToList();
            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
            //Console.WriteLine("Entity framework time taken:{0}", stopWatch.ElapsedMilliseconds);
            //Console.Read();
        }

        public static long GetEmployeesWithDappper()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            using (IDbConnection conn = new SqlConnection(
                @"Data Source = (local); Initial Catalog = AdventureWorks2012; Integrated Security = True;"))
            {
               List<GetAllPersons_Result> list = 
                    conn.Query<GetAllPersons_Result>("GetAllPersons", commandType: CommandType.StoredProcedure).ToList();
            }
            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
        }

    }

}
