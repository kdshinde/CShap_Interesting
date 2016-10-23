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

    //class Employee
    //{
    //    public string LastName { get; set; }
    //    public int Salary { get; set; }
    //    public string Address { get; set; }
    //}

    //Reflection, datatable and SQL Types
    //class Program
    //{
    //    static void Main()
    //    {
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

    //        List<Employee> list = new List<Employee> { emp, emptwo };

    //        var properties = typeof(Employee).GetProperties();

    //        var dataTable = new DataTable();
    //        foreach (var info in properties)
    //        {
    //            dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
    //        }

    //        foreach (var entity in list)
    //        {
    //            object[] values = new object[properties.Length];
    //            for (int i = 0; i < properties.Length; i++)
    //            {
    //                values[i] = properties[i].GetValue(entity);
    //            }

    //            dataTable.Rows.Add(values);
    //        }

    //        AddEmployee(dataTable);
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


    //Reflection vs Direct Call vs Delegates

    //class Program
    //{
    //    static void Main()
    //    {
    //        var stopwatch = new Stopwatch();
    //        var timing = new ReflectionTiming();

    //        MethodInfo info = typeof(ReflectionTiming).GetMethod("Wait");
    //        //To make sure method is JITed
    //        timing.Wait();

    //        //Call using Reflectin Invoke method
    //        stopwatch.Start();
    //        for (int i = 0; i < 1000000; i++)
    //        {
    //            info.Invoke(timing, null);
    //        }
    //        stopwatch.Stop();
    //        Console.WriteLine(stopwatch.ElapsedTicks + " for Reflection");
    //        stopwatch.Reset();

    //        Action converted = (Action)Delegate.CreateDelegate(typeof(Action), null, info);
    //        //Call using Delegate
    //        stopwatch.Start();
    //        for (int i = 0; i < 1000000; i++)
    //        {
    //            converted();
    //        }
    //        stopwatch.Stop();
    //        Console.WriteLine(stopwatch.ElapsedTicks + " for Delegate call");

    //        //Direct Call to the method
    //        stopwatch.Start();
    //        for (int i = 0; i < 1000000; i++)
    //        {
    //            timing.Wait();
    //        }
    //        stopwatch.Stop();
    //        Console.WriteLine(stopwatch.ElapsedTicks + " for Direct call");
    //        stopwatch.Reset();

    //        Console.Read();

    //        /*Result: Performce measured in Ticks, Delegate definitely is faster than reflection invoke :)
    //          	Direct	Delegate	Reflection
    //    Run1	19	    7       	418
    //    Run2	18	    11	        400
    //    Run3	14	    7	        421

    //         */
    //    }
    //}

    //public class ReflectionTiming
    //{
    //    public void Wait()
    //    {
    //        //Doing nothing
    //    }
    //}

    class Program
    {
        static void Main()
        {
            int rootValue = 16;
            int[] nodeValues = { 12, 20, 10, 14, 17, 25, 13, 15 };
            //Create tree with provided values
            BinarySearchTree tree = new BinarySearchTree(rootValue);
            tree.PopulateBinarySearchTree(nodeValues);
        }
    }

    /// <summary>
    /// Represents a Binary Search Tree. Provides methods to populate the BST, finding height
    /// </summary>
    public class BinarySearchTree
    {
        public Node RootNode { get; set; }

        public BinarySearchTree(int rootValue)
        {
            RootNode = new Node(rootValue);
        }

        /// <summary>
        /// Create nodes from array of values and add them to BST
        /// </summary>
        /// <param name="nodeValues"></param>
        public void PopulateBinarySearchTree(int[] nodeValues)
        {
            foreach (var value in nodeValues)
            {
                InsertNode(RootNode, value);
            }
        }

        /// <summary>
        /// Add node to the BST
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Node InsertNode(Node node, int data)
        {
            if (node == null)
            {
                node = new Node(data);
                return node;
            }
            if (data <= node.Data)
            {
                //Fill up left node with data
                node.LeftNode = InsertNode(node.LeftNode, data);
            }
            else if (data >= node.Data)
            {
                //Fill up right node with data
                node.RightNode = InsertNode(node.RightNode, data);
            }
            return node;
        }

        /// <summary>
        /// Height of a Binary tree =  Max(Left subtree, Right subtree) + 1
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int FindHeight(Node root)
        {
            if (root == null) return -1;
            int left = FindHeight(root.LeftNode);
            int right = FindHeight(root.RightNode);
            return Math.Max(left, right) + 1;
        }
    }

    public class Node
    {
        public int Data { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }

        public Node(int data)
        {
            Data = data;
        }
    }
}


