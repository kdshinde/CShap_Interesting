using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using FastMember;

namespace LearnDotNet
{
    ///// <summary>
    ///// Exception handling: https://msdn.microsoft.com/library/ms229005(v=vs.100).aspx
    ///// </summary>
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        try
    //        {
    //            string[] str = { "1", "2", "3" };
    //            string newstring = GetNewstring(str);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.StackTrace);
    //            Console.Read();
    //        }
    //    }

    //    private static string GetNewstring(string[] s)
    //    {
    //        try
    //        {
    //            return GetAnotherNewstring(s);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw;
    //        }
    //    }

    //    private static string GetAnotherNewstring(string[] s)
    //    {
    //        try
    //        {
    //            int i = 3;
    //            int j = 6;
    //            return s[5];
    //        }
    //        catch (IndexOutOfRangeException e)
    //        {
    //            throw;
    //        }
    //    }
    //}

    //class Program
    //{
    //    static void Main()
    //    {
    //        int n = 12;
    //        List<int> list = new List<int>();
    //        Solve(n, 0, list);
    //        int ans = list.Min();
    //    }

    //    static void Solve(int n, int count, List<int> list)
    //    {
    //        if (n == 0)
    //        {
    //            list.Add(count);
    //        }
    //        else
    //        {
    //            for (int i = 1; i * i <= n; i++)
    //            {
    //                Solve(n - i * i, count + 1, list);
    //            }
    //        }
    //    }
    //}

    //Reflection, datatable and SQL Types
    //class Program
    //{
    //    static void Main()
    //    {
    //        IRun fillDataTable = new Reflection();
    //        var emp = new Employee
    //        {
    //            LastName = "Shinde",
    //            Salary = 10,
    //            Address = "Boston"
    //        };

    //        var emptwo = new Employee
    //        {
    //            LastName = "Shinde",
    //            Salary = 20,
    //            Address = "Mumbai"
    //        };

    //        IEnumerable<Employee> list = new List<Employee> { emp, emptwo };

    //        object employeesObj;
    //        fillDataTable.Run(fillDataTable.Methods.FillDataTableFromCollection,out employeesObj, list);

    //        AddEmployee((DataTable)employeesObj);
    //    }

    //    public static void AddEmployee(DataTable dataTable)
    //    {
    //        using (var conn = new SqlConnection(
    //                        @"Data Source = (local); Initial Catalog = MyCompany; Integrated Security = True;"))
    //        {
    //            conn.Open();
    //            using (var cmd = new SqlCommand("AddEmployee"))
    //            {
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Connection = conn;
    //                cmd.Parameters.AddWithValue("@EmpList", dataTable);
    //                cmd.ExecuteNonQuery();
    //            }
    //        }
    //    }
    //}

    //class Program
    //{
    //    static void Main()
    //    {
    //        // Display powers of 2 up to the exponent of 8:
    //        foreach (int i in Power(2, 8))
    //        {
    //            Console.Write("{0} ", i);
    //        }
    //    }

    //    public static IEnumerable<int> Power(int number, int exponent)
    //    {
    //        int result = 1;

    //        for (int i = 0; i < exponent; i++)
    //        {
    //            result = result * number;
    //            yield return result;
    //        }
    //    }
    //}

    //Reflection Timing
    //class Program
    //{
    //    static void Main()
    //    {
    //        IRun reflectionTiming = new Reflection();
    //        reflectionTiming.Run();
    //    }
    //}


    //class Program
    //{
    //    static void Main()
    //    {
    //        int rootValue = 16;
    //        int[] nodeValues = { 12, 20, 10, 14, 17, 25, 13, 15 };
    //        //Create tree with provided values
    //        BinarySearchTree tree = new BinarySearchTree(rootValue);
    //        tree.PopulateBinarySearchTree(nodeValues);
    //        int height = tree.GetHeight(tree.RootNode);
    //    }
    //}

    class Program
    {
        static void Main()
        {
            var ormTest = new OrmTest();
            ormTest.Run(ormTest.Methods.GetEmployeesWithEFramework, null);
            ormTest.Run(ormTest.Methods.GetEmployeesWithAdo, null);
            ormTest.Run(ormTest.Methods.GetEmployeesWithDappper,null);
            
            Console.Read();
            //Test results for 1000 loops each loading around 20K rows, 11/6/2016
            //Entity Framework 6:Avg 544 ms
            //ADO net, reflection time taken: Avg 638 ms
            //Dapper : 523 ms
            //Note: Asked question on Stack Overflow: http://stackoverflow.com/questions/40453321/why-is-c-sharp-ado-net-slower-than-entity-framework-6-1-3
            //I ran the code for a single employee and Ado .Net was around 50% faster than Entity Framework. I kept ..
            //on increasing the number of employees and reached threshold at around 100 employees when EF started taking over

        }
    }
}


