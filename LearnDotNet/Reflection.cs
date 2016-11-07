using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FastMember;

namespace LearnDotNet
{
    /// <summary>
    /// Reflection vs Direct Call vs Delegates
    /// </summary>
    public class Reflection : IRun
    {
        public Methods Methods { get; } = new Methods();
        public int Iterations = 100;

        public void Run(string method, params object[] parameters)
        {
            switch (method)
            {
                case "ReflectionTiming":
                    ReflectionTiming();
                    break;
                case "PropertyAccessorTest":
                    long time = 0;
                    DataTable dt = GetEmployees();
                    for (int i = 0; i < Iterations; i++)
                    {
                        time = GetEmployeesWithReflection(dt);
                    }
                    Console.WriteLine("Property set with Reflection took:{0}", time);
                    time = 0;
                    for (int i = 0; i < Iterations; i++)
                    {
                        time = GetEmployeesWithFastMember(dt);
                    }
                    Console.WriteLine("Property set with Fast Member took:{0}", time);
                    break;
            }
        }

        public void Run(string method, out object obj, params object[] parameters)
        {
            obj = null;
            switch (method)
            {
                case "FillDataTableFromCollection":
                    obj = FillDataTableFromCollection(parameters);
                    break;
            }
        }

        public void Wait()
        {
            //Doing nothing
        }

        public DataTable FillDataTableFromCollection(params object[] parameters)
        {
            var employees = (IEnumerable<Employee>)parameters[0];

            var properties = typeof(Employee).GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name,
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (var entity in employees)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static DataTable GetEmployees()
        {
            var dt = new DataTable();
            var conn = new SqlConnection(
                @"Data Source = (local); Initial Catalog = AdventureWorks2012; Integrated Security = True;");
            var adaptor = new SqlDataAdapter("GetAllPersons", conn);
            adaptor.Fill(dt);
            return dt;
        }

        public static long GetEmployeesWithFastMember(DataTable dt)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var accessor = TypeAccessor.Create(typeof(GetAllPersons_Result));
            MemberSet members = accessor.GetMembers();

            var list = new List<GetAllPersons_Result>();
            foreach (DataRow row in dt.Rows)
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
            return stopWatch.ElapsedTicks;
        }

        public static long GetEmployeesWithReflection(DataTable dt)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            PropertyInfo[] properties = typeof(GetAllPersons_Result).GetProperties();

            var list = new List<GetAllPersons_Result>();
            foreach (DataRow row in dt.Rows)
            {
                var person = new GetAllPersons_Result();
                foreach (var property in properties)
                {
                    if (row[property.Name] != DBNull.Value)
                    {
                        property.SetValue(person, row[property.Name], null);
                    }
                }
                list.Add(person);
            }

            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
        }

        private static void ReflectionTiming()
        {
            var stopwatch = new Stopwatch();
            var timing = new Reflection();

            MethodInfo info = typeof(Reflection).GetMethod("Wait");
            //To make sure method is JITed
            timing.Wait();

            //Call using Reflectin Invoke method
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                info.Invoke(timing, null);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks + " for Reflection");
            stopwatch.Reset();

            Action converted = (Action)Delegate.CreateDelegate(typeof(Action), null, info);
            //Call using Delegate
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                converted();
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks + " for Delegate call");

            //Direct Call to the method
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                timing.Wait();
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks + " for Direct call");
            stopwatch.Reset();

            Console.Read();

            /*Result: Performce measured in Ticks, Delegate definitely is faster than reflection invoke :)
              	Direct	Delegate	Reflection
        Run1	19	    7       	418
        Run2	18	    11	        400
        Run3	14	    7	        421

             */
        }
    }

}
